public sealed class Systems : Feature
{
    public Systems(Contexts contexts)
    {
        Add(new InputSystems(contexts));
        Add(new CommandSystems(contexts));
        Add(new GameSystems(contexts));
//        Add(new EventSystems(contexts));
    }
}