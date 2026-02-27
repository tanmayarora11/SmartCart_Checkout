namespace SmartCart.Api.DTOs;

public class AddCartItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}