import { useEffect, useState } from "react";
import API from "../api/api";
import type { Cart, OrderResponse } from "../types/types";
import axios from "axios";
import "../App.css"


interface Props {
  cartId: string;
  onCheckout: (order: OrderResponse) => void;
  refreshTrigger: number;
}

const CartComponent = ({ cartId, onCheckout, refreshTrigger }: Props) => {
  const [cart, setCart] = useState<Cart | null>(null);
  const [coupon, setCoupon] = useState("");
  const [loading, setLoading] = useState(false);

  const fetchCart = async () => {
    try {
      const res = await API.get(`/cart/${cartId}`);
      setCart(res.data);
    } catch {
      setCart(null);
    }
  };

  useEffect(() => {
    fetchCart();
  }, [refreshTrigger]);

  const updateQuantity = async (productId: string, quantity: number) => {
    try {
    await API.post(`/cart/items?cartId=${cartId}`, {
      productId,
      quantity: quantity,
    });

    fetchCart();
  }
    catch (err: unknown) {
      if (axios.isAxiosError(err)) {
        alert(err.response?.data?.error ?? "Failed to update quantity");
      } else {
        alert("Unexpected error");
      }
    }
  };

  const applyCoupon = async () => {
    try {
      await API.post(`/cart/${cartId}/apply-coupon`, {
        couponCode: coupon,
      });
      setCoupon("");
      alert("Coupon applied!");
      fetchCart();
    } catch (err: unknown) {
        if (axios.isAxiosError(err)) {
            alert(err.response?.data?.error ?? "Request failed");
        } else {
            alert("Unexpected error occurred");
        }
    }
  };

  const checkout = async () => {
    try {
      setLoading(true);
      const res = await API.post(`/cart/${cartId}/checkout`);
      onCheckout(res.data);
    } catch (err: unknown) {
        if (axios.isAxiosError(err)) {
            alert(err.response?.data?.error ?? "Request failed");
        } else {
            alert("Unexpected error occurred");
        }
    } finally {
      setLoading(false);
    }
  };

  // const subtotal =
  //   cart?.items.reduce((sum, item) => sum + item.quantity, 0) ?? 0;

  if (!cart || cart.items.length === 0)
    return <p>Your cart is empty.</p>;

  return (
    <div className="cart">
      <h2>Cart</h2>

      {cart.items.map((item) => (
        <div key={item.productId} style={{ marginBottom: 10 }}>
          <strong>{item.productName}</strong>
          <div>
            ₹{item.unitPrice} × {item.quantity} = ₹{item.lineTotal}
          </div>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <button onClick={() => updateQuantity(item.productId, -1)}>
              −
            </button>

            <span style={{ minWidth: 30, textAlign: "center" }}>
              {item.quantity}
            </span>

            <button onClick={() => updateQuantity(item.productId, +1)}>
              +
            </button>
          </div>
        </div>
      ))}

      <hr />
      <h3>Subtotal: ₹{cart.subtotal}</h3>

      {cart.appliedCoupon && (
        <p>Applied Coupon: {cart.appliedCoupon}</p>
      )}

      <input
        placeholder="Enter coupon"
        value={coupon}
        onChange={(e) => setCoupon(e.target.value)}
      />

      <button style={{marginLeft: '5px'}} onClick={applyCoupon}>Apply Coupon</button>

      <br /><br />

      <button disabled={loading} onClick={checkout}>
        Checkout
      </button>
    </div>
  );
};

export default CartComponent;