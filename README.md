# newsletter

dotnet ef migrations add InitialCreate --project .\Infrastructure.csproj --startup-project ..\Web\Web.csproj -o Persistance\Migrations

dotnet ef database update --project .\Infrastructure.csproj --startup-project ..\Web\Web.csproj

dotnet ef migrations add InitialCreate --project ./Infrastructure.csproj --startup-project ../Web/Web.csproj -o Persistance/Migrations

dotnet ef database update --project ./Infrastructure.csproj --startup-project ../Web/Web.csproj