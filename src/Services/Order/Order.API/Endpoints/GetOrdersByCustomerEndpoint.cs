using Carter;
using Mapster;
using MediatR;
using Order.Application.Dtos;
using Order.Application.Features.Orders.Queries.GetOrdersByCustomer;

namespace Order.API.Endpoints
{
    public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByCustomerEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //TODO: Use Handler
            app.MapGet("/orders/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

                var response = result.Adapt<GetOrdersByCustomerResponse>();

                return Results.Ok(response);
            })
                .WithName("GetOrdersByCustomer")
                .Produces(StatusCodes.Status200OK)
                .WithSummary("Get Orders By Customer")
                .WithDescription("Get Orders By Customer")
                .WithOpenApi();
        }
    }
}