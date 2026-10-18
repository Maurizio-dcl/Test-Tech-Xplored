using System;
using Map;
using ActorAction = Actors.Actions.Action;

namespace Game.Input
{
    public interface IActorInput
    {
        event Action<Tile> TileSelected;
        event Action<ActorAction> ActionSelected;
        event Action EndMovementRequested;
        event Action EndActionRequested;
        event Action CancelActionRequested;
        event Action ConfirmTargetsRequested;
    }
}
