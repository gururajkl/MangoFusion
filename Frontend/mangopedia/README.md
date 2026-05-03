# Mangopedia

Mangopedia is the frontend application for the MangoFusion project. It is built with React and Vite, and provides a responsive UI for browsing menu items, managing carts, placing orders, and handling admin workflows through role-based routing.

## Key Features

- Authentication: login and registration flows
- Role-based UI routes for admin and regular users
- Menu browsing and detailed item views
- Shopping cart and checkout experience
- Order confirmation and order management pages
- Redux Toolkit for app state management
- Bootstrap for responsive layout and UI styling

## Tech Stack

- React 19
- Vite
- React Router v7
- Redux Toolkit
- Bootstrap 5
- React Toastify
- SweetAlert2

## Available Scripts

From the project root, run:

- `npm install` — install dependencies
- `npm run dev` — start the development server
- `npm run build` — build the production bundle
- `npm run preview` — preview the production build locally
- `npm run lint` — run ESLint across the codebase

## Project Structure

- `src/` — application source code
  - `components/` — reusable UI components and route wrappers
  - `layouts/` — app header and footer layout components
  - `pages/` — feature pages for home, auth, cart, menu, and orders
  - `store/` — Redux store configuration, slices, and API services
  - `utility/` — shared constants and helper utilities

## Getting Started

1. Clone the repository.
2. Open the project in your editor.
3. Run `npm install`.
4. Run `npm run dev`.
5. Open the local URL shown in the terminal to access the app.

## Notes

This frontend is designed to support MangoFusion's menu and order management workflows while keeping the app lightweight and easy to extend.
