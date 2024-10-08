namespace Order.Domain.Events
{
    public class OrderUpdatedDomainEvent(Entities.Order order) : IDomainEvent;
}