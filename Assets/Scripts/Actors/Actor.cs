using System;
using System.Collections.Generic;
using Actors.Actions;
using static Actors.StatDefinition;
using Action = Actors.Actions.Action;

namespace Actors
{
    public sealed class Actor
    {
        private readonly int _uniqueId;
        private readonly ActorDefinition _definition;

        private readonly Dictionary<StatTag, Stat> _stats = new();
        private readonly List<Action> _actions = new();

        public int UniqueId => _uniqueId;
        public ActorDefinition Definition => _definition;
        public IReadOnlyCollection<Stat> Stats => _stats.Values;
        public IReadOnlyList<Action> Actions => _actions;
        public bool IsDead => TryGetStat(StatTag.Hp, out Stat hp) && hp.Value == hp.MinValue;

        public Actor(int uniqueId, ActorDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            _uniqueId = uniqueId;
            _definition = definition;

            foreach (StatConfiguration statConfiguration in definition.StatConfigurations)
            {
                if (statConfiguration.Definition == null)
                    throw new ArgumentException($"Actor '{definition.name}' has a stat without a definition.");

                Stat stat = new Stat(statConfiguration);
                StatTag tag = statConfiguration.Definition.Tag;

                if (!_stats.TryAdd(tag, stat))
                    throw new ArgumentException($"Actor '{definition.name}' has more than one stat tagged '{tag}'.");
            }

            foreach (ActionDefinition actionDefinition in definition.ActionDefinitions)
            {
                Action action = new Action(actionDefinition);
                _actions.Add(action);
            }
        }

        public bool TryGetStat(StatTag tag, out Stat stat) =>
            _stats.TryGetValue(tag, out stat);
    }
}
