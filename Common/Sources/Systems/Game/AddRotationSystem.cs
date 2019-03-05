using System.Collections.Generic;
using Entitas;
using Microsoft.Xna.Framework;
using  System;

public class AddRotationSystem:ReactiveSystem<GameEntity>
{
    private readonly Contexts m_contexts;
    public AddRotationSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Direction.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasRotation;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var direction = e.direction.value;
            var quat = Quaternion.CreateFromAxisAngle(Vector3.Backward,
                    (float) Math.Atan2(direction.Y, direction.X));
            e.AddRotation(quat);
        }
    }
}