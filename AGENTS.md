# AGENTS.md

## Project

UIComponents is a source-generation-driven front-end framework for Unity's UI Toolkit.
It provides declarative, attribute-based component authoring with automatic layout/stylesheet loading,
dependency injection, DOM querying, and event registration.

**Package:** `io.savolainen.uicomponents`

## Repository Structure

```
Assets/UIComponents/              # Unity package (runtime code)
  Core/                           # UIComponent base class, attributes, DI, asset resolver
  Addressables/                   # AddressableAssetResolver (optional)
  Editor/                         # Editor utilities
  Roslyn/                         # Pre-built Roslyn DLLs consumed by Unity
  Testing/                        # TestBed<T>

Assets/UIComponents.Tests/        # Unity-side tests (NUnit, NSubstitute)

UIComponents.Roslyn/              # Roslyn solution (source generators + analyzers)
  UIComponents.Roslyn.Generation/ # Source generators
  UIComponents.Roslyn.Analyzers/  # Diagnostic analyzers + code fixes
  UIComponents.Roslyn.Common/     # Shared Roslyn utilities
  *.Tests/                        # xUnit + Verify snapshot tests

Assets/Samples/                   # Package samples
```

## Building and Testing

### Roslyn solution

```bash
dotnet build UIComponents.Roslyn/UIComponents.Roslyn.sln
dotnet test UIComponents.Roslyn/UIComponents.Roslyn.sln
```

The test projects target `net6.0`. Snapshot tests use the Verify library — new tests produce `.received.cs` files that must be renamed to `.verified.cs` after review.

### After changing generators or analyzers

Rebuild in Release and copy DLLs to the Unity package:

```bash
dotnet build UIComponents.Roslyn/UIComponents.Roslyn.sln -c Release
cp UIComponents.Roslyn/UIComponents.Roslyn.Generation/bin/Release/netstandard2.0/UIComponents.Roslyn.Generation.dll Assets/UIComponents/Roslyn/
cp UIComponents.Roslyn/UIComponents.Roslyn.Common/bin/Release/netstandard2.0/UIComponents.Roslyn.Common.dll Assets/UIComponents/Roslyn/
```

### Unity tests

Unity tests require the Unity Editor test runner. They cannot be run from CLI. The CI pipeline uses `game-ci/unity-test-runner`.

## Key Conventions

### Unity baseline

Unity 2022.3 LTS is the baseline. Do not use APIs unavailable in 2022.3. CI also tests on 2021.3 and Unity 6.

### Source generators

Generators use the `ISourceGenerator` API (not incremental generators) targeting `netstandard2.0`.
Each generator extends `UIComponentAugmentGenerator` → `AugmentGenerator<ClassSyntaxReceiver>`.

Generated methods follow the `UIC_` prefix convention:
- `UIC_StartLayoutLoad()` — layout loading
- `UIC_StartStyleSheetLoad()` — stylesheet loading
- `UIC_PopulateQueryFields()` — query population
- `UIC_PopulateProvideFields()` — DI field injection
- `UIC_ApplyEffects()` — effect application
- `UIC_RegisterEventCallbacks()` — events

### Asset loading conventions

`[Layout]` and `[Stylesheet]` support two forms:
- Explicit path: `[Layout("path/to/file")]` — path fed directly to resolver
- Convention (parameterless): `[Layout]` — class name used as path; `[Stylesheet]` → `ClassName.style`

Convention paths use the declaring type's name for inheritance. `[AssetPrefix]` prepends to both forms. `[SharedStylesheet("name")]` loads shared stylesheets by explicit name.

### Test patterns

- **Roslyn tests:** xUnit snapshot tests with Verify. Test source is inline strings compiled via `GeneratorTester.Verify<T>()`. Test stubs in `Resources/` mirror runtime attributes.
- **Unity tests:** NUnit with `[UnityTest]` returning `IEnumerator`. Use `TestBed<T>` for DI, `NSubstitute` for mock resolver/logger. Layout tests use `.WithSingleton(_mockResolver)`, stylesheet tests use `.WithSingleton(_mockLogger).WithTransient(_mockResolver)`.

### Commits

- Run tests before committing. Every commit should be self-contained with tests covering new behavior.
- Include `.meta` files when adding new files under `Assets/`. Do NOT create them yourself: let the Unity editor do it.
- Keep commit messages concise.
