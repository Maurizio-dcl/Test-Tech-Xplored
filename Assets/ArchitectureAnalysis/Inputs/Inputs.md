## Inputs

The idea behind inputs is routing events through a wrapper `IActorInput`,
rather than consuming UI events directly, and using an `IActorInputResolver` in systems
to resolve different inputs for each actor.

This means that inputs are freely mixable at runtime, and also has the flexibility of allowing,
as in this project example (since I did not have the physical time to implement the boss AI) to 
have the boss actor be controllable by the user locally. (see `NetworkActorInputExample` and `AiActorInputExample`)

This leverages the same concepts used extensively throughout the project, which basically are
composability + dependency injection.

## Devices and Networking

For a host + peers version, the host would create the game, owns the
`GameLoop` and state, and registers a local, AI, or network input in the
`IActorInputResolver` for each actor.

A peer would only send input intents containing its actor and commands. The
host checks ownership, translates commands, and publishes the
intent through that actor's `IActorInput`. The existing systems then validate and apply 
them as it is right now independently of the actual source, so this is perfectly supported by the 
main game logic.

After the state changes the host would broadcast game state to peers. This is not currently supported
since it would have been too much work in this short amount of time, and would not have 
a real use case for the scope of the project.