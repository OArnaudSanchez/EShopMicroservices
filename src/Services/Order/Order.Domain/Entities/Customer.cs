namespace Order.Domain.Entities
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public static Customer Create(CustomerId customerId, string name, string email)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(email);

            var customer = new Customer
            {
                Id = customerId,
                Name = name,
                Email = email
            };
            return customer;
        }
    }
}