using System;
using System.Collections.Generic;
using Common;
using Entitas;
using Microsoft.Xna.Framework;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Random = System.Random;
using Vector3 = Microsoft.Xna.Framework.Vector3;

public sealed class MoveCommandSystem : ReactiveSystem<CommandEntity>, ICleanupSystem
{
    private const float ANGLE_TOLERANCE = 0.0001f;
    private readonly Contexts m_contexts;
    readonly IGroup<CommandEntity> m_commands;
    readonly List<CommandEntity> m_commandBuffer = new List<CommandEntity>();
    private readonly ITimeService m_timeService;

    public MoveCommandSystem(Contexts contexts) : base(contexts.command)
    {
        m_timeService = Services.Get<ITimeService>();
        m_contexts = contexts;
        m_commands = contexts.command.GetGroup(CommandMatcher.MoveCommand);
    }

    protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
    {
        return context.CreateCollector(CommandMatcher.MoveCommand);
    }

    protected override bool Filter(CommandEntity entity)
    {
        return entity.hasMoveCommand && entity.hasCommandOwner;
    }

    protected override void Execute(List<CommandEntity> entities)
    {
        foreach (var e in entities)
        {
            var playerEntities = m_contexts.game.GetEntitiesWithPlayerId(e.commandOwner.value);
            if (playerEntities.Count == 0)
                continue;
            var playerEntity = playerEntities.SingleEntity();
            if (e.moveCommand.value.LengthSquared() > 0.1f)
            {
                float angularSpeed = playerEntity.angularSpeed.value;
                Vector3 dir = playerEntity.direction.value;
                Vector3 targetDir = e.moveCommand.value;
                // dot(dir, rot90CCW(targetDir))
                float angle = 0f;
                float dot = dir.X * -targetDir.Y + dir.Y * targetDir.X;
                // parallel/antiparallel
                if (Math.Abs(dot) <= ANGLE_TOLERANCE)
                {
                    Services.Get<ILogService>().LogInfo("parallel/antiparallel");
                    if (Vector3.Dot(dir, targetDir) < 0f)
                    {
                        angle = angularSpeed * m_timeService.FrameInterval;
                    }
                }
                // targetDir is in right of dir,
                else if (dot > float.Epsilon)
                {
                    Services.Get<ILogService>().LogInfo("targetDir is in right of dir "+dot.ToString());
                    angle = -angularSpeed * m_timeService.FrameInterval;
                    float d = MathHelper.ToDegrees((float)Math.Acos(dot)) - 90f;
                    // over rotation
                    if (Math.Abs(angle) > Math.Abs(d))
                    {
                        angle = d;
                    }
                }
                // targetDir is in left of dir
                else
                {
                    Services.Get<ILogService>().LogInfo("targetDir is in left of dir"+dot.ToString());
                    angle = angularSpeed * m_timeService.FrameInterval;
                    float d = MathHelper.ToDegrees((float)Math.Acos(dot)) - 90f;
                    // over rotate
                    if (Math.Abs(angle) > Math.Abs(d))
                    {
                        angle = d;
                    }
                }
                var quat = Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(angle));
                var targetQuat = Quaternion.Multiply(playerEntity.rotation.value, quat);
                playerEntity.ReplaceRotation(targetQuat);
                playerEntity.ReplaceDirection(Vector3.Normalize(Vector3.Transform(Vector3.Right, targetQuat)));
                playerEntity.ReplaceSpeed(playerEntity.speed.maxValue, playerEntity.speed.maxValue);
            }
            else
            {
                playerEntity.ReplaceSpeed(0f, playerEntity.speed.maxValue);
            }
        }
    }

    public void Cleanup()
    {
        foreach (var e in m_commands.GetEntities(m_commandBuffer))
        {
            e.Destroy();
        }
    }
}