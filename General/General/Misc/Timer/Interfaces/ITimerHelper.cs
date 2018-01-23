using System;

namespace Devdog.General
{
    public interface ITimerHelper : IDisposable
    {
//        void StopAllTimers();
        void StopTimer(int id);
        int StartTimer(float time, Action callbackContinous, Action callbackWhenTimeIsOver);
        int StartTimer(float time, Action callbackWhenTimeIsOver);
    }
}
