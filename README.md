# TraineeManagement.Api

## Project Overview

TraineeManagement.Api is an ASP.NET Core Web API developed for managing trainee records.

The project demonstrates:

- ASP.NET Core Web API
- Controllers
- DTOs
- Service Layer
- EF Core InMemory Database
- CRUD Operations
- Validation
- Swagger

---

## Technology Stack

- C#
- .NET 9 
- ASP.NET Core Web API
- Entity Framework Core
- EF Core InMemory Database
- Swagger

---

## Project Structure

TraineeManagement.Api
│
├── Controllers
│   ├── HealthController.cs
│   └── TraineeController.cs
│
├── Models
│   └── Trainee.cs
│
├── DTOs
│   ├── CreateTraineeDto.cs
│   ├── UpdateTraineeDto.cs
│   └── TraineeResponseDto.cs
│
├── Services
│   ├── Interfaces
│   │   └── ITraineeService.cs
│   │
│   └── TraineeService.cs
│
├── Data
│   └── DbContext.cs
│
└── Program.cs

---

## How to Run
 
### Clone the repo first

### Navigate to Project
cd TraineeManagement.Api
### Restore Packages
dotnet restore
### Run Application
dotnet run

### Open Swagger
https://localhost:<port>/swagger


## API Endpoints

### Health Check
GET  /api/health 

### Trainee APIs
 GET  /api/trainees 
 GET  /api/trainees/{id} 
 POST  /api/trainees 
 PUT  /api/trainees/{id} 
 DELETE  /api/trainees/{id} 
 GET  /api/trainees?search=value 


## Sample POST Request

### POST /api/trainees
{
  "firstName": "Amit",
  "lastName": "Sharma",
  "email": "amit.sharma@training.com",
  "techStack": "HTML, CSS, JavaScript",
  "status": "Active"
}


## Sample Success Response
{
  "id": 1,
  "firstName": "Amit",
  "lastName": "Sharma"
}

## Sample GET Response

[
  {
    "id": 1,
    "firstName": "Amit",
    "lastName": "Sharma"
  }
]

## Sample Validation Error

{
  "errors": {
    "firstName": [
      "First name is required"
    ],
    "email": [
      "Valid email is required"
    ]
  }
}


