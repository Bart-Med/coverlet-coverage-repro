## Option 1: Using coverlet.msbuild

Before running, uncomment the coverlet.msbuild package reference in `Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj`:

```xml
<PackageReference Include="coverlet.msbuild" Version="6.0.4" />
```

Run the following commands:

```bash
# Clean the project
dotnet clean Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configuration Debug

# Restore packages
dotnet restore Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configfile NuGet.config --verbosity Minimal /p:Configuration=Debug /p:UsePackageReference=true /p:UseExternalPackageReference=true

# Build the project
dotnet build Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configuration Debug --no-restore /p:UsePackageReference=true /p:UseExternalPackageReference=true

# Run tests with code coverage
dotnet test Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --logger trx --configuration Debug --no-restore /p:UsePackageReference=true /p:UseExternalPackageReference=true --settings coverlet.runsettings /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

## Option 2: Using coverlet.collector

Before running, uncomment the coverlet.collector package reference in `Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj`:

```xml
<PackageReference Include="coverlet.collector" Version="6.0.4" />
```

`coverlet.runsettings`:
```xml
<DataCollectionRunSettings>
...
</DataCollectionRunSettings>
```

Run the following commands:

```bash
# Clean the project
dotnet clean Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configuration Debug

# Restore packages
dotnet restore Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configfile NuGet.config --verbosity Minimal /p:Configuration=Debug /p:UsePackageReference=true /p:UseExternalPackageReference=true

# Build the project
dotnet build Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configuration Debug --no-restore /p:UsePackageReference=true /p:UseExternalPackageReference=true

# Run tests with code coverage
dotnet test Demo.Api.IntegrationTests/Demo.Api.IntegrationTests.csproj --configuration Debug --no-restore /p:UsePackageReference=true /p:UseExternalPackageReference=true --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

## Note: Test Concurrency and Code Coverage

`DemoApiIntegrationTestsStartup.cs`:

```csharp
[assembly: CollectionBehavior(MaxParallelThreads = 1)]
```

This forces tests to run sequentially, which "resolves" code coverage accuracy issues.

