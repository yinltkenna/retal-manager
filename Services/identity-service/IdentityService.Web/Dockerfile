# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy file csproj của Shared Project
COPY ["EventContracts/EventContracts.csproj", "EventContracts/"]

# 2. Copy từng file csproj vào đúng cấu trúc thư mục (Cách này an toàn nhất cho Docker Cache)
COPY ["Services/identity-service/IdentityService.Domain/IdentityService.Domain.csproj", "Services/identity-service/IdentityService.Domain/"]
COPY ["Services/identity-service/IdentityService.Application/IdentityService.Application.csproj", "Services/identity-service/IdentityService.Application/"]
COPY ["Services/identity-service/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj", "Services/identity-service/IdentityService.Infrastructure/"]
COPY ["Services/identity-service/IdentityService.Web/IdentityService.Web.csproj", "Services/identity-service/IdentityService.Web/"]
COPY ["Services/identity-service/IdentityService.Tests/IdentityService.Tests.csproj", "Services/identity-service/IdentityService.Tests/"]

# 3. Restore dependencies
# Chỉ cần restore tại dự án Web vì nó tham chiếu đến tất cả các tầng còn lại
RUN dotnet restore "Services/identity-service/IdentityService.Web/IdentityService.Web.csproj"

# 4. Copy mã nguồn thực tế
# Lưu ý: Chúng ta copy toàn bộ folder để giữ nguyên cấu trúc cho dotnet build
COPY ["Services/identity-service/", "Services/identity-service/"]
COPY ["EventContracts/", "EventContracts/"]

# 5. Build ứng dụng
WORKDIR "/src/Services/identity-service/IdentityService.Web"
RUN dotnet publish "IdentityService.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
# Port mặc định của .NET 10 là 8080
EXPOSE 8080 

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.Web.dll"]