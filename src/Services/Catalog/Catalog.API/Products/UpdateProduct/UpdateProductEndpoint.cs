namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(
       Guid Id,
       string Name,
       List<string> Categories,
       string Description,
       string ImageFile,
       decimal Price);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{productId}", async (Guid productId, UpdateProductRequest productRequest, ISender sender) =>
            {
                if (productId != productRequest.Id) return Results.BadRequest();

                var command = productRequest.Adapt<UpdateProductCommand>();
                await sender.Send(command);
                return Results.NoContent();
            })
            .WithName("UpdateProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
        }
    }
}