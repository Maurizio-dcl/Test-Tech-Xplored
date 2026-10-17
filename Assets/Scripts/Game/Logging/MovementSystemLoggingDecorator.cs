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
        
        private MovementPhaseContext _context;

        public MovementSystemLoggingDecorator(IMovementSystem system, ILogSink logSink)
        {
            _system = system;
            _logSink = logSink;
        }

        public void Enter(MovementPhaseContext context) => _system.Enter(context);

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            _context = context;
            _context.Turn.Map.ActorMoved += LogMovement;

            try
            {
                return _system.Tick(context);
            }
            finally
            {
                context.Turn.Map.ActorMoved -= LogMovement;
                _context = null;
            }
        }

        public void Exit(MovementPhaseContext context) => _system.Exit(context);
        
        private void LogMovement(Actor actor, Tile origin, Tile destination)
        {
            // An action might move the actor (i.e. "push")
            if (actor != _context.Turn.Actor)
                return;

            string actorName = actor.Definition.GivenName;
            string originPosition = origin == null ? "outside map" : origin.Position.ToString();

            _logSink.Log(
                $"[Movement] Round: {_context.Turn.Round}, " +
                $"{actorName} [id: {actor.UniqueId}], " +
                $"Origin: {originPosition}, Destination: {destination.Position}.");
        }
    }
}
