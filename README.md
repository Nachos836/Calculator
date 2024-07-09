# Calculator
Unity3D Calculator App with only the ability to add integers. But with a big architectural under-hood

## Main goals of this project
1. Strictly establish Clear Architecture without workarounds and hacks
2. Implement as much as reasonable via TDD
3. Provide Unit and Functional tests — they always should pass
4. Follow major good programming practices
5. Make use of Railway Oriented Programming with a grain of Functional Style (all logic branches must be explicit)
6. Unity Solution should be clean and not produce garbage on the start|run|first build
7. Project should be as Reproducible (reproducing builds support) as possible
8. Strictly follow git flow
9. Strictly follow Semantic Versioning
10. Strictly follow Keep a Changelog
11. The project should be created in a fashion as if it real production project and maintained by the team
12. All changes must have a task at the Kanban Board (provided by GitHub Project)
13. Every merge produces a release
14. Every release has a build
15. Avoid plugins and external dependencies in the Asset folder
16. Prefer external dependencies via OpenUMP and NuGet for Unity
17. All code should follow IOC principle
18. All MonoBehaviours should be internal
19. Only contract-code should be public (tests shall access internals using assembly attributes)
20. The project should use Addressables for resources and scene management
21. Prefer reasonable naming and stuff placement over traditional one (Scrips folder is useless))

### HOWTO:
1. In the root of the `Assets` folder is placed `Calculator-Initial` — the only starting scene
2. If you want to, you could initialize any other scene
>> In Inspector override `Main Scene` field value in `pref.scope_root_01` in the `Calculator-Initial` to any other scene you may wish
3. Execution started at `ApplicationEntryPoint.cs`
>> Located in `Assets`/`Architecture`/`Presentation`
4. DI Flow started in 'RootScope.cs'
>> Located in `Assets`/`Architecture`/`Presentation`/`DI`/`Root`
5. Scopes are Parent-Child related and must define this relation via Inspector
>> Scopes prefab naming schema is "pref.scope_*". Part of the configuration is done in prefab, the other — on scenes via overrides
