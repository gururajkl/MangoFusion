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

    // Create menu items.
    createMenuItem: builder.mutation({
      query: (formData) => ({
        url: "/MenuItem",
        method: "POST",
        body: formData,
      }),
      invalidatesTags: ["MenuItem"],
    }),

    deleteMenuItem: builder.mutation({
      query: (id) => ({
        url: `/MenuItem?id=${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["MenuItem"],
    }),

    updateMenuItem: builder.mutation({
      query: ({ id, formData }) => ({
        url: `/MenuItem?id=${id}`,
        method: "PUT",
        body: formData,
      }),
      invalidatesTags: ["MenuItem"],
    }),
  }),
});

export const {
  useGetMenuItemsQuery,
  useCreateMenuItemMutation,
  useDeleteMenuItemMutation,
  useUpdateMenuItemMutation,
} = menuItemsApi;
