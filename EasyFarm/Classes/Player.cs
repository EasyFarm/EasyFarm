using MemoryAPI;
using System.Threading;

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
                Thread.Sleep(50);
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
                Thread.Sleep(50);
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
    }
}
