using Microsoft.EntityFrameworkCore.Storage;

namespace MyCompany.MyProduct.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    void UpdateAuditableEntities();
    void UpdateSoftDeletableEntities();
}