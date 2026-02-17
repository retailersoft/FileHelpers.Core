# FileHelpers.Core Publishing - Quick Start

## What Was Done

1. ? Built the FileHelpers.Core project successfully
2. ? Created NuGet package: `FileHelpers.Core.9.0.4.nupkg`
3. ? Created comprehensive publishing guide
4. ? Created automated publishing script
5. ? Prepared updated project file with RetailerSoft branding

## Package Location

The NuGet package is ready at:
```
C:\GitHub\FileHelpers.Core\nupkg\FileHelpers.Core.9.0.4.nupkg
```

## Next Steps - Choose One Option:

### Option 1: Publish to NuGet.org (Public, Recommended)

This makes the package available to everyone via `dotnet add package`.

**Steps:**
1. Run the automated script:
   ```powershell
   cd C:\GitHub\FileHelpers.Core
   .\publish-package.ps1
   ```
2. Choose option 1
3. Enter your NuGet API key when prompted

**OR manually:**
```bash
cd C:\GitHub\FileHelpers.Core
dotnet nuget push nupkg\FileHelpers.Core.9.0.4.nupkg --source https://api.nuget.org/v3/index.json --api-key YOUR_API_KEY
```

**Get API Key:** https://www.nuget.org/account/apikeys

---

### Option 2: Use Locally in Margin Master (Private, No Publishing)

This keeps the package private and only available on your machine.

**Steps:**
```powershell
cd C:\GitHub\FileHelpers.Core
.\publish-package.ps1
```
Choose option 2

**Then in MarginMaster.Common:**
```bash
cd C:\GitHub\MarginMaster.Core\MarginMaster.Common
dotnet add package FileHelpers.Core --version 9.0.4 --source FileHelpersLocal
```

---

### Option 3: Use with Custom Package ID (Avoid Conflicts)

If you want to publish but avoid potential naming conflicts:

**Steps:**
1. Replace the project file:
   ```powershell
   cd C:\GitHub\FileHelpers.Core\FileHelpers.Core
   copy FileHelpers.Core.csproj FileHelpers.Core.csproj.backup
   copy ..\FileHelpers.Core.csproj.new FileHelpers.Core.csproj
   ```

2. Rebuild the package:
   ```bash
   cd C:\GitHub\FileHelpers.Core
   dotnet pack FileHelpers.Core\FileHelpers.Core.csproj -c Release -o ./nupkg
   ```

3. This creates: `RetailerSoft.FileHelpers.Core.9.0.4.nupkg`

4. Publish:
   ```bash
   dotnet nuget push nupkg\RetailerSoft.FileHelpers.Core.9.0.4.nupkg --source https://api.nuget.org/v3/index.json --api-key YOUR_API_KEY
   ```

5. Use in Margin Master:
   ```bash
   cd C:\GitHub\MarginMaster.Core\MarginMaster.Common
   dotnet add package RetailerSoft.FileHelpers.Core --version 9.0.4
   ```

---

## Recommended: Option 3 with Custom Package ID

I recommend **Option 3** because:
- ? Avoids naming conflicts with other FileHelpers packages
- ? Clearly identifies it as your fork
- ? Allows you to publish without coordination with original maintainers
- ? Makes it easy to find your specific version

## After Publishing

Once published to NuGet.org (either option 1 or 3), update MarginMaster.Common:

**Remove the old DLL reference if it exists, then add:**
```xml
<!-- In MarginMaster.Common.csproj -->
<ItemGroup>
  <PackageReference Include="RetailerSoft.FileHelpers.Core" Version="9.0.4" />
  <!-- OR if using option 1: -->
  <PackageReference Include="FileHelpers.Core" Version="9.0.4" />
</ItemGroup>
```

**Remove any direct FileHelpers DLL references:**
```xml
<!-- DELETE these if they exist: -->
<Reference Include="FileHelpers">
  <HintPath>...</HintPath>
</Reference>
```

## Documentation

- **Detailed Guide:** `C:\GitHub\FileHelpers.Core\PUBLISHING_GUIDE.md`
- **Automated Script:** `C:\GitHub\FileHelpers.Core\publish-package.ps1`
- **Updated Project File:** `C:\GitHub\FileHelpers.Core\FileHelpers.Core.csproj.new`

## Troubleshooting

**"Package already exists"**
- The package ID is taken. Use Option 3 with custom package ID.

**"API key required"**
- Get your API key from https://www.nuget.org/account/apikeys

**"Build failed"**
- Run: `dotnet restore` first
- Check that .NET 9 SDK is installed: `dotnet --version`

**"Can't find package after publishing"**
- Wait 5-10 minutes for NuGet.org to index
- Check https://www.nuget.org/packages/YourPackageId

## Questions?

See the comprehensive guide at:
`C:\GitHub\FileHelpers.Core\PUBLISHING_GUIDE.md`
