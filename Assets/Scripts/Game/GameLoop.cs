using System.Collections.Generic;
using Actors;
using Game.Logging;
using Game.Phases;
using Game.Phases.Action;
using Game.Phases.Action.Systems;
using Game.Phases.Maintenance;
using Game.Phases.Maintenance.Systems;
using Game.Phases.Movement;
using Game.Phases.Movement.Systems;
using Game.Turns;
using Game.Views;
using Map.Views;
using UnityEngine;
using static Game.BossSpawnsDefinition;

namespace Game
{
    public sealed class GameLoop : MonoBehaviour
    {
        [SerializeField] private MapView mapView;
        [SerializeField] private ActorControlsView actorControlsView;
        [SerializeField] private BossSpawnsDefinition bossSpawnsDefinition;

        [SerializeField] private Color reachableTileColor = Color.green;
        [SerializeField] private Color targetableTileColor = Color.red;

        private Map.Map _map;
        private TurnDirector _turnDirector;

        private void Start()
        {
            _map = new();
            mapView.Initialize(_map);

            List<Actor> actors = InitializeActors();

            InitializeTurnDirector(actors);
        }

        private void Update() => _turnDirector?.Tick();

        private void OnDestroy() => _turnDirector?.Dispose();

        private List<Actor> InitializeActors()
        {
            int uniqueId = 0;
            List<ActorSpawn> heroSpawns = bossSpawnsDefinition.HeroSpawns;
            var actors = new List<Actor>(heroSpawns.Count + 1);

            for (int i = 0; i < heroSpawns.Count; i++)
                PlaceActor(uniqueId++, heroSpawns[i], actors);

            PlaceActor(uniqueId, bossSpawnsDefinition.BossSpawn, actors);

            return actors;
        }

        private void PlaceActor(int uniqueId, ActorSpawn spawn, List<Actor> actors)
        {
            if (spawn.definition == null)
            {
                Debug.LogError($"Initial actor placement {uniqueId} has no definition.", this);
                return;
            }

            if (spawn.definition.Visuals == null)
            {
                Debug.LogError($"Actor definition '{spawn.definition.name}' has no visuals.", this);
                return;
            }

            var actor = new Actor(uniqueId, spawn.definition);
            if (!_map.Move(actor, spawn.position))
            {
                Debug.LogError($"Could not place '{spawn.definition.name}' at {spawn.position}.", this);
                return;
            }

            actors.Add(actor);
        }

        private void InitializeTurnDirector(List<Actor> actors)
        {
            ILogSink logSink = new UnityConsoleLogSink();

            // Initialize new phases here
            MaintenancePhase maintenancePhase = InitializeMaintenancePhase();
            MovementPhase movementPhase = InitializeMovementPhase(logSink);
            ActionPhase actionPhase = InitializeActionPhase(logSink);

            List<ITurnPhase> phases = new List<ITurnPhase> { maintenancePhase, movementPhase, actionPhase };

            _turnDirector = new TurnDirector(
                _map,
                actors,
                phases);
        }

        private MaintenancePhase InitializeMaintenancePhase()
        {
            IMaintenanceSystem[] maintenanceSystems =
            {
                new MovementMaintenanceSystem(),
                new ActionMaintenanceSystem(actorControlsView)
            };

            return new(maintenanceSystems);
        }

        private MovementPhase InitializeMovementPhase(ILogSink logSink)
        {
            IMovementSystem[] movementSystems = { };

            IMovementSystem[] repeatingMovementSystems = new IMovementSystem[]
            {
                new MovementCheckSystem(mapView, reachableTileColor),
                new MovementSelectionSystem(mapView, actorControlsView),
                new MovementSystemLoggingDecorator(new MovementExecuteSystem(), logSink)
            };

            return new(mapView, movementSystems, repeatingMovementSystems);
        }

        private ActionPhase InitializeActionPhase(ILogSink logSink)
        {
            IActionSystem[] actionSystems = { };

            IActionSystem[] repeatingActionSystems = {
                new ActionSelectionSystem(actorControlsView),
                new ActionTargetCheckSystem(),
                new ActionTargetSelectionSystem(mapView, actorControlsView, targetableTileColor),
                new ActionTargetConfirmationSystem(mapView, actorControlsView, targetableTileColor),
                new ActionSystemLoggingDecorator(new ActionExecuteSystem(), logSink)
            };

            return new(mapView, actionSystems, repeatingActionSystems);
        }
    }
}
