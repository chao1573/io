using Entitas;
using Entitas.CodeGeneration.Attributes;
[Game, Event(true)]
public class PositionComponent : IComponent
{
    public UnityEngine.Vector3 value;
}
