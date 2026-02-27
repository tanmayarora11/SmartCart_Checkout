using SmartCart.Api.Data;
using SmartCart.Api.Exceptions;
using SmartCart.Api.Interfaces;
using SmartCart.Api.Models;

public class CartService : ICartService
{
    private readonly ICouponService _couponService;
    private readonly IStore _store;

    public CartService(IStore store, ICouponService couponService)
    {
        _store = store;
        _couponService = couponService;
    }
    public Cart GetOrCreateCart(Guid cartId)
    {
        var cart = _store.Carts.FirstOrDefault(c => c.Id == cartId);
        if (cart == null)
        {
            cart = new Cart { Id = cartId };
            _store.Carts.Add(cart);
        }
        return cart;
    }

    public void AddOrUpdateItem(Guid cartId, Guid productId, int quantity)
    {
        var product = _store.Products.FirstOrDefault(p => p.Id == productId)
            ?? throw new NotFoundException("Product not found.");

        var cart = GetOrCreateCart(cartId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        var currentQuantity = item?.Quantity ?? 0;
        var newQuantity = currentQuantity + quantity;

        // Final quantity must not exceed stock
        if (newQuantity > product.Stock)
            throw new BusinessRuleException("Requested quantity exceeds available stock.");

        // Final quantity must not be negative
        if (newQuantity < 0)
            throw new ValidationException("Quantity cannot be negative.");

        // If quantity becomes zero → remove item
        if (newQuantity == 0)
        {
            if (item != null)
                cart.Items.Remove(item);

            return;
        }

        if (item == null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = newQuantity
            });
        }
        else
        {
            item.Quantity = newQuantity;
        }
    }

    public Order Checkout(Guid cartId)
    {
        lock (_store.LockObject)
        {
            var cart = _store.Carts.FirstOrDefault(c => c.Id == cartId)
                ?? throw new NotFoundException("Cart not found.");

            if (!cart.Items.Any())
                throw new BusinessRuleException("Cart is empty.");

            decimal subtotal = 0;

            foreach (var item in cart.Items)
            {
                var product = _store.Products.First(p => p.Id == item.ProductId);

                if (item.Quantity > product.Stock)
                    throw new BusinessRuleException($"Insufficient stock for {product.Name}");

                subtotal += product.Price * item.Quantity;
            }

            var discount = 0m;

            if (!string.IsNullOrEmpty(cart.AppliedCoupon))
                discount = _couponService.CalculateDiscount(cart.AppliedCoupon, subtotal);

            var tax = subtotal * 0.05m;
            var grandTotal = subtotal - discount + tax;

            foreach (var item in cart.Items)
            {
                var product = _store.Products.First(p => p.Id == item.ProductId);
                product.Stock -= item.Quantity;
            }

            var order = new Order
            {
                CartId = cart.Id,
                Subtotal = subtotal,
                Discount = discount,
                Tax = tax,
                GrandTotal = grandTotal,
                Items = cart.Items.Select(ci =>
                {
                    var product = _store.Products.First(p => p.Id == ci.ProductId);
                    return new OrderItem
                    {
                        ProductId = ci.ProductId,
                        ProductName = product.Name,
                        UnitPrice = product.Price,
                        Quantity = ci.Quantity
                    };
                }).ToList()
            };

            _store.Orders.Add(order);
            _store.Carts.Remove(cart);

            return order;
        }
    }

    public Cart GetCart(Guid cartId)
    {
        return GetOrCreateCart(cartId);
    }

    public void ApplyCoupon(Guid cartId, string couponCode)
    {
        var cart = GetOrCreateCart(cartId);

        if (!cart.Items.Any())
            throw new BusinessRuleException("Cannot apply coupon to empty cart.");

        var subtotal = cart.Items.Sum(i =>
        {
            var product = _store.Products.First(p => p.Id == i.ProductId);
            return product.Price * i.Quantity;
        });

        // Validate coupon (will throw if invalid)
        _couponService.CalculateDiscount(couponCode, subtotal);

        cart.AppliedCoupon = couponCode.ToUpper();
    }

    public CartResponseDto GetCartDetails(Guid cartId)
    {
        var cart = GetOrCreateCart(cartId);

        var items = cart.Items.Select(ci =>
        {
            var product = _store.Products.First(p => p.Id == ci.ProductId);

            return new CartItemResponseDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = ci.Quantity,
                LineTotal = product.Price * ci.Quantity
            };
        }).ToList();

        var subtotal = items.Sum(i => i.LineTotal);

        return new CartResponseDto
        {
            CartId = cart.Id,
            Items = items,
            Subtotal = subtotal,
            AppliedCoupon = cart.AppliedCoupon
        };
    }
}