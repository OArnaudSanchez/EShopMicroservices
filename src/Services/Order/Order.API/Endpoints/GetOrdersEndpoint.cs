using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using Order.Application.Dtos;
using Order.Application.Features.Orders.Queries.GetOrders;

//TODO: Use GlobalUsing

namespace Order.API.Endpoints
{
    public record GetOrdersRequest(PaginationRequest Pagination);
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

    public class GetOrdersEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] GetOrdersRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request.Pagination));

                var response = result.Adapt<GetOrdersResponse>();

                return Results.Ok(response);
            })
                .WithName("GetOrders")
                .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
                .WithSummary("Get Orders")
                .WithDescription("Get Orders")
                .WithOpenApi();
        }
    }
}