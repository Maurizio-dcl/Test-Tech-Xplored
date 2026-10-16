using System;
using System.Collections.Generic;
using Actors;
using Game.Phases;

namespace Game.Turns
{
    public sealed class TurnDirector : IDisposable
    {
        private readonly Map.Map _map;
        private readonly IReadOnlyList<Actor> _actors;
        private readonly IReadOnlyList<ITurnPhase> _phases;

        private int _actorIndex;
        private int _phaseIndex;
        private bool _phaseStarted;
        private int _round;

        public TurnDirector(
            Map.Map map,
            IReadOnlyList<Actor> actors,
            IReadOnlyList<ITurnPhase> phases)
        {
            _map = map;
            _actors = actors;
            _phases = phases;

            _round = 1;
        }

        public void Tick()
        {
            // This is basically a nested state machine, this class moves through rounds and phases in each round
            // TurnContext is used as a wrapper to pass each phase relevant information about this turn
            
            if (_actors.Count == 0)
                return;

            ITurnPhase phase = _phases[_phaseIndex];

            // Begin a new phase
            if (!_phaseStarted)
            {
                Actor activeActor = _actors[_actorIndex];
                phase.Begin(new TurnContext(_round, activeActor, _map));
                _phaseStarted = true;
            }

            // Execute a phase for the active actor and keep trying.
            // This is due to the current approach coming from unity player loop Update() function.
            if (phase.Tick() == TurnPhaseStatus.Running)
                return;

            // End the current phase for this actor
            phase.End();
            _phaseStarted = false;
            
            // Advance to next phase for this actor
            Advance();
        }

        private void Advance()
        {
            // Update phase index for this actor
            _phaseIndex++;
            if (_phaseIndex < _phases.Count)
                return;
            
            // If we finished phases for this actor go ahead with the next actor
            _phaseIndex = 0;
            _actorIndex++;

            if (_actorIndex < _actors.Count)
                return;

            // If we finished all phases for all actors, new round
            _actorIndex = 0;
            _round++;
        }

        public void Dispose()
        {
            if (!_phaseStarted || _actors.Count == 0)
                return;

            _phases[_phaseIndex].End();
            _phaseStarted = false;
        }
    }
}
