# Use the ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET SDK image for build and publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["src/NewsletterService/Web/Web.csproj", "Web/"]
COPY ["src/NewsletterService/Application/Application.csproj", "Application/"]
COPY ["src/NewsletterService/Domain/Domain.csproj", "Domain/"]
COPY ["src/NewsletterService/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "./Web/Web.csproj"

# Copy the remaining source files
COPY src/NewsletterService/Web/ ./Web/
COPY src/NewsletterService/Application/ ./Application/
COPY src/NewsletterService/Domain/ ./Domain/
COPY src/NewsletterService/Infrastructure/ ./Infrastructure/

# Print the directory contents for debugging
RUN ls -la /src/Web
RUN cat /src/Web/Program.cs

# Build the project
WORKDIR "/src/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the ASP.NET base image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
