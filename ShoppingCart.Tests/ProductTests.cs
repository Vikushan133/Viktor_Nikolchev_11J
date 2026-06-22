using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Tests;

public class ProductTests
{
    private ShoppingCartContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ShoppingCartContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ShoppingCartContext(options);
    }

    [Test]
    public void AddProduct_ShouldIncreaseProductCount()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        var product = new Product
        {
            Name = "Тестов продукт",
            Description = "Описание",
            Price = 10.00m,
            StockQuantity = 5,
            Category = "Тест"
        };

        service.Create(product);

        var products = service.GetAll();
        Assert.That(products.Count, Is.EqualTo(1));
        Assert.That(products[0].Name, Is.EqualTo("Тестов продукт"));
    }

    [Test]
    public void GetProductById_ShouldReturnCorrectProduct()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        var product = new Product
        {
            Name = "Мишка",
            Description = "Оптична мишка",
            Price = 25.99m,
            StockQuantity = 10,
            Category = "Периферия"
        };

        service.Create(product);
        var retrieved = service.GetById(product.ProductId);

        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved.Name, Is.EqualTo("Мишка"));
        Assert.That(retrieved.Price, Is.EqualTo(25.99m));
    }

    [Test]
    public void UpdateProduct_ShouldChangePrice()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        var product = new Product
        {
            Name = "Клавиатура",
            Description = "Механична",
            Price = 70.00m,
            StockQuantity = 20,
            Category = "Периферия"
        };

        service.Create(product);

        product.Price = 65.00m;
        service.Update(product);

        var updated = service.GetById(product.ProductId);
        Assert.That(updated.Price, Is.EqualTo(65.00m));
    }

    [Test]
    public void DeleteProduct_ShouldRemoveProduct()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        var product = new Product
        {
            Name = "Монитор",
            Description = "24 инча",
            Price = 350.00m,
            StockQuantity = 5,
            Category = "Дисплеи"
        };

        service.Create(product);
        Assert.That(service.GetAll().Count, Is.EqualTo(1));

        service.Delete(product.ProductId);
        Assert.That(service.GetAll().Count, Is.EqualTo(0));
    }

    [Test]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        service.Create(new Product { Name = "A", Description = "D1", Price = 10, StockQuantity = 1, Category = "Cat1" });
        service.Create(new Product { Name = "B", Description = "D2", Price = 20, StockQuantity = 2, Category = "Cat2" });
        service.Create(new Product { Name = "C", Description = "D3", Price = 30, StockQuantity = 3, Category = "Cat3" });

        var products = service.GetAll();
        Assert.That(products.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetProductById_NonExistent_ShouldReturnNull()
    {
        var context = CreateContext();
        var service = new ProductService(context);

        var result = service.GetById(999);
        Assert.That(result, Is.Null);
    }
}
