using BuildingBlocks.Abstractions.CQRS;
using FluentValidation;
using Order.Application.Dtos;
using Order.Application.Exceptions;
using Order.Application.Interfaces;
using Order.Domain.ValueObjects;

namespace Order.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order) : ICommand<Guid>;

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order.OrderId)
                .NotEmpty()
                .WithMessage("Id is required");

            RuleFor(x => x.Order.OrderName)
                .NotEmpty()
                .WithMessage("OrderName is required");

            RuleFor(x => x.Order.CustomerId)
                .NotNull()
                .WithMessage("CustomerId is required");
        }
    }

    public class UpdateOrderCommandHandler(IOrderRepository orderRepository) : ICommandHandler<UpdateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            //TODO: Logic to update order
            var order = await orderRepository.GetOrderByIdAsync(command.Order.OrderId, cancellationToken)
                ?? throw new OrderNotFoundException(command.Order.OrderId);

            //TODO: use mapping configuration, in this microservice we can use AutoMapper
            var shippingAddress = Address.Of(
                command.Order.ShippingAddress.FirstName,
                command.Order.ShippingAddress.LastName,
                command.Order.ShippingAddress.EmailAddress,
                command.Order.ShippingAddress.AddressLine,
                command.Order.ShippingAddress.Country,
                command.Order.ShippingAddress.State,
                command.Order.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(
                command.Order.BillingAddress.FirstName,
                command.Order.BillingAddress.LastName,
                command.Order.BillingAddress.EmailAddress,
                command.Order.BillingAddress.AddressLine,
                command.Order.BillingAddress.Country,
                command.Order.BillingAddress.State,
                command.Order.BillingAddress.ZipCode);

            var payment = Payment.Of(
                command.Order.Payment.CardName,
                command.Order.Payment.CardNumber,
                command.Order.Payment.Expiration,
                command.Order.Payment.Cvv,
                command.Order.Payment.PaymentMethod);

            order.Update(
                OrderName.Of(command.Order.OrderName),
                shippingAddress,
                billingAddress,
                payment,
                command.Order.Status);

            //TODO: Persist changes in db
            await orderRepository.UpdateOrderAsync(order, cancellationToken);

            //TODO: Use UnitOfWork

            return command.Order.OrderId;
        }
    }
}