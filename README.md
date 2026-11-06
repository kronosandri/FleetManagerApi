# Fleet Manager API

A robust backend RESTful Web API built with **ASP.NET Core** and **Entity Framework Core (SQLite)** designed to manage vehicle fleets, customer profiles, and rental contracts efficiently.

It features a built-in single-page frontend dashboard, clean architectural patterns using DTOs, entity relationships, custom model validation rules, and automated API documentation with Scalar.

## Key Features

* **Interactive Web Control Panel:** Embedded single-page web UI (`index.html`) for managing fleet status, customers, and rental agreements directly from the browser.

* **Vehicle Management:** Register new vehicles, update fleet details, track rental statuses (`Available`, `Rented`, `Maintenance`), and soft-delete/decommission vehicles.

* **Customer Management:** Maintain customer records with strict duplicate checks for unique emails and driver's licenses, alongside validation rules protecting against deleting customers with contract history.

* **Contract Operations:** Handle lease agreements with automated state transitions (changing vehicle status from `Available` to `Rented`), custom date validation, and dedicated cancellation endpoints that release vehicles back to `Available`.

* **Interactive API Documentation:** Integrated with Scalar API reference (`/scalar/v1`) and OpenAPI endpoints for rapid testing.

## Tech Stack

* **Backend Framework:** ASP.NET Core Web API (.NET)

* **Database & ORM:** SQLite, Entity Framework Core (with Migrations support)

* **Frontend Dashboard:** HTML5 / CSS / JavaScript (Served via static files middleware)

* **API Documentation:** Microsoft OpenAPI / Scalar

* **Data Validation:** Data Annotations & Custom `IValidatableObject` rules

## Project Architecture

```
FleetManagerApi/
│
├── Controllers/         # API Endpoints (ContractsController, CustomersController, VehiclesController)
├── Data/                # FleetDbContext & DesignTimeDbContextFactory
├── DTOs/                # Data Transfer Objects for creation and updates
├── Models/              # Domain Models (Contract, Customer, Vehicle) & Validation Logic
├── Migrations/          # EF Core Database Migrations
└── wwwroot/             # Web Panel UI (index.html, static assets)

```

## Quick Start & Installation

You can run this project either by using the pre-compiled standalone executable or by running the source code directly with the .NET SDK.

### Option 1: Run the Pre-Compiled Executable (Quickest)

1. Download and extract the packaged release ZIP archive.

2. Double-click `FleetManagerApi.exe` to launch the application server.

3. Open your web browser and navigate to `http://localhost:5000` (or configured port) to access the interactive web panel immediately.

### Option 2: Build & Run from Source Code

1. **Clone the repository:**

   ```bash
   git clone https://github.com/kronosandri/FleetManagerApi.git
   cd FleetManagerApi
   ```

2. **Restore dependencies and build:**

   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the application:**

   ```bash
   dotnet run
   ```

4. **Access the Web Panel:**
   Navigate to the local server URL displayed in your console to interact with the dashboard panel (`index.html`).

## API Endpoints Overview

### **Vehicles (`/api/vehicles`)**

| Method | Endpoint | Description | 
| ----- | ----- | ----- | 
| **GET** | `/api/vehicles` | Retrieve all vehicles | 
| **GET** | `/api/vehicles/{id}` | Get vehicle details by ID | 
| **POST** | `/api/vehicles` | Create a vehicle (defaults status to `Available`) | 
| **PUT** | `/api/vehicles/{id}` | Update vehicle details and rental status | 
| **DELETE** | `/api/vehicles/{id}` | Decommission vehicle (updates status to `Decommissioned`) | 

### **Customers (`/api/customers`)**

| Method | Endpoint | Description | 
| ----- | ----- | ----- | 
| **GET** | `/api/customers` | Retrieve all registered customers | 
| **GET** | `/api/customers/{id}` | Get customer details by ID | 
| **POST** | `/api/customers` | Register a new customer (validates email & license uniqueness) | 
| **PUT** | `/api/customers/{id}` | Update customer profile details | 
| **DELETE** | `/api/customers/{id}` | Remove customer (blocked if contract history exists) | 

### **Contracts (`/api/contracts`)**

| Method | Endpoint | Description | 
| ----- | ----- | ----- | 
| **GET** | `/api/contracts` | Get all contracts (includes Vehicle and Customer details) | 
| **GET** | `/api/contracts/{id}` | Get contract details by ID | 
| **POST** | `/api/contracts` | Create a lease contract (transitions vehicle status to `Rented`) | 
| **POST** | `/api/contracts/{id}/cancel` | Cancel contract and release vehicle back to `Available` | 
