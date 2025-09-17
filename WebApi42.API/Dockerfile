# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApi42.API/WebApi42.API.csproj", "WebApi42.API/"]
COPY ["WebApi42.Bussiness/WebApi42.Core.csproj", "WebApi42.Bussiness/"]
COPY ["WebApi42.DAO/WebApi42.Infrastructure.csproj", "WebApi42.DAO/"]
COPY ["WebApi42.DTO/WebApi42.Contracts.csproj", "WebApi42.DTO/"]
RUN dotnet restore "./WebApi42.API/WebApi42.API.csproj"
COPY . .
WORKDIR "/src/WebApi42.API"
RUN dotnet build "./WebApi42.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi42.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi42.API.dll"]