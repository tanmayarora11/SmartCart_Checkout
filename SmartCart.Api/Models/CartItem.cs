namespace SmartCart.Api.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}