namespace SmartCart.Api.Interfaces;
public interface ICouponService
{
    decimal CalculateDiscount(string code, decimal subtotal);
}
