using System;
using System.Collections.Generic;
using Actors;

namespace Game.Input
{
    public sealed class ActorInputResolver : IActorInputResolver
    {
        private readonly IReadOnlyDictionary<Actor, IActorInput> _inputs;

        public ActorInputResolver(IReadOnlyDictionary<Actor, IActorInput> inputs)
        {
            _inputs = inputs;
        }

        public IActorInput Resolve(Actor actor)
        {
            if (!_inputs.TryGetValue(actor, out IActorInput input) || input == null)
                throw new InvalidOperationException($"No input source is registered for actor {actor.UniqueId}.");

            return input;
        }
    }
}
