# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Services/identity-service/IdentityService/IdentityService.csproj", "Services/identity-service/IdentityService/"]
COPY ["EventContracts/EventContracts.csproj", "EventContracts/"]

RUN dotnet restore "Services/identity-service/IdentityService/IdentityService.csproj"

# copy everything else and build
COPY Services/identity-service/IdentityService/ Services/identity-service/IdentityService/
COPY EventContracts/ EventContracts/

WORKDIR "/src/Services/identity-service/IdentityService"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.dll"]