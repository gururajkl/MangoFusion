export const ROUTES = {
  HOME: "/",
  LOGIN: "/login",
  REGISTER: "/register",
  CART: "/cart",
  CHECKOUT: "/checkout",
  ORDER_CONFIRMATION: "/order-confirmation",
  ORDER_MANAGEMENT: "/order-management",
  MENU_MANAGEMENT: "/menu-management",
  MENU_DETAIL: "/menu/:id",
};

export const API_BASE_URL = "https://localhost:7137";

export const CATEGORIES = ["Appetizer", "Entrée", "Dessert"];

export const SPECIAL_TAGS = [
  "Chef's Special",
  "Top Rated",
  "New",
  "",
  "Best Seller",
];

export const ROLES = { CUSTOMER: "Customer", ADMIN: "Admin" };

export const STORAGE_KEYS = {
  TOKENMANGO: "token-mango",
  USER: "user-mango",
  CART: "cart-mango",
};

export const ORDER_STATUS = {
  CONFIRMED: "Confirmed",
  READY_FOR_PICKUP: "Ready for Pickup",
  COMPLETED: "Completed",
  CANCELLED: "Cancelled",
};

export const ORDER_STATUS_OPTIONS = [
  {
    value: ORDER_STATUS.CONFIRMED,
    label: ORDER_STATUS.CONFIRMED,
    color: "warning",
  },
  {
    value: ORDER_STATUS.READY_FOR_PICKUP,
    label: ORDER_STATUS.READY_FOR_PICKUP,
    color: "info",
  },
  {
    value: ORDER_STATUS.COMPLETED,
    label: ORDER_STATUS.COMPLETED,
    color: "success",
  },
  {
    value: ORDER_STATUS.CANCELLED,
    label: ORDER_STATUS.CANCELLED,
    color: "danger",
  },
];
