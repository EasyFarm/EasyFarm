using System;
using System.Diagnostics;
using MemoryAPI;
using System.Threading;
using EliteMMO.API;

namespace EasyFarm.Classes
{
    public class Player
    {
        private static Player _instance = new Player();

        public bool IsMoving { get; set; }

        public static Player Instance
        {
            get { return _instance = _instance ?? new Player(); ; }
            private set { _instance = value; }
        }

        /// <summary>
        ///     Makes the character rest
        /// </summary>
        public static void Rest(IMemoryAPI fface)
        {
            if (!fface.Player.Status.Equals(Status.Healing))
            {
                fface.Windower.SendString(Constants.RestingOn);
                TimeWaiter.Pause(50);
            }
        }

        /// <summary>
        ///     Makes the character stop resting
        /// </summary>
        public static void Stand(IMemoryAPI fface)
        {
            if (fface.Player.Status.Equals(Status.Healing))
            {
                fface.Windower.SendString(Constants.RestingOff);
                TimeWaiter.Pause(50);
            }
        }

        /// <summary>
        ///     Switches the player to attack mode on the current unit
        /// </summary>
        public static void Engage(IMemoryAPI fface)
        {
            if (!fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackTarget);
            }
        }

        /// <summary>
        ///     Stop the character from fight the target
        /// </summary>
        public static void Disengage(IMemoryAPI fface)
        {
            if (fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackOff);
            }
        }

        public static void StopRunning(IMemoryAPI fface)
        {
            fface.Navigator.Reset();
            TimeWaiter.Pause(100);
        }

        public static void SetTarget(IMemoryAPI fface, IUnit target)
        {
            if (!Config.Instance.EnableTabTargeting)
            {
                SetTargetUsingMemory(fface, target);
            }
            else
            {
                SetTargetByTabbing(fface, target);
            }
        }

        private static void SetTargetByTabbing(IMemoryAPI fface, IUnit target)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (target.Id != fface.Target.ID)
            {
                if (stopwatch.Elapsed >= TimeSpan.FromSeconds(1))
                {
                    break;
                }

                fface.Windower.SendKeyPress(Keys.TAB);
                TimeWaiter.Pause(200);
            }
        }

        private static void SetTargetUsingMemory(IMemoryAPI fface, IUnit target)
        {
            if (target.Id != fface.Target.ID)
            {
                fface.Target.SetNPCTarget(target.Id);
                fface.Windower.SendString("/ta <t>");
            }
        }
    }
}
