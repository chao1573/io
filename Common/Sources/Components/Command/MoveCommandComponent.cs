using Entitas;
using Microsoft.Xna.Framework;

[Command]
public sealed class MoveCommandComponent:IComponent
{
    public Vector2 value;
}