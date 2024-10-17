using Carter;
using MediatR;
using Order.Application.Dtos;
using Order.Application.Features.Orders.Commands.UpdateOrder;

namespace Order.API.Endpoints
{
    public record UpdateOrderRequest(OrderDto Order);

    public class UpdateOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders/{orderId}", async (Guid orderId, UpdateOrderRequest request, ISender sender) =>
            {
                if (orderId != request.Order.OrderId) return Results.BadRequest();

                await sender.Send(new UpdateOrderCommand(request.Order));

                return Results.NoContent();
            })
               .WithName("UpdateOrder")
               .Produces(StatusCodes.Status204NoContent)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .WithSummary("Update Order")
               .WithDescription("Update Order")
               .WithOpenApi();
        }
    }
}