using Game.Phases.Action;
using UnityEngine;

namespace Actors.Actions
{
    public sealed class Action
    {
        public int CooldownValue { get; private set; }
        public int MaxCooldown { get; }
        public ActionDefinition Definition { get; }
        public bool IsAvailable => CooldownValue == 0;
        
        private int EffectiveMaxCooldown => MaxCooldown + 1;

        public Action(ActionDefinition definition)
        {
            Definition = definition;
            MaxCooldown = definition.Cooldown;
        }

        public void Execute(ActionPhaseContext context) => Definition.Execute(context);

        public void SetOnCooldown() => SetCooldown(EffectiveMaxCooldown);

        public void ModifyCooldown(int amount) => 
            SetCooldown(Mathf.Clamp(CooldownValue + amount, 0, EffectiveMaxCooldown));

        private void SetCooldown(int value) => CooldownValue = Mathf.Clamp(value, 0, EffectiveMaxCooldown);
    }
}
