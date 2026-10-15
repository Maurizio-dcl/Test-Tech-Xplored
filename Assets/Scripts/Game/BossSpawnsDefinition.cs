using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(
        fileName = "BossSpawnsDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Game/BossSpawnsDefinition",
        order = 1)]
    public sealed class BossSpawnsDefinition : ScriptableObject
    {
        [Serializable]
        public struct ActorSpawn
        {
            public ActorDefinition definition;
            public Vector2Int position;
        }
        
        [SerializeField] private ActorSpawn bossSpawn;
        [SerializeField] private List<ActorSpawn> heroSpawns;

        public ActorSpawn BossSpawn => bossSpawn;

        public List<ActorSpawn> HeroSpawns => heroSpawns;
    }
}