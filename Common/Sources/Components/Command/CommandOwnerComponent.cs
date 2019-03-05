using Entitas;
using Entitas.CodeGeneration.Attributes;

[Command]
public sealed class CommandOwnerComponent:IComponent
{
    [EntityIndex]
    public string value;
}