using Actors;

namespace Game.Input
{
    public interface IActorInputResolver
    {
        IActorInput Resolve(Actor actor);
    }
}
