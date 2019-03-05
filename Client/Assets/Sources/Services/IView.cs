using Entitas;
namespace IO
{
    public interface IView 
    {
        void Link(IEntity entity, IContext context);
    }
}