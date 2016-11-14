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
            get { return _instance = _instance ?? new Player(); }
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
        ///     Stop the character from fight the target
        /// </summary>
        public static void Disengage(IMemoryAPI fface)
        {
            if (fface.Player.Status.Equals(Status.Fighting))
            {
                fface.Windower.SendString(Constants.AttackOff);
            }
        }

        /// <summary>
        /// Makes the player stop moving. 
        /// </summary>
        /// <param name="fface"></param>
        public static void StopRunning(IMemoryAPI fface)
        {
            fface.Navigator.Reset();
            Thread.Sleep(100);
        }

        /// <summary>
        /// Makes the player move closer towards the target mob. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fface"></param>
        public static void ApproachMob(Unit target, IMemoryAPI fface)
        {
            // Has the user decided that we should approach targets?
            if (Config.Instance.IsApproachEnabled)
            {
                // Move to target if out of melee range. 
                if (target.Distance > Config.Instance.MeleeDistance)
                {
                    // Move to unit at max buff distance. 
                    fface.Navigator.DistanceTolerance = Config.Instance.MeleeDistance;
                    fface.Navigator.GotoNPC(target.Id, Config.Instance.IsObjectAvoidanceEnabled);
                }
            }
        }

        /// <summary>
        /// Make player look at the target mob. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fface"></param>
        public static void FaceMob(Unit target, IMemoryAPI fface)
        {
            // Face mob. 
            fface.Navigator.FaceHeading(target.Position);
        }

        /// <summary>
        /// Places cursor on target mob. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fface"></param>
        public static void SetTarget(Unit target, IMemoryAPI fface)
        {
            // target mob if not currently targeted. 
            if (target.Id != fface.Target.ID)
            {
                // Set as target. 
                fface.Target.SetNPCTarget(target.Id);
                fface.Windower.SendString("/ta <t>");
            }
        }

        /// <summary>
        /// Make the player pull out weapon and engage target. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fface"></param>
        public static void Engage(Unit target, IMemoryAPI fface)
        {
            // Has the user decided we should engage in battle. 
            if (Config.Instance.IsEngageEnabled)
            {
                // Not engaged and in range. 
                if (!fface.Player.Status.Equals(Status.Fighting) && target.Distance < 25)
                {
                    // Engage the target. 
                    fface.Windower.SendString(Constants.AttackTarget);
                }
            }
        }

        public static void SwitchTarget(Unit target, IMemoryAPI fface)
        {
            if (target.Id != fface.Target.ID)
            {
                Player.Disengage(fface);
                Player.FaceMob(target, fface);
                Player.SetTarget(target, fface);
                Player.Engage(target, fface);
            }
        }
    }
}
