# CommentApi

A lightweight, feature-organized REST API for managing threaded comments on posts — built with **ASP.NET Core 8**, **Entity Framework Core**, and a custom lightweight mediator (CQRS-style request/handler pipeline). Designed as a clean, extensible backend for a blog, portfolio, or CMS commenting system.

---

## Features

- **CRUD operations** for comments — create, read (single/all), update, and soft-delete
- **Threaded replies** via `ParentCommentId` for nested comment support
- **CQRS-style architecture** — each operation is an isolated Command/Query + Handler, no bloated service classes
- **Automatic request validation** via [FluentValidation](https://docs.fluentvalidation.net/), wired transparently into the mediator pipeline
- **Consistent API responses** — every endpoint returns a uniform `Result` / `Result<T>` envelope with success/error metadata
- **Soft deletes** — comments are flagged as deleted (`IsDeleted`) rather than removed, preserving thread integrity
- **Swagger / OpenAPI** documentation available in development
- **CORS** pre-configured for a portfolio frontend

---

## Tech Stack

| Layer          | Technology                                   |
|----------------|-----------------------------------------------|
| Framework      | ASP.NET Core 8 (Web API)                     |
| ORM            | Entity Framework Core 8                      |
| Database       | SQL Server (default) — Postgres-compatible   |
| Validation     | FluentValidation                             |
| API Docs       | Swashbuckle (Swagger / OpenAPI 3.0)          |
| Architecture   | Vertical Slice / Feature folders + custom lightweight mediator |

---

## Project Structure

The project follows a **Vertical Slice Architecture** — code is organized by *feature*, not by technical layer. Each operation under `Features/Comments/` is self-contained with its own Command/Query, Handler, and Validator.

```
CommentApi/
├── Common/
│   ├── Abstraction/
│   │   ├── IRequest.cs              # Marker interface for commands/queries
│   │   ├── IRequestHandler.cs       # Handler contract
│   │   ├── ISender.cs / Sender.cs   # Custom mediator — dispatches requests to handlers, runs validation
│   │   └── ServiceCollectionExtensions.cs  # Auto-registers all handlers via reflection
│   ├── ExceptionType.cs             # Error classification (NotFound, Validation, Conflict, etc.)
│   ├── GlobalSettings.cs            # Strongly-typed configuration binding
│   ├── PagedList.cs                 # Pagination helper
│   └── Result.cs                    # Uniform success/failure response envelope
│
├── Controllers/
│   └── BaseController.cs            # Maps Result -> appropriate HTTP status code
│
├── Data/
│   └── CommentDbContext.cs          # EF Core DbContext + entity configuration
│
├── Features/
│   └── Comments/
│       ├── Comment.cs               # Domain entity
│       ├── CommentDto.cs            # Data transfer object
│       ├── CommentMapper.cs         # Entity <-> DTO mapping extensions
│       ├── CommentsController.cs    # HTTP endpoints
│       ├── Create/                  # CreateCommand, Handler, Validator
│       ├── GetAll/                  # GetAllQuery, Handler
│       ├── GetById/                 # GetByIdQuery, Handler
│       ├── Update/                  # UpdateCommand, Handler, Validator
│       └── Delete/                  # DeleteCommand, Handler
│
├── Migrations/                      # EF Core migrations
├── Repositories/                    # ICommentRepository + implementation
├── Properties/
├── Program.cs                       # App composition root
└── appsettings.Development.json
```

### Why this structure?

Instead of grouping code by technical type (`Controllers/`, `Services/`, `Repositories/`), each **feature** (e.g. `Create`, `Update`, `Delete`) owns its own command/query, handler, and validation logic. This keeps related code physically close together and makes the codebase easier to navigate and extend — adding a new operation means adding a new folder, not touching five different layers.

---

## Architecture: The Mediator Pattern

This project implements a **minimal custom mediator** (not MediatR) to decouple controllers from business logic:

1. Controllers build a `Command` or `Query` object and pass it to `ISender.Send(...)`.
2. `Sender` looks up a matching `IValidator<T>` (if one is registered) and runs FluentValidation automatically.
3. If validation fails, a standardized `Result.Failure(...)` is returned immediately — handlers never run on invalid input.
4. If validation passes, `Sender` resolves the correct `IRequestHandler<TRequest, TResponse>` via DI and reflection, and invokes it.
5. All handlers are **auto-registered** at startup by scanning the assembly (`AddRequestHandlers`), so no manual DI wiring is needed per feature.

This gives the benefits of CQRS/mediator libraries (clean separation, single-responsibility handlers, centralized validation) without adding an external dependency.

---

## API Response Envelope

Every endpoint returns a consistent `Result` / `Result<T>` shape:

```json
{
  "isSuccess": true,
  "isFailure": false,
  "data": { "...": "..." },
  "message": "Success",
  "errors": [],
  "exceptionType": "None"
}
```

`BaseController.Handle()` maps the `Result`'s `ExceptionType` to the appropriate HTTP status code:

| ExceptionType         | HTTP Status                |
|-----------------------|-----------------------------|
| `None` (success)      | `200 OK`                   |
| `Validation`          | `400 Bad Request`           |
| `Unauthorized`        | `401 Unauthorized`          |
| `Forbidden`           | `403 Forbidden`             |
| `NotFound`            | `404 Not Found`             |
| `AlreadyExists`       | `409 Conflict`              |
| `InternalServerError` | `500 Internal Server Error` |

---

## API Endpoints

Base route: `api/v1/Comments`

| Method   | Route                    | Description                          |
|----------|---------------------------|--------------------------------------|
| `POST`   | `/api/v1/Comments`         | Create a new comment                 |
| `GET`    | `/api/v1/Comments`         | Get all (non-deleted) comments       |
| `GET`    | `/api/v1/Comments/{id}`    | Get a single comment by ID            |
| `PUT`    | `/api/v1/Comments`         | Update an existing comment            |
| `DELETE` | `/api/v1/Comments/{id}`    | Soft-delete a comment                 |

### Example — Create a Comment

**Request**
```http
POST /api/v1/Comments
Content-Type: application/json

{
  "postId": "post-123",
  "authorName": "Dabananda Mitra",
  "authorEmail": "example@email.com",
  "content": "Great article, thanks for sharing!",
  "parentCommentId": null
}
```

**Response — `200 OK`**
```json
{
  "isSuccess": true,
  "message": "Success",
  "errors": []
}
```

**Validation error — `400 Bad Request`**
```json
{
  "isSuccess": false,
  "message": "Validation failed",
  "errors": [
    "AuthorName is required",
    "Content must be between 2 and 500 characters"
  ],
  "exceptionType": "Validation"
}
```

### Comment Entity

| Field             | Type       | Notes                                  |
|--------------------|------------|------------------------------------------|
| `Id`               | `Guid`     | Primary key                              |
| `PostId`           | `string`   | Required — identifies the parent post    |
| `AuthorName`       | `string`   | Required, max 100 characters             |
| `AuthorEmail`      | `string?`  | Optional, must be a valid email if set   |
| `Content`          | `string`   | Required, 2–500 characters               |
| `CreatedAt`        | `DateTime` | Set automatically (UTC)                  |
| `ParentCommentId`  | `Guid?`    | Set for threaded replies                 |
| `IsDeleted`        | `bool`     | Soft-delete flag                         |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A SQL Server instance (LocalDB, Docker, or Azure SQL) — or a Postgres-compatible database with the `Npgsql.EntityFrameworkCore.PostgreSQL` provider swapped in

### 1. Clone the repository

```bash
git clone https://github.com/dabananda/CommentApi.git
cd CommentApi/CommentApi
```

### 2. Configure the database connection

This project reads its connection string via strongly-typed configuration (`GlobalSettings`), sourced from `appsettings.json` or user-secrets. Don't commit real credentials — use `dotnet user-secrets` for local development:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:SqlServer" "Server=(localdb)\\mssqllocaldb;Database=CommentApiDb;Trusted_Connection=True;"
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

By default, Swagger UI is available at `https://localhost:<port>/swagger` in the `Development` environment.

---

## Configuration

| Setting                        | Description                                  |
|----------------------------------|-----------------------------------------------|
| `ConnectionStrings:SqlServer`   | Database connection string                    |
| CORS policy `"Portfolio"`       | Allows `http://localhost:3000` and the deployed portfolio origin — update in `Program.cs` for your own frontend |

---

## Tech Notes

- **Validation** is opt-in per feature — only requests with a registered `IValidator<T>` are validated; queries without side effects (e.g. `GetAllQuery`) skip this step entirely.
- **Soft delete** means `DeleteCommentAsync` never removes a row — it sets `IsDeleted = true`. All read queries filter `IsDeleted == false`.
- **Handler discovery** is fully reflection-based (`AddRequestHandlers`), so new features require zero changes to `Program.cs`.

---

## License

See [LICENSE.txt](./LICENSE.txt) for details.
