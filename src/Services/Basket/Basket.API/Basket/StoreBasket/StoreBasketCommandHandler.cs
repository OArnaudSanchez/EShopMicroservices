namespace Basket.API.Basket.StoreBasket
{
    //TODO: Use Dtos instead of entities (ShoppingCart)
    //TODO: Organize this in Commands/Queries/Validators folders
    public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repository, IUnitOfWork unitOfWork)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            repository.UpsertBasket(command.ShoppingCart, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new StoreBasketResult(command.ShoppingCart.UserName);
        }
    }
}