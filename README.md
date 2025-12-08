# 🏨 Bookify - Premium Hotel Booking Solution

Bookify is a modern, full-featured hotel booking web application built with **ASP.NET Core MVC**. It provides a seamless experience for customers to find and book stays, while offering powerful tools for hotel owners and administrators to manage their properties and reservations.

**Live Demo:** http://bookingo.tryasp.net/

## ✨ Key Features

### 🌍 For Customers
*   **Smart Search:** Find hotels by destination, dates, and guest count with a premium, user-friendly search interface.
*   **Detailed Hotel Views:** Explore hotels with high-quality image galleries, amenities lists, and guest reviews.
*   **Secure Booking:** Easy-to-use checkout flow integrated with **Stripe** for secure payments.
*   **Responsive Design:** Fully optimized for desktop, tablet, and mobile devices.

### 👨‍💼 For Hotel Owners & Admins
*   **Interactive Dashboard:** Real-time statistics on revenue, active reservations, and occupancy rates.
*   **Property Management:** Complete control over hotel details, room types, and availability.
*   **Reservation Management:** View, approve, or cancel bookings with ease.
*   **Invoicing:** Automated invoice generation and tracking.
*   **Role-Based Access:** Secure environment with distinct roles for Admins, Hotel Owners, and Customers.

## 🛠️ Technology Stack

*   **Framework:** ASP.NET Core 8.0 MVC
*   **Language:** C#
*   **Database:** SQL Server with Entity Framework Core
*   **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5
*   **Authentication:** ASP.NET Core Identity
*   **Payments:** Stripe API
*   **Notifications:** Toastr.js

## 🏗️ Architecture

The project follows a clean **N-Tier Architecture** to ensure scalability and maintainability:

*   **Bookify.PL (Presentation Layer):** Controllers, Views, and ViewModels.
*   **Bookify.BL (Business Layer):** Services and Interfaces implementing business logic.
*   **Bookify.DL (Data Layer):** Repositories, DbContext, and Migrations.
*   **Bookify.Models:** Domain entities and data models.
*   **Bookify.Utility:** Helper classes and constants (e.g., Static Details).

## 🚀 Getting Started

### Prerequisites
*   .NET 8.0 SDK
*   SQL Server
*   Visual Studio 2022 or VS Code


2.  **Configure Database**
    Update the connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=BookifyDb;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```

3.  **Configure Stripe**
    Add your Stripe API keys to `appsettings.json`:
    ```json
    "Stripe": {
      "SecretKey": "your_secret_key",
      "PublishableKey": "your_publishable_key"
    }
    ```

4.  **Run Migrations**
    Open Package Manager Console and run:
    ```bash
    update-database
    ```

5.  **Run the Application**
    ```bash
    dotnet run --project Bookify.PL
    ```

