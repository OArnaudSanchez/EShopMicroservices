using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Domain.Abstractions;

namespace Order.Infrastructure.Data.Interceptors
{
    internal class DispatchDomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            DispatchEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            await DispatchEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchEvents(DbContext? context)
        {
            if (context is null) return;

            var aggregates = context
                .ChangeTracker
                .Entries<IAggregate>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = aggregates
                .SelectMany(x => x.DomainEvents)
                .ToList();

            aggregates.ForEach(x => x.ClearDomainEvents());

            var publishTasks = domainEvents.Select(domainEvent => mediator.Publish(domainEvent));
            await Task.WhenAll(publishTasks);
        }
    }
}