# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Services/gateway/ApiGateway/ApiGateway.csproj", "Services/gateway/ApiGateway/"]
RUN dotnet restore "Services/gateway/ApiGateway/ApiGateway.csproj"

COPY . .
WORKDIR "/src/Services/gateway/ApiGateway"
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
