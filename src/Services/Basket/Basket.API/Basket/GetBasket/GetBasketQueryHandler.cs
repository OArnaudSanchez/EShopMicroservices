namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

    //TODO: These classes should finish with QueryResult
    public record GetBasketResult(ShoppingCart ShoppingCart);

    public class GetBasketQueryHandler(IBasketRepository repository)
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasketAsync(query.UserName, cancellationToken);
            if (basket is null) throw new BasketNotFoundException(query.UserName);
            return new GetBasketResult(basket);
        }
    }
}