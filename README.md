**TeamHub** is a full-stack project designed for managing employees, projects, and tasks in a role-based system. 
Built with **ASP.NET Core 8**, **Entity Framework Core** and **ReactJS**, TeamHub is designed using **Clean Architecture** principles with full separation of concerns.

---

## 🧱 Architecture Overview
**TeamHub.API** - ASP.NET Core Web API
**TeamHub.Application** - Application logic, interfaces and services
**TeamHub.Common** - Helpers and utilities
**TeamHub.Domain** - Core domain models
**TeamHub.Infrastructure.Data** – EF Core DbContext and data access logic
**TeamHub.Infrastructure.Net** – External services 
**TeamHub.Contracts** – Shared message contracts for events
**TeamHub.Worker** – Background worker 
**TeamHub.UI** - The ReactJS frontend interface
**TeamHub.Tests** - xUnit-based Unit Tests

---

## ✨ Features

- JWT-based Login (Admin-managed users only)
- User roles: Administrator and Employee
- Admin can manage and assign users, projects, tasks
- Employees can view project-related tasks, update their profile, mark own tasks as complete, and assign tasks to peers in shared projects
- Profile picture upload
- Swagger API documentation
- CORS policy for frontend integration
- Strongly-typed settings via `JwtSettings`, `CorsSettings`
- Admin user and roles seeded on app start
- Role-based React UI with dashboards
- Exception middleware
- Event-driven email notifications using RabbitMQ and MassTransit
- Unit testing with xUnit, Moq & EF Core InMemory
- Docker support for full stack deployment

---

## 🧩 Prerequisites

To run TeamHub locally, ensure you have the following tools installed:

- .NET SDK 8.0
- React JS
- Entity Framework Core
- Microsoft SQL Server 2019/2022
- SQL Server Management Studio 19 (SSMS)
- Microsoft Visual Studio 2022
- RabbitMQ server
- Docker & Docker Compose

---

## 🚀 Getting Started

## ⚙️ Backend Setup & Run (Manual)
1. Clone the repo and open in **Visual Studio 2022**
2. Update `appsettings.json` → `DefaultConnection` with your SQL Server
3. Run EF Core migration: Update-Database
4. Start the API: dotnet run --project TeamHub.API

## ⚙️ Frontend Setup & Run (Manual)
cd TeamHub.UI
npm install
npm run dev

## ⚙️ Worker Setup & Run (Manual)
- dotnet run --project TeamHub.Worker (Make sure RabbitMQ is running first)

## Manual Setup Notes (Non-Docker)
- Make sure CORS, UI base URL, and API base URL match across TeamHub.API, TeamHub.UI, and launch settings.
- Update RabbitMQ host to localhost in appsettings.json when not using Docker:

<pre> ```json "RabbitMQ": { "Host": "localhost", "Username": "guest", "Password": "guest" } ``` </pre>

- Ensure your local database is properly configured and accessible with correct SQL user credentials.
(Check out the settings changes in the last commit)
 
---

## Docker Setup (Recommended)
To run the entire stack with Docker:
- Make sure Docker Desktop is running
- At the root of the solution, run: **docker-compose up --build**
- Open the React UI at http://localhost:3000
- Swagger UI (API) should be available at http://localhost:5000/swagger
If any ports conflict, you can adjust them in docker-compose.yml and respective launchSettings.json files.

⚠️ Note:
Email functionality uses a **private Gmail account configured locally for testing**.  
For security reasons, **SMTP credentials are not included** in the repository.
Please configure your own SMTP provider (e.g., Gmail, Outlook, SendGrid, Mailgun) 

---
