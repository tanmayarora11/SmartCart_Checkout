export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
}

export interface CartItem {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export interface Cart {
  cartId: string;
  items: CartItem[];
  subtotal: number;
  appliedCoupon?: string;
}

export interface OrderItem {
  productName: string;
  unitPrice: number;
  quantity: number;
}

export interface OrderResponse {
  id: string;
  subtotal: number;
  discount: number;
  tax: number;
  grandTotal: number;
  items: OrderItem[];
}