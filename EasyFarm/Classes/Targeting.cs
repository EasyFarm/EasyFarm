using System;
using System.Threading;
using FFACETools;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Methods for targeting targets.
    /// </summary>
    public class TargetingUtils
    {
        /// <summary>
        ///     Attempts to target by tabbing and on failure will target by memory.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="unit"></param>
        /// <param name="attemptCount"></param>
        /// <returns></returns>
        public static bool TargetByTabAndMemory(FFACE fface, Unit unit, int attemptCount)
        {
            if (fface == null) throw new ArgumentNullException("fface");
            if (unit == null) throw new ArgumentNullException("unit");

            // Attempt to target by tabbing first. 
            if (TargetByTab(fface, unit, attemptCount)) return true;

            // Attempt to target by setting target in memory. 
            // Note: always succeeds since target.ID will be overriden. 
            if (TargetByMemory(fface, unit)) return true;

            return fface.Target.ID == unit.ID;
        }

        /// <summary>
        ///     Targets unit by our target in memory.
        ///     !!! Note !!!
        ///     once you override your target in memory it will be almost impossible
        ///     to determine if you are targeting the wrong target.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool TargetByMemory(FFACE fface, Unit unit)
        {
            if (fface == null) throw new ArgumentNullException("fface");
            if (unit == null) throw new ArgumentNullException("unit");

            // Set target in memory. 
            fface.Target.SetNPCTarget(unit.ID);

            // Face the target. 
            fface.Navigator.FaceHeading(unit.ID);

            // Place cursor upon target. 
            fface.Windower.SendString("/ta <t>");

            return fface.Target.ID == unit.ID;
        }

        /// <summary>
        ///     Targets unit by tabbing and attempts to target until the
        ///     attempt count is reached.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool TargetByTab(FFACE fface, Unit unit, int attemptCount)
        {
            if (fface == null) throw new ArgumentNullException("fface");
            if (unit == null) throw new ArgumentNullException("unit");
            if (attemptCount <= 0)
                throw new ArgumentException("attemptCount does not allow tabbing; possible logic error?");

            // Set view to first person. 
            fface.Navigator.SetViewMode(ViewMode.FirstPerson);

            // Bring up cursor upon player. 
            fface.Windower.SendString("/ta <t>");

            // Attempt to tab to target allowing max ten attempts. 
            var count = 0;
            while (fface.Target.ID != unit.ID && count++ < attemptCount)
            {
                fface.Windower.SendKeyPress(KeyCode.TabKey);
                Thread.Sleep(30);
            }

            return fface.Target.ID == unit.ID;
        }
    }
}