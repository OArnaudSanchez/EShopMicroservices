namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record ProductPagination(int PageNumber, int PageSize, int PageCount, int TotalPages, bool HasPreviousPage, bool HasNextPage);
    public record GetProductsResponse(IEnumerable<Product> Products, ProductPagination Pagination);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", HandleGetProducts)
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }

        public async Task<IResult> HandleGetProducts([AsParameters] GetProductsRequest request, ISender sender)
        {
            var query = request.Adapt<GetProductQuery>();

            var products = await sender.Send(query);
            var response = products.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        }
    }
}