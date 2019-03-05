using Common;
using UnityEngine;
namespace IO
{
    public class LogService :ILogService
    {
        public void LogInfo(string message)
        {
            Debug.Log(message);
        }
    }
}
