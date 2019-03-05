using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;
[Game, Event(EventTarget.Self)]
public class PositionComponent : IComponent
{
    public Vector3 value;
}
