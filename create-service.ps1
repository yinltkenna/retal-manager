param (
    [string]$SolutionName = "rentalManagment.slnx"
)

# ==============================
# INPUT SERVICE NAME
# ==============================

$inputName = Read-Host "Enter service name (e.g. Billing, Core, Identity)"

if ([string]::IsNullOrWhiteSpace($inputName)) {
    Write-Host "Service name cannot be empty."
    exit
}

# Normalize name
$serviceName = ($inputName.Substring(0,1).ToUpper() + $inputName.Substring(1))
$serviceFolder = ($serviceName.ToLower()) + "-service"

$apiProjectName = "$serviceName`Service"
$testProjectName = "$serviceName`Service.Tests"

# ==============================
# PATH CONFIG
# ==============================

$rootPath = Get-Location
$basePath = Join-Path $rootPath "Services\$serviceFolder"
$apiPath = Join-Path $basePath $apiProjectName
$testPath = Join-Path $basePath $testProjectName

Write-Host ""
Write-Host "Creating service: $serviceName"
Write-Host "Location: $basePath"
Write-Host ""

# ==============================
# HELPER FUNCTION
# ==============================

function Create-Folder($path) {
    if (Test-Path $path) {
        Write-Host "[EXISTS] $path"
    }
    else {
        New-Item -ItemType Directory -Path $path | Out-Null
        Write-Host "[CREATED] $path"
    }
}

# ==============================
# CREATE ROOT FOLDER
# ==============================

Create-Folder $basePath

# ==============================
# CREATE API PROJECT
# ==============================

if (-not (Test-Path $apiPath)) {
    dotnet new webapi -n $apiProjectName -f net10.0 -o $apiPath
    Write-Host "[CREATED] API project"
}
else {
    Write-Host "[EXISTS] API project"
}

# ==============================
# CREATE TEST PROJECT
# ==============================

if (-not (Test-Path $testPath)) {
    dotnet new xunit -n $testProjectName -f net10.0 -o $testPath
    Write-Host "[CREATED] Test project"
}
else {
    Write-Host "[EXISTS] Test project"
}
# ==============================
# CREATE FOLDER STRUCTURE (Organized by Layers)
# ==============================

$folders = @(
    # --- Tầng Domain (Chứa cốt lõi) ---
    "src\Domain\Entities",
    "src\Domain\Enums",
    "src\Domain\Interfaces",

    # --- Tầng Infrastructure (Chứa kết nối ngoại vi) ---
    "src\Infrastructure\Data",             # Thay cho DbContext
    "src\Infrastructure\Repositories",
    "src\Infrastructure\Caching",
    "src\Infrastructure\Messaging",
    "src\Infrastructure\Messaging\Publishers",
    "src\Infrastructure\Messaging\Consumers",
    "src\Infrastructure\Messaging\Outbox",

    # --- Tầng Application (Chứa Logic nghiệp vụ) ---
    "src\Application\Services",
    "src\Application\Interfaces",
    "src\Application\DTOs",
    "src\Application\DTOs\Requests",
    "src\Application\DTOs\Responses",
    "src\Application\Mappings",

    # --- Tầng Web/Presentation (Chứa API & Cấu hình) ---
    "src\Web\Controllers",
    "src\Web\Middlewares",
    "src\Web\Configurations",
    "src\Web\Common",
    "src\Web\Common\Helpers",
    "src\Web\Common\Constants",
    "src\Web\Common\Extensions"
)

foreach ($folder in $folders) {
    Create-Folder (Join-Path $apiPath $folder)
}

# ==============================
# ADD TO SOLUTION
# ==============================

$solutionPath = Join-Path $rootPath $SolutionName

if (Test-Path $solutionPath) {

    $apiProjFile = Join-Path $apiPath "$apiProjectName.csproj"
    $testProjFile = Join-Path $testPath "$testProjectName.csproj"

    $slnList = dotnet sln $solutionPath list

    if ($slnList -notmatch $apiProjectName) {
        dotnet sln $solutionPath add $apiProjFile
        Write-Host "[ADDED] API project added to solution"
    }
    else {
        Write-Host "[EXISTS] API project already in solution"
    }

    if ($slnList -notmatch $testProjectName) {
        dotnet sln $solutionPath add $testProjFile
        Write-Host "[ADDED] Test project added to solution"
    }
    else {
        Write-Host "[EXISTS] Test project already in solution"
    }

}
else {
    Write-Host "[WARNING] Solution file not found: $SolutionName"
}

Write-Host ""
Write-Host "Service creation completed."