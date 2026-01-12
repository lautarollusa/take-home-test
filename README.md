# Loan Management System

A .NET Core backend API with Angular frontend for managing loans.

## Prerequisites

- Docker Desktop
- Node.js 18+ (for local frontend development)
- .NET 6 SDK (for local backend development)

## Quick Start with Docker (Recommended)

```bash
docker compose up --build
```

Access the application:
- **Frontend**: http://localhost
- **API**: http://localhost:5000
- **API Docs (Swagger)**: http://localhost:5000/swagger
- **Database**: localhost:1433 (SQL Server)

## Local Development

### Run Backend Only

```bash
cd backend/src/Fundo.Applications.WebApi
dotnet run
```

API runs at: `https://localhost:5001`

### Run Frontend Only

```bash
cd frontend
npm install
ng serve --open
```

Frontend runs at: `http://localhost:4200`

### Database Setup (if running locally)

```bash
cd backend/src/Fundo.Infrastructure
dotnet ef database update --startup-project ../Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj
```

## Testing

### Backend Tests

```bash
cd backend
dotnet test
```

### Frontend Tests

```bash
cd frontend
npm test
```

## API Endpoints

- `GET /api/loans` - List all loans
- `GET /api/loans/{id}` - Get loan by ID
- `POST /api/loans` - Create loan
- `POST /api/loans/{id}/payment` - Make payment

## Architecture

- **Backend**: .NET Core 6 with Clean Architecture
- **Frontend**: Angular 
- **Database**: SQL Server
- **DevOps**: Docker & Docker Compose