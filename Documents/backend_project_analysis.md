# Backend Project Architecture Analysis

The GameStore backend is a modern .NET 8 ASP.NET Core Web API application implementing a game store with features for managing games, genres, and shopping baskets. Here's a comprehensive overview of its architecture:

## Technology Stack
- **Framework**: .NET 8 with ASP.NET Core minimal APIs
- **Database**: SQLite with Entity Framework Core 8.0
- **Authentication**: JWT Bearer tokens via Keycloak identity provider
- **Documentation**: Swagger/OpenAPI integration
- **Serialization**: System.Text.Json (implicit via minimal APIs)

## Project Structure
```
GameStore.Backend/
├── Backend.sln (Visual Studio solution)
├── src/GameStore.Api/ (Main API project)
│   ├── Data/ (EF Core context and migrations)
│   ├── Features/ (Feature-based organization)
│   │   ├── Games/ (CRUD operations for games)
│   │   ├── Genres/ (Genre management)
│   │   └── Baskets/ (Shopping cart functionality)
│   ├── Models/ (Entity models)
│   ├── Shared/ (Cross-cutting concerns)
│   └── wwwroot/ (Static files)
└── local-infra/ (Docker setup for Keycloak)
```

## Architecture Patterns

### Feature-Based Architecture (Vertical Slices)
- Each feature (Games, Genres, Baskets) is self-contained with its own:
  - Endpoint definitions
  - Data transfer objects (DTOs)
  - Business logic
- Promotes high cohesion and low coupling
- Easy to maintain and extend

### Minimal APIs Pattern
- Uses ASP.NET Core's minimal APIs for endpoint definition
- Extension methods for organizing endpoints by feature
- Clean, concise route definitions with built-in parameter binding

### CQRS-Style Separation
- Separate DTOs for requests and responses
- Clear separation between read and write operations
- Pagination support for list endpoints

## Data Layer
- **Entity Framework Core** with SQLite
- **Models**: Game, Genre, CustomerBasket, BasketItem
- **Relationships**: Games belong to Genres, BasketItems link Games to CustomerBaskets
- **Migrations**: Automated database schema management
- **Seeding**: Initial data population for genres

## Security & Authorization
- **JWT Authentication** via Keycloak
- **Role-Based Authorization**: Admin and User access levels
- **Policy-Based Access Control**: UserAccess (fallback), AdminAccess
- **Custom Authorization Handler** for basket operations

## Cross-Cutting Concerns
- **Global Exception Handling**: Centralized error management with structured logging
- **HTTP Logging**: Request/response logging for debugging
- **Request Timing**: Middleware for performance monitoring
- **File Upload**: Secure image upload with validation (size, type, storage)

## Local Development Infrastructure
- **Docker Compose** setup for Keycloak identity server
- **Realm Configuration**: Pre-configured "gamestore" realm with roles and clients
- **Development Database**: SQLite file-based database

## Key Features
1. **Games Management**: Full CRUD with image upload, genre association, and audit tracking
2. **Genres**: Simple reference data management
3. **Shopping Baskets**: User-specific cart management with authorization

## Development Practices
- **Nullable Reference Types**: Enabled for better null safety
- **Implicit Usings**: Clean namespace management
- **User Secrets**: Secure configuration management
- **Logging**: Structured logging with different levels
- **Validation**: Built-in model validation with custom attributes

This architecture demonstrates modern .NET practices with a focus on maintainability, security, and developer experience through its feature-based organization and use of ASP.NET Core's latest features.