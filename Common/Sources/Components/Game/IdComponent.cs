using Entitas;
using Entitas.CodeGeneration.Attributes;
public class IdComponent:IComponent
{
    [PrimaryEntityIndex]
    public int value;
}