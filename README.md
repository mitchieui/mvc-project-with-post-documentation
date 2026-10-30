# MVC Project with Post Documentation

An ASP.NET Core MVC classroom project demonstrating form submission with HTTP POST, model binding, and the Post/Redirect/Get pattern.

## Run locally

Install the .NET 9 SDK, then run from the repository root:

```bash
dotnet restore MyMvcApp/MyMvcApp.csproj
dotnet run --project MyMvcApp/MyMvcApp.csproj --launch-profile http
```

Open [the user form](http://localhost:5023/User/Create), enter a first name, last name, and email, then submit. The success page displays the submitted values. The terminal shows the actual listening address if your configuration differs.

## Documentation

- [Student guide (Markdown)](docs/MVC-Form-Student-Guide.md)
- [Student guide (Word)](docs/MVC-Form-Student-Guide.docx)

The guide explains the model, controller, Razor views, form Tag Helpers, model binding, redirects, validation improvements, and a student exercise.

## Project layout

```text
MyMvcApp/
  Controllers/   Request handlers for Home, Student, and User
  Models/        Form and student models, plus an EF Core context
  Views/         Razor pages and shared layouts
  wwwroot/       CSS, JavaScript, and bundled frontend libraries
docs/            Markdown and Word student guides
```

## Database configuration

The user form receives and displays values without saving them to a database. The project includes Entity Framework Core SQL Server packages and model mappings, but database context registration in `Program.cs` is commented out.

Database passwords in the published configuration are placeholders. If you enable database functionality, configure the connection string locally using .NET user secrets or the `ConnectionString` environment variable, and enable context registration using that configuration. Keep real credentials out of source control.

The example redirects submitted values through query parameters, so use sample data while following the classroom guide.
