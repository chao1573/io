using Entitas;
using Microsoft.Xna.Framework;

[Input]
public sealed class MoveInputComponent:IComponent
{
    public Vector2 direction;
}
