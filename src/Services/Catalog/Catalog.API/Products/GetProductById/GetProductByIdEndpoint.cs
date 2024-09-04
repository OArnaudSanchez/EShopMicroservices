namespace Catalog.API.Products.GetProductById
{
    public record GetProductResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{productId}", async (Guid productId, ISender sender) =>
            {
                var product = await sender.Send(new GetProductByIdQuery(productId));
                var response = product.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProduct")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product")
            .WithDescription("Get Product");
        }
    }
}