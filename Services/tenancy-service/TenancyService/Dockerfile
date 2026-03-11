# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Services/tenancy-service/TenancyService/TenancyService.csproj", "Services/tenancy-service/TenancyService/"]
COPY ["EventContracts/EventContracts.csproj", "EventContracts/"]

RUN dotnet restore "Services/tenancy-service/TenancyService/TenancyService.csproj"

# copy everything else and build
COPY Services/tenancy-service/TenancyService/ Services/tenancy-service/TenancyService/
COPY EventContracts/ EventContracts/

WORKDIR "/src/Services/tenancy-service/TenancyService"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TenancyService.dll"]