using Microsoft.AspNetCore.Mvc;
using SmartCart.Api.Data;
using SmartCart.Api.Interfaces;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IStore _store;

    public OrdersController(IStore store)
    {
        _store = store;
    }

    [HttpGet("{orderId}")]
    public IActionResult GetOrder(Guid orderId)
    {
        var order = _store.Orders.FirstOrDefault(o => o.Id == orderId);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        return Ok(order);
    }
}