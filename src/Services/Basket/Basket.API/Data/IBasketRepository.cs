namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default);

        void UpsertBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);

        void DeleteBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    }
}