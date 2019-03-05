public sealed class GameSystems :Feature
{
    public GameSystems(Contexts contexts)
    {
        Add(new AddRotationSystem(contexts));
        Add(new MoveSystem(contexts));
//        Add(new TransformSystems(contexts));
        Add(new DestroyEntitySystem(contexts));
        Add(new GameEventSystems(contexts));
    }
}