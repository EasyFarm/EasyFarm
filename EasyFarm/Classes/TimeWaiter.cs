using System.Threading;

namespace EasyFarm.Classes
{
    public class TimeWaiter
    {
        public static bool IsEnabled { get; set; } = true;

        public static void Pause(int milliseconds)
        {
            if (IsEnabled)
            {
                Thread.Sleep(milliseconds);
            }
        }
    }
}
