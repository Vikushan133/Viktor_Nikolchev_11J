using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;
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
        var categories = products.Select(p => p.Category).Distinct().ToList();
        ViewBag.Categories = products
            .GroupBy(p => p.Category)
            .Select(g => g.First())
            .Take(8)
            .ToList();
        return View(products.Take(8).ToList());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
