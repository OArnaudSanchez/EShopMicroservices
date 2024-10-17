namespace Order.Domain.ValueObjects
{
    public record Payment
    {
        public string CardName { get; } = string.Empty;

        public string CardNumber { get; } = string.Empty;

        public string Expiration { get; } = string.Empty; //TODO: The format must be Month/Year

        public string Cvv { get; } = string.Empty;

        public int PaymentMethod { get; } = default!;

        private const int CVV_MAXIMUM_LENGHT = 3;

        protected Payment() { }
        private Payment(
            string cardName,
            string cardNumber,
            string expiration,
            string cvv,
            int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            Cvv = cvv;
            PaymentMethod = paymentMethod;
        }

        public static Payment Of(
            string cardName,
            string cardNumber,
            string expiration,
            string cvv,
            int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, CVV_MAXIMUM_LENGHT);

            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }
    }
}