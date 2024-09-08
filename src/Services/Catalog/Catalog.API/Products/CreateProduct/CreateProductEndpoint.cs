namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price);

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", HandleCreateProduct)
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }

        public async Task<IResult> HandleCreateProduct(CreateProductRequest createProductRequest, ISender sender)
        {
            var command = createProductRequest.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        }
    }
}