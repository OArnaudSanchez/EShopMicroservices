namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(
       Guid Id,
       string Name,
       List<string> Categories,
       string Description,
       string ImageFile,
       decimal Price) : ICommand<Unit>;

    internal class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand>
    {
        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            //TODO: Add pipeline behaviour
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null) throw new ProductNotFoundException();

            product = Product.Update(
                product,
                command.Name,
                command.Categories,
                command.Description,
                command.ImageFile,
                command.Price);

            session.Update(product);
            await session.SaveChangesAsync();
            return Unit.Value;
        }
    }
}