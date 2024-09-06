namespace Catalog.API.Products.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(command => command.ProductId)
                .NotEmpty().WithMessage("Product Id is required");
        }
    }
}