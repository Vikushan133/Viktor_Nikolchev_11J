using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;
using ShoppingCart.Models;
using ShoppingCart.Web.Models;

namespace ShoppingCart.Web.Controllers;

public class HomeController : Controller
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;

    public HomeController(ProductService productService, CartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var products = _productService.GetAll();
        var carts = _cartService.GetAllCarts();
        ViewBag.TotalProducts = products.Count;
        ViewBag.TotalCarts = carts.Count;
        ViewBag.ActiveCarts = carts.Count(c => c.Status == CartStatus.Active);
        return View(products.Take(3).ToList());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
