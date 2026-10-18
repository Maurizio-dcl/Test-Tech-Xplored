## Armed & Adventurous

A game like Armed & Adventurous would be a scaled-up version of this ToBeKilled example project.
Even tho the scale would be much greater I believe the same concepts I applied to this project 
to allow flexibility:

- Type-Object pattern for separation between configuration and runtime instances (Aka ScriptableObjects)
- Component pattern (over inheritance/polymorphism) to compose functionality
- Dependency injection pattern

Could be used to achieve it, avoiding the bottlenecks of too tighly coupled code.

I would keep the same idea of having a game loop to handle the phase and systems nested state-machines
to allow composability and easy reorder/addition/removal of systems.

Systems would need to be kept as focused as possible on single responsabilities.

For example, an equip system should be sliced into different systems as atomically as possible (or needed,
there is a limit to how far you want something to be flexible and composable):

- a system to handle physical equipping/unequipping of items
- items would have composable effects (much like the runtime `Actor` and its whole hierarchy of definitions/configurations)
- a system to separately handle buff/debuffs (either coming from equipped items, actions, events, etc. I think this project shows how you can completely decouple all of these things)

An inventory system would follow the same philosophy.

The map in this game would also be more complex than the one in this project.

`Tile`s should be extended to have walls and so on, but I think well-designed composability would allow
a flexible destruction/creation system for walls as well.

I would essentially keep extending the architecture shown in this project going with more atomic configuration + composability,
extending systems and adding much more focused ones (the more atomic the better in this case).