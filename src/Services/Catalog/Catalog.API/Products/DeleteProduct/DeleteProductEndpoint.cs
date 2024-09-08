namespace Catalog.API.Products.DeleteProduct
{
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{productId}", HandleDeleteProduct)
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
        }

        public async Task<IResult> HandleDeleteProduct(Guid productId, ISender sender)
        {
            await sender.Send(new DeleteProductCommand(productId));
            return Results.NoContent();
        }
    }
}