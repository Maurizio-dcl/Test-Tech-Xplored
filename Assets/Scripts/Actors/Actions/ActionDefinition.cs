using System.Collections.Generic;
using Game.Phases.Action;
using UnityEngine;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "ActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/ActionDefinition")]
    public sealed class ActionDefinition : ScriptableObject
    {
        [SerializeField] private string actionName = "default name";
        [SerializeField] private string actionDescription = "default description";
        [SerializeField] private int cooldown = 0;
        // 0 = self, 1 = melee, 2+ = ranged
        [SerializeField] private int minRange = 0;
        [SerializeField] private int maxRange = 0;
        [SerializeField] private Targeting targeting = new(TargetingMode.Single, true, CharacterType.Boss);
        [SerializeField] private Sprite icon;
        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeReference] private List<ActionEffectDefinition> effects = new();

        public string ActionName => actionName;
        public string ActionDescription => actionDescription;
        public int Cooldown => cooldown;
        public int MinRange => minRange;
        public int MaxRange => maxRange;
        public Targeting Targeting => targeting;
        public Sprite Icon => icon;
        public Color BackgroundColor => backgroundColor;
        public IReadOnlyList<ActionEffectDefinition> Effects => effects;

        public ActionOutcome Execute(ActionPhaseContext context)
        {
            List<IActionEffect> outcomes = new();
            var effectContext = new ActionEffectExecutionContext(context, outcomes);

            for (int i = 0; i < effects.Count; i++)
                effects[i]?.Execute(effectContext);

            return new ActionOutcome(
                context.Turn.Round,
                context.Turn.Actor,
                context.SelectedAction,
                outcomes);
        }
    }
}
