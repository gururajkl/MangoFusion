import { createSlice } from "@reduxjs/toolkit";
import { STORAGE_KEYS } from "../../../utility/constants";

const getStoredCart = () => {
  try {
    var cart = localStorage.getItem(STORAGE_KEYS.CART);
    var parsedCart = cart ? JSON.parse(cart) : [];
    return Array.isArray(parsedCart) ? parsedCart : [];
  } catch {
    localStorage.removeItem(STORAGE_KEYS.CART);
    return [];
  }
};

const saveCart = (items) => {
  try {
    localStorage.setItem(STORAGE_KEYS.CART, JSON.stringify(items));
  } catch {
    console.error("Failed to save cart to localStorage");
  }
};

const calculateTotals = (items = []) => {
  let totalItems = 0;
  let totalAmount = 0;

  for (const item of items) {
    totalItems += item.quantity;
    totalAmount += item.quantity * item.price;
  }
  return { totalItems, totalAmount };
};

const cartSlice = createSlice({
  name: "cart",
  initialState: {
    items: getStoredCart() || [],
    ...calculateTotals(getStoredCart()),
  },
  reducers: {
    addToCart: (state, action) => {
      const existingItem = state.items.find(
        (item) => item.id === action.payload.id,
      );

      if (existingItem) {
        existingItem.quantity += 1;
      } else {
        state.items.push({
          ...action.payload,
          quantity: action.payload.quantity || 1,
        });
      }

      Object.assign(state, calculateTotals(state.items));
      saveCart(state.items);
    },
  },
});

export const { addToCart } = cartSlice.actions;
export default cartSlice.reducer;
