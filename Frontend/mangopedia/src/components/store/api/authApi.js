import { baseApi } from "./baseApi";

export const authApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    // Login user.
    loginUser: builder.mutation({
      query: (formData) => ({
        url: "/Auth/login",
        method: "POST",
        body: formData,
      }),
    }),

    // Register user.
    registerUser: builder.mutation({
      query: (formData) => ({
        url: "/Auth/register",
        method: "POST",
        body: formData,
      }),
    }),
  }),
});

export const { useLoginUserMutation, useRegisterUserMutation } = authApi;
