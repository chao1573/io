public abstract class ActionHandlerSystem <TEntity> : ITearDownSystem where TEntity: class, IEntity
{
    private readonly IGroup<TEntity> _group;
    private readonly bool _destroyActionEntities;

    public ActionHandlerSystem(IContext<TEntity> context, bool destroyActionEntities = false)
    {
        _group = GetGroup(context);
        _group.OnEntityAdded += OnEntityAdded;
        _destroyActionEntities = destroyActionEntities;
    }

    private void OnEntityAdded(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
    {
        Execute(entity);
        entity.RemoveComponent(index);
        if(_destroyActionEntities) entity.Destroy();
    }

    protected abstract IGroup<TEntity> GetGroup(IContext<TEntity> context);

    protected abstract void Execute(TEntity entity);

    public void TearDown()
    {
        _group.OnEntityAdded -= OnEntityAdded;
    }
}