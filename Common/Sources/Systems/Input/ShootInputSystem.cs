using System.Collections.Generic;
using Entitas;

public sealed class ShootInputSystem : ReactiveSystem<InputEntity>, ICleanupSystem
{
    readonly Contexts m_contexts;
    readonly IGroup<InputEntity> m_inputGroup;
    readonly List<InputEntity> m_inputBuffer = new List<InputEntity>();
    
    public ShootInputSystem(Contexts contexts):base(contexts.input)
    {
        m_contexts = contexts;
        m_inputGroup = contexts.input.GetGroup(InputMatcher.ShootInput);
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.ShootInput);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.isShootInput && entity.hasInputOwner;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach(var e in entities)
        {
            var commandEntity = m_contexts.command.CreateEntity();
            commandEntity.AddCommandOwner(e.inputOwner.value);
            commandEntity.isShootCommand = true;
        }
    }

    public void Cleanup()
    {
        foreach(var e in m_inputGroup.GetEntities(m_inputBuffer))
        {
            e.Destroy();
        }
    }
}