namespace SmartCart.Api.Models;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<CartItem> Items { get; set; } = new();
    public string? AppliedCoupon { get; set; }
}