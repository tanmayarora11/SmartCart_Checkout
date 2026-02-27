using SmartCart.Api.Exceptions;
using SmartCart.Api.Interfaces;

namespace SmartCart.Api.Services;

public class CouponService : ICouponService
{
    private readonly IEnumerable<ICouponStrategy> _strategies;

    public CouponService(IEnumerable<ICouponStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal CalculateDiscount(string code, decimal subtotal)
    {
        var strategy = _strategies
            .FirstOrDefault(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (strategy == null)
            throw new BusinessRuleException("Invalid coupon code.");

        if (!strategy.IsApplicable(subtotal))
            throw new BusinessRuleException("Coupon conditions not met.");

        return strategy.CalculateDiscount(subtotal);
    }
}