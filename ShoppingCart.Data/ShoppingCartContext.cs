using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Category).HasMaxLength(50);
                entity.Property(p => p.ImageUrl).HasMaxLength(500);
                entity.Property(p => p.Size).HasMaxLength(20);
                entity.Property(p => p.Color).HasMaxLength(30);
                entity.Property(p => p.Brand).HasMaxLength(50);

                entity.HasOne(p => p.CategoryRef)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.ImageUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                entity.Property(c => c.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(c => c.Address).HasMaxLength(200);
                entity.Property(c => c.UserId).HasMaxLength(450);
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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).HasMaxLength(20);
                entity.Property(o => o.ShippingAddress).HasMaxLength(300);
                entity.Property(o => o.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.OrderItemId);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var categories = new[]
            {
                new Category { CategoryId = 1, Name = "T-Shirts", Description = "Classic and trendy t-shirts for every style", ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&h=400&fit=crop" },
                new Category { CategoryId = 2, Name = "Jeans", Description = "Perfect fit jeans for any occasion", ImageUrl = "https://images.unsplash.com/photo-1542272456-7c6c7b4b08e2?w=600&h=400&fit=crop" },
                new Category { CategoryId = 3, Name = "Sneakers", Description = "Stylish and comfortable footwear", ImageUrl = "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=600&h=400&fit=crop" },
                new Category { CategoryId = 4, Name = "Jackets", Description = "Stay warm and look cool", ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=600&h=400&fit=crop" },
                new Category { CategoryId = 5, Name = "Shorts", Description = "Summer essentials for everyone", ImageUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=600&h=400&fit=crop" },
                new Category { CategoryId = 6, Name = "Dresses & Skirts", Description = "Elegant and casual dresses", ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=600&h=400&fit=crop" },
                new Category { CategoryId = 7, Name = "Accessories", Description = "Complete your look", ImageUrl = "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=600&h=400&fit=crop" },
                new Category { CategoryId = 8, Name = "Sweaters & Hoodies", Description = "Cozy and comfortable layers", ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=600&h=400&fit=crop" },
            };

            modelBuilder.Entity<Category>().HasData(categories);

            var products = new[]
            {
                // T-Shirts (1-8)
                new Product { ProductId = 1, Name = "Classic White T-Shirt", Description = "Timeless white cotton tee, essential for any wardrobe", Price = 29.99m, StockQuantity = 100, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&h=700&fit=crop", Size = "S-XXL", Color = "White", Brand = "FashionBase" },
                new Product { ProductId = 2, Name = "Black Essential Tee", Description = "Premium black t-shirt, soft-touch fabric", Price = 27.99m, StockQuantity = 120, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Black", Brand = "FashionBase" },
                new Product { ProductId = 3, Name = "Vintage Graphic Tee", Description = "Retro graphic print, faded finish", Price = 34.99m, StockQuantity = 60, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=600&h=700&fit=crop", Size = "M-XL", Color = "Grey", Brand = "RetroWear" },
                new Product { ProductId = 4, Name = "Striped Cotton Tee", Description = "Classic striped pattern, breathable cotton", Price = 32.99m, StockQuantity = 75, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=600&h=700&fit=crop", Size = "S-XL", Color = "Navy/White", Brand = "FashionBase" },
                new Product { ProductId = 5, Name = "Oversized Fit Tee", Description = "Trendy oversized fit, super comfortable", Price = 36.99m, StockQuantity = 90, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=600&h=700&fit=crop", Size = "M-XXL", Color = "Olive", Brand = "StreetStyle" },
                new Product { ProductId = 6, Name = "Crew Neck Plain Tee", Description = "Essential crew neck, everyday basic", Price = 25.99m, StockQuantity = 150, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=600&h=700&fit=crop", Size = "XS-XXL", Color = "Heather Grey", Brand = "FashionBase" },
                new Product { ProductId = 7, Name = "Printed Logo Tee", Description = "Bold logo print, urban style", Price = 31.99m, StockQuantity = 55, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1562157873-818bc0726f68?w=600&h=700&fit=crop", Size = "S-XL", Color = "Black", Brand = "StreetStyle" },
                new Product { ProductId = 8, Name = "Pocket Tee", Description = "Classic chest pocket tee, pre-shrunk cotton", Price = 28.99m, StockQuantity = 85, Category = "T-Shirts", CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1598033129183-c4f50c736e10?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Navy", Brand = "FashionBase" },
                // Jeans (9-14)
                new Product { ProductId = 9, Name = "Blue Slim Fit Jeans", Description = "Modern slim fit, stretch denim", Price = 59.99m, StockQuantity = 80, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1542272456-7c6c7b4b08e2?w=600&h=700&fit=crop", Size = "28-38", Color = "Blue", Brand = "DenimPro" },
                new Product { ProductId = 10, Name = "Black Skinny Jeans", Description = "Sleek black skinny jeans, stretch comfort", Price = 64.99m, StockQuantity = 70, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1565084888279-3b3e4f2c9d5b?w=600&h=700&fit=crop", Size = "28-36", Color = "Black", Brand = "DenimPro" },
                new Product { ProductId = 11, Name = "Light Wash Straight Jeans", Description = "Classic straight leg, light wash denim", Price = 54.99m, StockQuantity = 65, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=700&fit=crop", Size = "28-38", Color = "Light Blue", Brand = "DenimPro" },
                new Product { ProductId = 12, Name = "Ripped Boyfriend Jeans", Description = "Distressed boyfriend fit, casual cool", Price = 69.99m, StockQuantity = 45, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=600&h=700&fit=crop", Size = "26-34", Color = "Medium Blue", Brand = "StreetStyle" },
                new Product { ProductId = 13, Name = "Classic Bootcut Jeans", Description = "Timeless bootcut, comfortable mid-rise", Price = 57.99m, StockQuantity = 55, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=600&h=700&fit=crop", Size = "28-38", Color = "Dark Blue", Brand = "DenimPro" },
                new Product { ProductId = 14, Name = "High Waist Mom Jeans", Description = "Trendy high waist, relaxed fit", Price = 62.99m, StockQuantity = 60, Category = "Jeans", CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=600&h=700&fit=crop", Size = "26-34", Color = "Medium Wash", Brand = "DenimPro" },
                // Sneakers (15-21)
                new Product { ProductId = 15, Name = "Black Leather Sneakers", Description = "Premium leather sneakers, minimalist design", Price = 89.99m, StockQuantity = 50, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=600&h=700&fit=crop", Size = "39-45", Color = "Black", Brand = "UrbanWalk" },
                new Product { ProductId = 16, Name = "White Court Sneakers", Description = "Clean white court style, leather upper", Price = 79.99m, StockQuantity = 65, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=600&h=700&fit=crop", Size = "38-44", Color = "White", Brand = "UrbanWalk" },
                new Product { ProductId = 17, Name = "Running Mesh Sneakers", Description = "Lightweight mesh, responsive cushioning", Price = 94.99m, StockQuantity = 40, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=600&h=700&fit=crop", Size = "39-46", Color = "Blue/White", Brand = "SportFlex" },
                new Product { ProductId = 18, Name = "Canvas Low Top Sneakers", Description = "Classic canvas sneakers, casual everyday", Price = 49.99m, StockQuantity = 90, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1525966222134-fcfa99b594ae?w=600&h=700&fit=crop", Size = "37-44", Color = "Navy", Brand = "UrbanWalk" },
                new Product { ProductId = 19, Name = "Retro Style Sneakers", Description = "Vintage-inspired retro sneakers", Price = 109.99m, StockQuantity = 35, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=600&h=700&fit=crop", Size = "38-44", Color = "White/Red", Brand = "RetroWear" },
                new Product { ProductId = 20, Name = "Platform Sneakers", Description = "Chunky platform sneakers, bold style", Price = 84.99m, StockQuantity = 45, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1582582621959-48d27397dc69?w=600&h=700&fit=crop", Size = "37-42", Color = "White", Brand = "StreetStyle" },
                new Product { ProductId = 21, Name = "Slip-On Casual Sneakers", Description = "Easy slip-on, memory foam insole", Price = 44.99m, StockQuantity = 70, Category = "Sneakers", CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=600&h=700&fit=crop", Size = "38-44", Color = "Grey", Brand = "UrbanWalk" },
                // Jackets (22-27)
                new Product { ProductId = 22, Name = "Classic Denim Jacket", Description = "Timeless denim jacket, medium wash", Price = 119.99m, StockQuantity = 40, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Medium Blue", Brand = "DenimPro" },
                new Product { ProductId = 23, Name = "Leather Biker Jacket", Description = "Genuine leather biker jacket, zip front", Price = 189.99m, StockQuantity = 20, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1520975954732-35dd22299614?w=600&h=700&fit=crop", Size = "M-XXL", Color = "Black", Brand = "UrbanWalk" },
                new Product { ProductId = 24, Name = "Puffer Winter Jacket", Description = "Warm puffer jacket, water-resistant", Price = 149.99m, StockQuantity = 35, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Black", Brand = "SportFlex" },
                new Product { ProductId = 25, Name = "Bomber Jacket", Description = "Classic bomber, satin finish", Price = 99.99m, StockQuantity = 50, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1594744803329-e58b31de8bf5?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Olive", Brand = "StreetStyle" },
                new Product { ProductId = 26, Name = "Trench Coat", Description = "Elegant trench coat, double-breasted", Price = 159.99m, StockQuantity = 25, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1592878904946-b3cd8ae243d0?w=600&h=700&fit=crop", Size = "S-XL", Color = "Beige", Brand = "FashionBase" },
                new Product { ProductId = 27, Name = "Windbreaker Rain Jacket", Description = "Lightweight windbreaker, packable hood", Price = 79.99m, StockQuantity = 60, Category = "Jackets", CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Navy", Brand = "SportFlex" },
                // Shorts (28-32)
                new Product { ProductId = 28, Name = "Denim Shorts", Description = "Classic denim shorts, frayed hem", Price = 39.99m, StockQuantity = 80, Category = "Shorts", CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=600&h=700&fit=crop", Size = "XS-XL", Color = "Light Blue", Brand = "DenimPro" },
                new Product { ProductId = 29, Name = "Cargo Shorts", Description = "Multi-pocket cargo shorts, cotton twill", Price = 34.99m, StockQuantity = 70, Category = "Shorts", CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1562157873-818bc0726f68?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Khaki", Brand = "SportFlex" },
                new Product { ProductId = 30, Name = "Athletic Running Shorts", Description = "Performance mesh shorts, built-in liner", Price = 29.99m, StockQuantity = 95, Category = "Shorts", CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Black", Brand = "SportFlex" },
                new Product { ProductId = 31, Name = "Linen Beach Shorts", Description = "Lightweight linen shorts, elastic waist", Price = 27.99m, StockQuantity = 65, Category = "Shorts", CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=700&fit=crop", Size = "S-XL", Color = "Beige", Brand = "FashionBase" },
                new Product { ProductId = 32, Name = "Tailored Shorts", Description = "Smart tailored shorts, crisp finish", Price = 44.99m, StockQuantity = 40, Category = "Shorts", CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=600&h=700&fit=crop", Size = "28-36", Color = "Navy", Brand = "FashionBase" },
                // Dresses & Skirts (33-38)
                new Product { ProductId = 33, Name = "Floral Summer Dress", Description = "Beautiful floral print, flowy silhouette", Price = 54.99m, StockQuantity = 55, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=600&h=700&fit=crop", Size = "XS-XL", Color = "Multi", Brand = "FashionBase" },
                new Product { ProductId = 34, Name = "Little Black Dress", Description = "Essential LBD, classic fit", Price = 69.99m, StockQuantity = 45, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=700&fit=crop", Size = "XS-XL", Color = "Black", Brand = "FashionBase" },
                new Product { ProductId = 35, Name = "Pleated Mini Skirt", Description = "Cute pleated mini skirt, high waist", Price = 39.99m, StockQuantity = 70, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1583496661160-fb5886a0aaaa?w=600&h=700&fit=crop", Size = "XS-L", Color = "Black", Brand = "StreetStyle" },
                new Product { ProductId = 36, Name = "Midi Wrap Dress", Description = "Flattering wrap dress, midi length", Price = 64.99m, StockQuantity = 35, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=600&h=700&fit=crop", Size = "XS-XL", Color = "Navy", Brand = "FashionBase" },
                new Product { ProductId = 37, Name = "Maxi Bohemian Dress", Description = "Boho style maxi dress, embroidered details", Price = 59.99m, StockQuantity = 30, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=700&fit=crop", Size = "S-XL", Color = "White/Multi", Brand = "RetroWear" },
                new Product { ProductId = 38, Name = "Denim Skirt", Description = "Classic denim skirt, A-line cut", Price = 42.99m, StockQuantity = 50, Category = "Dresses & Skirts", CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1583496661160-fb5886a0aaaa?w=600&h=700&fit=crop", Size = "XS-XL", Color = "Medium Blue", Brand = "DenimPro" },
                // Accessories (39-48)
                new Product { ProductId = 39, Name = "Leather Belt", Description = "Genuine leather belt, metal buckle", Price = 34.99m, StockQuantity = 100, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=700&fit=crop", Size = "One Size", Color = "Brown", Brand = "FashionBase" },
                new Product { ProductId = 40, Name = "Canvas Backpack", Description = "Durable canvas backpack, laptop compartment", Price = 49.99m, StockQuantity = 60, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=700&fit=crop", Size = "One Size", Color = "Olive", Brand = "UrbanWalk" },
                new Product { ProductId = 41, Name = "Aviator Sunglasses", Description = "Classic aviator style, UV protection", Price = 19.99m, StockQuantity = 150, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=600&h=700&fit=crop", Size = "One Size", Color = "Gold/Green", Brand = "FashionBase" },
                new Product { ProductId = 42, Name = "Silk Scarf", Description = "Luxury silk scarf, vibrant print", Price = 24.99m, StockQuantity = 80, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=600&h=700&fit=crop", Size = "One Size", Color = "Multi", Brand = "FashionBase" },
                new Product { ProductId = 43, Name = "Leather Wallet", Description = "Slim leather wallet, RFID blocking", Price = 39.99m, StockQuantity = 90, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=700&fit=crop", Size = "One Size", Color = "Black", Brand = "UrbanWalk" },
                new Product { ProductId = 44, Name = "Baseball Cap", Description = "Classic baseball cap, adjustable strap", Price = 17.99m, StockQuantity = 120, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1588850561407-ed78c282e89b?w=600&h=700&fit=crop", Size = "One Size", Color = "Black", Brand = "StreetStyle" },
                new Product { ProductId = 45, Name = "Knit Beanie", Description = "Warm knit beanie, ribbed finish", Price = 14.99m, StockQuantity = 110, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1576871337632-b9aef4c17ab9?w=600&h=700&fit=crop", Size = "One Size", Color = "Charcoal", Brand = "StreetStyle" },
                new Product { ProductId = 46, Name = "Stainless Steel Watch", Description = "Minimalist stainless steel watch, quartz movement", Price = 129.99m, StockQuantity = 30, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&h=700&fit=crop", Size = "One Size", Color = "Silver", Brand = "FashionBase" },
                new Product { ProductId = 47, Name = "Gold Hoop Earrings", Description = "14k gold plated hoop earrings", Price = 22.99m, StockQuantity = 85, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1635767798638-3665c7b0a045?w=600&h=700&fit=crop", Size = "One Size", Color = "Gold", Brand = "FashionBase" },
                new Product { ProductId = 48, Name = "Leather Crossbody Bag", Description = "Genuine leather crossbody, adjustable strap", Price = 59.99m, StockQuantity = 40, Category = "Accessories", CategoryId = 7, ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=600&h=700&fit=crop", Size = "One Size", Color = "Tan", Brand = "UrbanWalk" },
                // Sweaters & Hoodies (49-54)
                new Product { ProductId = 49, Name = "Zip-Up Hoodie", Description = "Full zip hoodie, fleece lined", Price = 54.99m, StockQuantity = 70, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Grey", Brand = "StreetStyle" },
                new Product { ProductId = 50, Name = "Pullover Hoodie", Description = "Classic pullover hoodie, kangaroo pocket", Price = 49.99m, StockQuantity = 85, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Black", Brand = "StreetStyle" },
                new Product { ProductId = 51, Name = "Cashmere Crew Sweater", Description = "Luxury cashmere crew neck, ultra soft", Price = 89.99m, StockQuantity = 25, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=600&h=700&fit=crop", Size = "S-XL", Color = "Camel", Brand = "FashionBase" },
                new Product { ProductId = 52, Name = "Knit Cardigan", Description = "Open front knit cardigan, long sleeve", Price = 69.99m, StockQuantity = 35, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=600&h=700&fit=crop", Size = "S-XL", Color = "Cream", Brand = "FashionBase" },
                new Product { ProductId = 53, Name = "Quarter Zip Sweater", Description = "Quarter zip pullover, fleece interior", Price = 59.99m, StockQuantity = 50, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=600&h=700&fit=crop", Size = "S-XXL", Color = "Navy", Brand = "SportFlex" },
                new Product { ProductId = 54, Name = "Oversized Knit Sweater", Description = "Chunky oversized knit, drop shoulder", Price = 64.99m, StockQuantity = 40, Category = "Sweaters & Hoodies", CategoryId = 8, ImageUrl = "https://images.unsplash.com/photo-1576871337632-b9aef4c17ab9?w=600&h=700&fit=crop", Size = "M-XXL", Color = "Rust", Brand = "RetroWear" },
            };

            modelBuilder.Entity<Product>().HasData(products);

            var carts = new[]
            {
                new Cart { CartId = 1, CustomerName = "Иван Петров", DateCreated = new DateTime(2026, 6, 1), TotalPrice = 95.99m, Status = CartStatus.Active, Address = "ул. Главна 1, София", UserId = "" },
                new Cart { CartId = 2, CustomerName = "Мария Иванова", DateCreated = new DateTime(2026, 6, 5), TotalPrice = 350.00m, Status = CartStatus.Ordered, Address = "бул. България 25, Пловдив", UserId = "" },
                new Cart { CartId = 3, CustomerName = "Георги Стоянов", DateCreated = new DateTime(2026, 6, 10), TotalPrice = 1580.00m, Status = CartStatus.Shipped, Address = "ул. Вардар 8, Варна", UserId = "" },
                new Cart { CartId = 4, CustomerName = "Елена Димитрова", DateCreated = new DateTime(2026, 6, 15), TotalPrice = 80.00m, Status = CartStatus.Active, Address = "ж.к. Тракия 45, Бургас", UserId = "" },
                new Cart { CartId = 5, CustomerName = "Николай Христов", DateCreated = new DateTime(2026, 6, 18), TotalPrice = 1575.99m, Status = CartStatus.Active, Address = "ул. Пирин 12, Русе", UserId = "" },
            };

            modelBuilder.Entity<Cart>().HasData(carts);

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

            modelBuilder.Entity<CartItem>().HasData(cartItems);
        }
    }
}
