using Entitas;
using Entitas.CodeGeneration.Attributes;

[Input]
public sealed class InputIdComponent : IComponent
{
    [EntityIndex]
    public string value;
}

