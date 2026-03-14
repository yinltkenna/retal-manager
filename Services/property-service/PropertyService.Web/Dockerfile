# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["EventContracts/EventContracts.csproj", "EventContracts/"]

COPY ["Services\property-service\PropertyService.Application\PropertyService.Application.csproj", "Services/property-service/PropertyService.Application/"]
COPY ["Services\property-service\PropertyService.Domain\PropertyService.Domain.csproj", "Services/property-service/PropertyService.Domain/"]
COPY ["Services\property-service\PropertyService.Infrastructure\PropertyService.Infrastructure.csproj", "Services/property-service/PropertyService.Infrastructure/"]
COPY ["Services\property-service\PropertyService.Web\PropertyService.Web.csproj", "Services/property-service/PropertyService.Web/"]
COPY ["Services\property-service\PropertyService.Tests\PropertyService.Tests.csproj", "Services/property-service/PropertyService.Tests/"]

# Restore dependencies
RUN dotnet restore "Services\property-service\PropertyService.Web\PropertyService.Web.csproj"

# Copy the remaining source code
COPY Services\property-service Services\property-service
COPY EventContracts/ EventContracts/

# Build the application
WORKDIR "/src/Services/property-service/PropertyService.Web"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PropertyService.Web.dll"]