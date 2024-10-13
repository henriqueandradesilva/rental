# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base stage for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["mottu/src/Application/Application.csproj", "mottu/src/Application/"]
COPY ["mottu/src/Infrastructure/Infrastructure.csproj", "mottu/src/Infrastructure/"]
COPY ["mottu/src/MotorcycleWorker/MotorcycleWorker.csproj", "mottu/src/MotorcycleWorker/"]
RUN dotnet restore "./mottu/src/MotorcycleWorker/MotorcycleWorker.csproj"
COPY . .
WORKDIR "/src/mottu/src/MotorcycleWorker"
RUN dotnet build "./MotorcycleWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MotorcycleWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["mottu/src/MotorcycleWorker/appsettings.json", "./"]
ENTRYPOINT ["dotnet", "MotorcycleWorker.dll"]
