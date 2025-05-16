## Book-Event-Task For Areeb Company

# 🚀 Web API Project

The Book - Event Management System is a web-based application designed to help organizations and 
individuals create, manage, events. It allows event Admin to create and publish 
events, manage registrations, and track attendees. The system supports role-based authentication 
(admin, User) with various levels of access and functionality.
Admin

## Features

### Phase 01: Foundations
- Implemented RESTful API principles
- Structured the project with Onion Architecture for modularity and maintainability
- Documented and tested APIs using Postman, Swagger, and HTTP files
- Built the Modules with:
  - DbContext configuration
  - DbInitializer for seeding data
  - Generic Repository and Unit of Work patterns for streamlined data access

### Phase 02: Core Features
- Developed Modules Services and Controllers for business logic and endpoint exposure
- Implemented Specification Pattern for advanced filtering, sorting,  dynamic query evaluation ,And Ordering
- Added a Picture URL Resolver for dynamic image handling
- Integrated Redis for performance optimization
- Configured production-ready deployment with Kestrel
- Implement Caching To Cache Data
- Implement Attachment Service To Enable Admin  Upload Image When Create Or Update Event
- Implement Validation For Dtos With Package Fluent Validation
- Add Some Interseptors To Set Specific Users When Creation Or Updation
- Implement Result Pattern And Rejex Pattern

### Phase 03: Event Modoule
- Make Full CRUD Opearetion For Event
- User Can Book Event And Track Event For Book 



### Phase 03: Book Modoule
- Admin  Can Track Event And Use Crud Operation For Event Modoule
- Implement Fluent Validation To Validate On Dtos
- Complex Logic For This Modoule
- Make Full CRUD Opearetion For Event

### Phase 04: Security and Authentication
- Configured Identity for user management and role-based access control.
- mplemented JWT-based Authentication for secure API access.
- Extended Swagger for testing secured endpoints.
- Implement Refresh Token

### Phase 05: Performance and Deployment
- Introduced caching via a Caching Service and Cache Attribute.
- Deployed the application using Kestrel for production readiness.

## 🛠️ Technologies Used

- **Framework:** .NET Core 8  
- **Database:** SQL Server, Redis  
- **Authentication:** Identity, JWT  
- **Documentation & Testing:** Swagger, Postman, HTTP files
- **Front End:** Implement Full Componet Of Project With React.js And Folder Name Front End

## 🚀 Deployment

1. **Production-ready deployment:** Configured with **MonsterApi**.
2. **Caching:** Integrated **Redis** for performance optimization.

## 📘 Key Learnings

This project helped me:

1. Understand and implement scalable **Onion Architecture**.
2. Build APIs with advanced features like filtering, sorting, and pagination.
3. Integrate external services like  and **Redis** seamlessly.
4. Deploy a production-ready API with robust **security** and **caching mechanisms**.

## 📘 Archetcture
### Layer Responsibilities

| Layer          | Responsibilities                          | Projects |
|----------------|------------------------------------------|----------|
| **API**        | HTTP endpoints, authentication, DTOs     | BookEvent.Apis, BookEvent.Apis.Controller |
| **Core**       | Business logic, domain models, interfaces| BookEvent.Core.* |
| **Infrastructure** | Data access, external services       | BookEvent.Infrastructure.* |
| **Shared**     | Common utilities, shared contracts       | BookEvent.Shared |


BookEventTask/
├── Apis/ # API Layer
│ ├── BookEvent.Apis/ # Main API project
│ └── BookEvent.Apis.Controller/ # API Controllers
│
├── Core/ # Core Business Logic
│ ├── BookEvent.Core.Application/ # Application services
│ ├── BookEvent.Core.Application.Abstracti # Interfaces
│ └── BookEvent.Core.Domain/ # Domain models
│
├── Infrastructure/ # Infrastructure
│ ├── BookEvent.Infrastructure/ # Infrastructure impl
│ └── BookEvent.Infrastructure.Persistence/ # Persistence
│
└── BookEvent.Shared/ # Shared components


 ## 📬 Contact

Feel free to reach out to me:

- 📧 Email: [mohammedhamdi726@gmail.com](mailto:mohammedhamdi726@gmail.com)  
- 💼 LinkedIn: [www.linkedin.com/in/mohamedhamdy23](www.linkedin.com/in/mohamedhamdy23)  


