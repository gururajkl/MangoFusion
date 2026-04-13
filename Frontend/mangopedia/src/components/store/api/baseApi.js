import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query";
import { API_BASE_URL } from "../../../utility/constants";

// Base query function for RTK Query with authentication handling.
const baseQuery = fetchBaseQuery({
  baseUrl: API_BASE_URL + "/api",
});

// Wrapper around the base query to include authentication token in headers.
const baseQueryWithAuth = async (args, _api, extraOptions) => {
  const result = await baseQuery(args, _api, extraOptions);
  return result;
};

// Create the base API slice using RTK Query.
export const baseApi = createApi({
  baseQuery: baseQueryWithAuth,
  reducerPath: "api",
  tagTypes: [],
  endpoints: () => ({}),
});
