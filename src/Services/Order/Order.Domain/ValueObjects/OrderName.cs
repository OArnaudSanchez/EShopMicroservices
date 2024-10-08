namespace Order.Domain.ValueObjects
{
    public record OrderName
    {
        private const int DEFAULT_LENGHT = 5;
        public string Value { get; } = string.Empty;

        private OrderName(string value) => Value = value;

        public static OrderName Of(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DEFAULT_LENGHT);

            return new OrderName(value);
        }
    }
}