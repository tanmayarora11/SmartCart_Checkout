import { useState } from "react";
import ProductList from "./components/ProductList";
import Cart from "./components/Cart";
import CheckoutSuccess from "./components/CheckoutSuccess";
import type { OrderResponse } from "./types/types";
import "./App.css";

function App() {
  const [cartId] = useState(() => crypto.randomUUID());
  const [order, setOrder] = useState<OrderResponse | null>(null);
  const [cartVersion, setCartVersion] = useState(0);

  const refreshCart = () => {
    setCartVersion(prev => prev + 1);
  };

  return (
    <div className="container">
      <h1>Smart Cart & Checkout</h1>

      {!order ? (
        <div className="layout">
          <div className="products">
            <ProductList cartId={cartId} refreshCart={refreshCart} />
          </div>
          <div className="cart-wrapper">
            <Cart cartId={cartId} onCheckout={setOrder} refreshTrigger={cartVersion} />
          </div>
        </div>
      ) : (
        <CheckoutSuccess order={order} onReset={() => setOrder(null)}/>
      )}
    </div>
  );
}

export default App;