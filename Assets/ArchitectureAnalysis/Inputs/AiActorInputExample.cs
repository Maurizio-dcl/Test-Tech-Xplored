using System;
using Game.Input;
using Map;
using ActorAction = Actors.Actions.Action;

namespace ArchitectureAnalysis.Inputs
{
    // An AI input can implement logic using the turn/phase context
    // and publish its decisions through the intent events
    public abstract class AiActorInputExample : IActorInput
    {
        public abstract event Action<Tile> TileSelected;
        public abstract event Action<ActorAction> ActionSelected;
        public abstract event Action EndMovementRequested;
        public abstract event Action EndActionRequested;
        public abstract event Action CancelActionRequested;
        public abstract event Action ConfirmTargetsRequested;
    }
}
