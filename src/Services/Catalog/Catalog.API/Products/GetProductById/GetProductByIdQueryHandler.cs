namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);

    internal class GetProductByIdQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            //TODO: Create a Pipeline Behaviour to log all the requests automatically
            var product = await session.LoadAsync<Product>(query.ProductId, cancellationToken);

            //TODO: This can be simplified using ternary operator in the return line of this method.
            if (product is null) throw new ProductNotFoundException();

            return new GetProductByIdResult(product);
        }
    }
}