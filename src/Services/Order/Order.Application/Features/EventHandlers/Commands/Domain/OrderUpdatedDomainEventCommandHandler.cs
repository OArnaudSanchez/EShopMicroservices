using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.Features.EventHandlers.Commands.Domain
{
    public class OrderUpdatedDomainEventCommandHandler(ILogger logger) : INotificationHandler<OrderUpdatedDomainEvent>
    {
        public Task Handle(OrderUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}