MusicBand is a full-stack web application for managing musicians, bands, and instruments. Built with Angular, ASP.NET Core, Entity Framework Core, MSSQL, Docker, it provides a scalable REST API, authentication with JWT, and a modern responsive frontend for organizing and browsing music-related data.

MusicBand is a full-stack web application designed to manage musicians, bands, and musical instruments. The platform allows users to create, browse, and organize music-related entities through a modern web interface and a scalable backend API.

The application demonstrates a clean architecture approach with separation between frontend, backend, and infrastructure, making it suitable for real-world applications and scalable deployments.

Features Manage musicians and their associated instruments Register and authenticate users with JWT authentication Paginated API responses for efficient data loading Error handling using structured API responses Responsive user interface for browsing and managing data RESTful API built with clean architecture principles

Tech Stack Frontend Angular TypeScript RxJS Angular Signals Angular Http Interceptors

Backend ASP.NET Core C# REST API JWT Authentication Clean Architecture CQRS pattern

ORM Entity Framework Core

Database MongoDB

Infrastructure Docker Nginx

Architecture The project follows a layered architecture:

Frontend (Angular) │ ▼ REST API (ASP.NET Core) │ ▼ MSSLQ Database

The backend uses a clean architecture approach separating domain, application, infrastructure, and API layers. API Response Pattern The backend uses a consistent response wrapper: APIOperationResult This structure ensures that every response includes: operation result payload data structured error messages

Running the Project Backend cd backend dotnet run Frontend cd frontend npm install ng serve Docker docker-compose up Purpose

This project was created as a full-stack architecture example demonstrating modern development practices using Angular and ASP.NET Core
