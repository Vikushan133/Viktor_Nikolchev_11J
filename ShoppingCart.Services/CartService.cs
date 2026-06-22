using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class CartService
    {
        private readonly ShoppingCartContext _context;

        public CartService(ShoppingCartContext context)
        {
            _context = context;
        }

        public void CreateCart(Cart cart)
        {
            cart.DateCreated = DateTime.Now;
            cart.TotalPrice = 0;
            cart.Status = CartStatus.Active;
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public Cart GetCart(int id)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.CartId == id);
        }

        public Cart GetActiveCartByUser(string userId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId && c.Status == CartStatus.Active);
        }

        public List<Cart> GetAllCarts()
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .OrderByDescending(c => c.DateCreated)
                .ToList();
        }

        public void AddToCart(int cartId, int productId, int quantity)
        {
            var cart = _context.Carts.Find(cartId);
            var product = _context.Products.Find(productId);

            if (cart == null || product == null)
                return;

            if (product.StockQuantity < quantity)
                return;

            var existingItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    AddedDate = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }

            product.StockQuantity -= quantity;
            RecalculateTotal(cartId);
            _context.SaveChanges();
        }

        public void UpdateStatus(int cartId, CartStatus status)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart != null)
            {
                cart.Status = status;
                _context.SaveChanges();
            }
        }

        public void DeleteCart(int cartId)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public decimal CalculateTotal(int cartId)
        {
            var cart = GetCart(cartId);
            return cart?.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice) ?? 0;
        }

        private void RecalculateTotal(int cartId)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart != null)
            {
                cart.TotalPrice = _context.CartItems
                    .Where(ci => ci.CartId == cartId)
                    .Sum(ci => ci.Quantity * ci.UnitPrice);
            }
        }
    }
}
