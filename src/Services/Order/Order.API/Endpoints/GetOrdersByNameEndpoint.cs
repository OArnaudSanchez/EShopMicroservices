using Carter;
using Mapster;
using MediatR;
using Order.Application.Dtos;
using Order.Application.Features.Orders.Queries.GetOrderByName;

namespace Order.API.Endpoints
{
    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Order);

    public class GetOrdersByNameEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //TODO: Use Handler
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(orderName));

                var response = result.Adapt<GetOrdersByNameResponse>();

                return Results.Ok(response);
            })
                .WithName("GetOrdersByName")
                .Produces(StatusCodes.Status200OK)
                .WithSummary("Get Orders By Name")
                .WithDescription("Get Orders By Name")
                .WithOpenApi();
        }
    }
}