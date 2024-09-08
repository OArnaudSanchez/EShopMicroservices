namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(
       Guid Id,
       string Name,
       List<string> Categories,
       string Description,
       string ImageFile,
       decimal Price) : ICommand<Unit>;

    public class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand>
    {
        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null) throw new ProductNotFoundException(command.Id);

            product = Product.Update(
                product,
                command.Name,
                command.Price);

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}