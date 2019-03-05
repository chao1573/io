using System.Collections.Generic;
using Common;
using Entitas;
using Microsoft.Xna.Framework;
using Debug = System.Diagnostics.Debug;

public sealed class JoinGameCommandSystem : ReactiveSystem<CommandEntity>, ICleanupSystem
{
    private readonly Contexts m_contexts;
    readonly IGroup<CommandEntity> m_commands;
    readonly  List<CommandEntity> m_commandBuffer = new List<CommandEntity>();
    private readonly BlueprintService m_blueprintService;
    private readonly EntityIdService m_entityIdService;
    public JoinGameCommandSystem(Contexts contexts) : base(contexts.command)
    {
        m_blueprintService = Services.Get<BlueprintService>();
        m_entityIdService = Services.Get<EntityIdService>();
        
        m_contexts = contexts;
        m_commands = contexts.command.GetGroup(CommandMatcher.JoinGameCommand);
    }

    protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
    {
        return context.CreateCollector(CommandMatcher.JoinGameCommand);
    }

    protected override bool Filter(CommandEntity entity)
    {
        return entity.isJoinGameCommand && entity.hasCommandOwner;
    }

    protected override void Execute(List<CommandEntity> entities)
    {
        foreach (var e in entities)
        {
            var gameEntity = m_contexts.game.CreateEntity();
            gameEntity.AddPlayerId(e.commandOwner.value);
            var blueprint =  m_blueprintService.GetBlueprint("Player");
            Debug.Assert(blueprint!=null, "Player blueprint not found");
            blueprint.Apply(gameEntity);
            gameEntity.AddId(m_entityIdService.GetId());

//            var weaponEntity = m_contexts.game.CreateEntity();
//            weaponEntity.AddId(m_entityIdService.GetId());
//            weaponEntity.AddParent(gameEntity.id.value);
//            var localPos = new Vector3(0.1f, 0.7f, 0f);
//            weaponEntity.AddLocalPosition(localPos);
//            weaponEntity.AddLocalTransform(localPos,Quaternion.identity, Matrix4x4.Translate(localPos));
//            weaponEntity.AddLocalPosition()
//            weaponEntity.AddLocalPosition(new UnityEngine.Vector2(0.1f, 0.7f));
//            e.Destroy();
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