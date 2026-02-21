# FoodieApp

FoodieApp is a web application built with ASP.NET Core MVC for managing a restaurant's menu, categories, and orders. This repository includes both the customer-facing interface and a secure administrative dashboard.

## Features

### Customer Interface
- View available dishes categorized by families (Burgers, Pizzas, Pasta, etc.)
- Browse the complete menu with images, descriptions, and prices.
- Place orders.

### Admin Dashboard
To access the administrative panel, you must log in with the default administrator credentials:
- **Username:** `admin`
- **Password:** `admin123`

- **Product Management (CRUD):** 
  - **Create:** Add new dishes with images, prices, and descriptions.
  - **Read:** View the complete list of available dishes.
  - **Update:** Modify the details and prices of existing dishes.
  - **Delete:** Remove dishes from the menu.
- **Category Management:** Organize products into different categories.
- **Order Tracking:** Overview of all customer orders, including user details, ordered items, and dates.
- **Security:** Secure access using ASP.NET Core Identity / Cookie Authentication. Only users with the "Admin" role can access the management pages.

## Technology Stack

- **Framework:** ASP.NET Core MVC (.NET)
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Security:** ASP.NET Core Identity

## Setup and Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```bash
   cd FoodieApp
   ```
3. Update the database connection string in `appsettings.json` to point to your SQL Server instance, and apply Entity Framework Migrations to create the database:
   ```bash
   dotnet ef database update
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

## Architecture

This project follows the **MVC (Model-View-Controller)** pattern:
- **Controllers:** Contains the business logic (`ProductsController`, `CategoriesController`, `OrdersController`). Admin controllers are secured with `[Authorize(Roles = "Admin")]`.
- **Views:** The user interface built using Razor (`.cshtml`). Unauthenticated users see the standard menu, while admins have additional navigation links dynamically displayed.
- **Models:** Defines data structures like `Product` and `Category`. Admin functionalities feature data validation (e.g., preventing negative prices).

---
*Developed for managing restaurant operations effectively and securely.*
