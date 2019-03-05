using Entitas;
using Entitas.CodeGeneration.Attributes;

[Input]
public sealed class InputOwnerComponent : IComponent
{
    [EntityIndex]
    public string value;
}

