using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(cachedBasket);
            }

            var basket = await basketRepository.GetBasketAsync(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonConvert.SerializeObject(basket), cancellationToken);
            return basket;
        }

        public void UpsertBasket(ShoppingCart shoppingCart)
        {
            basketRepository.UpsertBasket(shoppingCart);
            cache.SetString(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart.UserName));
        }

        public void DeleteBasket(ShoppingCart shoppingCart)
        {
            basketRepository.DeleteBasket(shoppingCart);
            cache.Remove(shoppingCart.UserName);
        }
    }
}