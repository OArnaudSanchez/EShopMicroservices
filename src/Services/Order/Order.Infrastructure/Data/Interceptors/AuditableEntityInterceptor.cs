using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Domain.Abstractions;
using Order.Infrastructure.Extensions;

namespace Order.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            AuditEntity(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AuditEntity(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public static void AuditEntity(DbContext? context)
        {
            if (context is not null)
            {
                foreach (var entry in context.ChangeTracker.Entries<IEntity>())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = "demo-user";
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                    {
                        entry.Entity.LastModifiedBy = "demo-user";
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}