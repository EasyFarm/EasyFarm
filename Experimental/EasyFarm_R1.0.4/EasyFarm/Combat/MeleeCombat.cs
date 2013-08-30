using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyFarm.Profiles;
using EasyFarm.Engine;
using EasyFarm.Interfaces;

namespace EasyFarm.Combat
{
    class MeleeCombat : ICombat
    {
        private IProfile Profile;
        private GameState GameState;

        public MeleeCombat(IProfile Profile, ref GameState GameState)
        {
            this.Profile = Profile;
            this.GameState = GameState;
        }

        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Engage()
        {
            throw new NotImplementedException();
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
