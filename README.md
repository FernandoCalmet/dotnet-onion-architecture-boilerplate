# 🦄 DOTNET TEMPLATE BOILERPLATE BASED ON ONION ARCHITECTURE

[![Github][github-shield]][github-url]
[![Kofi][kofi-shield]][kofi-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Khanakat][khanakat-shield]][khanakat-url]

## Table of Contents

* 🔥 [Description](#description)
* ⚙️ [Install and Setup](#installation-and-setup)
* 📓 [Project Overview](#project-overview)
* 📄 [License](#license)
* ⭐️ [Support the Project](#support-the-project)

## Description

This project provides a robust template for a .NET solution, based on the principles of Onion Architecture. It serves as an excellent starting point for any application, ensuring a clean and maintainable codebase.

## Installation and Setup

This section will guide you through the process of setting up the project on your local machine.

### Step 1: Clone the Repository
Start by cloning the repository to your local machine. You can accomplish this by using the following gh command:

```bash
gh repo clone FernandoCalmet/dotnet-onion-architecture-boilerplate
```

### Step 2: Database Migrations
Once you have the repository on your local machine, the next step is to set up the database. Start by running the migrations.

For the IdentityDbContext use the following command:

```bash
dotnet-ef migrations add InitialCreate --startup-project MyCompany.MyProduct.WebApi --project MyCompany.MyProduct.Persistence --output-dir Migrations\Identity --context IdentityDbContext
```

For the ApplicationDbContext, use the following command:

```bash
dotnet-ef migrations add InitialCreate --startup-project MyCompany.MyProduct.WebApi --project MyCompany.MyProduct.Persistence --output-dir Migrations\Application --context ApplicationDbContext
```

### Step 3: Database Update
After running the migrations, update the database.

For the IdentityDbContext use the following command:

```bash
dotnet-ef database update --startup-project MyCompany.MyProduct.WebApi --project MyCompany.MyProduct.Persistence --context IdentityDbContext
```

For the ApplicationDbContext, use the following command:

```bash
dotnet-ef database update --startup-project MyCompany.MyProduct.WebApi --project MyCompany.MyProduct.Persistence --context ApplicationDbContext
```

Congratulations, you have now set up the project on your local machine and you're ready to start developing!

## Project Overview

### The Challenge of Software Development
Crafting robust, scalable, and maintainable software architecture presents a significant challenge for developers. Onion Architecture emerges as a solution, offering a software development approach designed to mitigate these difficulties. This project provides a deep dive into Onion Architecture, highlighting its benefits and demonstrating its implementation in a .NET environment.

### Unveiling Onion Architecture
Developed by Jeffrey Palermo and influenced by Uncle Bob's Clean Architecture, Onion Architecture aims to make software independent of external dependencies such as frameworks, databases, UI, and more. It promotes the decoupling of an application into distinct layers, each layer tackling a specific concern or responsibility. The architecture typically consists of four layers:

### Domain Layer
The heart of Onion Architecture, the Domain Layer, encapsulates the core business logic of the application, including entities, value objects, business rules, and interfaces that outline contracts with other layers.

### Application Layer
The Application Layer serves as a bridge between the Presentation and Domain layers. It houses application services that dictate application flow and map data between the Domain and Presentation layers.

### Infrastructure Layer
The Infrastructure Layer encompasses all technical components of the application, such as data storage, logging, messaging, and more. It also implements the interfaces defined in the Domain Layer.

### Persistence Layer
Handling all data storage and retrieval operations, the Persistence Layer communicates directly with the underlying database or other persistent storage mechanisms. This layer encapsulates and implements the data access logic, safeguarding data consistency and integrity. It interacts with the Domain Layer via the defined interfaces, translating between the language of the domain and that of the database.

### Presentation Layer
The Presentation layer is responsible for presenting the application output to the users, like web pages, APIs, and user interfaces. It communicates with the Application layer to get the information from the Domain layer.

### Advantages of Onion Architecture in .NET
Onion Architecture brings several advantages to the table, including improved testability, maintainability, and flexibility. With Onion Architecture, unit tests that depend solely on the Domain Layer can be written, unaffected by any framework or external dependencies. This makes transitioning the UI or the database layer feasible without impacting the core business logic. The architecture also adheres to the Single Responsibility Principle, making the code more maintainable and easier to refactor.

### Project Structure
Below is a representation of a potential directory structure for this architecture:

```
MyCompany.MyProduct.sln
│
├───src
│   ├───MyCompany.MyProduct.Core
│   │   ├───Domain
│   │   │   ├───Entities
│   │   │   ├───Enums
│   │   │   ├───Events
│   │   │   └───ValueObjects
│   │   ├───Errors
│   │   ├───Exceptions
│   │   ├───Primitives
│   │   ├───Repositories
│   │   ├───Services
│   │   ├───Shared
│   │   └───Specifications
│   │
│   ├───MyCompany.MyProduct.Infrastructure
│   │   ├───Authentication
│   │   ├───BackgroundJobs
│   │   ├───Common
│   │   ├───Emails
│   │   ├───Identity
│   │   ├───Logging
│   │   ├───Mapping
│   │   ├───Messaging
│   │   ├───Notifications
│   │   └───OpenApi
│   │
│   ├───MyCompany.MyProduct.Persistence
│   │   ├───Configurations
│   │   ├───Constants
│   │   ├───Identity
│   │   ├───Migrations
│   │   └───Repositories
│   │
│   ├───MyCompany.MyProduct.Application
│   │   ├───Abstractions
|   |   |   ├───Authentication
│   │   │   ├───Common
│   │   │   ├───Data
│   │   │   ├───Emails
│   │   │   ├───Identity
│   │   │   ├───Messaging
│   │   │   └───Notifications
│   │   ├───Behaviors
│   │   ├───Exceptions
│   │   ├───Extensions
│   │   └───UsesCases
│   │
│   └───MyCompany.MyProduct.Presentation
│       ├───Abstractions
│       ├───Contracts
│       ├───Controllers
│       └───Middlewares
│
└───tests
    ├───MyCompany.MyProduct.Core.UnitTests
    ├───MyCompany.MyProduct.Infrastructure.UnitTests
    ├───MMyCompany.MyProduct.Application.UnitTests
    └───MyCompany.MyProduct.Presentation.UnitTests
```

In this design, the Core project contains the domain entities and business logic, the Application project hosts the use cases and services, the Infrastructure project covers technical components like logging and messaging, the Persistence project is responsible for data access, and the Presentation project includes user interface components. The project dependencies are as follows:

- The Core project has no dependencies.
- The Application project depends on the Core project.
- The Infrastructure project depends on the Core project and any necessary third-party libraries for its responsibilities.
- The Persistence project depends on the Core project and any necessary third-party libraries for data access.
- The Presentation project depends on the Application, Core, and potentially the Persistence and Infrastructure projects.

`Please note, this is one example of implementing Onion Architecture in .NET; adaptations may be required to align with specific needs and preferences.`

### Conclusion
In summary, Onion Architecture is an exceptional design approach that aids in building resilient, maintainable applications. It encourages decoupling and testability by separating the application into distinct layers. This project aims to provide an effective illustration of Onion Architecture in a .NET setting. We invite you to utilize this architecture in your next project and experience the benefits firsthand!

## License
This project is licensed under the MIT License. For more information, please refer to the [LICENSE](LICENSE.md) file.

## Support the Project
Your support means a lot! If you find this project useful or have used it in your own work, please consider giving it a star. This small act of appreciation helps maintain momentum and encourages further development. If you're inclined to contribute more substantially, you can [make a small donation here](https://ko-fi.com/fernandocalmet). Thank you for your support!

<!--- reference style links --->
[github-shield]: https://img.shields.io/badge/-@fernandocalmet-%23181717?style=flat-square&logo=github
[github-url]: https://github.com/fernandocalmet
[kofi-shield]: https://img.shields.io/badge/-@fernandocalmet-%231DA1F2?style=flat-square&logo=kofi&logoColor=ff5f5f
[kofi-url]: https://ko-fi.com/fernandocalmet
[linkedin-shield]: https://img.shields.io/badge/-fernandocalmet-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/fernandocalmet
[linkedin-url]: https://www.linkedin.com/in/fernandocalmet
[khanakat-shield]: https://img.shields.io/badge/khanakat.com-brightgreen?style=flat-square
[khanakat-url]: https://khanakat.com
