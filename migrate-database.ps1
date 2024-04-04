The Controller-Service-Repository pattern is widely adopted in many .NET applications. At the top level, the Controller layer serves as the entry point for external sources (e.g., APIs, UI components), exposing the application's functionality. The Repository layer, positioned at the bottom, manages the storage and retrieval of data from the underlying data source. The Service layer acts as an intermediary, housing the business logic of the application.

Controller Layer: This layer handles incoming requests from external sources and delegates them to the appropriate service. It exposes endpoints that can be accessed by clients to perform various operations.

Service Layer: The Service layer encapsulates the application's business logic and performs necessary operations to fulfill specific use cases. It operates at a higher level of abstraction, offering a meaningful API for the application's functionality. Services may interact with multiple repositories and other services to accomplish their tasks.

Repository Layer: Repositories abstract the data access layer of the application, providing methods for CRUD operations on the underlying data source. They encapsulate database interactions and map data between the application and the database. Each repository typically corresponds to a specific entity or model in the application.

To revert back any migration 
Before apply mogration 
in CLI : dotnet ef migrations remove
PMC : Remove-Migration

if Migration Applied
Update-Database -TargetMigrationName InitialMigration


