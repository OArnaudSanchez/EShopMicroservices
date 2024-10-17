namespace Basket.API.Basket.StoreBasket
{
    //TODO: Use Dtos instead of entities
    //TODO: Organize this in Request / Response folders
    public record StoreBastetRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //TODO: Use Handler method to refactor this
            app.MapPost("/basket", async (StoreBastetRequest request, ISender sender) =>
            {
                var result = await sender.Send(new StoreBasketCommand(request.Cart));
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserName}", response.UserName);
            })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket")
            .WithOpenApi();
        }
    }
}