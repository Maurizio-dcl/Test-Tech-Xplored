## Inputs

The idea behind inputs is routing events through a wrapper IActorInput.cs,
rather than consuming UI events directly, and using an IActorInputResolver in systems
to resolve different inputs for each actor.

This means that inputs are freely mixable at runtime, and also has the flexibility of allowing,
as in this project example (since I did not have the physical time to implement the boss AI) to 
have the boss actor be controllable by the user locally.

This leverages the same concepts used extensively throughout the project, which basically are
composability + dependency injection.

## Devices and Networking

