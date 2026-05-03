# MangoFusion

MangoFusion is a full-stack food ordering platform with separate backend and frontend applications.

## Repository Layout

- `Backend/MangoFusion.API/` - ASP.NET Core Web API for authentication, menu management, orders, and data persistence.
- `Frontend/mangopedia/` - React + Vite frontend application for browsing menu items, managing carts, placing orders, and handling role-based admin workflows.

## Backend

The backend is built with .NET 10, Entity Framework Core, ASP.NET Core Identity, JWT authentication, and SQL Server.

### Key Backend Features

- Authentication and authorization using ASP.NET Core Identity and JWT bearer tokens
- Menu, order header, and order details APIs
- EF Core migrations and SQL Server database support
- OpenAPI support in development

### Run the Backend

1. Open a terminal in `Backend/MangoFusion.API`
2. Run `dotnet restore`
3. Run `dotnet ef database update` to apply migrations (if the database is not yet created)
4. Run `dotnet run`

## Frontend (Mangopedia)

The frontend is a React application built with Vite. It provides a responsive UI for menu browsing, cart checkout, order management, and admin role-based pages.

### Frontend Tech Stack

- React 19
- Vite
- React Router v7
- Redux Toolkit
- Bootstrap 5
- React Toastify
- SweetAlert2

### Run the Frontend

1. Open a terminal in `Frontend/mangopedia`
2. Run `npm install`
3. Run `npm run dev`
4. Open the local URL shown in the terminal to access the app

## Available Scripts

### Backend

- `dotnet restore` - install .NET dependencies
- `dotnet build` - build the backend project
- `dotnet run` - start the backend server
- `dotnet ef database update` - apply EF Core migrations

### Frontend

- `npm install` - install dependencies
- `npm run dev` - start the development server
- `npm run build` - build the production bundle
- `npm run preview` - preview the production build locally
- `npm run lint` - run ESLint across the frontend codebase

## Notes

- Ensure SQL Server is available and the backend `DefaultConnection` string in `Backend/MangoFusion.API/appsettings.json` is configured correctly.
- The frontend is designed to support MangoFusion’s menu and order workflows while keeping the app lightweight and easy to extend.
