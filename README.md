# ⚡ Transformer Monitor - Enterprise Full-Stack Showcase

A robust, real-time IoT monitoring and maintenance management system built with **.NET 8** and **Vue 3**. This project serves as a showcase of modern software engineering practices, featuring **Clean Onion Architecture**, **Domain-Driven Design (DDD)**, and real-time telemetry.

---

## 🏗️ Architectural Overview

The project is strictly divided into two main parts, coordinated by a central configuration system.

### 1. Backend: Clean Onion Architecture (.NET 8)
Designed for high testability and complete independence from external frameworks.

*   **Domain Layer:** The "heart" of the application. Contains pure entities, enums, and repository interfaces. All base repository methods are marked as `virtual` to allow specialized overrides in the infrastructure layer.
*   **Application Layer (CQRS):** Orchestrates business logic using the **MediatR** pattern. 
    *   **Queries & Commands:** Complete separation of read and write operations.
    *   **Behaviors:** Integrated **FluentValidation** pipeline that intercepts commands and validates them before they reach the handler.
    *   **Mapping:** Specialized **AutoMapper** profiles for seamless Entity-to-DTO transformations.
*   **Infrastructure Layer:** Handles data persistence via **Entity Framework Core** (SQL Server).
    *   **Background Workers:** A hosted simulation service that generates real-time voltage fluctuations and persists them to the database.
    *   **Repository Pattern:** Concrete implementations with specialized `Include` logic for nested entities.
*   **API Layer:** The entry point. Features **SignalR Hubs** for real-time data broadcasting and a **Global Exception Middleware** that ensures every error is returned as a standardized JSON response.

### 2. Frontend: Domain-Driven Design (Vue 3 + TypeScript)
Organized into logical domains to ensure scalability and maintainability.

*   **`application/`**: Contains **Pinia** stores for reactive state management and core services like `ConfigService`.
*   **`domain/`**: Defines the frontend data models and specialized mappers to ensure type-safety across the system.
*   **`data/`**: Repository implementations that handle API communication.
*   **`presentation/`**: The UI layer.
    *   **Views:** Complex pages like the Maintenance Dispatcher and Dashboard.
    *   **Modals:** Specialized forms with real-time validation.
    *   **Components:** Atomic UI elements and domain-specific widgets (Charts, Tables).

---

## ✨ Key Features & Technical Highlights

*   **Real-time Telemetry:** Uses **SignalR (WebSockets)** to push live voltage readings from the .NET background engine to the Vue dashboard without page refreshes.
*   **Maintenance Dispatcher:** A comprehensive ticketing system allowing dispatchers to create work orders, track status, and assign/re-assign teams.
*   **Dynamic UX:**
    *   **Drag & Drop:** Custom implementation allowing users to reorganize UI widgets, with the state persisted in local storage.
    *   **Active/Active State:** Immediate visual feedback on all interactions (scale effects, loading states).
*   **Centralized Configuration:** Both projects read from a shared `app-config.json` in the root folder. Changing a port or URL in one place updates the entire full-stack environment, including CORS policies.
*   **Robustness:** 100% "if-block" brace coverage, strict TypeScript typing, and backend unit testing with **xUnit** and **Moq**.

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js (LTS)
- SQL Server LocalDB (installed by default with Visual Studio)

### 1. Global Configuration
Network settings are managed in `/app-config.json`. 
- **Default Backend:** http://localhost:61471
- **Default Frontend:** http://localhost:5173

### 2. Backend Setup
```bash
cd backend/src/TransformerMonitor.Api
dotnet run
```
*The system will automatically create the `TransformerTestDataBase` and apply migrations on the first run.*

### 3. Frontend Setup
```bash
cd presentation
npm install
npm run dev
```

---

## 🛠 Engineering Standards Applied

1.  **Zero-Warning Policy:** The solution is configured to be warning-free during the build process.
2.  **Clean Code:** Naming follows strict conventions (PascalCase for C#, camelCase for TS).
3.  **Encapsulation:** Controllers are kept "thin," delegating all logic to MediatR handlers.
4.  **Persistence Integrity:** Using the Unit of Work pattern to ensure atomic database operations.

---
*Developed as a demonstration of production-ready Full-Stack Architecture.*
