using BuildingBlocks.Abstractions.CQRS;
using FluentValidation;
using Order.Application.Dtos;
using Order.Application.Interfaces;
using Order.Domain.ValueObjects;

namespace Order.Application.Features.Orders.Commands.CreateOrder
{
    //TODO: Separate these classes
    public record CreateOrderCommand(OrderDto Order) : ICommand<Guid>;

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order.OrderName)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Order.CustomerId)
                .NotNull()
                .WithMessage("CustomerId is required");

            RuleFor(x => x.Order.OrderItems)
                .NotEmpty()
                .WithMessage("OrderItems must not be empty");
        }
    }

    public class CreateOrderCommandHandler(IOrderRepository orderRepository) : ICommandHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(command.Order);

            await orderRepository.AddOrderAsync(order, cancellationToken);

            //TODO: Use UnitOfWork

            return order.Id.Value;
        }

        private static Domain.Entities.Order CreateNewOrder(OrderDto orderDto)
        {
            var shippingAddress = Address.Of(
                orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(
                orderDto.BillingAddress.FirstName,
                orderDto.BillingAddress.LastName,
                orderDto.BillingAddress.EmailAddress,
                orderDto.BillingAddress.AddressLine,
                orderDto.BillingAddress.Country,
                orderDto.BillingAddress.State,
                orderDto.BillingAddress.ZipCode);

            var payment = Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod);

            var order = Domain.Entities.Order.Create(
                OrderId.Of(Guid.NewGuid()), //This should be handled from inside
                CustomerId.Of(orderDto.CustomerId),
                OrderName.Of(orderDto.OrderName),
                shippingAddress,
                billingAddress,
                payment);

            foreach (var orderItem in orderDto.OrderItems)
            {
                order.Add(ProductId.Of(orderItem.ProductId), orderItem.Quantity, orderItem.Price);
            }

            return order;
        }
    }
}