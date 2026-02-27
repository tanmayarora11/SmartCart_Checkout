import { useEffect, useState } from "react";
import API from "../api/api";
import type { Product } from "../types/types";
import axios from "axios";
import "../App.css"

interface Props {
  cartId: string;
  refreshCart: () => void;
}

const ProductList = ({ cartId, refreshCart }: Props) => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      setLoading(true);
      const res = await API.get("/products");
      setProducts(res.data);
    } catch {
      setError("Failed to load products");
    } finally {
      setLoading(false);
    }
  };

  const addToCart = async (productId: string) => {
    try {
      await API.post(`/cart/items?cartId=${cartId}`, {
        productId,
        quantity: 1,
      });
      refreshCart();
    } catch (err: unknown) {
        if (axios.isAxiosError(err)) {
            alert(err.response?.data?.error || "Error adding item");
        } else {
            alert("Something went wrong");
        }
    }
  };

  if (loading) return <p>Loading products...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <div>
      <h2>Products</h2>
      {products?.map((p) => (
        <div
          key={p.id}
          className="product-card"
        >
          <h4>{p.name}</h4>
          <p>Price: ₹{p.price}</p>
          <p>Stock: {p.stock}</p>
          <button onClick={() => addToCart(p.id)}>Add to Cart</button>
        </div>
      ))}
    </div>
  );
};

export default ProductList;