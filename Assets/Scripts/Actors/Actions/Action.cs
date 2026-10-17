using UnityEngine;

namespace Actors.Actions
{
    public sealed class Action
    {
        public int CooldownValue { get; private set; }
        public int MaxCooldown { get; }
        public ActionDefinition Definition { get; }
        public bool IsAvailable => CooldownValue == 0;

        public Action(ActionDefinition definition)
        {
            Definition = definition;
            MaxCooldown = definition.Cooldown;
        }

        public void Execute() => Definition.Execute();

        public void SetOnCooldown() => SetCooldown(MaxCooldown);

        public void ModifyCooldown(int amount) => SetCooldown(Mathf.Clamp(CooldownValue + amount, 0, MaxCooldown));

        private void SetCooldown(int value) => CooldownValue = Mathf.Clamp(value, 0, MaxCooldown);
    }
}