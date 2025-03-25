AuthService - Unit Test Plan

---

This document outlines the unit testing strategy for the `AuthService` class in the `TeamHub.Application` layer.  
The goal is to ensure secure and expected authentication behavior using JWT.

---

Testing Stack

- Test Framework: xUnit  
- Mocking Library: Moq  
- Database: InMemory (for `ApplicationDbContext`)  
- Logging: `ILogger<T>` via `LoggerFactory`  
- Target Class: `AuthService`

---

Methods & Test Cases

1. `AuthenticateUser(LoginModel model)`

 Test Case Description                                        Expected Result                          
 ----------------------------------------------------        ------------------------------------------
 Should return `null` if email/password are missing           `null` 
 Should return `null` if user is not found                    `null` 
 Should return `null` if password is invalid                  `null` 
 Should return a valid JWT token for correct credentials      `JWT string returned (`not null or empty`)`  
 Should throw exception if JWT secret is too short            `ArgumentException` thrown    

---