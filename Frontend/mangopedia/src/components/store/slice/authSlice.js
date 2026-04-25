import { createSlice } from "@reduxjs/toolkit";
import { STORAGE_KEYS } from "../../../utility/constants";
import {
  getUserInfoFromToken,
  isTokenExpired,
} from "../../../utility/jwtUtility";

const getInitialAuthState = () => {
  const storedToken = localStorage.getItem(STORAGE_KEYS.TOKENMANGO);
  const storedUser = localStorage.getItem(STORAGE_KEYS.USER);

  if (
    !storedToken ||
    storedToken === "undefined" ||
    storedToken === "null" ||
    isTokenExpired(storedToken)
  ) {
    localStorage.removeItem(STORAGE_KEYS.TOKENMANGO);
    localStorage.removeItem(STORAGE_KEYS.USER);
    return {
      token: null,
      user: null,
      isAuthenticated: false,
    };
  }

  let user = null;
  try {
    if (storedUser && storedUser !== "undefined" && storedUser !== "null") {
      user = JSON.parse(storedUser);
    }
  } catch {
    user = getUserInfoFromToken(storedToken);
    if (user) {
      localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
    }
  }

  return {
    token: storedToken,
    user: user,
    isAuthenticated: !!storedToken && !!user,
  };
};

const authSlice = createSlice({
  name: "auth",
  initialState: { ...getInitialAuthState() },
  reducers: {
    setAuth: (state, action) => {
      const { user, token } = action.payload;
      state.user = user;
      state.token = token;
      state.isAuthenticated = !!(token && user);

      if (user) localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
      if (token) localStorage.setItem(STORAGE_KEYS.TOKENMANGO, token);
    },

    logout: (state) => {
      localStorage.removeItem(STORAGE_KEYS.TOKENMANGO);
      localStorage.removeItem(STORAGE_KEYS.USER);
      state.user = null;
      state.token = null;
      state.isAuthenticated = false;
    },
  },
});

export const { setAuth, logout } = authSlice.actions;
export default authSlice.reducer;
