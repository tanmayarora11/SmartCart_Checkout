using Microsoft.AspNetCore.Mvc;
using SmartCart.Api.DTOs;
using SmartCart.Api.Interfaces;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("items")]
    public IActionResult AddItem([FromQuery] Guid cartId, [FromBody] AddCartItemDto dto)
    {
        _cartService.AddOrUpdateItem(cartId, dto.ProductId, dto.Quantity);
        return Ok(new { message = "Item added/updated successfully." });
    }

    [HttpGet("{cartId}")]
    public IActionResult GetCart(Guid cartId)
    {
        var cart = _cartService.GetCartDetails(cartId);
        return Ok(cart);
    }

    [HttpPost("{cartId}/apply-coupon")]
    public IActionResult ApplyCoupon(Guid cartId, [FromBody] ApplyCouponDto dto)
    {
        _cartService.ApplyCoupon(cartId, dto.CouponCode);
        return Ok(new { message = "Coupon applied successfully." });
    }

    [HttpPost("{cartId}/checkout")]
    public IActionResult Checkout(Guid cartId)
    {
        var order = _cartService.Checkout(cartId);
        return Ok(order);
    }
}