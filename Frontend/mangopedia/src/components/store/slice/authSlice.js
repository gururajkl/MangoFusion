import { STORAGE_KEYS } from "../../../utility/constants";
import { isTokenExpired } from "../../../utility/jwtUtility";

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
};
