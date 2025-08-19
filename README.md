Forklift API
============

A .NET Core Web API for managing forklift data, following the principles of Clean Architecture.

1\. Overview
------------

This project is a RESTful API designed to manage a list of forklifts. It is built using .NET 9 and structured according to the Clean Architecture pattern, which promotes a separation of concerns, testability, and maintainability.

### 1.1. Architecture

The solution is divided into the following projects, representing the different layers of Clean Architecture:

*   **ForkliftAPI.Api**: The presentation layer. This is the entry point of the application, containing the ASP.NET Core controllers and the API configuration. It depends on all other layers.
    
*   **ForkliftAPI.Application**: The application layer. This layer contains the business logic, DTOs (Data Transfer Objects), and service interfaces. It depends on the Domain layer.
    
*   **ForkliftAPI.Domain**: The core domain layer. This layer contains the essential business entities and rules. It has no dependencies on any other project.
    
*   **ForkliftAPI.Infrastructure**: The infrastructure layer. This layer provides the implementation details for services and data persistence, such as Entity Framework Core for database interactions. It depends on the Domain and Application layers.
    
*   **ForkliftAPI.UnitTests**: A dedicated project for unit testing the Domain and Application layers.
    

2\. API Endpoints
-----------------

The API exposes the following endpoints through the ForkliftsController.

API Endpoints

The API exposes the following endpoints through the `ForkliftsController`.

| Method   | Endpoint                      | Description                                   | Request Body                         |
|----------|-------------------------------|-----------------------------------------------|--------------------------------------|
| `GET`    | `/api/forklifts`              | Retrieves all forklifts.                      | *None* |
| `POST`   | `/api/forklifts/upload`       | Uploads a new list of forklifts.              | `List<ForkliftDto>`                  |
| `DELETE` | `/api/forklifts/clear`        | Clears all existing forklift data. 

### 2.1. Example DTO

The ForkliftDto object is used for data transfer between the client and the API. An example of this object might look like this:

```json
[
  {
    "name": "Titan Lifter",
    "modelNumber": "XA123",
    "manufacturingDate": "2015-05-12"
  },
  {
    "name": "Zephyr Mover",
    "modelNumber": "YB456",
    "manufacturingDate": "2018-07-23"
  }
]
```

3\. Getting Started
-------------------

### 3.1. Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
    
*   [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
    

### 3.2. Setup and Installation

1.  git clone https://github.com/your-username/forklift-api.git
  
2.  cd forklift-api
    
3.  dotnet restore ForkliftAPISolution.sln
    
4.  (optional) dotnet ef database update --project ForkliftAPI.Infrastructure --startup-project ForkliftAPI.Api

    Note: Ensure your database connection string in appsettings.json is correctly configured.
    
5.  dotnet run --project ForkliftAPI.Api

    The API will be available at http://localhost:5000 (or https://localhost:5001 with HTTPS, https://localhost:5001/swagger/index.html for Swagger).

**If you are running on this project on the VS Code, please use Debug mode with launch.json file (.vscode) to "Run and Debug"**
    
4\. Contributing
----------------

Feel free to open an issue or submit a pull request if you find any bugs or have suggestions for improvements.
