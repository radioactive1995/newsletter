# newsletter

dotnet ef migrations add InitialCreate --project .\Infrastructure.csproj --startup-project ..\Web\Web.csproj -o Persistance\Migrations

dotnet ef database update InitialCreate --project .\Infrastructure.csproj --startup-project ..\Web\Web.csproj