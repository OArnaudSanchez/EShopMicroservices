using BuildingBlocks.Exceptions;

namespace Order.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid orderId) : base("Order", orderId)
        {
        }
    }
}