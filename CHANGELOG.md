# CHANGELOG

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
