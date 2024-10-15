using BuildingBlocks.Abstractions.CQRS;
using FluentValidation;
using Order.Application.Exceptions;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId) : ICommand<Guid>;

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("{0} Must not be null");
        }
    }

    public class DeleteOrderCommandHandler(IOrderRepository orderRepository) : ICommandHandler<DeleteOrderCommand, Guid>
    {
        public async Task<Guid> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(command.OrderId, cancellationToken)
                ?? throw new OrderNotFoundException(command.OrderId);

            //TODO: Delete order from DB
            await orderRepository.DeleteOrderAsync(order, cancellationToken);

            //TODO: Use UnitOfWork

            return command.OrderId;
        }
    }
}