namespace Order.Domain.Entities
{
    public class Order : Aggregate<OrderId>
    {
        public CustomerId CustomerId { get; private set; } = default!;

        public OrderName OrderName { get; private set; } = default!;

        public Address ShippingAddress { get; private set; } = default!;

        public Address BillingAddress { get; private set; } = default!;

        public Payment Payment { get; private set; } = default!;

        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            set { }
        }

        private readonly List<OrderItem> _orderItems = new();

        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public static Order Create(
            OrderId orderId,
            CustomerId customerId,
            OrderName orderName,
            Address shippingAddress,
            Address billingAddress,
            Payment payment)
        {
            var order = new Order
            {
                Id = orderId, //TODO: We should use factory method of value object
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };

            //Raise domain events
            order.AddDomainEvent(new OrderCreatedDomainEvent(order));

            return order;
        }

        public void Update(
            OrderName orderName,
            Address shippingAddress,
            Address billingAddress,
            Payment payment,
            OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedDomainEvent(this));
        }

        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.Find(x => x.ProductId == productId);

            if (orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}