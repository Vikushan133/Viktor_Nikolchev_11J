using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Web.Controllers;

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
        var carts = _cartService.GetAllCarts();
        return View(carts);
    }

    public IActionResult Details(int id)
    {
        var cart = _cartService.GetCart(id);
        if (cart == null)
            return NotFound();
        return View(cart);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Cart cart)
    {
        if (ModelState.IsValid)
        {
            _cartService.CreateCart(cart);
            return RedirectToAction(nameof(Index));
        }
        return View(cart);
    }

    public IActionResult AddItem(int cartId, int? productId)
    {
        ViewBag.CartId = cartId;
        ViewBag.Products = new SelectList(_productService.GetAll(), "ProductId", "Name", productId);
        return View();
    }

    [HttpPost]
    public IActionResult AddItem(int cartId, int productId, int quantity)
    {
        _cartService.AddToCart(cartId, productId, quantity);
        return RedirectToAction(nameof(Details), new { id = cartId });
    }

    public IActionResult EditStatus(int id)
    {
        var cart = _cartService.GetCart(id);
        if (cart == null)
            return NotFound();

        ViewBag.Statuses = new SelectList(
            new[] { CartStatus.Active, CartStatus.Ordered, CartStatus.Shipped },
            cart.Status);

        return View(cart);
    }

    [HttpPost]
    public IActionResult EditStatus(int id, CartStatus status)
    {
        _cartService.UpdateStatus(id, status);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var cart = _cartService.GetCart(id);
        if (cart == null)
            return NotFound();
        return View(cart);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _cartService.DeleteCart(id);
        return RedirectToAction(nameof(Index));
    }
}
