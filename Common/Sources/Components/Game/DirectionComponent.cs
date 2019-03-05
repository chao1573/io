using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;

[Game]
public sealed class DirectionComponent : IComponent
{
    public Vector3 value;
}