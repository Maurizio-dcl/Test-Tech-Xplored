using System;
using Game.Input;
using Map;
using Action = Actors.Actions.Action;

namespace ArchitectureAnalysis.Inputs
{
    // A network input can translate received actor commands into IActorInput intent events
    public abstract class NetworkActorInputExample : IActorInput
    {
        public abstract event Action<Tile> TileSelected;
        public abstract event Action<Action> ActionSelected;
        public abstract event System.Action EndMovementRequested;
        public abstract event System.Action EndActionRequested;
        public abstract event System.Action CancelActionRequested;
        public abstract event System.Action ConfirmTargetsRequested;
    }
}