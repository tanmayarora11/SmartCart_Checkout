# 🛒 Smart Cart & Checkout

A full-stack mini e-commerce checkout system built using:

- **Backend:** ASP.NET Core Web API (.NET 8)
- **Frontend:** React + TypeScript (Vite)
- **Architecture:** Clean layered architecture
- **Testing:** xUnit (Unit + Integration-style tests)

This project demonstrates backend business rule enforcement, clean API design, atomic checkout handling, and frontend state management with a responsive UI.

---

# 🚀 Features

## ✅ Backend (ASP.NET Core Web API)

- Product listing endpoint
- Add / update cart items
- Coupon validation and discount application
- Atomic checkout (order creation + stock reduction)
- Order retrieval endpoint
- Centralized exception handling middleware
- Swagger / OpenAPI documentation
- In-memory data store (for demo simplicity)
- DTO-based API contracts (separation of domain models and response models)

## ✅ Frontend (React + TypeScript)

- Product listing page
- Add to cart functionality
- Quantity increment / decrement controls
- Coupon input with validation messaging
- Checkout flow with confirmation screen
- Responsive layout
- Loading & error handling states
- Strong typing via `types.ts`

---

# 📌 Functional Endpoints

## Products

### `GET /api/products`
Returns all available products:

- Id
- Name
- Price
- Stock

---

## Cart

### `POST /api/cart/items`
Add or update cart items.

Request body:

```json
{
  "productId": "GUID",
  "quantity": 1
}
```

Quantity acts as a delta:

- `+1` increases quantity  
- `-1` decreases quantity  
- When quantity becomes `0`, item is removed  

---

### `GET /api/cart/{cartId}`

Returns cart details:

- Product name  
- Unit price  
- Quantity  
- Line total  
- Subtotal  
- Applied coupon (if any)  

---

### `POST /api/cart/{cartId}/apply-coupon`

Apply a coupon to the cart.

Request body:

```json
{
  "couponCode": "FLAT50"
}
```

---

### `POST /api/cart/{cartId}/checkout`

Creates an order and reduces stock atomically.

Returns:

- Subtotal  
- Discount  
- Tax (5%)  
- Grand total  
- Purchased items  

---

## Orders

### `GET /api/orders/{orderId}`

Returns order summary details.

---

# 💰 Business Rules Implemented

## Cart Rules

- Final cart quantity must be > 0  
- If quantity becomes 0 → item removed  
- Cannot exceed available stock  

## Coupon Rules

| Coupon  | Rule |
|----------|------|
| FLAT50   | ₹50 discount if subtotal ≥ ₹500 |
| SAVE10   | 10% discount (max ₹200) if subtotal ≥ ₹1000 |

## Checkout Rules

- Checkout is atomic  
- If stock validation fails → no partial update  
- Stock is reduced only after successful validation  

## Tax

- 5% tax applied at checkout  
- Included in final order summary  

---

# 🧠 Architecture Overview

The backend follows a layered architecture:

```
Controller → Service → Store (Repository-like abstraction)
```

### Folder Structure

```
SmartCart.Api
 ├── Controllers
 ├── Services
 │    ├── CartService
 │    ├── CouponService
 │    └── Coupons (Strategy Pattern)
 ├── Interfaces
 ├── Models
 ├── DTOs
 ├── Data (InMemoryStore)
 ├── Middleware (Exception handling)
 └── Program.cs
```

### Frontend Structure

```
SmartCart.Frontend
 ├── components
 │    ├── ProductList.tsx
 │    ├── Cart.tsx
 │    ├── CheckoutSuccess.tsx
 ├── services
 │    └── api.ts
 ├── types
 │    └── types.ts
 ├── App.tsx
 ├── App.css
 └── main.tsx
```

### Design Principles Used

- SOLID principles  
- Dependency Injection  
- Strategy Pattern (Coupon logic)  
- Separation of Concerns  
- Centralized error handling  
- DTO-based API responses  

---

# 🧪 Testing

Implemented using **xUnit**.

### Test Types

**Unit Tests**
- Coupon validation logic
- Checkout pricing calculation
- Stock exceeded scenario
- Quantity validation

**Integration-style Tests**
- API-level checkout validation using `WebApplicationFactory`

Run tests from solution root:

```bash
dotnet test
```

---

# 📦 Sample Data

Seeded products (in-memory):

| Product      | Price  | Stock |
|--------------|--------|--------|
| Laptop       | ₹60000 | 10     |
| Headphones   | ₹2000  | 25     |
| Keyboard     | ₹1500  | 20     |

Available coupons:

- `FLAT50`
- `SAVE10`

---

# ⚙️ How to Run the Project

## Run Entire Solution (Recommended)

From solution root:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project SmartCart.Api --launch-profile https
```

Swagger available at:

```
https://localhost:7180/swagger
```

> If HTTPS certificate warning appears, run:
>
> ```
> dotnet dev-certs https --trust
> ```

---

## Frontend

Navigate to frontend folder:

```bash
cd SmartCart.Frontend
```

Install dependencies:

```bash
npm install
```

Start development server:

```bash
npm run dev
```

App runs at:

```
http://localhost:5173
```

> Ensure backend is running on `https://localhost:7180` before starting frontend.

---

# 🔒 Non-Functional Considerations

- Backend validation on all endpoints  
- Centralized exception handling middleware  
- Atomic checkout handling  
- Clean separation of layers  
- Type-safe frontend using TypeScript  
- Responsive UI layout  
- Swagger documentation enabled  

---

# 📈 Future Improvements

- Persistent database integration  
- Authentication & user-based carts  
- Payment gateway integration  
- Docker containerization  
- CI/CD pipeline setup  

---

# 🏁 Conclusion

This project demonstrates:

- Full-stack development capability  
- Clean architecture implementation  
- Business rule enforcement  
- Proper API design  
- State management in frontend  
- Testing practices  
- Scalable and maintainable code structure  