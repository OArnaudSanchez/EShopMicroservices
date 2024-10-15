using BuildingBlocks.Abstractions.CQRS;
using Mapster;
using Order.Application.Dtos;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Queries.GetOrdersByCustomer
{
    public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerQueryResult>;

    public record GetOrdersByCustomerQueryResult(IEnumerable<OrderDto> Orders);

    public class GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository)
        : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerQueryResult>
    {
        public async Task<GetOrdersByCustomerQueryResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersByCustomerAsync(query.CustomerId, cancellationToken);

            var ordersDto = orders.Adapt<IEnumerable<OrderDto>>(); //TODO: Test this

            return new GetOrdersByCustomerQueryResult(ordersDto);
        }
    }
}