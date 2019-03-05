using System.Collections.Generic;
using Entitas;

public sealed class JoinGameSystem : ReactiveSystem<CommandEntity>
{
    private readonly Contexts m_contexts;
    public JoinGameSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
    {
        return context.CreateCollector(CommandMatcher.JoinGameCommand);
    }

    protected override bool Filter(CommandEntity entity)
    {
        throw new System.NotImplementedException();
    }

    protected override void Execute(List<CommandEntity> entities)
    {
        throw new System.NotImplementedException();
    }
}