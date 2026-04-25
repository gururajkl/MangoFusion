import { jwtDecode } from "jwt-decode";

const decodeJWT = (token) => {
  try {
    return jwtDecode(token);
  } catch {
    return null;
  }
};

const isTokenExpired = (token) => {
  const decodedToken = decodeJWT(token);
  if (!decodedToken || !decodedToken.exp) return true;
  return Date.now() >= decodedToken.exp * 1000;
};

const getUserInfoFromToken = (token) => {
  const decodedToken = decodeJWT(token);

  if (!decodedToken) return null;

  return {
    id: decodedToken.id,
    email: decodedToken.email,
    name: decodedToken.fullName,
    role: decodedToken.role,
    exp: decodedToken.exp,
  };
};

export { decodeJWT, isTokenExpired, getUserInfoFromToken };
