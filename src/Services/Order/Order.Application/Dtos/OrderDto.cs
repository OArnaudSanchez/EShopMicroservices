﻿using Order.Domain.Enums;

namespace Order.Application.Dtos
{
    public record OrderDto(
        Guid OrderId,
        Guid CustomerId,
        string OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        OrderStatus Status,
        List<OrderItemDto> OrderItems);
}