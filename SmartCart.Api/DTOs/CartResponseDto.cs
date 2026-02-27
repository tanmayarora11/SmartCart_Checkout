public class CartResponseDto
{
    public Guid CartId { get; set; }
    public List<CartItemResponseDto> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public string? AppliedCoupon { get; set; }
}