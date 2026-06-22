using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Web.Controllers;

public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index(int? categoryId)
    {
        var products = categoryId.HasValue
            ? _productService.GetByCategory(categoryId.Value)
            : _productService.GetAll();
        ViewBag.CategoryId = categoryId;
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();
        return View(product);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.Create(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public IActionResult Edit(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.Update(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public IActionResult Delete(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
