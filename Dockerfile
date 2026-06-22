# Use the official .NET 10 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET 10 SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AiCore.WebApi/AiCore.WebApi.csproj", "AiCore.WebApi/"]
COPY ["AiCore.Application/AiCore.Application.csproj", "AiCore.Application/"]
COPY ["AiCore.Infrastructure/AiCore.Infrastructure.csproj", "AiCore.Infrastructure/"]
COPY ["AiCore.Domain/AiCore.Domain.csproj", "AiCore.Domain/"]
RUN dotnet restore "./AiCore.WebApi/AiCore.WebApi.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/AiCore.WebApi"
RUN dotnet build "./AiCore.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AiCore.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Build the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "AiCore.WebApi.dll"]
