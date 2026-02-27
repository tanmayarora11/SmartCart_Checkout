using SmartCart.Api.Models;

namespace SmartCart.Api.Interfaces;

public interface IStore
{
    List<Product> Products { get; }
    List<Cart> Carts { get; }
    List<Order> Orders { get; }
    object LockObject { get; }
}