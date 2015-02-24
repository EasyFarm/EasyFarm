using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarmTests.MachineComponentTests
{
    public class TestBattleAbility
    {
        private string _name = string.Empty;
        private bool _enabled = false;
        private bool _isBuff = false;
        private bool _effectWore = false;

        public TestBattleAbility(String name, bool enabled , bool isbuff, bool effectwore)
        {
            _name = name;
            _enabled = enabled;
            _isBuff = isbuff;
            _effectWore = effectwore;
        }

        public string Name 
        {
            get 
            {
                return _name;
            }
        }

        public bool Enabled
        {
            get 
            {
                return _enabled;
            }
        }   

        public bool IsBuff 
        { 
            get 
            { 
                return _isBuff; 
            } 
        }

        public bool HasEffectWore 
        {
            get 
            {
                return _effectWore;
            }
        }
    }
}
