using Entitas;
using Entitas.CodeGeneration.Attributes;
[Game]
public sealed class PlayerIdComponent : IComponent
{
    [EntityIndex]
    public string value;
}

