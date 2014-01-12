using EasyFarm.Engine;
using EasyFarm.PlayerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.FSM
{
    class HealingState : BaseState
    {
        public HealingState(ref GameEngine gameEngine) : base(ref gameEngine) { }

        public override bool CheckState()
        {
            return gameEngine.Player.shouldHeal && !gameEngine.Player.shouldRest;
        }

        public override void EnterState()
        {
            gameEngine.Player.RestingOff();
        }

        public override void RunState()
        {
            if(gameEngine.Player.HealingList.Count > 0)
            {
                var act = gameEngine.Player.HealingList.FirstOrDefault();
                if (act == null) { return; }
                int SleepDuration = act.IsSpell ? (int)gameEngine.FFInstance.Instance.Player.CastMax + 500 : 50;
                gameEngine.FFInstance.Instance.Windower.SendString(act.ToString());
                System.Threading.Thread.Sleep(SleepDuration);
            }
        }

        public override void ExitState() { }
    }
}
