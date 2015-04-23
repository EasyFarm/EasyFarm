using FFACETools;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Contains common methods for moving our character.
    /// </summary>
    public class MovementUtils
    {
        /// <summary>
        ///     Moves to the unit without altering the current
        ///     distance tolerance.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="unit"></param>
        /// <param name="distance"></param>
        public static void MoveToUnit(FFACE fface, Unit unit, double distance)
        {
            // If the target is out of range move into range.
            if (fface.Navigator.DistanceTo(unit.Position) > distance)
            {
                // Save old tolerance
                var old = fface.Navigator.DistanceTolerance;

                // Set to max engagement distance.
                fface.Navigator.DistanceTolerance = distance;

                // Goto target at max engagement distance.
                fface.Navigator.GotoNPC(unit.Id);

                // Restore old tolerance. 
                fface.Navigator.DistanceTolerance = old;
            }
        }
    }
}