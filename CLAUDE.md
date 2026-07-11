# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Backports .NET 6's `System.DateOnly` and `System.TimeOnly` to earlier .NET targets, plus a `System.Text.Json` converter package for them. Two NuGet packages, two projects:

- **DateTimeOnly** → `Portable.System.DateTimeOnly` — the `DateOnly`/`TimeOnly` types themselves.
- **DateTimeOnly.Json** → `Portable.System.DateTimeOnly.Json` — `System.Text.Json` converters for those types.

Types live under `namespace System` / `namespace System.Text.Json` (matching BCL namespaces exactly), so source in this repo often shadows real framework types — pay attention to which TFM a file is compiled under.

## Commands

```
dotnet build                                    # build all projects (Debug)
dotnet build -c Release                         # release build (as CI does)
dotnet test                                      # run all tests (DateTimeOnly.Tests, net481 only)
dotnet test --filter "FullyQualifiedName~DateOnlyTests"   # run one test class
dotnet test --filter "DisplayName~SomeTestName"            # run one test method
dotnet tool restore                              # required once, before dotcover
dotnet dotcover test --dcXML=Configuration.xml   # coverage run, same as CI (JetBrains dotCover CLI)
dotnet restore --force-evaluate                  # regenerate packages.lock.json (needed after bumping a PackageReference)
```

Tests are xUnit and currently only target `net481`, so they must be run on Windows (or under `dotnet test` with the Windows desktop targeting pack available).

## Architecture

### Multi-targeting trick: real types vs. backported types

`DateTimeOnly.csproj` targets `netstandard2.0;netstandard2.1;net462;net6.0`. The `net6.0` TFM already has real `System.DateOnly`/`System.TimeOnly` in the BCL, so for that TFM the csproj excludes every source file except `TypeForwarding.cs`, which uses `[assembly: TypeForwardedTo(...)]` to forward to the framework's own types. All other TFMs compile the full backported implementation (`DateOnly.cs`, `TimeOnly.cs`, `Helpers/`). When editing the actual type implementation, remember `net6.0` never sees that code — don't expect a `net6.0` build to exercise it.

`DateTimeOnly.Json.csproj` targets `netstandard2.0;net8.0`. Unlike the main project, this one uses `#if NET8_0_OR_GREATER` preprocessor branches (not MSBuild item excludes) inside files like `DateOnlyConverterAttribute.cs` to pick between the BCL's built-in `JsonMetadataServices.DateOnlyConverter` (net8+) and the backported `DateOnlyConverter` (else). Both branches compile in both configurations conceptually, so check both `#if` arms when changing converter selection logic.

### Parsing strategy (double-parse vs. fast-parse)

`Helpers/DateTimeParse.cs` and `Helpers/DateTimeFormat.cs` implement parsing that matches .NET 6's exact `FormatException` behavior (e.g. rejecting a time part in `DateOnly.Parse`). Because porting the internal BCL parser wasn't practical, correctness is achieved by parsing twice with `DateTime.TryParseExact` using explicit format strings. An `AppContext` switch, `Portable.System.DateTimeOnly.UseFastParsingLogic` (see `DateTimeParse.cs`), reverts to the old single-pass `DateTime`-based parsing for callers who want speed over strict format compatibility. `FastStrictParsingLogicTests.cs` exercises both modes — when touching parsing, run tests with the switch both on and off.

### Source-generator workaround for JSON

`System.Text.Json` source generators choke if a type named `DateOnly`/`TimeOnly` conflicts with the BCL ones in the object graph. `JsonDateOnly.cs`/`JsonTimeOnly.cs` provide differently-named wrapper structs (`System.Text.Json.JsonDateOnly` / `JsonTimeOnly`) with implicit conversions to/from the real types and a `[JsonConverter]` attribute pointing at delegating converters (`JsonDateOnlyConverter`, etc.) — use these wrapper types in any DTO that goes through source-generated `JsonSerializerContext`, not the attribute-based converters.

### Conditional test skipping

`DateTimeOnly.Tests/Helpers/ConditionalFactAttribute.cs` + `ConditionalFactDiscoverer.cs` implement a custom xUnit discoverer: `[ConditionalFact(nameof(SomeStaticBoolProperty))]` skips the test at discovery time if the named static property returns `false`, wrapping it in `SkippedTestCase`. Used for tests that only make sense under specific runtime/AppContext conditions.

### Lock files

Both library projects use `RestorePackagesWithLockFile` + `RestoreLockedMode`, so `packages.lock.json` must stay in sync with `PackageReference`s or restore fails. `.github/workflows/lockfiles.yml` auto-regenerates and commits lock files on Dependabot PRs via `dotnet restore --force-evaluate` — do the same locally after manually bumping a dependency version.

### Strong naming & versioning

Both packages are strong-named with `DateTimeOnly.snk`. `DateTimeOnly.Json` takes a `ProjectReference` on `DateTimeOnly` for non-`net6.0` TFMs (falls back to real BCL types on `net6.0`). Package versions/release notes are hand-maintained in each `.csproj`'s `PropertyGroup` (`AssemblyVersion`, `FileVersion`, `Version`, `PackageReleaseNotes`) — bump these when preparing a release. CI (`release.yml`) publishes to NuGet on tags matching `v*` (main package) or `j*` (Json package), only from the `OlegRa/System.DateTimeOnly` repo.
