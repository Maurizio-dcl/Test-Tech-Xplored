using System;
using System.Collections.Generic;

namespace Actors
{
    public sealed class Actor
    {
        private readonly int _uniqueId;
        private readonly ActorDefinition _definition;

        private readonly Dictionary<StatDefinition, Stat> _stats = new();

        public int UniqueId => _uniqueId;
        public ActorDefinition Definition => _definition;

        public IEnumerable<Stat> Stats => _stats.Values;

        public Actor(int uniqueId, ActorDefinition definition)
        {
            _uniqueId = uniqueId;
            _definition = definition;

            foreach (var config in definition.StatConfigurations)
            {
                var stat = new Stat(config);

                if (!_stats.TryAdd(config.Definition, stat))
                    throw new ArgumentException($"Duplicate stat: {config.Definition.name}");
            }
        }

        public bool TryGetStat(StatDefinition definition, out Stat stat) => 
            _stats.TryGetValue(definition, out stat);
    }
}