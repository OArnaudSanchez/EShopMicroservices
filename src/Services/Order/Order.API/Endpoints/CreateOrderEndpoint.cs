using Carter;
using MediatR;
using Order.Application.Dtos;
using Order.Application.Features.Orders.Commands.CreateOrder;

namespace Order.API.Endpoints
{
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid OrderId);

    public class CreateOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", HandleCreateOrder)
                .WithName("CreateOrder")
                .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Order")
                .WithDescription("Create Order")
                .WithOpenApi();
        }

        public async Task<IResult> HandleCreateOrder(CreateOrderRequest request, ISender sender)
        {
            var result = await sender.Send(new CreateOrderCommand(request.Order));

            var response = new CreateOrderResponse(result);

            return Results.Created($"/order/{response}", request.Order);
        }
    }
}