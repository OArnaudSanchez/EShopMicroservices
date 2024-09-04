namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(
       Guid Id,
       string Name,
       List<string> Categories,
       string Description,
       string ImageFile,
       decimal Price) : ICommand<Unit>;

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Product Id is required");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session)
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
            await session.SaveChangesAsync();
            return Unit.Value;
        }
    }
}