using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using Entitas;

public sealed class ShootCommandSystem : ReactiveSystem<CommandEntity>, ICleanupSystem
{
    private readonly Contexts m_contexts;
    readonly IGroup<CommandEntity> m_commands;
    readonly List<CommandEntity> m_commandBuffer = new List<CommandEntity>();
    private readonly BlueprintService m_blueprintService;

    public ShootCommandSystem(Contexts contexts) : base(contexts.command)
    {
        m_blueprintService = Services.Get<BlueprintService>();
        m_contexts = contexts;
        m_commands = contexts.command.GetGroup(CommandMatcher.ShootCommand);
    }

    protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
    {
        return context.CreateCollector(CommandMatcher.ShootCommand);
    }

    protected override bool Filter(CommandEntity entity)
    {
        return entity.isShootCommand && entity.hasCommandOwner;
    }

    protected override void Execute(List<CommandEntity> entities)
    {
        foreach (var e in entities)
        {
            var playerEntities = m_contexts.game.GetEntitiesWithPlayerId(e.commandOwner.value);
            if(playerEntities.Count == 0)
                continue;

            var playerEntity = playerEntities.SingleEntity();
            var blueprint = m_blueprintService.GetBlueprint("Bullet");
            Debug.Assert(blueprint != null, "Bullet blueprint not found");

//            var gunPointId = playerEntity.children.value[0];
//            var gunPointEntity = m_contexts.game.GetEntityWithId(gunPointId);
            var bulletEntity = m_contexts.game.CreateEntity();
            blueprint.Apply(bulletEntity);
            bulletEntity.AddPosition(playerEntity.position.value);
            bulletEntity.AddRotation(playerEntity.rotation.value);
            bulletEntity.AddDirection(playerEntity.direction.value);
//            bulletEntity.AddDirection(playerEntity.direction.value);
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