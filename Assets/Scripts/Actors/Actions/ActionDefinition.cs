using Game.Phases.Action;
using UnityEngine;

namespace Actors.Actions
{
    public abstract class ActionDefinition : ScriptableObject
    {
        [SerializeField] private string actionName;
        [SerializeField] private string actionDescription;
        [SerializeField] private int cooldown;
        // 0 = self, 1 = melee, 2+ = ranged
        [SerializeField] private int minRange;
        [SerializeField] private int maxRange;
        [SerializeField] private Targeting targeting;
        [SerializeField] private Sprite icon;
        [SerializeField] private Color backgroundColor;

        public string ActionName => actionName;
        public int Cooldown => cooldown;
        public int MinRange => minRange;
        public int MaxRange => maxRange;
        public Targeting Targeting => targeting;
        public Sprite Icon => icon;
        public Color BackgroundColor => backgroundColor;

        public abstract void Execute(ActionPhaseContext context);
    }
}
