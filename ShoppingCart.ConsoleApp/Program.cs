using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;

var options = new DbContextOptionsBuilder<ShoppingCartContext>()
    .UseSqlite("Data Source=shoppingcart.db")
    .Options;

using var context = new ShoppingCartContext(options);
context.Database.EnsureCreated();

var productService = new ProductService(context);
var cartService = new CartService(context);

while (true)
{
    try { Console.Clear(); } catch { /* no console available */ }
    Console.WriteLine("=== ПАЗАРСКА КОЛИЧКА ===");
    Console.WriteLine();
    Console.WriteLine("1. Покажи всички продукти");
    Console.WriteLine("2. Добави продукт");
    Console.WriteLine("3. Редактирай продукт");
    Console.WriteLine("4. Изтрий продукт");
    Console.WriteLine();
    Console.WriteLine("5. Създай количка");
    Console.WriteLine("6. Добави продукт в количка");
    Console.WriteLine("7. Покажи количката");
    Console.WriteLine("8. Промени статус на количка");
    Console.WriteLine("9. Изтрий количка");
    Console.WriteLine("10. Изчисли обща цена");
    Console.WriteLine();
    Console.WriteLine("0. Изход");
    Console.Write("\nИзбери опция: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowAllProducts(productService);
            break;
        case "2":
            AddProduct(productService);
            break;
        case "3":
            EditProduct(productService);
            break;
        case "4":
            DeleteProduct(productService);
            break;
        case "5":
            CreateCart(cartService);
            break;
        case "6":
            AddToCart(cartService, productService);
            break;
        case "7":
            ShowCart(cartService);
            break;
        case "8":
            UpdateCartStatus(cartService);
            break;
        case "9":
            DeleteCart(cartService);
            break;
        case "10":
            CalculateTotal(cartService);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Невалидна опция!");
            break;
    }

    Console.WriteLine("\nНатисни произволен клавиш за да продължиш...");
    Console.ReadKey();
}

static void ShowAllProducts(ProductService service)
{
    var products = service.GetAll();
    Console.WriteLine("\n=== ВСИЧКИ ПРОДУКТИ ===");
    Console.WriteLine($"{"ID",-5} {"Име",-20} {"Цена",-10} {"Наличност",-10} {"Категория",-15}");
    Console.WriteLine(new string('-', 60));
    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId,-5} {p.Name,-20} {p.Price,-10:F2} {p.StockQuantity,-10} {p.Category,-15}");
    }
}

static void AddProduct(ProductService service)
{
    Console.Write("Име: ");
    var name = Console.ReadLine();
    Console.Write("Описание: ");
    var desc = Console.ReadLine();
    Console.Write("Цена: ");
    var price = decimal.Parse(Console.ReadLine());
    Console.Write("Количество: ");
    var qty = int.Parse(Console.ReadLine());
    Console.Write("Категория: ");
    var category = Console.ReadLine();

    var product = new Product
    {
        Name = name,
        Description = desc,
        Price = price,
        StockQuantity = qty,
        Category = category
    };

    service.Create(product);
    Console.WriteLine("Продуктът е добавен успешно!");
}

static void EditProduct(ProductService service)
{
    Console.Write("ID на продукта за редактиране: ");
    var id = int.Parse(Console.ReadLine());
    var product = service.GetById(id);

    if (product == null)
    {
        Console.WriteLine("Продуктът не е намерен!");
        return;
    }

    Console.Write($"Име ({product.Name}): ");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

    Console.Write($"Описание ({product.Description}): ");
    var desc = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(desc)) product.Description = desc;

    Console.Write($"Цена ({product.Price}): ");
    var price = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(price)) product.Price = decimal.Parse(price);

    Console.Write($"Количество ({product.StockQuantity}): ");
    var qty = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(qty)) product.StockQuantity = int.Parse(qty);

    Console.Write($"Категория ({product.Category}): ");
    var category = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(category)) product.Category = category;

    service.Update(product);
    Console.WriteLine("Продуктът е обновен успешно!");
}

static void DeleteProduct(ProductService service)
{
    Console.Write("ID на продукта за изтриване: ");
    var id = int.Parse(Console.ReadLine());
    service.Delete(id);
    Console.WriteLine("Продуктът е изтрит успешно!");
}

static void CreateCart(CartService service)
{
    Console.Write("Име на клиент: ");
    var name = Console.ReadLine();
    Console.Write("Адрес: ");
    var address = Console.ReadLine();

    var cart = new Cart
    {
        CustomerName = name,
        Address = address
    };

    service.CreateCart(cart);
    Console.WriteLine($"Количката е създадена с ID: {cart.CartId}");
}

static void AddToCart(CartService cartService, ProductService productService)
{
    Console.Write("ID на количка: ");
    var cartId = int.Parse(Console.ReadLine());
    Console.Write("ID на продукт: ");
    var productId = int.Parse(Console.ReadLine());
    Console.Write("Количество: ");
    var qty = int.Parse(Console.ReadLine());

    cartService.AddToCart(cartId, productId, qty);
    Console.WriteLine("Продуктът е добавен в количката!");
}

static void ShowCart(CartService service)
{
    Console.Write("ID на количка: ");
    var cartId = int.Parse(Console.ReadLine());
    var cart = service.GetCart(cartId);

    if (cart == null)
    {
        Console.WriteLine("Количката не е намерена!");
        return;
    }

    Console.WriteLine($"\n=== КОЛИЧКА #{cart.CartId} ===");
    Console.WriteLine($"Клиент: {cart.CustomerName}");
    Console.WriteLine($"Дата: {cart.DateCreated}");
    Console.WriteLine($"Статус: {cart.Status}");
    Console.WriteLine($"Адрес: {cart.Address}");
    Console.WriteLine();

    if (cart.CartItems.Count == 0)
    {
        Console.WriteLine("Количката е празна.");
        return;
    }

    Console.WriteLine($"{"Продукт",-20} {"Цена",-10} {"Кол-во",-8} {"Общо",-10}");
    Console.WriteLine(new string('-', 48));
    foreach (var item in cart.CartItems)
    {
        var productName = item.Product?.Name ?? "Неизвестен";
        var total = item.Quantity * item.UnitPrice;
        Console.WriteLine($"{productName,-20} {item.UnitPrice,-10:F2} {item.Quantity,-8} {total,-10:F2}");
    }
    Console.WriteLine(new string('-', 48));
    Console.WriteLine($"Total: {cart.TotalPrice,40:F2}");
}

static void UpdateCartStatus(CartService service)
{
    Console.Write("ID на количка: ");
    var cartId = int.Parse(Console.ReadLine());

    Console.WriteLine("Избери статус:");
    Console.WriteLine("1. Active");
    Console.WriteLine("2. Ordered");
    Console.WriteLine("3. Shipped");
    Console.Write("Избор: ");
    var choice = Console.ReadLine();

    CartStatus status = choice switch
    {
        "2" => CartStatus.Ordered,
        "3" => CartStatus.Shipped,
        _ => CartStatus.Active
    };

    service.UpdateStatus(cartId, status);
    Console.WriteLine("Статусът е обновен!");
}

static void DeleteCart(CartService service)
{
    Console.Write("ID на количка за изтриване: ");
    var cartId = int.Parse(Console.ReadLine());
    service.DeleteCart(cartId);
    Console.WriteLine("Количката е изтрита!");
}

static void CalculateTotal(CartService service)
{
    Console.Write("ID на количка: ");
    var cartId = int.Parse(Console.ReadLine());
    var total = service.CalculateTotal(cartId);
    Console.WriteLine($"Обща цена: {total:F2} лв.");
}
