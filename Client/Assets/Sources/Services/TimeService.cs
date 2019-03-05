using Common;
using UnityEngine;
namespace IO
{
    public class TimeService:ITimeService
    {
        public float FrameInterval => Time.deltaTime;
    }
}