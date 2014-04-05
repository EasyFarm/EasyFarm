using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// This class is responsible for holding all of the abilities our character may use in battle.
    /// </summary>
    public class PlayerActions
    {
        private Config Config;
        private FFACE.PlayerTools PlayerTools;
        private FFACE.TimerTools TimerTools;
        private AbilityService AbilityService;
        private ActionBlocked ActionBlocked;
        private GameEngine m_gameEngine;


        public PlayerActions(ref GameEngine m_gameEngine)
        {
            this.m_gameEngine = m_gameEngine;

            this.AbilityService = m_gameEngine.AbilityService;
            this.Config = m_gameEngine.Config;
            this.ActionBlocked = m_gameEngine.ActionBlocked;
            this.TimerTools = m_gameEngine.FFInstance.Instance.Timer;
            this.PlayerTools = m_gameEngine.FFInstance.Instance.Player;
        }

        /// <summary>
        /// Returns the list of usable pulling abilities and spells.
        /// </summary>
        public List<Ability> StartList
        {
            get { return FilterValidActions(Config.ActionInfo.StartList); }
        }

        /// <summary>
        /// Returns the list of usable ending abilities and spells.
        /// </summary>
        public List<Ability> EndList
        {
            get { return FilterValidActions(Config.ActionInfo.EndList); }
        }

        /// <summary>
        /// Returns the list of usable battle abilities and spells.
        /// </summary>
        public List<Ability> BattleList
        {
            get { return FilterValidActions(Config.ActionInfo.BattleList); }
        }

        /// <summary>
        /// Returns the list of currently usuable Healing Abilities and Spells.
        /// </summary>
        public List<Ability> HealingList
        {
            get
            {
                return FilterValidActions(
                        Config.ActionInfo.HealingList
                            .Where(x => x.Item.IsEnabled)
                            .Where(x => x.Item.TriggerLevel >= PlayerTools.HPPCurrent)
                            .Select(x => AbilityService.CreateAbility(x.Item.Name))
                            .ToList()
                    );
            }
        }

        /// <summary>
        /// Do we have moves we can use in battle again the creature?
        /// </summary>
        public bool HasBattleMoves
        {
            get { return BattleList.Count > 0; }
        }

        /// <summary>
        /// Do we have any moves we can pull the enemy with?
        /// </summary>
        public bool HasStartMoves
        {
            get { return StartList.Count > 0; }
        }

        /// <summary>
        /// Do we have instruction on what to do when the creature is dead?
        /// </summary>
        public bool HasEndMoves
        {
            get { return EndList.Count > 0; }
        }

        /// <summary>
        /// Do we have any moves that can heal the player.
        /// </summary>
        public bool HasHealingMoves
        {
            get { return HealingList.Count > 0; }
        }

        /// <summary>
        /// Returns the list of usable abilities
        /// </summary>
        /// <param name="Actions"></param>
        /// <returns></returns>
        public List<Ability> FilterValidActions(IList<Ability> Actions)
        {
            return Actions
                    .Where(x => x.IsValidName)
                    .Where(x => AbilityRecastable(x))
                    .Where(x => x.MPCost <= PlayerTools.MPCurrent && x.TPCost <= PlayerTools.TPCurrent)
                    .Where(x => (x.IsSpell && !ActionBlocked.IsCastingBlocked) || (x.IsAbility && !ActionBlocked.IsAbilitiesBlocked))
                    .ToList();
        }

        /// <summary>
        /// Checks to  see if we can cast/use 
        /// a job ability or spell.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AbilityRecastable(Ability value)
        {
            int Recast = -1;

            if (value.IsSpell)
            {
                Recast = TimerTools.GetSpellRecast((SpellList)value.Index);
            }
            else
            {
                Recast = TimerTools.GetAbilityRecast((AbilityList)value.Index);
            }

            return 0 == Recast;
        }
    }
}
