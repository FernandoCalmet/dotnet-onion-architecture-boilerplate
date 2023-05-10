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
        _dbContext = dbContext;
        _publisher = publisher;
        _dateTime = dateTime;
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
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc)
                    .CurrentValue = _dateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc)
                    .CurrentValue = _dateTime.UtcNow;
            }
        }
    }

    public void UpdateSoftDeletableEntities()
    {
        foreach (EntityEntry<ISoftDeletableEntity> entityEntry in _dbContext.ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State != EntityState.Deleted)
            {
                continue;
            }

            entityEntry.Property(nameof(ISoftDeletableEntity.DeletedOnUtc)).CurrentValue = DateTime.UtcNow;
            entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
        {
            return;
        }

        foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
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

        await PublishEventsAsync(domainEvents, cancellationToken);
    }

    private List<EntityEntry<AggregateRoot>> GetChangedAggregateRoots()
    {
        return _dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.GetDomainEvents().Any())
            .ToList();
    }

    private static IEnumerable<IDomainEvent> GetDomainEvents(IEnumerable<EntityEntry<AggregateRoot>> aggregateRoots) =>
        aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.GetDomainEvents());

    private static void ClearDomainEvents(List<EntityEntry<AggregateRoot>> aggregateRoots) =>
        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

    private async Task PublishEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => _publisher.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}