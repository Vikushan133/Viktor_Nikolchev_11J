---
title: "Fashion Store - Онлайн магазин за модни дрехи"
subtitle: "Курсов проект по Информатика"
author: "Виктор Николчев, 11J клас"
date: "Юни 2026"
---

# Съдържание

1. Въведение
2. Цел на проекта
3. Използвани технологии
4. Архитектура на проекта
5. Модели на данните
6. База данни и връзки
7. Identity и потребителски акаунти
8. Бизнес логика (Services)
9. Контролери и маршрути
10. Представяния (Views)
11. Административен панел
12. Seed данни
13. Стартиране на проекта
14. Бъдещо развитие
15. Източници

---

# 1. Въведение

Настоящият курсов проект представя разработката на **Fashion Store** – ASP.NET Core MVC уеб приложение за онлайн магазин за модни дрехи и аксесоари. Приложението включва пълна функционалност за потребителски акаунти, пазарска количка, поръчки и административен панел.

Проектът демонстрира:
- ASP.NET Core MVC архитектура
- Entity Framework Core Code First подход
- ASP.NET Core Identity за автентикация и роли
- Многопластов софтуерен дизайн
- Работа със SQLite база данни
- Seed данни с 54 продукта и 8 категории

---

# 2. Цел на проекта

Основната цел е да се разработи функционален онлайн магазин, който позволява:

**За потребителите:**
- Разглеждане на продукти по категории
- Регистрация и вход в системата
- Добавяне на продукти в персонална количка
- Преглед и промяна на количката
- Създаване на поръчка
- Преглед на история на поръчките

**За администраторите:**
- Табло с обща статистика
- Управление на продукти (CRUD)
- Управление на категории (CRUD)
- Преглед и промяна на статус на поръчки
- Управление на потребители и роли

Втората цел е да се демонстрират умения за работа с:
- ASP.NET Core MVC
- Entity Framework Core + SQLite
- ASP.NET Core Identity
- Bootstrap 5
- GitHub и Git version control

---

# 3. Използвани технологии

| Технология | Версия |
|------------|--------|
| **Език за програмиране** | C# (.NET 10.0) |
| **Framework** | ASP.NET Core MVC |
| **ORM** | Entity Framework Core 10.0 |
| **База данни** | SQLite |
| **Автентикация** | ASP.NET Core Identity |
| **Frontend** | Bootstrap 5, Bootstrap Icons |
| **Тестване** | NUnit 4.3.2 |
| **IDE** | Visual Studio / VS Code |
| **Version control** | Git + GitHub |

---

# 4. Архитектура на проекта

Проектът е организиран в многопластова архитектура с четири основни проекта:

```
Viktor_Nikolchev_11J/
├── Viktor_Nikolchev_11J.slnx
├── ShoppingCart.Models/           # Модели на данните
│   ├── AppUser.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── CartStatus.cs
│   ├── Category.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Product.cs
├── ShoppingCart.Data/             # Достъп до данни
│   └── ShoppingCartContext.cs
├── ShoppingCart.Services/         # Бизнес логика
│   ├── CartService.cs
│   └── ProductService.cs
├── ShoppingCart.Web/              # Уеб приложение
│   ├── Program.cs
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── CartController.cs
│   │   ├── HomeController.cs
│   │   ├── OrderController.cs
│   │   └── ProductController.cs
│   ├── Areas/Admin/
│   │   └── Controllers/
│   │       ├── CategoriesController.cs
│   │       ├── DashboardController.cs
│   │       ├── OrdersController.cs
│   │       ├── ProductsController.cs
│   │       └── UsersController.cs
│   ├── Views/
│   ├── Extensions/
│   └── Models/
│       └── ErrorViewModel.cs
├── ShoppingCart.ConsoleApp/       # Конзолно приложение
└── ShoppingCart.Tests/            # Тестове
```

## Зависимости между проектите

```
ShoppingCart.Web
  ├── ShoppingCart.Services
  │     └── ShoppingCart.Data
  │           └── ShoppingCart.Models
  └── ShoppingCart.Models
```

---

# 5. Модели на данните

## AppUser (Потребител)

Наследява `IdentityUser` и добавя допълнителни полета:

| Поле | Тип | Описание |
|------|-----|----------|
| Id | string (PK) | Наследено от IdentityUser |
| UserName | string | Потребителско име |
| Email | string | Имейл адрес |
| FullName | string | Име и фамилия |
| PasswordHash | string | Хеш на паролата |

Използва се с ASP.NET Core Identity и се съхранява в таблица `AspNetUsers`.

## Category (Категория)

| Поле | Тип | Описание |
|------|-----|----------|
| CategoryId | int (PK) | Уникален идентификатор |
| Name | string | Име на категорията |
| Description | string | Описание |
| ImageUrl | string | URL на изображение |

## Product (Продукт)

| Поле | Тип | Описание |
|------|-----|----------|
| ProductId | int (PK) | Уникален идентификатор |
| Name | string | Име на продукта |
| Description | string | Описание |
| Price | decimal | Цена |
| StockQuantity | int | Наличност |
| Category | string | Категория (текстово поле) |
| ImageUrl | string | URL на снимка (Unsplash) |
| Size | string | Размери |
| Color | string | Цвят |
| Brand | string | Марка |
| CategoryId | int (FK) | Връзка към Category |

## Cart (Количка)

| Поле | Тип | Описание |
|------|-----|----------|
| CartId | int (PK) | Уникален идентификатор |
| CustomerName | string | Име на клиента |
| DateCreated | DateTime | Дата на създаване |
| TotalPrice | decimal | Обща стойност |
| Status | CartStatus (enum) | Active / Ordered / Shipped |
| Address | string | Адрес за доставка |
| UserId | string | ID на потребителя |

## CartItem (Елемент в количка)

| Поле | Тип | Описание |
|------|-----|----------|
| CartItemId | int (PK) | Уникален идентификатор |
| CartId | int (FK) | Връзка към Cart |
| ProductId | int (FK) | Връзка към Product |
| Quantity | int | Количество |
| UnitPrice | decimal | Цена на единица |
| AddedDate | DateTime | Дата на добавяне |

## Order (Поръчка)

| Поле | Тип | Описание |
|------|-----|----------|
| OrderId | int (PK) | Уникален идентификатор |
| UserId | string | ID на потребителя |
| OrderDate | DateTime | Дата на поръчката |
| TotalPrice | decimal | Обща стойност |
| Status | string | Статус (Ordered / Shipped / Delivered) |
| ShippingAddress | string | Адрес за доставка |

## OrderItem (Артикул в поръчка)

| Поле | Тип | Описание |
|------|-----|----------|
| OrderItemId | int (PK) | Уникален идентификатор |
| OrderId | int (FK) | Връзка към Order |
| ProductId | int (FK) | Връзка към Product |
| Quantity | int | Количество |
| UnitPrice | decimal | Цена на единица |

---

# 6. База данни и връзки

## Диаграма на връзките

```
Category (1) ---→ (N) Product
Product  (1) ---→ (N) CartItem
Cart     (1) ---→ (N) CartItem
Order    (1) ---→ (N) OrderItem
Product  (1) ---→ (N) OrderItem
AppUser  (1) ---→ (N) Cart
AppUser  (1) ---→ (N) Order
```

## Identity таблици

ASP.NET Core Identity автоматично създава следните таблици:

| Таблица | Описание |
|---------|----------|
| AspNetUsers | Потребители (разширена с FullName) |
| AspNetRoles | Роли (Admin) |
| AspNetUserRoles | Връзка потребител-роля |
| AspNetUserClaims | Claims на потребителите |
| AspNetRoleClaims | Claims на ролите |
| AspNetUserLogins | Външни логини (Google, Facebook и др.) |
| AspNetUserTokens | Tokens за двуфакторна автентикация |

## Конфигурация в Fluent API

Връзките между таблиците са конфигурирани в `ShoppingCartContext.OnModelCreating()` чрез Fluent API:

- **Product → Category**: `HasOne(p => p.CategoryRef).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId)`
- **CartItem → Cart**: `HasOne(ci => ci.Cart).WithMany(c => c.CartItems).HasForeignKey(ci => ci.CartId)`
- **CartItem → Product**: `HasOne(ci => ci.Product).WithMany(p => p.CartItems).HasForeignKey(ci => ci.ProductId)`
- **OrderItem → Order**: `HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId)`
- **OrderItem → Product**: `HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId)`

---

# 7. Identity и потребителски акаунти

## Конфигурация

В `Program.cs` е конфигуриран ASP.NET Core Identity с `AppUser` и `IdentityRole`:

```csharp
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ShoppingCartContext>()
.AddDefaultTokenProviders();
```

## Маршрути за Identity

- `/Account/Register` – регистрация на нов потребител
- `/Account/Login` – вход в системата
- `/Account/Logout` – изход от системата
- `/Account/AccessDenied` – страница за отказан достъп

## Създаване на Admin потребител

При първо стартиране на приложението, `Program.cs` автоматично създава:

1. **Роля "Admin"** – ако не съществува
2. **Admin потребител**: `admin@fashionstore.com` / `admin123`
3. Добавя потребителя в роля "Admin"

## User-specific Cart

Всеки регистриран потребител има собствена активна количка, идентифицирана чрез `Cart.UserId`. При добавяне на продукт в количката се използва `User.GetUserId()` extension метод.

---

# 8. Бизнес логика (Services)

## ProductService

| Метод | Описание |
|-------|----------|
| `GetAll()` | Всички продукти с категории |
| `GetById(int id)` | Продукт по ID |
| `GetByCategory(int categoryId)` | Продукти по категория |
| `Create(Product product)` | Добавяне на продукт |
| `Update(Product product)` | Редактиране на продукт |
| `Delete(int id)` | Изтриване на продукт |

## CartService

| Метод | Описание |
|-------|----------|
| `GetActiveCartByUser(string userId)` | Активната количка на потребител |
| `GetCartById(int cartId)` | Количка по ID с елементи |
| `HasActiveCart(string userId)` | Проверка за активна количка |

---

# 9. Контролери и маршрути

## Публични контролери

| Контролер | Маршрут | Описание |
|-----------|---------|----------|
| **HomeController** | `/` | Начална страница с категории и популярни продукти |
| **ProductController** | `/Product` | Списък с продукти, филтър по категория |
| **ProductController** | `/Product/Details/{id}` | Детайли за продукт |
| **AccountController** | `/Account/Register` | Регистрация |
| **AccountController** | `/Account/Login` | Вход |
| **AccountController** | `/Account/Logout` | Изход |
| **CartController** | `/Cart` | Количка (изисква вход) |
| **CartController** | `/Cart/Checkout` | Плащане |
| **OrderController** | `/Order` | Моите поръчки (изисква вход) |

## Admin контролери ([Authorize(Roles = "Admin")])

| Контролер | Маршрут | Описание |
|-----------|---------|----------|
| **DashboardController** | `/Admin/Dashboard` | Табло със статистика |
| **ProductsController** | `/Admin/Products` | CRUD за продукти |
| **CategoriesController** | `/Admin/Categories` | CRUD за категории |
| **OrdersController** | `/Admin/Orders` | Преглед и статус на поръчки |
| **UsersController** | `/Admin/Users` | Управление на потребители и роли |

---

# 10. Представяния (Views)

## Структура на папките

```
Views/
├── Shared/
│   └── _Layout.cshtml           # Основен layout с навигация и footer
├── Home/
│   └── Index.cshtml             # Hero банер, категории, продукти
├── Product/
│   ├── Index.cshtml             # Продуктова мрежа с филтър
│   └── Details.cshtml          # Детайли за продукт
├── Cart/
│   ├── Index.cshtml             # Количка с артикули
│   └── Checkout.cshtml         # Форма за плащане
├── Account/
│   ├── Register.cshtml          # Регистрационен формуляр
│   ├── Login.cshtml             # Форма за вход
│   └── AccessDenied.cshtml     # Отказан достъп
└── Order/
    ├── Index.cshtml             # История на поръчки
    └── Details.cshtml          # Детайли за поръчка
```

## Admin Views

```
Areas/Admin/Views/
├── Shared/
│   └── _AdminLayout.cshtml      # Admin layout със странично меню
├── Dashboard/
│   └── Index.cshtml             # Табло с 4 статистики
├── Products/
│   ├── Index.cshtml             # Списък продукти
│   ├── Create.cshtml            # Добавяне на продукт
│   ├── Edit.cshtml              # Редактиране на продукт
│   └── Delete.cshtml            # Потвърждение за изтриване
├── Categories/
│   ├── Index.cshtml             # Списък категории
│   ├── Create.cshtml            # Нова категория
│   ├── Edit.cshtml              # Редактиране на категория
│   └── Delete.cshtml            # Потвърждение за изтриване
├── Orders/
│   ├── Index.cshtml             # Всички поръчки
│   └── Details.cshtml          # Детайли + промяна на статус
└── Users/
    ├── Index.cshtml             # Списък потребители
    └── Edit.cshtml              # Управление на роли
```

## Layout структура

Основният layout (`_Layout.cshtml`) включва:
- **Навигация**: лого "Fashion Store", падащо меню с категории, връзки за количка, поръчки, вход/регистрация
- **Admin бутон**: видим само за потребители в роля "Admin"
- **Footer**: информация за магазина, бързи връзки, контакт

Admin layout (`_AdminLayout.cshtml`) включва:
- **Странично меню** (250px): връзки към Табло, Продукти, Категории, Поръчки, Потребители
- **Top bar**: заглавие на страницата и бутон "Към магазина"

---

# 11. Административен панел

## Достъп

- **URL**: `/Admin/Dashboard`
- **Изискване**: Потребителят трябва да е в роля "Admin"

## Dashboard (Табло)

Показва четири статистики в цветни карти:
- **Продукти** (синьо) – общ брой продукти
- **Категории** (зелено) – общ брой категории
- **Поръчки** (жълто) – общ брой поръчки
- **Потребители** (синьо) – общ брой регистрирани потребители

## Управление на продукти

- **Списък**: таблица с ID, изображение, име, марка, цена, категория, наличност
- **Добавяне**: форма с всички полета + падащо меню за категория
- **Редактиране**: същата форма, попълнена със съществуващите данни
- **Изтриване**: страница за потвърждение с визуализация на продукта

## Управление на категории

- **Списък**: таблица с ID, име, описание
- **Защита при изтриване**: проверка дали категорията съдържа продукти

## Управление на поръчки

- **Списък**: всички поръчки с потребител, дата, сума, статус
- **Детайли**: артикули в поръчката + форма за промяна на статус
- **Статуси**: Ordered, Shipped, Delivered, Cancelled

## Управление на потребители

- **Списък**: имейл и пълно име на всички потребители
- **Роли**: добавяне/премахване на роли (напр. Admin) чрез checkbox форми

---

# 12. Seed данни

При първо стартиране на приложението (`EnsureCreated()`), базата данни се запълва със следните данни:

## Категории (8)

| ID | Име | Описание |
|----|-----|----------|
| 1 | T-Shirts | Classic and trendy t-shirts |
| 2 | Jeans | Perfect fit jeans for any occasion |
| 3 | Sneakers | Stylish and comfortable footwear |
| 4 | Jackets | Stay warm and look cool |
| 5 | Shorts | Summer essentials for everyone |
| 6 | Dresses & Skirts | Elegant and casual dresses |
| 7 | Accessories | Complete your look |
| 8 | Sweaters & Hoodies | Cozy and comfortable layers |

## Продукти (54)

Разпределени по категории:
- **T-Shirts**: 8 продукта (Classic White T-Shirt, Black Essential Tee, Vintage Graphic Tee, Striped Cotton Tee, Oversized Fit Tee, Crew Neck Plain Tee, Printed Logo Tee, Pocket Tee)
- **Jeans**: 6 продукта (Blue Slim Fit Jeans, Black Skinny Jeans, Light Wash Straight Jeans, Ripped Boyfriend Jeans, Classic Bootcut Jeans, High Waist Mom Jeans)
- **Sneakers**: 7 продукта (Black Leather Sneakers, White Court Sneakers, Running Mesh Sneakers, Canvas Low Top Sneakers, Retro Style Sneakers, Platform Sneakers, Slip-On Casual Sneakers)
- **Jackets**: 6 продукта (Classic Denim Jacket, Leather Biker Jacket, Puffer Winter Jacket, Bomber Jacket, Trench Coat, Windbreaker Rain Jacket)
- **Shorts**: 5 продукта (Denim Shorts, Cargo Shorts, Athletic Running Shorts, Linen Beach Shorts, Tailored Shorts)
- **Dresses & Skirts**: 6 продукта (Floral Summer Dress, Little Black Dress, Pleated Mini Skirt, Midi Wrap Dress, Maxi Bohemian Dress, Denim Skirt)
- **Accessories**: 10 продукта (Leather Belt, Canvas Backpack, Aviator Sunglasses, Silk Scarf, Leather Wallet, Baseball Cap, Knit Beanie, Stainless Steel Watch, Gold Hoop Earrings, Leather Crossbody Bag)
- **Sweaters & Hoodies**: 6 продукта (Zip-Up Hoodie, Pullover Hoodie, Cashmere Crew Sweater, Knit Cardigan, Quarter Zip Sweater, Oversized Knit Sweater)

Всички продукти имат Unsplash изображения.

## Колички (5) и елементи (8)

Примерни колички с български имена на клиенти и адреси.

## Admin потребител

Автоматично създаден при стартиране:
- **Имейл**: `admin@fashionstore.com`
- **Парола**: `admin123`
- **Роля**: Admin

---

# 13. Стартиране на проекта

## Предварителни изисквания

- .NET SDK 10.0
- Git

## Локално стартиране

```powershell
git clone https://github.com/Vikushan133/Viktor_Nikolchev_11J.git
cd Viktor_Nikolchev_11J
dotnet restore
dotnet run --project ShoppingCart.Web
```

Сайтът се отваря на `http://localhost:5016`.

## Пускане на тестовете

```powershell
dotnet test ShoppingCart.Tests
```

## Демо акаунти

| Роля | Имейл | Парола |
|------|-------|--------|
| Admin | admin@fashionstore.com | admin123 |

## Конфигурация

- **База данни**: `fashionstore.db` (SQLite) – създава се автоматично при първо стартиране
- **Connection string**: `Data Source=fashionstore.db` (в `Program.cs`)
- **Порт**: `http://localhost:5016` (в `Properties/launchSettings.json`)

---

# 14. Бъдещо развитие

1. **REST API** – излагане на функционалностите чрез Web API
2. **Файлово качване** – качване на снимки вместо Unsplash URLs
3. **Търсене** – търсене по име, марка, ценови диапазон
4. **Плащане** – интеграция с платежен шлюз (Stripe, PayPal)
5. **Имейл известия** – потвърждение на поръчка по имейл
6. **Продуктови ревюта** – потребителите да оставят мнения и оценки
7. **Wishlist** – списък с желани продукти
8. **Купони и промоции** – код за отстъпка
9. **Мобилна версия** – PWA или React Native мобилно приложение

---

# 15. Източници

1. **ASP.NET Core Documentation** - https://learn.microsoft.com/en-us/aspnet/core/
2. **Entity Framework Core Documentation** - https://learn.microsoft.com/en-us/ef/core/
3. **ASP.NET Core Identity** - https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity
4. **Bootstrap 5** - https://getbootstrap.com/docs/5.3/
5. **Bootstrap Icons** - https://icons.getbootstrap.com/
6. **Unsplash** - https://unsplash.com/ (снимки на продуктите)
7. **SQLite Documentation** - https://www.sqlite.org/docs.html
8. **NUnit Documentation** - https://docs.nunit.org/
9. **Git Documentation** - https://git-scm.com/doc
