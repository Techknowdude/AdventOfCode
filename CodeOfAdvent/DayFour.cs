using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeOfAdvent
{
    internal static class DayFour
    {
        const int MinutesInHour = 60;
        public static void Run(List<string> inputs)
        {
            GuardState lastState = GuardState.Off;
            GuardInfo lastGuard = null;
            int lastMinute = 0;
            List<InputData> inputDatas = new List<InputData>();
            List<GuardInfo> Guards = new List<GuardInfo>();

            foreach (var input in inputs)
            {
                inputDatas.Add(new InputData(input));
            }

            foreach (var data in inputDatas.OrderBy( i => i.Time))
            {
                var currentMinute = data.Time.Minute;
                if (data.Action == "Guard")
                {
                    if (lastState == GuardState.Sleep) // if last guard was sleeping, finish his shift sleeping
                    {
                        lastGuard.Sleep(lastMinute, MinutesInHour);
                    }

                    lastGuard = Guards.FirstOrDefault(g => g.GuardNum == data.GuardNum);
                    if (lastGuard == null)
                    {
                        lastGuard = new GuardInfo(data.GuardNum);
                        Guards.Add(lastGuard);
                    }
                    lastState = GuardState.Off;
                }
                else if (data.Action == "wakes")
                {
                    if (lastState == GuardState.Sleep)
                    {
                        lastGuard.Sleep(lastMinute, currentMinute);
                    }
                    lastState = GuardState.On;
                }
                else
                {
                    lastState = GuardState.Sleep;
                }

                lastMinute = currentMinute;
            }

            //part 1
            var lazyGuard =
                Guards.Aggregate((guard1, guard2) => guard1.GetSleepTime() > guard2.GetSleepTime() ? guard1 : guard2);

            var sleepTime = lazyGuard.GetHighestSleptHour();
            var answer1 = lastGuard.GuardNum*sleepTime;

            //part 2

            var mostFrequentlySleepingGuard =
                Guards.Aggregate(
                    (guard1, guard2) => guard1.GetMostSleptTime() > guard2.GetMostSleptTime() ? guard1 : guard2);
            var answer2 = mostFrequentlySleepingGuard.GetHighestSleptHour()*mostFrequentlySleepingGuard.GuardNum;
        }

        struct InputData
        {
            public int GuardNum;
            public string Action;
            public DateTime Time;

            public InputData(string input)
            {
                /*  example input
                 *   0           1       2     3     4
                    [1518-08-26 23:50] Guard #3251 begins shift
                    [1518-02-27 00:34] wakes up
                    [1518-02-20 00:32] falls asleep
                 */

                var parse = input.Split(new char[] {']', '[', ' ', '#'}, StringSplitOptions.RemoveEmptyEntries);

                Time = Convert.ToDateTime(parse[0] +" "+ parse[1]);
                Action = parse[2];
                GuardNum = Action.Equals("Guard") ? Convert.ToInt32(parse[3]) : -1;
            }
        }

        enum GuardState
        {
            Off,
            On,
            Sleep
        }

        class GuardInfo
        {
            public int GuardNum = -1;
            private int[] SleepTimes = new int[MinutesInHour];

            public GuardInfo(int guardNum)
            {
                GuardNum = guardNum;
            }

            /// <summary>
            /// Set gaurd as slept
            /// </summary>
            /// <param name="start">inclusive</param>
            /// <param name="stop">non-inclusive</param>
            public void Sleep(int start, int stop)
            {
                for (int hour = start; hour < stop; hour++)
                {
                    SleepTimes[hour]++;
                }
            }

            public int GetSleepTime()
            {
                return SleepTimes.Sum(d => d);
            }

            public int GetHighestSleptHour()
            {
                return SleepTimes.ToList().IndexOf(SleepTimes.Max());
            }

            public int GetMostSleptTime()
            {
                return SleepTimes.Max();
            }
        }
    }
}