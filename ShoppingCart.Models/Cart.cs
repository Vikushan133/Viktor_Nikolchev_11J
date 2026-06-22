using System;
using System.Collections.Generic;

namespace ShoppingCart.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public CartStatus Status { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
