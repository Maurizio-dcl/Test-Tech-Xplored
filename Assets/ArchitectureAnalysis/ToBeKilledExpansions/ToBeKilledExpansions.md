## ToBeKilled Expansions

The existing approach can also cover expansions, the main change would be to make the bootstrap 
consume collections and extension points instead of one hard-coded boss and fixed system lists.

Replace `BossSpawn` with a list of boss spawn, then have logic to choose which boss/spawns combination
to use based on the map used (I think the physical interface recognizes the map binding it to
a specific boss and therefore spawn configurations). 

Adding bosses is only asset configuration.

## Leveling actions

Keep progression on the runtime `Action` with something like `Level`, `Experience`, and `GainExperience(int)`. 
Put experience thresholds for level-ups and per-level values on `ActionDefinition`. 

Each `ActionDefinition` concrete implementation could define experience gain logic and on execution
drive the experience gain.

This could be composed even more encapsulating experience logic by itself and making it interchangeable,
or decoupled by having a dedicated system for experience running.

## Humanity stat

Add a tag such as `Humanity` to `StatDefinition.StatTag`, create a Humanity `StatDefinition` asset, 
and add a `StatConfiguration` to the required actors. 

`ActionDefinition` could then do `TryGetStat(Humanity, out humanity)`, have logic to drive increase/decrease at action
resolution on whatever actor involved.

Much like leveling, the humanity stat could be used (with thresholds/levels) to change its logic arbitrarily.

## Expansions bootstrapping

This could be done by implementing an abstract `GameExpansionDefinition : ScriptableObject` with something like `Install(GameBuilder builder)`. 
`GameLoop` receives a serialized list of enabled expansion assets, creates the builder with the base spawns/phases/systems, 
calls `Install` on each expansion, and finally builds the `TurnDirector` injecting everything at runtime.

For example, `ExpansionXYZ.Install` could add a new boss' spawn, register an `ActionExperienceSystem` after action execution, 
and add any expansion-specific phase. 

The base game and every expansion use the same small builder API, while `GameLoop` only decides which expansion assets are enabled.
