namespace Order.Domain.Events
{
    public record OrderUpdatedDomainEvent(Entities.Order Order) : IDomainEvent;
}