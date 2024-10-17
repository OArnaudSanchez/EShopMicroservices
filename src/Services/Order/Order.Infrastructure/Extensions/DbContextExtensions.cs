using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Order.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry)
        {
            return entry.References.Any(x =>
                x.TargetEntry is not null &&
                x.TargetEntry.Metadata.IsOwned() &&
                (x.TargetEntry.State == EntityState.Added || x.TargetEntry.State == EntityState.Modified));
        }
    }
}