# MockSys

Mock system to prototype functionality and practice integrating with a larger system

---

## Setup

1. Install Docker
1. Open the solution in Visual Studio
1. Make sure `MockSys.AppHost` is set as the Startup Project
1. Press Start with the `https` profile

### (Optional) Enable Gulp for Easy Migration Scripting

1. Run the project as outlined above
1. From the Resources tab in the default MockSys Dashboard, click the `mocksys-postgres` container to open its properties
1. Under `Resource`, find the `Connection String` section, click the peek icon, then the inspect icon
1. Copy the connection string
1. In Visual Studio, Right-click the `MockSys.Web` project
1. Select `Manage User Secrets` in the dropdown
1. Add the copied connection string to `{ "ConnectionStrings": {"mocksys": YOUR_CONNECTION_STRING } }`
1. In a developer console, run `npm i` at the solution root
1. In Visual Studio, open Task Runner Explorer (if it's not already a panel in your IDE, click Search at the top of the window and use Feature Search to find it)
1. The following tasks should be available to you
	- addMigration: creates a new migration file for the `ReportingDbContext` in the project's `Migrations` folder
	- updateDatabase: applies the existing migrations to the `mocksys-postres` PostgreSQL database
	- aaddMigrationAndUpdateDatabase: does the first two tasks in succession
	- rollbackMigration: rolls back the most recent migration in the database

### Notes

- Database migrations are automatically applied when `MockSys.Web` launches through `app.UseDatabaseAutoMigrate();` in `Program.cs`
