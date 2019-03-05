using Entitas;
public sealed class InputSystems:Feature
{
        public InputSystems(Contexts contexts)
        {
            Add(new JoinGameInputSystem(contexts));
            Add(new MoveInputSystem(contexts));
            Add(new ShootInputSystem(contexts));
        }
}