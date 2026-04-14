import { baseApi } from "./baseApi";

export const menuItemsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    // Get menu items.
    getMenuItems: builder.query({
      query: () => "/MenuItem",
      providesTags: ["MenuItem"],
      transformResponse: (response) => {
        if (response && response.result && Array.isArray(response.result)) {
          return response.result;
        }

        if (response && Array.isArray(response)) {
          return response;
        }

        return [];
      },
    }),
  }),
});

export const { useGetMenuItemsQuery } = menuItemsApi;
