namespace Order.Domain.Enums
{
    public enum OrderStatus
    {
        Draft = 1,

        Pending = 2,

        Completed = 3,

        Canceled = 4,

        Failed = 5,

        Unknown = 6,
    }
}