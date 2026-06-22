using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShoppingCartContext>(options =>
    options.UseSqlite("Data Source=shoppingcart.db"));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    context.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
