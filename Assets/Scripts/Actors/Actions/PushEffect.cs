using Actors;
using UnityEngine;

namespace Actors.Actions
{
    public sealed class PushEffect : IActionEffect
    {
        public Actor Target { get; }
        public Vector2Int Origin { get; }
        public Vector2Int Destination { get; }

        public PushEffect(Actor target, Vector2Int origin, Vector2Int destination)
        {
            Target = target;
            Origin = origin;
            Destination = destination;
        }

        public string Format() =>
            $"pushed {Target.Definition.GivenName} [id: {Target.UniqueId}] " +
            $"from {Origin} to {Destination}";
    }
}
