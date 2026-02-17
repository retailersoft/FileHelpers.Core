# FileHelpers.Core NuGet Package Publishing Guide

This guide will help you publish the FileHelpers.Core library as a NuGet package.

## Prerequisites

1. **NuGet Account**: Create an account at [nuget.org](https://www.nuget.org/) if you don't have one
2. **API Key**: Generate an API key from your NuGet account
   - Go to https://www.nuget.org/account/apikeys
   - Click "Create" and give it a name (e.g., "FileHelpers.Core Upload")
   - Set the glob pattern to `FileHelpers.Core*`
   - Copy the generated API key (you'll only see it once!)

## Step 1: Update Package Metadata (Optional)

The project file already contains good metadata. You may want to update it to reflect your fork:

```bash
cd C:\GitHub\FileHelpers.Core\FileHelpers.Core
code FileHelpers.Core.csproj
```

Key properties to consider updating:
- `<Version>` - Currently 9.0.4, increment if making changes
- `<RepositoryUrl>` - Currently points to MarcosMeli/FileHelpers.git, consider updating to retailersoft/FileHelpers.Core
- `<Authors>` - Add your organization name if desired
- `<PackageId>` - Consider changing to `RetailerSoft.FileHelpers.Core` to avoid conflicts

## Step 2: Build the Package

The package has already been created, but here's the command for future reference:

```bash
cd C:\GitHub\FileHelpers.Core
dotnet pack FileHelpers.Core\FileHelpers.Core.csproj -c Release -o ./nupkg
```

This creates: `nupkg\FileHelpers.Core.9.0.4.nupkg`

## Step 3: Test the Package Locally (Recommended)

Before publishing, test the package in your Margin Master project:

```bash
# Add a local package source
dotnet nuget add source C:\GitHub\FileHelpers.Core\nupkg --name FileHelpersLocal

# In your Margin Master project, add the package reference
cd C:\GitHub\MarginMaster.Core\MarginMaster.UI.WPF
dotnet add package FileHelpers.Core --version 9.0.4 --source FileHelpersLocal
```

## Step 4: Publish to NuGet.org

### Option A: Using dotnet CLI (Recommended)

```bash
cd C:\GitHub\FileHelpers.Core

# Set your API key (only needed once)
dotnet nuget setapikey YOUR_API_KEY_HERE --source https://api.nuget.org/v3/index.json

# Push the package
dotnet nuget push nupkg\FileHelpers.Core.9.0.4.nupkg --source https://api.nuget.org/v3/index.json
```

### Option B: Using NuGet CLI

```bash
cd C:\GitHub\FileHelpers.Core

# If you don't have nuget.exe, download it first
# https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

nuget setApiKey YOUR_API_KEY_HERE
nuget push nupkg\FileHelpers.Core.9.0.4.nupkg -Source https://api.nuget.org/v3/index.json
```

### Option C: Manual Upload via Web

1. Go to https://www.nuget.org/packages/manage/upload
2. Click "Browse" and select `C:\GitHub\FileHelpers.Core\nupkg\FileHelpers.Core.9.0.4.nupkg`
3. Click "Upload"
4. Review the package details and click "Submit"

## Step 5: Verify Publication

After publishing (it may take a few minutes to index):

1. Visit https://www.nuget.org/packages/FileHelpers.Core
2. Verify the version appears in the list
3. Check that the package details are correct

## Step 6: Use in Margin Master Project

Once published, update your MarginMaster.Common project:

```bash
cd C:\GitHub\MarginMaster.Core\MarginMaster.Common
dotnet add package FileHelpers.Core --version 9.0.4
```

Or manually edit `MarginMaster.Common.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="FileHelpers.Core" Version="9.0.4" />
</ItemGroup>
```

## Alternative: Publish to Azure Artifacts (Private Feed)

If you prefer to keep this package private within your organization:

### Step 1: Create Azure Artifacts Feed

1. Go to Azure DevOps
2. Navigate to Artifacts
3. Create a new feed (e.g., "RetailerSoft-Packages")

### Step 2: Push to Azure Artifacts

```bash
# Add the Azure Artifacts source
dotnet nuget add source https://pkgs.dev.azure.com/{organization}/_packaging/{feed}/nuget/v3/index.json --name AzureArtifacts

# Push the package
dotnet nuget push nupkg\FileHelpers.Core.9.0.4.nupkg --source AzureArtifacts --api-key az
```

## Alternative: Use as Local Package Reference

If you don't want to publish at all, you can reference it as a project:

1. Add FileHelpers.Core solution to your workspace
2. Add project reference in MarginMaster.Common.csproj:

```xml
<ItemGroup>
  <ProjectReference Include="..\..\FileHelpers.Core\FileHelpers.Core\FileHelpers.Core.csproj" />
</ItemGroup>
```

## Troubleshooting

### Package ID Conflicts

If `FileHelpers.Core` is already taken on NuGet.org, you'll need to:

1. Update the `<PackageId>` in FileHelpers.Core.csproj:
   ```xml
   <PackageId>RetailerSoft.FileHelpers.Core</PackageId>
   ```

2. Rebuild the package:
   ```bash
   dotnet pack FileHelpers.Core\FileHelpers.Core.csproj -c Release -o ./nupkg
   ```

3. Use the new package name in your projects:
   ```bash
   dotnet add package RetailerSoft.FileHelpers.Core
   ```

### Build Warnings

The project builds with 28 warnings (mostly XML documentation related). These are non-critical and don't affect functionality.

### Strong Name Signing

The project is configured for strong-name signing with `FileHelpers.snk`. Make sure this key file exists in the project directory.

## Recommended Next Steps

1. **Version Strategy**: Decide on versioning scheme (e.g., 9.0.4 -> 9.0.5 for patches)
2. **Release Notes**: Update package release notes for each version
3. **Documentation**: Consider adding a README.md to the package
4. **CI/CD**: Set up automated builds and publishing with GitHub Actions or Azure DevOps

## GitHub Actions Example (Optional)

Create `.github/workflows/publish-nuget.yml`:

```yaml
name: Publish NuGet Package

on:
  push:
    tags:
      - 'v*'

jobs:
  publish:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      
      - name: Restore dependencies
        run: dotnet restore FileHelpers.Core/FileHelpers.Core.csproj
      
      - name: Build
        run: dotnet build FileHelpers.Core/FileHelpers.Core.csproj -c Release --no-restore
      
      - name: Pack
        run: dotnet pack FileHelpers.Core/FileHelpers.Core.csproj -c Release -o nupkg
      
      - name: Publish to NuGet
        run: dotnet nuget push nupkg/*.nupkg --api-key ${{ secrets.NUGET_API_KEY }} --source https://api.nuget.org/v3/index.json
```

## Questions?

For more information, see:
- [Microsoft NuGet Documentation](https://docs.microsoft.com/en-us/nuget/)
- [FileHelpers Official Site](http://www.filehelpers.net)
