using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using MyCompany.MyProduct.Application.Abstractions.Common;
using MyCompany.MyProduct.Application.Abstractions.Data;
using MyCompany.MyProduct.Core.Primitives;

namespace MyCompany.MyProduct.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly IDateTime _dateTime;

    public UnitOfWork(ApplicationDbContext dbContext, IPublisher publisher, IDateTime dateTime)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync(cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _dbContext.Database.BeginTransactionAsync(cancellationToken);

    public void UpdateAuditableEntities()
    {
        var entries = _dbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = _dateTime.UtcNow;

            if (entityEntry.State == EntityState.Modified)
                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = _dateTime.UtcNow;
        }
    }

    public void UpdateSoftDeletableEntities()
    {
        foreach (var entityEntry in _dbContext.ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted) continue;

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = DateTime.UtcNow;
            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        foreach (var referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
        {
            referenceEntry.TargetEntry.State = EntityState.Unchanged;
            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregateRoots = GetChangedAggregateRoots();
        var domainEvents = GetDomainEvents(aggregateRoots);
        ClearDomainEvents(aggregateRoots);

        await Task.WhenAll(domainEvents.Select(domainEvent => _publisher.Publish(domainEvent, cancellationToken)));
    }

    private List<EntityEntry<AggregateRoot>> GetChangedAggregateRoots()
        => _dbContext.ChangeTracker.Entries<AggregateRoot>().Where(e => e.Entity.GetDomainEvents().Any()).ToList();

    private static IEnumerable<IDomainEvent> GetDomainEvents(IEnumerable<EntityEntry<AggregateRoot>> aggregateRoots)
        => aggregateRoots.SelectMany(e => e.Entity.GetDomainEvents());

    private static void ClearDomainEvents(IEnumerable<EntityEntry<AggregateRoot>> aggregateRoots)
        => aggregateRoots.ToList().ForEach(e => e.Entity.ClearDomainEvents());
}
