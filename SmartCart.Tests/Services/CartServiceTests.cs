using SmartCart.Api.Data;
using SmartCart.Api.Exceptions;
using SmartCart.Api.Interfaces;
using SmartCart.Api.Models;
using SmartCart.Api.Services;
using SmartCart.Api.Services.Coupons;
using Xunit;

public class CartServiceTests
{
    private ICartService CreateService(IStore store)
    {
        var strategies = new List<ICouponStrategy>
        {
            new Flat50Coupon(),
            new Save10Coupon()
        };

        var couponService = new CouponService(strategies);

        return new CartService(store, couponService);
    }

    [Fact]
    public void Should_Throw_When_Quantity_Exceeds_Stock()
    {
        var store = new InMemoryStore();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Price = 100,
            Stock = 1
        };

        store.Products.Add(product);

        var service = CreateService(store);

        Assert.Throws<BusinessRuleException>(() =>
            service.AddOrUpdateItem(Guid.NewGuid(), product.Id, 5));
    }

    [Fact]
    public void Checkout_Should_Calculate_Correct_Total()
    {
        var store = new InMemoryStore();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Price = 1000,
            Stock = 10
        };

        store.Products.Add(product);

        var service = CreateService(store);

        var cartId = Guid.NewGuid();

        service.AddOrUpdateItem(cartId, product.Id, 1);
        service.ApplyCoupon(cartId, "SAVE10");

        var order = service.Checkout(cartId);

        Assert.Equal(1000, order.Subtotal);
        Assert.Equal(100, order.Discount);
    }
}