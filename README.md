TeamHub is a full-stack project designed for managing employee records, projects, and tasks, following a role-based system with Administrator and Employee roles. 
It is built using ASP.NET Core 8, Entity Framework Core, and React JS. 

This solution follows a clean architecture by separating concerns across multiple projects:
TeamHub.API - The Web API layer
TeamHub.Application - Application logic, services, interfaces
TeamHub.Common - Helpers
TeamHub.Domain - Core domain entities
TeamHub.Infrastructure - Database context and data access
TeamHub.UI - The React frontend interface

Features Implemented

- JWT-based Login (Admin-managed users only)
- User Roles: Administrator / Employee
- Admin can CRUD Users, Projects, Tasks, and assign users to projects/tasks
- Employees can view project-related tasks, update their profile, mark own tasks as complete, and assign tasks to peers in shared projects
- Profile picture upload support (stored as virtual path)
- Swagger API documentation
- CORS policy setup for React development
- Project-wide settings via strongly-typed JwtSettings and CorsSettings classes
- Seed roles and admin user on app start
- React UI: Role-based views with navigation (Admin Dashboard, Employee Dashboard, Login)
- Project structure follows best practices: Controllers, Services/Interfaces, Models/ViewModels
- Code-first EF Core database approach
- Exception middleware

Getting Started
- Clone the repo and open in Visual Studio 2022.
- Update the connection string in appsettings.json to point to your local SQL Server instance.
-  Run the following command to create the database: Update-Database

Seeded Admin Account (used during initial startup):
- Email: admin@teamhub.com
- Password: Admin@123

Launch the API (TeamHub.API) and React UI (TeamHub.UI) using:
- dotnet run
- npm install
- npm run dev
