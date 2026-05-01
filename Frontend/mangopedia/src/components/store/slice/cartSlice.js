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
