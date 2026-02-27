import type { OrderResponse } from "../types/types";

interface Props {
  order: OrderResponse;
  onReset: () => void;
}

const CheckoutSuccess = ({ order, onReset }: Props) => {
  return (
    <div style={{ marginTop: 20 }}>
      <h2>Order Confirmed 🎉</h2>

      <p>Order ID: {order.id}</p>
      <p>Subtotal: ₹{order.subtotal}</p>
      <p>Discount: ₹{order.discount}</p>
      <p>Tax: ₹{order.tax}</p>
      <h3>Grand Total: ₹{order.grandTotal}</h3>

      <h4>Items:</h4>
      {order.items.map((item, index) => (
        <div key={index}>
          {item.productName} - {item.quantity} × ₹{item.unitPrice}
        </div>
      ))}

      <button style={{marginTop: '10px'}} onClick={onReset}>
        Continue Shopping
      </button>
    </div>
  );
};

export default CheckoutSuccess;