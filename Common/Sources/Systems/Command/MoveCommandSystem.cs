using Entitas;

public class MoveSystem : IExecuteSystem
{
    readonly IGroup<GameEntity> m_group;
    public MoveSystem(Contexts contexts)
    {
        m_group = contexts.game.GetGroup(GameMatcher.Direction);
    }

    public void Execute()
    {
        foreach(var e in m_group)
        {
            var move = e.direction;
            var speed = e.hasSpeed ? e.speed.value : 0f;
            e.ReplacePosition(e.position.value + move.value * speed);
        }
    }
}

