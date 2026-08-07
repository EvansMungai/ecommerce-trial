# 🛒 Order Processing System

A modular, event-driven order processing system built with ASP.NET Core 10, EF Core, RabbitMQ and PostgreSQL — containerized with Docker for seamless orchestration.

---

## ✨ Features

1. **Order Management**: Order Creation, Persistence, Computed Values.
2. **Catalog Management**: Product Creation, Categorization and Inventory Management.
3. **Clean Architecture**: Separation of concerns, Event-driven architecture, Domain-driven design, Generic Repository.
4. **Infrastructure & Orchestration**: Docker. Docker compose for local orchestration.
5. **Observability**: RabbitMQ Management UI, Container-level Database Inspection and Logs.

---

## 🏛 Architecture

### Domain-Driven Design

1. **Encapsulation and Rich Domain Model**
   - Properties cannot be modified from the outside. Changes can only happen through valid business invariants inside the domain.
   - OR-Mapping compromise: The parameter-less constructor allows the ORM to materialize objects from the database while keeping code from instantiating invalid empty orders.
2. **Guard clauses and Invariant Protection**
   - Defensive coding in entity constructors to ensure prevention of invalid states.
   - Business rules enforcements within the domain using methods.

### Event-Driven Architecture

1. **Mass Transit as Messagging Backbone**
   - The publishing component does not know who is listening. It simply broadcasts that an event has occurred using an asynchronous fire-and-forget patter.
2. **Choreographed Saga(Eventual Consistency)**
   - Reliance on message queues to achieve eventual consistency across system boundaries instead of relying on heavy, blocking HTTP call.
   - Immutable events via records guarantees that once an event is pushed into the broker, its data cannot be modified.
  
### Data Layer Strategy

1. **Context & Configuration Isolation**
   - Implementation of EntityTypeConfiguration for Domain Entities keeps the DbContext light.
   - Use of reflection-based discovery for configuration classes within the assembly.
   - Implementation of a custom IUnitOfWork interface to handle saving of data atomically, abstracting transaction boundaries.

### Orchestration

1. **Database-per-service (Microservices pattern)**
   - Physical Isolation: each microservice talks to its own database.
   - Decoupled Schemas: each database run container instances with their own dedicated, persistent named volumes, preventing cross-domain database mapping.
2. **Container Lifecycle & Policies**
   - Use of self-healing state hooks to ensure service availability.
  
## ⚠️ Tradeoffs
- EF Core constructor binding limitations require parameterless constructors
- RabbitMQ setup assumes local dev; cloud migration may require TLS and credential hardening

## 🚀 Tech Stack

1. **API Layer**
   - **Technology:** ASP.NET Core Web API  
   - **Rationale:** Enables clean separation of concerns, built-in Swagger support and extensibility for future endpoints.
2. **Domain Layer**
   - **Technology:** DDD-style entities
   - **Rationale:** Enforces business rules, immutability and encapsulation at the core of the system.
3. **Messaging Layer**
   - **Technology:** MassTransit with RabbitMQ  
   - **Rationale:** Facilitates decoupled communication via durable events and supports scalable message routing.
4. **Persistence Layer**
   - **Technology:** EF Core with PostgreSQL  
   - **Rationale:** Provides relational integrity, supports value object mapping and integrates seamlessly with .NET.
5. **Orchestration Layer**
   - **Technology:** Docker Compose  
   - **Rationale:** Enables reproducible, multi-container deployment with clear service dependencies and isolation.

---

## ⚙️ Setup Instructions

### 🔧 Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)  
- [Docker](https://www.docker.com/) + [Docker Compose](https://docs.docker.com/compose/)

### 📦 Build & Run

```bash
docker-compose down -v
docker-compose up --build
```

🧩 This Will
- 🚀 Build and run the **CatalogAPI** on [http://localhost:5001](http://localhost:5001)
- 🚀 Build and run the **OrderAPI** on [http://localhost:8081](http://localhost:8081)  
- 🧑‍🏭 Start the **Worker**, **PostgreSQL**, and **RabbitMQ** services for each microservice
- 🗃️ Apply **EF Core migrations** automatically on API startup

---
