using SmartCart.Api.Data;
using SmartCart.Api.Interfaces;
using SmartCart.Api.Models;
using SmartCart.Api.Services;
using SmartCart.Api.Services.Coupons;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IStore, InMemoryStore>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICouponStrategy, Flat50Coupon>();
builder.Services.AddScoped<ICouponStrategy, Save10Coupon>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

var store = app.Services.GetRequiredService<IStore>();

store.Products.AddRange(new List<Product>
{
    new() { Id = Guid.NewGuid(), Name = "Laptop", Price = 60000, Stock = 10 },
    new() { Id = Guid.NewGuid(), Name = "Headphones", Price = 2000, Stock = 25 },
    new() { Id = Guid.NewGuid(), Name = "Keyboard", Price = 1500, Stock = 20 }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
public partial class Program { }