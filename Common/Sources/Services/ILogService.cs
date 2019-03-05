namespace Common
{
    public interface ILogService:IService
    {
        void LogInfo(string message);
    }
}