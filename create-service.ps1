param (
    [string]$SolutionName = "rentalManagment.slnx"
)

$inputName = Read-Host "Enter service name (e.g. Billing, Utility, Identity)"
if ([string]::IsNullOrWhiteSpace($inputName)) { exit }

# 1. Chuẩn hóa tên và Đường dẫn tuyệt đối
$serviceName = ($inputName.Substring(0,1).ToUpper() + $inputName.Substring(1))
$serviceFolder = ($serviceName.ToLower()) + "-service"
$baseNamespace = "$serviceName`Service"

$rootPath = Get-Location
$basePath = Join-Path $rootPath "Services\$serviceFolder"

Write-Host "--- Creating Clean Architecture Service: $serviceName ---" -ForegroundColor Cyan

# 2. Tạo các Project (Domain, Application, Infrastructure là ClassLib; Web là WebAPI)
# Định nghĩa đường dẫn file .csproj trước
$projDomainPath = Join-Path $basePath "$baseNamespace.Domain\$baseNamespace.Domain.csproj"
$projAppPath    = Join-Path $basePath "$baseNamespace.Application\$baseNamespace.Application.csproj"
$projInfraPath  = Join-Path $basePath "$baseNamespace.Infrastructure\$baseNamespace.Infrastructure.csproj"
$projWebPath    = Join-Path $basePath "$baseNamespace.Web\$baseNamespace.Web.csproj"
$projTestPath   = Join-Path $basePath "$baseNamespace.Tests\$baseNamespace.Tests.csproj"

# Thực hiện lệnh tạo project (Dùng Out-Null để không trả về rác văn bản)
dotnet new classlib -n "$baseNamespace.Domain" -f net10.0 -o (Join-Path $basePath "$baseNamespace.Domain") | Out-Null
dotnet new classlib -n "$baseNamespace.Application" -f net10.0 -o (Join-Path $basePath "$baseNamespace.Application") | Out-Null
dotnet new classlib -n "$baseNamespace.Infrastructure" -f net10.0 -o (Join-Path $basePath "$baseNamespace.Infrastructure") | Out-Null
dotnet new webapi -n "$baseNamespace.Web" -f net10.0 -o (Join-Path $basePath "$baseNamespace.Web") | Out-Null
dotnet new xunit -n "$baseNamespace.Tests" -f net10.0 -o (Join-Path $basePath "$baseNamespace.Tests") | Out-Null

Write-Host "[CREATED] 5 Projects created successfully." -ForegroundColor Green

# 3. Thiết lập Tham chiếu chéo (Project References)
Write-Host "Setting up Project References..." -ForegroundColor Yellow
dotnet add $projAppPath reference $projDomainPath
dotnet add $projInfraPath reference $projAppPath
dotnet add $projWebPath reference $projInfraPath
dotnet add $projWebPath reference $projAppPath
dotnet add $projTestPath reference $projAppPath

# 4. Cài đặt Nuget Packages chuẩn xác theo tầng
Write-Host "Installing Nuget Packages..." -ForegroundColor Yellow

# Application Layer
dotnet add $projAppPath package AutoMapper --version 13.0.0

# Infrastructure Layer (Data, Redis, RabbitMQ)
dotnet add $projInfraPath package Microsoft.EntityFrameworkCore --version 10.0.0
dotnet add $projInfraPath package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.0
dotnet add $projInfraPath package StackExchange.Redis --version 2.7.4
dotnet add $projInfraPath package RabbitMQ.Client --version 7.0.0

# Web Layer (Auth, Swagger, EF Tools)
dotnet add $projWebPath package Microsoft.AspNetCore.Authentication.JwtBearer --version 10.0.0
dotnet add $projWebPath package Swashbuckle.AspNetCore --version 6.5.0
dotnet add $projWebPath package Microsoft.EntityFrameworkCore.Design --version 10.0.0
dotnet add $projWebPath package Microsoft.EntityFrameworkCore.Tools --version 10.0.0

# 5. Tạo cấu trúc thư mục nội bộ chi tiết
$projDomain = "$baseNamespace.Domain"
$projApp    = "$baseNamespace.Application"
$projInfra  = "$baseNamespace.Infrastructure"
$projWeb    = "$baseNamespace.Web"

$folders = @(
    "$projDomain\Entities", "$projDomain\Interfaces", "$projDomain\Enums",
    "$projApp\DTOs\Requests", "$projApp\DTOs\Responses", "$projApp\Interfaces", "$projApp\Services", "$projApp\Mappings", "$projApp\Configurations", "$projApp\Definitions", "$projApp\Validator", "$projApp\Common\Helper",
    "$projInfra\Caching", "$projInfra\Data", "$projInfra\Repositories", "$projInfra\Messaging\Publishers", "$projInfra\Messaging\Consumers", "$projInfra\Client",
    "$projWeb\Controllers", "$projWeb\Middlewares", "$projWeb\Common\Helpers", "$projWeb\Common\Constants", "$projWeb\Common\Extensions"
)

foreach ($folder in $folders) {
    $fullPath = Join-Path $basePath $folder
    if (!(Test-Path $fullPath)) { 
        New-Item -ItemType Directory -Path $fullPath | Out-Null 
    }
}

# 6. Thêm vào Solution sử dụng đường dẫn tuyệt đối
$solutionPath = Join-Path $rootPath $SolutionName
if (Test-Path $solutionPath) {
    Write-Host "Adding projects to Solution..." -ForegroundColor Yellow
    # Sử dụng mảng để truyền chính xác 5 đường dẫn
    dotnet sln $solutionPath add $projDomainPath $projAppPath $projInfraPath $projWebPath $projTestPath
    Write-Host "Success: All 5 projects added to $SolutionName" -ForegroundColor Green
}

Write-Host "Service $serviceName created and configured successfully!" -ForegroundColor Green