using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;

[Game, Event(EventTarget.Self)]
public class RotationComponent : IComponent
{
    public Quaternion value;
}