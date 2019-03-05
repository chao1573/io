using  Entitas;

public sealed class CommandSystems :Feature
{
    public CommandSystems(Contexts contexts)
    {
        Add(new JoinGameCommandSystem(contexts));
        Add(new MoveCommandSystem(contexts));
        Add(new ShootCommandSystem(contexts));
    }
}