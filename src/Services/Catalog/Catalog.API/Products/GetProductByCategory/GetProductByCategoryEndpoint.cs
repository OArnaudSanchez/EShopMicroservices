namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", HandleGetProductByCategory)
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
        }

        public async Task<IResult> HandleGetProductByCategory(string category, ISender sender)
        {
            var request = await sender.Send(new GetProductByCategoryQuery(category));
            var response = request.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response.Products);
        }
    }
}