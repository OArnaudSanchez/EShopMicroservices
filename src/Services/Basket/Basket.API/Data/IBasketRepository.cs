﻿namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default);

        void UpsertBasket(ShoppingCart shoppingCart);

        void DeleteBasket(ShoppingCart shoppingCart);
    }
}