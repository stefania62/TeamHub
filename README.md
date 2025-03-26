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

---

## 🚀 Getting Started

## ⚙️ Backend Setup & Run
1. Clone the repo and open in **Visual Studio 2022**
2. Update `appsettings.json` → `DefaultConnection` with your SQL Server
3. Run EF Core migration: Update-Database
4. Start the API: dotnet run --project TeamHub.API

## ⚙️ Frontend Setup & Run
cd TeamHub.UI
npm install
npm run dev

## ⚙️ Worker Setup & Run
- dotnet run --project TeamHub.Worker (Make sure RabbitMQ is running first)

⚠️ Note:
Email functionality uses a **private Gmail account configured locally for testing**.  
For security reasons, **SMTP credentials are not included** in the repository.
Please configure your own SMTP provider (e.g., Gmail, Outlook, SendGrid, Mailgun) 

---
