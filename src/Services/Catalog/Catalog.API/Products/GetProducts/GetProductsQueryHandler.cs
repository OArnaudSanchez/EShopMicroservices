namespace Catalog.API.Products.GetProducts
{
    public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IPagedList<Product> Products);

    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            const int DEFAULT_PAGE_NUMBER = 1;
            const int DEFAULT_PAGE_SIZE = 10;
            var products = await session.Query<Product>()
                .ToPagedListAsync(query.PageNumber ?? DEFAULT_PAGE_NUMBER, query.PageSize ?? DEFAULT_PAGE_SIZE, cancellationToken);
            return new GetProductsResult(products);
        }
    }
}