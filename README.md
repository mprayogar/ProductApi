# ProductApi (.NET 8 Web API)

ProductApi is a simple RESTful API built with ASP.NET Core (.NET 8) and Entity Framework Core. It provides basic features like product CRUD operations and JWT-based authentication.

## Features

- User registration and login using JWT token
- CRUD operations for products
- SQLite as the database
- Clean architecture (Controller - Service - DTO - Model)
- Swagger UI for easy API testing

## Requirements

- .NET 8 SDK
- Git
- (Optional) Visual Studio Code / Postman / SQLite Viewer

## How to Run (Locally)

1. Clone the repository:
   git clone https://github.com/mprayogar/ProductApi.git
   cd ProductApi
2. Restore dependencies:
   dotnet restore
3. Apply database migrations:
   dotnet ef database update
4. Run the application:
   dotnet run --launch-profile https
5. Open your browser and access Swagger UI:
   http://localhost:5156/swagger/index.html

Authentication Example
{
"username": "admin",
"password": "admin123"
}

Project Structure
ProductApi/
├── Controllers/
├── Services/
├── Dtos/
├── Models/
├── Data/
├── Middlewares/
├── Migrations/
├── Program.cs
├── appsettings.json
└── ProductApi.sln
