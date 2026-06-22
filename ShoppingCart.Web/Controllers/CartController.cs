using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.Web.Models;

namespace ShoppingCart.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly ProductService _productService;

    public CartController(CartService cartService, ProductService productService)
    {
        _cartService = cartService;
        _productService = productService;
    }

    public IActionResult Index()
    {
        var userId = User.GetUserId();
        var cart = _cartService.GetActiveCartByUser(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                CustomerName = User.Identity.Name,
                UserId = userId,
                DateCreated = DateTime.Now,
                TotalPrice = 0,
                Status = CartStatus.Active,
                Address = ""
            };
            _cartService.CreateCart(cart);
        }
        return View(cart);
    }

    public IActionResult AddItem(int productId)
    {
        var userId = User.GetUserId();
        var cart = _cartService.GetActiveCartByUser(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                CustomerName = User.Identity.Name,
                UserId = userId,
                DateCreated = DateTime.Now,
                TotalPrice = 0,
                Status = CartStatus.Active,
                Address = ""
            };
            _cartService.CreateCart(cart);
        }
        _cartService.AddToCart(cart.CartId, productId, 1);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        var userId = User.GetUserId();
        var cart = _cartService.GetActiveCartByUser(userId);
        if (cart == null || !cart.CartItems.Any())
            return RedirectToAction(nameof(Index));
        return View(cart);
    }

    [HttpPost]
    public IActionResult Checkout(string address)
    {
        var userId = User.GetUserId();
        var cart = _cartService.GetActiveCartByUser(userId);
        if (cart == null || !cart.CartItems.Any())
            return RedirectToAction(nameof(Index));

        cart.Address = address;
        cart.Status = CartStatus.Ordered;
        _cartService.UpdateStatus(cart.CartId, CartStatus.Ordered);

        return RedirectToAction("Index", "Order");
    }

    public IActionResult RemoveItem(int cartItemId)
    {
        var userId = User.GetUserId();
        var cart = _cartService.GetActiveCartByUser(userId);
        if (cart != null)
        {
            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (item != null)
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ShoppingCart.Data.ShoppingCartContext>();
                context.CartItems.Remove(item);
                context.SaveChanges();
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
