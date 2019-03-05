using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class PlayerIdComponent : IComponent
{
    [EntityIndex]
    public string value;
}

