namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand;

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class DeleteBasketCommandHandler(IBasketRepository repository, IUnitOfWork unitOfWork)
        : ICommandHandler<DeleteBasketCommand>
    {
        public async Task<Unit> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasketAsync(command.UserName, cancellationToken);
            if (basket is null) throw new BasketNotFoundException(command.UserName);

            repository.DeleteBasket(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}