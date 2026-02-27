using Microsoft.AspNetCore.Mvc;
using SmartCart.Api.Interfaces;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IStore _store;

    public ProductsController(IStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult GetProducts()
        => Ok(_store.Products);
}