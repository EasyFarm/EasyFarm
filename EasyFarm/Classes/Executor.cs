using EasyFarm.Components;
using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Classes
{
    public class Executor
    {
        private static MovingUnit Player;

        private FFACE FFACE;

        /// <summary>
        /// Must be set by caller. 
        /// </summary>
        public Unit Target { get; set; }

        public Executor(FFACE fface)
        {
            this.FFACE = fface;
            Player = Player ?? new MovingUnit(fface.Player.ID);
        }

        /// <summary>
        /// Execute actions by 
        /// </summary>
        /// <param name="actions"></param>
        public void ExecuteActions(IEnumerable<BattleAbility> actions)
        {
            foreach (var action in actions.ToList())
            {
                // Move to target if out of distance. 
                if (Target.Distance > action.Distance)
                {
                    // Move to unit at max buff distance. 
                    var oldTolerance = FFACE.Navigator.DistanceTolerance;
                    FFACE.Navigator.DistanceTolerance = action.Distance;
                    FFACE.Navigator.GotoNPC(Target.ID);
                    FFACE.Navigator.DistanceTolerance = action.Distance;
                }

                // Face unit
                FFACE.Navigator.FaceHeading(Target.Position);

                // Target mob if not currently targeted. 
                if (Target.ID != FFACE.Target.ID)
                {
                    FFACE.Target.SetNPCTarget(Target.ID);
                    FFACE.Windower.SendString("/ta <t>");
                }

                // Stop bot from running. 
                if (Player.IsMoving)
                {
                    Thread.Sleep(500);
                    FFACE.Navigator.Reset();
                }


                // Fire the spell off. 
                FFACE.Windower.SendString(action.Ability.ToString());
            }
        }
    }
}
