namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; } = [];

        public decimal TotalPrice => ShoppingCartItems.Sum(x => x.Price * x.Quantity);

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public ShoppingCart()
        {
        }
    }
}