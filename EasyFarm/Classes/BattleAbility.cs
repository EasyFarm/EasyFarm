using EasyFarm.Views;
using FFACETools;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroLimits.XITool.Classes;

namespace EasyFarm.Classes
{
    public class BattleAbility : BindableBase
    {
        private bool m_enabled = false;

        /// <summary>
        /// Is the move enabled?
        /// </summary>
        public bool Enabled 
        {
            get { return this.m_enabled; }
            set { SetProperty(ref this.m_enabled, value); }
        }

        private string m_name = String.Empty;

        /// <summary>
        /// The ability's name. I'm using NameX since it seems to 
        /// bind and Name and AbilityName do not. Will fix when I locate 
        /// the problem. 
        /// </summary>
        public String NameX
        {
            get { return this.m_name; }
            set { 
                SetProperty(ref this.m_name, value); 
            }
        }

        private string m_buff = String.Empty;

        /// <summary>
        /// Name of the buff that will be applied when using it. 
        /// </summary>
        public String Buff
        {
            get { return this.m_buff; }
            set { SetProperty(ref this.m_buff, value); }
        }

        private double m_distance = Constants.MELEE_DISTANCE;

        /// <summary>
        /// The distance the move should be used at. 
        /// </summary>
        public double Distance
        {
            get { return this.m_distance; }
            set { SetProperty(ref this.m_distance, value); }
        }

        public Ability m_ability = new Ability();

        /// <summary>
        /// The other ability's attributes. 
        /// </summary>
        public Ability Ability
        {
            get { return this.m_ability; }
            set { SetProperty(ref this.m_ability, value); }
        }

        public BattleAbility() { }

        /// <summary>
        /// Sets the ability field. 
        /// </summary>
        public bool SetAbility()
        {
            // We've already parsed the ability. 
            if (this.Ability.IsValidName) return true; 

            AbilityService Fetcher = new AbilityService();

            // Retriever all moves with the specified name. 
            var moves = Fetcher.GetAbilitiesWithName(this.NameX);

            if (moves.Any())
            {
                if (moves.Count > 1)
                {
                    // Let user pick one. 
                    this.Ability = new AbilitySelectionBox(this.NameX).SelectedAbility;
                }
                else
                {
                    // Grab the only move. 
                    this.Ability = moves.FirstOrDefault();
                }
            }

            // Return true if we've found and stored any ability. 
            return this.Ability.IsValidName;
        }

        public bool HasEffectWore(FFACE fface)
        {
            // Convert string status effect to enum. 
            StatusEffect status;
            var conversionSuccesful = Enum.TryParse<StatusEffect>(this.Buff, out status);

            // Check if player does not have the status effect. 
            if (conversionSuccesful && !fface.Player.StatusEffects.Contains(status))
            {
                return true;
            }
            
            return false;
        }

        public bool IsCastable(FFACE fface)
        {
            var executor = new AbilityExecutor(fface);

            // Check if the ability is valid. 
            return executor.IsActionValid(this.Ability);
        }

        public bool IsBuff()
        {
            return !String.IsNullOrWhiteSpace(this.Buff);
        }
    }
}