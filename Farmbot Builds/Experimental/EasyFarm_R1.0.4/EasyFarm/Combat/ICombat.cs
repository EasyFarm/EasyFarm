using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Combat
{
    interface ICombat
    {
        void Enter();
        void Engage();      
        void Attack();    
        void Exit();
    }
}
