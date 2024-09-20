namespace Basket.API.Exceptions
{
    [Serializable]
    internal class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string userName) : base("Basket", userName)
        {
        }
    }
}