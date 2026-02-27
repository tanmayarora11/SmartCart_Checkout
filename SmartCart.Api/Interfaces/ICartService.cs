using SmartCart.Api.Models;

namespace SmartCart.Api.Interfaces;

public interface ICartService
{
    void AddOrUpdateItem(Guid cartId, Guid productId, int quantity);
    Cart GetCart(Guid cartId);
    void ApplyCoupon(Guid cartId, string couponCode);
    Order Checkout(Guid cartId);
    CartResponseDto GetCartDetails(Guid cartId);
}