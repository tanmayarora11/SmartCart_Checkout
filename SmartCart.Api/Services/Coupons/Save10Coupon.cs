using SmartCart.Api.Interfaces;

namespace SmartCart.Api.Services.Coupons;

public class Save10Coupon : ICouponStrategy
{
    public string Code => "SAVE10";

    public bool IsApplicable(decimal subtotal)
        => subtotal >= 1000;

    public decimal CalculateDiscount(decimal subtotal)
    {
        var discount = subtotal * 0.10m;
        return Math.Min(discount, 200);
    }
}