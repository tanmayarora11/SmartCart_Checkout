namespace SmartCart.Api.Interfaces;

public interface ICouponStrategy
{
    string Code { get; }
    decimal CalculateDiscount(decimal subtotal);
    bool IsApplicable(decimal subtotal);
}