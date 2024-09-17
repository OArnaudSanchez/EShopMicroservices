namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session)
        : IBasketRepository
    {
        public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        }

        public void UpsertBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            session.Store(shoppingCart);
        }

        public void DeleteBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            session.Delete(shoppingCart);
        }
    }
}