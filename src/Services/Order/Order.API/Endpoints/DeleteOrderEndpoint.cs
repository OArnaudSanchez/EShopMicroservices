using Carter;
using MediatR;
using Order.Application.Features.Orders.Commands.DeleteOrder;

namespace Order.API.Endpoints
{
    public class DeleteOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{orderId}", async (Guid orderId, ISender sender) =>
            {
                await sender.Send(new DeleteOrderCommand(orderId));

                return Results.NoContent();
            })
                .WithName("DeleteOrder")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Order")
                .WithDescription("Delete Order")
                .WithOpenApi();
        }
    }
}