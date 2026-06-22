using System.Collections.Generic;

namespace ShoppingCart.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }

        public Category CategoryRef { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
