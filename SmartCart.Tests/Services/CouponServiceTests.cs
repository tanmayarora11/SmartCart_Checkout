using SmartCart.Api.Exceptions;
using SmartCart.Api.Interfaces;
using SmartCart.Api.Services;
using SmartCart.Api.Services.Coupons;
using Xunit;

public class CouponServiceTests
{
    private ICouponService CreateService()
    {
        var strategies = new List<ICouponStrategy>
        {
            new Flat50Coupon(),
            new Save10Coupon()
        };

        return new CouponService(strategies);
    }

    [Fact]
    public void FLAT50_Should_Return_50_When_Eligible()
    {
        var service = CreateService();

        var discount = service.CalculateDiscount("FLAT50", 600);

        Assert.Equal(50, discount);
    }

    [Fact]
    public void SAVE10_Should_Cap_At_200()
    {
        var service = CreateService();

        var discount = service.CalculateDiscount("SAVE10", 5000);

        Assert.Equal(200, discount);
    }

    [Fact]
    public void Invalid_Coupon_Should_Throw()
    {
        var service = CreateService();

        Assert.Throws<BusinessRuleException>(() =>
            service.CalculateDiscount("INVALID", 1000));
    }
}