using System.Collections.Generic;
using Common;
using Entitas;

public sealed class MoveSystem:IExecuteSystem
{
    private readonly IGroup<GameEntity> m_group;
    private readonly ITimeService m_timeService;
    public MoveSystem(Contexts contexts)
    {
        m_timeService = Services.Get<ITimeService>();
        m_group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.Speed));
    }

    public void Execute()
    {
        foreach (var e in m_group)
        {
            e.ReplacePosition(e.position.value + e.direction.value * e.speed.value * m_timeService.FrameInterval);
        }
    }
}