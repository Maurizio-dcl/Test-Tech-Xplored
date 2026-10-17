using System;
using System.Collections.Generic;
using System.Linq;
using Actors;

namespace Actors.Actions
{
    public sealed class ActionOutcome
    {
        public int Round { get; }
        public Actor Source { get; }
        public Action Action { get; }
        public IReadOnlyList<IActionEffect> Effects { get; }

        public ActionOutcome(
            int round,
            Actor source,
            Action action,
            IReadOnlyList<IActionEffect> effects)
        {
            Round = round;
            Source = source;
            Action = action;
            Effects = effects;
        }

        public string Format()
        {
            string formattedEffects = Effects.Count == 0
                ? "had no effect"
                : string.Join(", ", Effects.Select(effect => effect.Format()));

            return
                $"[Action] Round: {Round}, " +
                $"{Source.Definition.GivenName} [id: {Source.UniqueId}], " +
                $"{Action.Definition.ActionName}, " +
                $"Outcome: {formattedEffects}.";
        }
    }
}
