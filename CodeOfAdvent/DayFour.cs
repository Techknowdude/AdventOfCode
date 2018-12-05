using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeOfAdvent
{
    internal static class DayFour
    {
        const int HoursInDay = 24;
        public static void Run(List<string> input)
        {
            
        }

        enum GuardState
        {
            Off,
            On,
            Sleep
        }

        class GuardInfo
        {
            Dictionary<DateTime, GuardState[]> StateData = new Dictionary<DateTime, GuardState[]>();

            private DateTime LastStartedShift;

            public GuardInfo()
            {
                
            }

            public void StartShift(DateTime time)
            {
                CheckDayInfo(time);
                LastStartedShift = time;
            }

            public void Sleep(DateTime time)
            {
                CheckDayInfo(time);
                FillDayInfo(LastStartedShift, time, GuardState.On);
            }

            private void FillDayInfo(DateTime lastStartedShift, DateTime time, GuardState @on)
            {
                
            }

            public void EndShift(DateTime time)
            {
                CheckDayInfo(time);
            }

            public int GetSleepTime()
            {
                return StateData.Sum(d => d.Value.Count(t => t == GuardState.Sleep));
            }

            protected void CheckDayInfo(DateTime time)
            {
                if (StateData.All(data => data.Key.ToShortDateString() != time.ToShortDateString()))
                {
                    StateData[time.Date] = new GuardState[HoursInDay];
                }
            }
        }
    }
}