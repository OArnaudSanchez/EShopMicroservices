using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Pagination;
using Mapster;
using Order.Application.Dtos;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(PaginationRequest Pagination) : IQuery<GetOrdersQueryResult>;
    public record GetOrdersQueryResult(PaginatedResult<OrderDto> Orders);

    public class GetOrdersQueryHandler(IOrderRepository orderRepository)
        : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
    {
        public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersAsync(query.Pagination, cancellationToken);

            var ordersDto = orders.Adapt<IEnumerable<OrderDto>>(); //TODO: verify if this is working

            var paginatedResult = new PaginatedResult<OrderDto>(
                query.Pagination.PageIndex,
                query.Pagination.PageSize,
                orders.Count(),
                ordersDto);
            return new GetOrdersQueryResult(paginatedResult);
        }
    }
}