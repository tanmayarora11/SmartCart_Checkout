using SmartCart.Api.Interfaces;

namespace SmartCart.Api.Services.Coupons;

public class Flat50Coupon : ICouponStrategy
{
    public string Code => "FLAT50";

    public bool IsApplicable(decimal subtotal)
        => subtotal >= 500;

    public decimal CalculateDiscount(decimal subtotal)
        => 50;
}