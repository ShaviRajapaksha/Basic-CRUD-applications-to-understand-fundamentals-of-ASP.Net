## Create the Project
```bash
dotnet new webapi -n ProductCrudApi
cd ProductCrudApi
```
## Install Required NuGet Packages
###make sure to install packages related to dotnet version
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.4
```

## Create and Run Migrations
```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

## Run the Application
```bash
dotnet run
```
