using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Logging;

namespace Mars
{
    public class CustomTimerInfo
    {
        public string Name = "";
        public uint Milliseconds = 1000;
    }

    public static class CustomTimer
    {
        public static List<(CustomTimerInfo, CancellationTokenSource)> activeTimers = new List<(CustomTimerInfo, CancellationTokenSource)>();
        public static (CustomTimerInfo, CancellationTokenSource) CreateTimer(CustomTimerInfo info, Func<Task> action)
        {
            if (CheckName(info.Name))
            {
                throw new Exception("Timer doesn't start, because name already used in another timer...");
            }
            
            var cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                try
                {
                    using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(info.Milliseconds <= 0 ? 1 : info.Milliseconds ));
                    while (await timer.WaitForNextTickAsync(cts.Token))
                    {
                        await action();
                    }
                }
                catch(Exception e)
                {
                    Log.Warning(e.Message);
                }
            }).DisableAsyncWarning();

            activeTimers.Add((info, cts));
            return activeTimers.Last();
        }
        
        public static bool CheckName(string name)
        {
            return activeTimers.Any(x => x!.Item1.Name == name);
        }

    }
}