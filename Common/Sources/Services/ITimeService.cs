namespace Common
{
    public interface ITimeService:IService
    {
        float FrameInterval { get; }
    }
}