using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.Features.EventHandlers.Commands.Domain
{
    public class OrderCreatedDomainEventCommandHandler(ILogger logger) : INotificationHandler<OrderCreatedDomainEvent>
    {
        public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}