## ToBeKilled

In this project I wanted to implement a playable version of the ToBeKilled game, focusing on a scalable
architecture to allow:

- easy configuration of all the parts of the game even by non-coding designers, 
using the Type-Object pattern (aka ScriptableObjects)
- complex game logic but ease of maintenance, through the use of the Component pattern and dependency
injection

Here I give an explanation of the general architecture of the project, and under the `Assets/ArchitectureAnalysis`
I give my design suggestions for the whole architecture defined in the provided `test_tech_Xplored.md` instructions.

## Development Notes

There might be some improvements here and there in the architecture, like some allocations and closures that could
be easily avoided with caching, visibility improvements for properties/methods and some more abstraction
could be implemented in phases (I noticed while developing that phases might abstract away a lot of common
logic). Also in the `Map` APIs I left some naive-search methods, these could be removed and the flood-fill
method could be used for their purpose as well, since it offers better scalability with many actors.

There are also 2 functionalities I did not get to implement due to time constraints:

### Boss AI
The boss in the game is controlled by the player (which demonstrates flexibility I guess!).
This could be very easily implemented as a dedicated `IActorInput` input source as explained
in `ArchitectureAnalysis/Inputs/Inputs.md`, and it would work right-away, it is just time-consuming
but does not require any refactor.

### Focus Action

In this architecture a focus action would be implemented as some sort of "DiceRollModifierActionDefinition".
When executed it will grant the buff, that could be represented as an element of a list of buffs on the
actor instance itself.
The rolling system could be modified to accept (or look for) this types of buffs when rolling.

## Architecture

### GameLoop
`GameLoop` controls both the bootstrap phase and base game loop.
Dependency injection is used to manage instancing of the main objects of the game, basically `Actor`, `Action`,
`ITurnPhase` and `I*System`, injecting them with definition/configuration files and all the other needed dependencies.
Also, this makes it easier to swap one instance with another, one example is the use of the decorator
pattern for analytics in systems (see `ArchitectureAnalysis/Analytics/Analytics.md`)

The `GameLoop` handles turns (really just an int), phases and specific systems inside phases with a nested
state machine system:

`TurnDirector > Phase(s) > System(s)`

And the state machine themselves pass around context objects with all the needed dependencies to make
systems (and the objects they work with) work.

Systems are the one concept driving the actual game logic, but try to be as independent as they can from
the objects they control, one example is the very simple `ActionExecuteSystem` which physically executes
`Action`(s) knowing nothing about their implementation.

In turn runtime instances of the main objects of the game (i.e. `Actor`, which represents characters, and `Action` which 
represents actions of course) are decoupled from their configuration values.
I tried to encapsulate in the configuration/definition ScriptableObjects all the parameters a designer
would like to tweak about a specific object.

I draw a line when it comes to configuration in the actual action-specific logic, which in my architecture
is defined in each `*ActionDefinition`, this is because a full-fledged configurable expression system
that allows the flexibility of writing either "1 + 1D6 damage" and "1D6 for each hero alive" or "2 for
each tile moved this turn" is really complex to define and I thought was a stretch trying to implement it in 
this example project. I think this is good trade-off between ease of configuration and flexibility.

UI is not particularly nice or very user friedly, again my focus was on scalability of the architecture rather
than polish, so this is another tradeoff I made. 

A basic explanation of the game loop and controls:

1. each turn the player is given control of the actor which can be moved by selecting a tile in range (green).
2. movement phase can be ended at any time with the dedicated button, otherwise it is done automatically when
the actor cannot move further.
3. during the action phase the player can select actions, select targets if multiple are available, and confirm them or 
cancel the action with 2 dedicated buttons (action phase does not end automatically if no actions are available/usable, 
this could be improved)
4. repeat

By the way the console logs every move with details to better explain the gameplay. 
(see `Assets/ArchitectureAnalysis/Analytics/Analytics.md`)