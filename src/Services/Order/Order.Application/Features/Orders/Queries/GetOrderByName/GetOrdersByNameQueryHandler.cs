using BuildingBlocks.Abstractions.CQRS;
using Mapster;
using Order.Application.Dtos;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Queries.GetOrderByName
{
    public record GetOrdersByNameQuery(string OrderName) : IQuery<GetOrdersByQueryResult>;

    public record GetOrdersByQueryResult(IEnumerable<OrderDto> Orders);

    public class GetOrdersByNameQueryHandler(IOrderRepository orderRepository)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByQueryResult>
    {
        public async Task<GetOrdersByQueryResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersByNameAsync(query.OrderName, cancellationToken);

            var ordersDto = orders.Adapt<IEnumerable<OrderDto>>(); //TODO: Verify if this is working

            return new GetOrdersByQueryResult(ordersDto);
        }

        private static List<OrderDto> ProjectToOrderDto(List<OrderDto> orders)
        {
            return new List<OrderDto>();
        }
    }
}