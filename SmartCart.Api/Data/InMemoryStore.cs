using SmartCart.Api.Interfaces;
using SmartCart.Api.Models;

namespace SmartCart.Api.Data;

public class InMemoryStore : IStore
{
    public List<Product> Products { get; } = new();
    public List<Cart> Carts { get; } = new();
    public List<Order> Orders { get; } = new();
    public object LockObject { get; } = new();
}