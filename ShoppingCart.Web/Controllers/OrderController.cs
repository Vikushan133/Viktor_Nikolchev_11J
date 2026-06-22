using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;

namespace ShoppingCart.Web.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ShoppingCartContext _context;

    public OrderController(ShoppingCartContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userId = Models.IdentityExtensions.GetUserId(User);
        var orders = _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.OrderId == id);
        if (order == null)
            return NotFound();
        return View(order);
    }
}
