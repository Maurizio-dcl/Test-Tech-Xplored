## Armed & Adventurous

A game like Armed & Adventurous would be a scaled-up version of this ToBeKilled example project.
I believe that using the same concepts I applied to this project to allow flexibility:

- Type-Object pattern for separation between configuration and runtime instances (Aka ScriptableObjects)
- Component pattern (over inheritance/polymorphism) to compose functionality
- Dependency injection pattern

Could be used to achieve it, avoiding the bottlenecks of too tighly coupled code.

I would keep the same idea of having a game loop to handle the "phases and systems" nested state-machines
to allow composability and easy reorder/addition/removal of systems.

Systems would need to be kept as focused as possible on single responsabilities.

For example, an "equip system" should be sliced into different systems as atomically as possible:

- a system to handle physical equipping/unequipping of items
- items would have composable effects (much like the runtime `Actor` and its whole hierarchy of definitions/configurations)
- a system to separately handle their buff/debuffs (either coming from equipped items, actions, events, etc. 
I think this project shows how you can completely decouple all of these things)

The map in this game would also be more complex than the one in this project, I think a good way of designing it
would be implementing a new  layer on top of map tiles to handle rooms and areas, extending underlying tiles functionality
as well to allow multiple actors/items or "entities" in general to be on them.
Walls could actually be entities (much like the Actors of my project) like any item and such, and use dedicated 
systems to handle addition/removal.

An inventory system, pickup/drop system, spawn system for enemies/rooms, they would all follow the same philosophy,
divide functionality into atomic components driven by systems, only having minimal visibility of what they need to function,
and being completely oblivious of the whole context.