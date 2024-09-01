namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) : ICommand<Unit>;

    internal class DeleteProductCommandHandler(IDocumentSession session)
        : ICommandHandler<DeleteProductCommand>
    {
        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            //TODO: Add Pipeline Behaviour
            var product = await session.LoadAsync<Product>(command.ProductId, cancellationToken);
            if (product is null) throw new ProductNotFoundException();

            session.Delete(product);
            await session.SaveChangesAsync();

            return Unit.Value;
        }
    }
}