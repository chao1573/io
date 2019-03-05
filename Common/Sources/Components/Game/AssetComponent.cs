using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Event(EventTarget.Any)]
public sealed class AssetComponent : IComponent
{
    public string value;
}