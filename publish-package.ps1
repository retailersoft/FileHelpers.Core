# Quick Publish Script for FileHelpers.Core
# Run this script to build and publish the NuGet package

Write-Host "FileHelpers.Core NuGet Publishing Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = "C:\GitHub\FileHelpers.Core"
$projectFile = "$projectPath\FileHelpers.Core\FileHelpers.Core.csproj"
$outputPath = "$projectPath\nupkg"

# Step 1: Check if project exists
if (-not (Test-Path $projectFile)) {
    Write-Host "ERROR: Project file not found at $projectFile" -ForegroundColor Red
    exit 1
}

Write-Host "[1/4] Cleaning previous builds..." -ForegroundColor Yellow
Remove-Item -Path "$projectPath\FileHelpers.Core\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "$projectPath\FileHelpers.Core\obj" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path $outputPath -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $outputPath -Force | Out-Null

# Step 2: Build
Write-Host "[2/4] Building project..." -ForegroundColor Yellow
$buildResult = dotnet build $projectFile -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed" -ForegroundColor Red
    exit 1
}
Write-Host "Build successful!" -ForegroundColor Green

# Step 3: Pack
Write-Host "[3/4] Creating NuGet package..." -ForegroundColor Yellow
$packResult = dotnet pack $projectFile -c Release -o $outputPath --no-build
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Pack failed" -ForegroundColor Red
    exit 1
}

# Find the created package
$package = Get-ChildItem -Path $outputPath -Filter "*.nupkg" | Select-Object -First 1
if (-not $package) {
    Write-Host "ERROR: No package file found" -ForegroundColor Red
    exit 1
}

Write-Host "Package created: $($package.Name)" -ForegroundColor Green
Write-Host "Package location: $($package.FullName)" -ForegroundColor Green

# Step 4: Publishing options
Write-Host ""
Write-Host "[4/4] Publishing Options:" -ForegroundColor Yellow
Write-Host ""
Write-Host "Choose a publishing method:" -ForegroundColor Cyan
Write-Host "  1. Publish to NuGet.org (requires API key)" -ForegroundColor White
Write-Host "  2. Test locally in MarginMaster project" -ForegroundColor White
Write-Host "  3. Skip publishing (just create package)" -ForegroundColor White
Write-Host "  4. Show manual upload instructions" -ForegroundColor White
Write-Host ""

$choice = Read-Host "Enter your choice (1-4)"

switch ($choice) {
    "1" {
        Write-Host ""
        Write-Host "Publishing to NuGet.org..." -ForegroundColor Cyan
        
        # Check if API key is stored in environment variable
        $apiKey = $env:NUGET_API_KEY
        
        if ([string]::IsNullOrWhiteSpace($apiKey)) {
            $apiKey = Read-Host "Enter your NuGet API key (or press Enter to cancel)"
        } else {
            Write-Host "Using API key from environment variable NUGET_API_KEY" -ForegroundColor Green
        }
        
        if ([string]::IsNullOrWhiteSpace($apiKey)) {
            Write-Host "Cancelled." -ForegroundColor Yellow
        } else {
            Write-Host "Pushing package to NuGet.org..." -ForegroundColor Yellow
            dotnet nuget push $package.FullName --source https://api.nuget.org/v3/index.json --api-key $apiKey
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Successfully published to NuGet.org!" -ForegroundColor Green
                Write-Host "Note: It may take a few minutes for the package to appear in search." -ForegroundColor Yellow
            } else {
                Write-Host "Publishing failed. Check the error message above." -ForegroundColor Red
            }
        }
    }
    
    "2" {
        Write-Host ""
        Write-Host "Setting up local testing..." -ForegroundColor Cyan
        
        # Add local source if not exists
        $sourceName = "FileHelpersLocal"
        $sourceExists = dotnet nuget list source | Select-String $sourceName
        
        if (-not $sourceExists) {
            Write-Host "Adding local package source..." -ForegroundColor Yellow
            dotnet nuget add source $outputPath --name $sourceName
        }
        
        Write-Host ""
        Write-Host "Local package source configured!" -ForegroundColor Green
        Write-Host ""
        Write-Host "To use in MarginMaster.Common, run:" -ForegroundColor Cyan
        Write-Host "  cd C:\GitHub\MarginMaster.Core\MarginMaster.Common" -ForegroundColor White
        Write-Host "  dotnet add package $($package.BaseName) --source $sourceName" -ForegroundColor White
        Write-Host ""
        Write-Host "Or add manually to MarginMaster.Common.csproj:" -ForegroundColor Cyan
        Write-Host "  <PackageReference Include=`"$($package.BaseName)`" Version=`"9.0.5`" />" -ForegroundColor White
    }
    
    "3" {
        Write-Host ""
        Write-Host "Package created successfully. No publishing action taken." -ForegroundColor Green
    }
    
    "4" {
        Write-Host ""
        Write-Host "Manual Upload Instructions:" -ForegroundColor Cyan
        Write-Host "1. Go to https://www.nuget.org/packages/manage/upload" -ForegroundColor White
        Write-Host "2. Sign in to your NuGet account" -ForegroundColor White
        Write-Host "3. Click 'Browse' and select this file:" -ForegroundColor White
        Write-Host "   $($package.FullName)" -ForegroundColor Yellow
        Write-Host "4. Click 'Upload' and follow the prompts" -ForegroundColor White
        Write-Host ""
        Write-Host "The package file has been copied to your clipboard for easy pasting." -ForegroundColor Green
        Set-Clipboard -Value $package.FullName
    }
    
    default {
        Write-Host "Invalid choice. Package created but not published." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Process completed!" -ForegroundColor Green
Write-Host "Package location: $($package.FullName)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Tip: To avoid entering your API key each time, run:" -ForegroundColor Yellow
Write-Host '  $env:NUGET_API_KEY = "your-api-key-here"' -ForegroundColor Cyan
Write-Host "For more details, see: C:\GitHub\FileHelpers.Core\PUBLISHING_GUIDE.md" -ForegroundColor Cyan
