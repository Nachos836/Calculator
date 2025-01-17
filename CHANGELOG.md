# CHANGELOG

## v0.6.2-alpha

### Added:
1. Debug panel (for Debug Builds only)
### Changed:
1. Evaluated list disabled from inspector, not via Awake
2. Event System moved from Main to Initial scene
3. Cleanup views layout
### Fixed:
1. Enable and Instant Disable of Evaluated list
2. Documentation redundant quote symbols

---

## v0.6.1-alpha

### Added:
1. HOWTO section in documentation
2. Motivation part in README.md
3. WIKI documentation
### Changed:
1. Segregate LogLevel Critical and Exceptions
2. Main Scene segregated via prefabs
### Fixed:
1. Calculator Events Stream no longer stuck with ClosedEvent
2. Error Popup processed error only at button press
3. Main Window suffer for late events publishing
### Know Issues:
1. Saved state in the Calculator Build is not fetched initially

---

## v0.6.0-alpha
#### Establishing Presentation Layer

### Added:
1. Presentation Layer
   1. Application Entry Point
      1. Async loader for Main Scene
   2. Initial Scene
   3. Main Scene
      1. Calculator Main Window (MVP)
      2. Error Message Popup (MVP)
      3. View Routing
2. TextMeshPro Resources
3. Infrastructure Addressables Group
### Changed:
1. Updated dependencies
### Removed:
1. Redundant logging in Infrastructure DI
### Know Issues:
1. State preserved with extra error message
2. List of operations lacks latest element
3. Error Popup lacks latest message

---

## v0.5.0-alpha
#### Establishing Infrastructure Layer

### Added:
1. LiteDB as documented database
2. Infrastructure Layer
   1. Calculator Event Sourcing implementation
   2. Independent DI Registration
### Changed:
1. Updated dependencies

---

## v0.4.0-alpha
#### Establishing Application Layer

### Added:
1. Bypass Unity Limitations third party feature
2. Application Layer
   1. Unit Tests
   2. Functional Tests
   3. Calculator contract
   4. Event Sourcing contract
### Changed:
1. Result pattern moved to Third Party
2. Domain and Tests are tuned up to Application
3. Updated dependencies
### Removed:
1. Redundant Result tests in Domain Layer

---

## v0.3.0-alpha
#### Establishing Domain Layer

### Added:
1. Result and Result<T> pattern
   1. Contains Possible Outcomes and process errors
2. Unsigned Operand Value Object
3. Unsigned Binary Operator Value Object
4. Unsigned Operation Aggregate
5. Result initial tests
6. Operator TDD-based Unit Tests
7. Profiling package

---

## v0.2.0-alpha
#### Establishing DI and Application Entry Point

### Added:
1. Application Entry Point
2. DI via VContainer
3. MessagePipe
4. ZLogger
5. .gitattributes rules and enforce LF-endings
6. Root Scope with major app registrations
7. Dedicated Presentation Scope
8. Provide dedicated logging infrastructure via ILogger<T>
### Changed:
1. C# Lang set to the latest (for better logging and source generation)
2. Enforce nullables for whole solution
3. Disable suppression of common issues in Unity (prefer explicitness)
4. Updated Scriptable Build Pipeline

---

## v0.1.0-alpha
#### Establishing Clean Architecture

### Added:
1. Clean Architecture Layers
   1. Domain: no unity dependencies; Value Objects, Entities, Aggregates
   2. Application: no unity dependencies; Business rules and Domain Logic
   3. Infrastructure: optional unity dependencies; Storage, Event Sourcing, State management
   4. Presentation: unity-related; Entry Point, Project Setup, Visuals and User interaction
      1. Contains UnityEditorUtils
2. Third-party stuff
   1. Roslyn-related analyzers and source generators
3. AssetsReserialize feature for Presentation Layer
4. Addressables with Initial setup
   1. Group for Presentation Layer
5. Local CHANGELOG.md
### Changed:
1. SampleScene -> Calculator-Initial
2. Serialized meta-data
3. URP Global Settings asset moved to Settings

---

## v0.0.0-alpha
#### Project Bootstrap

### Added:
1. URP-based Unity 3D project
2. Calc as an origin to namespaces
3. Input System
### Changed:
1. MONO -> IL2CPP
2. Initial version 0.0.0
3. Default 3D API for platforms
4. Disabled incremental GC
### Removed:
1. Version Control
2. Visual Scripting
