import { STORAGE_KEYS } from "../../../utility/constants";
import { isTokenExpired } from "../../../utility/jwtUtility";

const getInitialAuthState = () => {
  const token = localStorage.getItem(STORAGE_KEYS.TOKENMANGO);
  const user = localStorage.getItem(STORAGE_KEYS.USER);

  if (
    !token ||
    token === "undefined" ||
    token === "null" ||
    isTokenExpired(token)
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
