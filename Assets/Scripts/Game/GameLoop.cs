using System.Collections.Generic;
using Actors;
using Map.Views;
using UnityEngine;
using static Game.BossSpawnsDefinition;

namespace Game
{
    public sealed class GameLoop : MonoBehaviour
    {
        [SerializeField] private MapView mapView;
        [SerializeField] private BossSpawnsDefinition bossSpawnsDefinition;

        private Map.Map _map;

        private void Start()
        {
            _map = new();
            mapView.Initialize(_map);

            InitializeActors();
        }

        private void InitializeActors()
        {
            int uniqueId = 0;
            List<ActorSpawn> heroSpawns = bossSpawnsDefinition.HeroSpawns;

            for (int i = 0; i < heroSpawns.Count; i++)
                PlaceActor(uniqueId++, heroSpawns[i]);

            PlaceActor(uniqueId, bossSpawnsDefinition.BossSpawn);
        }

        private void PlaceActor(int uniqueId, ActorSpawn spawn)
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
                Debug.LogError($"Could not place '{spawn.definition.name}' at {spawn.position}.", this);
        }
    }
}
