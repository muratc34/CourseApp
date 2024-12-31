import React, { createContext, useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./AuthContext";

const CartContext = createContext();

export const useCart = () => useContext(CartContext);

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState([]);
  const [purchasedItems, setPurchasedItems] = useState([]);
  const [pendingItem, setPendingItem] = useState(null);

  const { user } = useAuth();
  const navigate = useNavigate();

  const addToCart = (course) => {
    if (!user) {
      setPendingItem(course);
      navigate("/login");
      return;
    }
    setCart((prev) => {
      const itemExists = prev.find((item) => item.id === course.id);

      if (itemExists) {
        return prev.map((item) =>
          item.id === course.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        );
      }
      return [...prev, { ...course, quantity: 1 }];
    });
  };

  const addPendingItem = () => {
    if (pendingItem) {
      setCart((prev) => {
        const itemExists = prev.find((item) => item.id === pendingItem.id);

        if (itemExists) {
          return prev.map((item) =>
            item.id === pendingItem.id
              ? { ...item, quantity: item.quantity + 1 }
              : item
          );
        }
        return [...prev, { ...pendingItem, quantity: 1 }];
      });
      alertify.success("Bekleyen ürün sepete eklendi!");
      setPendingItem(null);
    }
  };

  const removeFromCart = (id) => {
    setCart((prev) => prev.filter((item) => item.id !== id));
  };

  const clearCart = () => {
    setCart([]);
  };

  const completePurchase = () => {
    if (cart.length > 0) {
      setPurchasedItems((prev) => [...prev, ...cart]);
      setCart([]);
    } 
  };

  return (
    <CartContext.Provider
      value={{
        cart,
        purchasedItems,
        addToCart,
        addPendingItem,
        removeFromCart,
        clearCart,
        completePurchase,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

export default CartContext;
