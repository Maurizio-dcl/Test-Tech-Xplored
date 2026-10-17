using System;
using Actors;
using Game.Phases.Movement;
using Game.Phases.Movement.Systems;
using Map;

namespace Game.Logging
{
    public sealed class MovementSystemLoggingDecorator : IMovementSystem
    {
        private readonly IMovementSystem _system;
        private readonly ILogSink _logSink;

        public MovementSystemLoggingDecorator(IMovementSystem system, ILogSink logSink)
        {
            _system = system;
            _logSink = logSink;
        }

        public void Enter(MovementPhaseContext context) => _system.Enter(context);

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            void LogMovement(Actor actor, Tile origin, Tile destination)
            {
                if (actor != context.Turn.Actor)
                    return;

                string actorName = actor.Definition.GivenName;
                string originPosition = origin == null ? "outside map" : origin.Position.ToString();

                _logSink.Log(
                    $"[Movement] Round: {context.Turn.Round}, " +
                    $"Actor: {actorName} ({actor.UniqueId}), " +
                    $"Origin: {originPosition}, Destination: {destination.Position}.");
            }

            context.Turn.Map.ActorMoved += LogMovement;
            try
            {
                return _system.Tick(context);
            }
            finally
            {
                context.Turn.Map.ActorMoved -= LogMovement;
            }
        }

        public void Exit(MovementPhaseContext context) => _system.Exit(context);
    }
}
