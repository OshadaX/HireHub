# HireHub 🚀

> **A Job Posting & Application Platform REST API** built with ASP.NET Core (.NET 10), following Clean Architecture principles.

---

## 📖 Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Running the API](#running-the-api)
- [API Endpoints](#api-endpoints)
- [Database](#database)
- [Project Structure](#project-structure)

---

## Overview

HireHub is a backend REST API that enables:

- **Job seekers** to browse job listings and submit applications
- **Employers** to post jobs under their company profile
- **Admins** to manage application statuses throughout the hiring pipeline

Authentication is handled via **JWT Bearer tokens** with role-based claims. Passwords are securely hashed using **BCrypt**.

---

## Tech Stack

| Layer          | Technology                                      |
|----------------|-------------------------------------------------|
| Framework      | ASP.NET Core Web API (.NET 10)                  |
| ORM            | Entity Framework Core 10                        |
| Database       | PostgreSQL (via Npgsql)                         |
| Auth           | JWT Bearer Tokens (HMAC-SHA256)                 |
| Password Hash  | BCrypt.Net-Next                                 |
| API Docs       | Scalar (OpenAPI)                                |
| Architecture   | Clean Architecture (4-layer)                    |

---

## Architecture

HireHub follows **Clean Architecture** with a clear separation of concerns across four projects:

```
HireHub/
└── src/
    ├── HireHub.Domain/          ← Core entities & enums (no dependencies)
    ├── HireHub.Application/     ← DTOs & business rules
    ├── HireHub.Infrastructure/  ← EF Core DbContext, PostgreSQL, migrations
    └── HireHub.API/             ← Controllers, HTTP layer, JWT setup
```

**Dependency flow:** `API` → `Application` → `Domain` ← `Infrastructure`

---

## Prerequisites

Make sure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (local) **or** a free [Supabase](https://supabase.com) project
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

Install the EF Core CLI tool if you haven't already:

```bash
dotnet tool install --global dotnet-ef
```

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/HireHub.git
cd HireHub
```

### 2. Configure the application

Copy the example config and fill in your values:

```bash
cp src/HireHub.API/appsettings.Development.example.json src/HireHub.API/appsettings.Development.json
```

Then edit `appsettings.Development.json` — see the [Configuration](#configuration) section below.

### 3. Apply database migrations

```bash
dotnet ef database update --project src/HireHub.Infrastructure --startup-project src/HireHub.API
```

### 4. Run the API

```bash
dotnet run --project src/HireHub.API
```

The API will be available at:

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

---

## Configuration

Edit `src/HireHub.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=hirehub;Username=postgres;Password=your_password"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "HireHub",
    "Audience": "HireHub",
    "ExpiryInDays": 7
  }
}
```

> **Note:** If using Supabase, your connection string will look like:
> `Host=db.xxxx.supabase.co;Database=postgres;Username=postgres;Password=your_password;SSL Mode=Require;Trust Server Certificate=true`

> ⚠️ **Never commit real secrets to Git.** Add `appsettings.Development.json` to your `.gitignore`.

---

## Running the API

### Development

```bash
dotnet run --project src/HireHub.API
```

### Interactive API Docs (Scalar)

Once running in Development mode, open:

```
https://localhost:5001/scalar/v1
```

This gives you a full interactive UI to explore and test all endpoints.

---

## API Endpoints

### Auth

| Method | Endpoint               | Auth Required | Description         |
|--------|------------------------|---------------|---------------------|
| POST   | `/api/auth/register`   | ❌            | Register a new user |
| POST   | `/api/auth/login`      | ❌            | Login & get JWT     |

**Register body:**
```json
{
  "fullName": "Jane Doe",
  "email": "jane@example.com",
  "phone": "0771234567",
  "password": "SecurePass123",
  "role": "JobSeeker"
}
```

**Login response:**
```json
{
  "token": "<JWT>",
  "fullName": "Jane Doe",
  "email": "jane@example.com",
  "role": "JobSeeker"
}
```

---

### Jobs

> All endpoints require a valid `Authorization: Bearer <token>` header.

| Method | Endpoint          | Description           |
|--------|-------------------|-----------------------|
| GET    | `/api/jobs`       | List all jobs         |
| GET    | `/api/jobs/{id}`  | Get a job by ID       |
| POST   | `/api/jobs`       | Create a new job      |
| PUT    | `/api/jobs/{id}`  | Update a job          |
| DELETE | `/api/jobs/{id}`  | Delete a job          |

**Create Job body:**
```json
{
  "title": "Backend Developer",
  "description": "Build APIs with .NET",
  "location": "Colombo",
  "salary": 150000,
  "jobType": "FullTime",
  "companyId": "<company-guid>"
}
```

---

### Companies

| Method | Endpoint              | Auth Required | Description           |
|--------|-----------------------|---------------|-----------------------|
| GET    | `/api/companies`      | ✅            | List all companies    |
| POST   | `/api/companies`      | ✅            | Create a company      |

---

### Applications

| Method | Endpoint                          | Auth Required | Description                  |
|--------|-----------------------------------|---------------|------------------------------|
| GET    | `/api/applications`               | ❌            | List all applications        |
| POST   | `/api/applications`               | ❌            | Submit a job application     |
| PUT    | `/api/applications/{id}/status`   | ❌            | Update application status    |

**Application Status values:** `Applied` → `Reviewed` → `Interview` → `Hired` / `Rejected`

---

## Database

### Entities

| Entity        | Key Fields                                              |
|---------------|---------------------------------------------------------|
| `User`        | Id, FullName, Email, PasswordHash, Role, CreatedAt      |
| `Company`     | Id, Name, ...                                           |
| `Job`         | Id, Title, Description, Location, Salary, JobType, Status, CompanyId |
| `Application` | Id, JobId, UserId, CoverLetter, Status, AppliedAt       |
| `Notification`| Id, ...                                                 |

### Migrations

```bash
# Create a new migration
dotnet ef migrations add <MigrationName> \
  --project src/HireHub.Infrastructure \
  --startup-project src/HireHub.API

# Apply migrations
dotnet ef database update \
  --project src/HireHub.Infrastructure \
  --startup-project src/HireHub.API

# Revert last migration
dotnet ef database update PreviousMigrationName \
  --project src/HireHub.Infrastructure \
  --startup-project src/HireHub.API
```

---

## Project Structure

```
HireHub/
├── HireHub.slnx
└── src/
    ├── HireHub.Domain/
    │   ├── Entities/
    │   │   ├── User.cs
    │   │   ├── Company.cs
    │   │   ├── Job.cs
    │   │   ├── Application.cs
    │   │   └── Notification.cs
    │   └── Enums/
    │       ├── JobStatus.cs
    │       └── ApplicationStatus.cs
    │
    ├── HireHub.Application/
    │   └── DTOs/
    │       ├── RegisterDto.cs
    │       ├── LoginDto.cs
    │       ├── CreateJobDto.cs
    │       └── ...
    │
    ├── HireHub.Infrastructure/
    │   ├── Data/
    │   │   └── AppDbContext.cs
    │   └── Migrations/
    │
    └── HireHub.API/
        ├── Controllers/
        │   ├── AuthController.cs
        │   ├── JobsController.cs
        │   ├── CompaniesController.cs
        │   └── ApplicationsController.cs
        ├── Program.cs
        └── appsettings.json
```

---

## License

MIT — see [LICENSE](LICENSE) for details.