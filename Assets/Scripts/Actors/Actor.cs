using System;
using System.Collections.Generic;
using static Actors.StatDefinition;

namespace Actors
{
    public sealed class Actor
    {
        private readonly int _uniqueId;
        private readonly ActorDefinition _definition;

        private readonly Dictionary<StatTag, Stat> _stats = new();

        public int UniqueId => _uniqueId;
        public ActorDefinition Definition => _definition;

        public Actor(int uniqueId, ActorDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            _uniqueId = uniqueId;
            _definition = definition;

            foreach (var config in definition.StatConfigurations)
            {
                if (config.Definition == null)
                    throw new ArgumentException($"Actor '{definition.name}' has a stat without a definition.");

                var stat = new Stat(config);
                StatTag tag = config.Definition.Tag;

                if (!_stats.TryAdd(tag, stat))
                    throw new ArgumentException($"Actor '{definition.name}' has more than one stat tagged '{tag}'.");
            }
        }

        public bool TryGetStat(StatTag tag, out Stat stat) =>
            _stats.TryGetValue(tag, out stat);
    }
}
