using System.Collections.Generic;
using  Entitas;
public sealed class JoinGameInputSystem:ReactiveSystem<InputEntity>, ICleanupSystem
{
    private readonly Contexts m_contexts;
    private readonly IGroup<InputEntity> m_inputGroup;
    private  readonly List<InputEntity> m_inputBuffer = new List<InputEntity>();
    public JoinGameInputSystem(Contexts contexts) : base(contexts.input)
    {
        m_contexts = contexts;
        m_inputGroup = contexts.input.GetGroup(InputMatcher.JoinGameInput);
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.JoinGameInput);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.isJoinGameInput && entity.hasInputOwner;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach(var e in entities)
        {
            var commandEntity = m_contexts.command.CreateEntity();
            commandEntity.AddCommandOwner(e.inputOwner.value);
            commandEntity.isJoinGameCommand = true;
        }
    }


    public void Cleanup()
    {
        foreach (var e in m_inputGroup.GetEntities(m_inputBuffer))
        {
            e.Destroy();
        }
    }
}