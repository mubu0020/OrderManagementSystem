# Order Management System

A backend **Order Management System (OMS)** built using **Domain-Driven Design (DDD)** and a layered architecture.  
The system is designed to clearly separate business logic from infrastructure and application concerns, making the project easier to maintain, test, and scale.

---

# Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Domain Layer](#domain-layer)
- [Application Layer](#application-layer)
- [Infrastructure Layer](#infrastructure-layer)
- [API Layer](#api-layer)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Future Improvements](#future-improvements)

---

# Overview

This project implements an **Order Management System** responsible for handling the lifecycle of orders within an application.

Typical responsibilities include:

- Creating orders
- Retrieving order data
- Updating order information
- Managing order related business rules

The system follows **Domain-Driven Design (DDD)** principles where the **domain layer contains the core business logic** and is independent of technical frameworks.

---

# Architecture

The project follows a **layered architecture inspired by Clean Architecture**.

```
API (Presentation Layer)
        │
        ▼
Application Layer
        │
        ▼
Domain Layer
        │
        ▼
Infrastructure Layer
```

### Dependency Direction

```
API → Application → Domain
Infrastructure → Domain
```

The **Domain layer has no dependencies** on other layers.

---

# Project Structure

```
OrderManagementSystem
│
├── Domain
│   ├── Entities
│   ├── ValueObjects
│   └── DomainServices
│
├── Application
│   ├── UseCases
│   ├── DTOs
│   └── ApplicationServices
│
├── Infrastructure
│   ├── Repositories
│   └── ExternalIntegrations
│
└── API
    ├── Controllers
    └── Request / Response Models
```

---

# Domain Layer

The **Domain layer contains the core business logic** of the system.

Responsibilities include:

- Entities (e.g. `Order`)
- Value Objects
- Domain Services
- Business Rules

The domain should be **framework independent** and not rely on infrastructure or external systems.

---

# Application Layer

The **Application layer orchestrates use cases** and coordinates domain logic.

Responsibilities:

- Application services
- Use cases
- DTOs
- Input validation
- Mapping between API models and domain models

This layer acts as a bridge between **API and Domain**.

---

# Infrastructure Layer

The **Infrastructure layer provides technical implementations** required by the application.

Examples:

- Repository implementations
- Database access
- External APIs
- Messaging systems
- File systems or other external resources

Infrastructure depends on the **Domain layer interfaces**.

---

# API Layer

The **API layer exposes the system through HTTP endpoints**.

Responsibilities:

- Controllers
- Request / Response models
- Routing
- Input validation

The API should remain **thin** and delegate business logic to the Application layer.

---

# Technologies

This project uses the following technologies:

- C#
- .NET
- ASP.NET Core
- REST API
- Dependency Injection
- Domain-Driven Design (DDD)

---

# Getting Started

## Prerequisites

Make sure you have the following installed:

- .NET SDK
- Git
- Visual Studio / Rider / VS Code

---

## Clone the repository

```bash
git clone https://github.com/mubu0020/OrderManagementSystem.git
```

---

## Navigate to the project

```bash
cd OrderManagementSystem
```

---

## Build the project

```bash
dotnet build
```

---

## Run the project

```bash
dotnet run
```

---

# Future Improvements

Potential future improvements for the system:

- Order status tracking
- Payment integration
- Inventory integration
- Event-driven architecture
- Authentication and authorization
- Integration tests

---

# Purpose of the Project

This repository is intended to:

- Demonstrate **Domain-Driven Design architecture**
- Provide a **clean backend structure**
- Explore **maintainable system design**

---

# License

This project is provided for educational and development purposes.
