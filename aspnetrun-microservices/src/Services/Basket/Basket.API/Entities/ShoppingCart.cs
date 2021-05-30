
using System.Collections.Generic;
using System.Linq;


namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public ShoppingCart()
        {

        }

        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get => Items.Select(k => k.Price * k.Quantity).Sum();
        }

    }
}
