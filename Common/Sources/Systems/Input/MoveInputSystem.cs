using System.Collections.Generic;
using Entitas;

public sealed class MoveInputSystem : ReactiveSystem<InputEntity>, ICleanupSystem
{
    readonly Contexts m_contexts;
    readonly IGroup<InputEntity> m_inputGroup;
    readonly List<InputEntity> m_inputBuffer = new List<InputEntity>();
    
    public MoveInputSystem(Contexts contexts):base(contexts.input)
    {
        m_contexts = contexts;
        m_inputGroup = contexts.input.GetGroup(InputMatcher.MoveInput);
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.MoveInput);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.hasMoveInput && entity.hasInputOwner;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach(var e in entities)
        {
            var commandEntity = m_contexts.command.CreateEntity();
            commandEntity.AddCommandOwner(e.inputOwner.value);
            commandEntity.AddMoveCommand(e.moveInput.direction);          
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