# Fashion Store

ASP.NET Core MVC онлайн магазин за модни дрехи и аксесоари.

## Технологии

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core + SQLite
- ASP.NET Core Identity (потребители и роли)
- Bootstrap 5
- Clean Architecture (Models, Data, Services, Web)

## Структура

| Проект | Описание |
|--------|----------|
| `ShoppingCart.Models` | Модели на данните (Product, Category, Order, AppUser...) |
| `ShoppingCart.Data` | EF Core контекст, миграции, seed данни |
| `ShoppingCart.Services` | Бизнес логика (ProductService, CartService) |
| `ShoppingCart.Web` | ASP.NET Core MVC уеб приложение |

## Стартиране

```powershell
dotnet run --project "ShoppingCart.Web"
```

Сайтът се отваря на `http://localhost:5016`.

## Администраторски панел

**URL:** `/Admin/Dashboard`

**Демо акаунт:**
- Имейл: `admin@fashionstore.com`
- Парола: `admin123`

## Функционалности

- **Публична част** – преглед на продукти по категории, детайли за продукт
- **Потребителски акаунти** – регистрация, вход, персонална количка
- **Количка** – добавяне/премахване на продукти, плащане (checkout)
- **Поръчки** – история на направените поръчки с детайли
- **Админ панел** – управление на продукти, категории, поръчки и потребители
- **Seed данни** – 8 категории, 54 продукта с Unsplash изображения

## Seeds

- **8 категории:** T-Shirts, Jeans, Sneakers, Jackets, Shorts, Dresses & Skirts, Accessories, Sweaters & Hoodies
- **54 продукта** с Unsplash снимки, марки, цветове и размери
- **5 примерни колички** с артикули

## License

MIT
