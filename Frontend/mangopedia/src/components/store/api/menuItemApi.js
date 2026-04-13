import { baseApi } from "./baseApi";

export const menuItemApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    // Get menu items.
    getMenuItems: builder.query({
      query: () => "/MenuItem",
      providesTags: ["MenuItem"],
      transformResponse: (response) => {
        return response;
      },
    }),
  }),
});

export const { useGetMenuItemsQuery } = menuItemApi;
