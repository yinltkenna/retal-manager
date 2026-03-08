# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Services/notification-service/NotificationService/NotificationService.csproj", "Services/notification-service/NotificationService/"]
COPY ["EventContracts/EventContracts.csproj", "EventContracts/"]

RUN dotnet restore "Services/notification-service/NotificationService/NotificationService.csproj"

# copy everything else and build
COPY Services/notification-service/NotificationService/ Services/notification-service/NotificationService/
COPY EventContracts/ EventContracts/

WORKDIR "/src/Services/notification-service/NotificationService"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]