using System.Collections.Generic;
using Entitas;

public sealed class TransformSystem : IExecuteSystem
{
    private readonly Contexts m_contexts;
//    private List<int> m_parent = new List<int>();
//    private readonly GameEntity m_root;
    private readonly int m_rootId;

    public TransformSystem(Contexts contexts)
    {
        m_contexts = contexts;
        m_rootId = m_contexts.game.rootEntity.id.value;
    }

    public void Execute()
    {
        UpdateWorldMatrix(m_rootId, false);
    }

    void UpdateWorldMatrix(int entityId, bool dirty)
    {
//        var entity = m_contexts.game.GetEntityWithId(entityId);
//        dirty |= entity.isDitryTransform;
//        if (dirty)
//        {
//            entity.ReplaceWorldMatrix(entity.WorldMatrixComponent.value * entity.localMatrix.value);
//            entity.isDitryTransform = false;
//        }
//
//        var children = entity.children.value;
//        foreach (var child in children)
//        {
//            UpdateWorldMatrix(child, dirty);
//        }
    }
}