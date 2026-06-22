# Fashion Store — Документация

**Автор:** Виктор Николчев, 11J клас  
**Дата:** Юни 2026

---

## 1. Описание

Fashion Store е ASP.NET Core MVC уеб приложение за онлайн магазин за модни дрехи и аксесоари. Включва потребителски акаунти, количка, поръчки и административен панел.

## 2. Технологии

| Технология | Версия |
|------------|--------|
| C# | .NET 10.0 |
| ASP.NET Core MVC | 10.0 |
| Entity Framework Core | 10.0 |
| SQLite | — |
| ASP.NET Core Identity | 10.0 |
| Bootstrap | 5 |
| NUnit | 4.3.2 |

## 3. Архитектура

```
Viktor_Nikolchev_11J/
├── ShoppingCart.Models/       # Модели данни
├── ShoppingCart.Data/         # EF Core контекст + seed
├── ShoppingCart.Services/     # Бизнес логика
├── ShoppingCart.Web/          # MVC уеб приложение
│   ├── Controllers/           # 5 публични + 5 admin
│   ├── Views/                 # 13 публични + 13 admin
│   ├── Areas/Admin/           # Admin панел
│   └── Models/AppUser.cs      # Разширен IdentityUser
├── ShoppingCart.ConsoleApp/   # Конзолна версия
└── ShoppingCart.Tests/        # NUnit тестове
```

## 4. Модели

| Модел | Ключови полета |
|-------|---------------|
| **AppUser** | Id, UserName, Email, FullName |
| **Category** | CategoryId, Name, ImageUrl |
| **Product** | ProductId, Name, Price, Size, Color, Brand, ImageUrl, CategoryId (FK) |
| **Cart** | CartId, UserId, CustomerName, TotalPrice, Status |
| **CartItem** | CartItemId, CartId (FK), ProductId (FK), Quantity, UnitPrice |
| **Order** | OrderId, UserId, OrderDate, TotalPrice, Status, ShippingAddress |
| **OrderItem** | OrderItemId, OrderId (FK), ProductId (FK), Quantity, UnitPrice |

## 5. Връзки

- `Category (1) → (N) Product`
- `Product (1) → (N) CartItem`, `Cart (1) → (N) CartItem`
- `Product (1) → (N) OrderItem`, `Order (1) → (N) OrderItem`
- `AppUser (1) → (N) Cart`, `AppUser (1) → (N) Order`

Identity таблици: AspNetUsers, AspNetRoles, AspNetUserRoles и др.

## 6. Identity

- Конфигуриран с `AppUser : IdentityUser`
- Password: min 3 символа, без изисквания за специални
- Маршрути: `/Account/Register`, `/Login`, `/Logout`, `/AccessDenied`
- Admin потребител: `admin@fashionstore.com` / `admin123` (създава се при първо стартиране)

## 7. Контролери

### Публични
| Контролер | Маршрут | Описание |
|-----------|---------|----------|
| HomeController | `/` | Начало с категории и продукти |
| ProductController | `/Product`, `/Product/Details/{id}` | Списък + детайли |
| AccountController | `/Account/Register`, `/Login` | Регистрация/вход |
| CartController | `/Cart`, `/Cart/Checkout` | Количка (auth) |
| OrderController | `/Order`, `/Order/Details/{id}` | Поръчки (auth) |

### Admin ([Authorize(Roles = "Admin")])
| Контролер | Маршрут | Описание |
|-----------|---------|----------|
| DashboardController | `/Admin/Dashboard` | Статистика |
| ProductsController | `/Admin/Products` | CRUD продукти |
| CategoriesController | `/Admin/Categories` | CRUD категории |
| OrdersController | `/Admin/Orders` | Статус на поръчки |
| UsersController | `/Admin/Users` | Роли на потребители |

## 8. Seed данни

- **8 категории**: T-Shirts, Jeans, Sneakers, Jackets, Shorts, Dresses & Skirts, Accessories, Sweaters & Hoodies
- **54 продукта** с Unsplash изображения, разпределени по категории
- **5 примерни колички** с артикули
- Всичко се зарежда при `EnsureCreated()` в `Program.cs`

## 9. Стартиране

```powershell
dotnet restore
dotnet run --project ShoppingCart.Web
# Отваря се на http://localhost:5016
```

Тестове: `dotnet test ShoppingCart.Tests`

## 10. Бъдещи подобрения

- REST API
- Файлово качване на снимки
- Търсене и филтриране
- Платежен шлюз
- Имейл известия
- Wishlist

## 11. Източници

- https://learn.microsoft.com/en-us/aspnet/core/
- https://learn.microsoft.com/en-us/ef/core/
- https://getbootstrap.com/
- https://unsplash.com/
