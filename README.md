# Used Car Sales Application

Web application to manage used car stock, customers, sales and billing.

**Stack:** Angular 21 (frontend), ASP.NET Core 8 Web API (backend), SQL Server (database, database-first).

## Features

- Car stock: add, edit, delete, search, filter by status, pagination
- Car profile with photo gallery (upload, set main photo, remove)
- Customer management
- Sales: selling a car creates a bill and marks the car as Sold in one transaction
- Billing: bill list, invoice view, print

## Project structure

```
database/    SQL scripts (schema + sample data)
backend/     .NET solution (Domain, Application, Infrastructure, Api)
frontend/    Angular application
```

Backend layers: Api -> Infrastructure -> Application -> Domain.
Domain has no dependencies. Application defines interfaces; Infrastructure implements them.

## Prerequisites

.NET 8 SDK, Node.js 20+, Angular CLI, SQL Server (Express is enough), SSMS.

## Setup

1. **Database:** run `database/01_schema.sql`, then `database/02_seed.sql` in SSMS.
2. **Backend:** set the connection string in
   `backend/UsedCarSales/src/UsedCarSales.Api/appsettings.json`
   (`ConnectionStrings:DefaultConnection`), then run the `UsedCarSales.Api` project.
   Swagger: `https://localhost:7152/swagger`
3. **Frontend:**

   ```
   cd frontend
   npm install
   ng serve
   ```

   Open `http://localhost:4200`.
   The API address is in `frontend/src/environments/environment.development.ts`.
   If your API runs on a different port, change it there.

## Key design decisions

- **Database-first:** the SQL script is the source of truth; entities were scaffolded from it.
- **Database enforces rules too:** CHECK constraints, unique VIN, one bill per car,
  one primary image per car. A car cannot be sold twice.
- **Concurrency:** `RowVersion` on Cars stops two users selling the same car at once.
- **Global exception middleware** converts errors to proper HTTP status codes (404, 409, 500).
- **Images** are stored on disk (`wwwroot/uploads/cars`); the database holds only the path.
  Files are validated (type, size) and saved under random names.
- **Tax** is 18%, set in one place: `Domain/Constants/BillingRules.cs`.
- **Authentication** is out of scope for this assignment.
