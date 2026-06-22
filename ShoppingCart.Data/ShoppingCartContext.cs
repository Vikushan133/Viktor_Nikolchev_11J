using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Category).HasMaxLength(50);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                entity.Property(c => c.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(c => c.Address).HasMaxLength(200);
                entity.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId);
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var products = new[]
            {
                new Product { ProductId = 1, Name = "Мишка", Description = "Оптична безжична мишка", Price = 25.99m, StockQuantity = 50, Category = "Периферия" },
                new Product { ProductId = 2, Name = "Клавиатура", Description = "Механична клавиатура с RGB", Price = 70.00m, StockQuantity = 30, Category = "Периферия" },
                new Product { ProductId = 3, Name = "Монитор", Description = "24-инчов Full HD монитор", Price = 350.00m, StockQuantity = 15, Category = "Дисплеи" },
                new Product { ProductId = 4, Name = "Слушалки", Description = "Безжични слушалки с шумопотискане", Price = 80.00m, StockQuantity = 40, Category = "Аудио" },
                new Product { ProductId = 5, Name = "Лаптоп", Description = "15.6-инчов лаптоп с 16GB RAM", Price = 1500.00m, StockQuantity = 10, Category = "Компютри" },
            };

            var carts = new[]
            {
                new Cart { CartId = 1, CustomerName = "Иван Петров", DateCreated = new DateTime(2026, 6, 1), TotalPrice = 95.99m, Status = CartStatus.Active, Address = "ул. Главна 1, София" },
                new Cart { CartId = 2, CustomerName = "Мария Иванова", DateCreated = new DateTime(2026, 6, 5), TotalPrice = 350.00m, Status = CartStatus.Ordered, Address = "бул. България 25, Пловдив" },
                new Cart { CartId = 3, CustomerName = "Георги Стоянов", DateCreated = new DateTime(2026, 6, 10), TotalPrice = 1580.00m, Status = CartStatus.Shipped, Address = "ул. Вардар 8, Варна" },
                new Cart { CartId = 4, CustomerName = "Елена Димитрова", DateCreated = new DateTime(2026, 6, 15), TotalPrice = 80.00m, Status = CartStatus.Active, Address = "ж.к. Тракия 45, Бургас" },
                new Cart { CartId = 5, CustomerName = "Николай Христов", DateCreated = new DateTime(2026, 6, 18), TotalPrice = 1575.99m, Status = CartStatus.Active, Address = "ул. Пирин 12, Русе" },
            };

            var cartItems = new[]
            {
                new CartItem { CartItemId = 1, CartId = 1, ProductId = 1, Quantity = 1, UnitPrice = 25.99m, AddedDate = new DateTime(2026, 6, 1) },
                new CartItem { CartItemId = 2, CartId = 1, ProductId = 4, Quantity = 1, UnitPrice = 70.00m, AddedDate = new DateTime(2026, 6, 1) },
                new CartItem { CartItemId = 3, CartId = 2, ProductId = 3, Quantity = 1, UnitPrice = 350.00m, AddedDate = new DateTime(2026, 6, 5) },
                new CartItem { CartItemId = 4, CartId = 3, ProductId = 5, Quantity = 1, UnitPrice = 1500.00m, AddedDate = new DateTime(2026, 6, 10) },
                new CartItem { CartItemId = 5, CartId = 3, ProductId = 2, Quantity = 1, UnitPrice = 80.00m, AddedDate = new DateTime(2026, 6, 10) },
                new CartItem { CartItemId = 6, CartId = 4, ProductId = 4, Quantity = 1, UnitPrice = 80.00m, AddedDate = new DateTime(2026, 6, 15) },
                new CartItem { CartItemId = 7, CartId = 5, ProductId = 1, Quantity = 2, UnitPrice = 25.99m, AddedDate = new DateTime(2026, 6, 18) },
                new CartItem { CartItemId = 8, CartId = 5, ProductId = 5, Quantity = 1, UnitPrice = 1500.00m, AddedDate = new DateTime(2026, 6, 18) },
            };

            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Cart>().HasData(carts);
            modelBuilder.Entity<CartItem>().HasData(cartItems);
        }
    }
}
