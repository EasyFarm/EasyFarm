using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Xml.Serialization;
using EasyFarm.Engine;
using EasyFarm.UnitTools;
using FFACETools;

namespace EasyFarm.PlayerTools
{
    public class Player
    {
        private GameState GameState;
        public Unit TargetUnit { get; set; }

        const string RESTING_ON = "/heal on";
        const string RESTING_OFF = "/heal off";
        const string ATTACK_OFF = "/attack off";
        const string ATTACK_ON = "/attack on";

        const int DIST_MIN = 3;
        const int DIST_MAX = 5;

        private Player()
        {
            TargetUnit = Unit.CreateUnit(0);
        }

        /// <summary>
        /// Sets up pathing, fface and units objs and
        /// initializes timers and threads.
        /// </summary>
        /// <param name="session"></param>
        public Player(ref GameState GameState): this()
        {
            this.GameState = GameState;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Info
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// If we have more than zero hp,
        /// return true
        /// </summary>
        /// <returns></returns>
        public bool HasHitpoints()
        {
            return GameState.FFInstance.Instance.Player.HPCurrent > 0;
        }

        /// <summary>
        /// Returns true if our player is healing.
        /// </summary>
        /// <returns></returns>
        public bool IsResting()
        {
            return GameState.FFInstance.Instance.Player.Status == Status.Healing;
        }

        /// <summary>
        /// Are we near the target unit?
        /// </summary>
        /// <returns>True if we are near our target unit</returns>
        public bool IsNear()
        {
            return GameState.FFInstance.Instance.Navigator.DistanceTo(TargetUnit.Position) <= DIST_MAX;
        }

        /// <summary>
        /// Is our status == fighting
        /// </summary>
        /// <returns></returns>
        public bool IsFighting()
        {
            return GameState.FFInstance.Instance.Player.Status == Status.Fighting;
        }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool PlayerCanWeaponskill()
        {
            return GameState.FFInstance.Instance.Player.TPCurrent >= GameState.Config.Weaponskill.TPCost &&
                                TargetUnit.HPPCurrent <= GameState.Config.WeaponSkillHP &&
                                IsFighting() && IsUnitFighting() &&
                                TargetUnit.Distance < GameState.Config.Weaponskill.MaxDistance &&
                                GameState.Config.Weaponskill.IsValidName;
        }

        /// <summary>
        /// Returns true if we can not cast a spell.
        /// </summary>
        /// <returns></returns>
        public bool IsCastingBlocked()
        {
            StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Silence,
                StatusEffect.Mute
            };

            // If we have effects that block,
            // return true.
            bool unableToCast = effectsThatBlock
                .Intersect(this.GameState.FFInstance.Instance.Player.StatusEffects)
                .Count() != 0;

            // 
            bool unableToReact = IsUnable();

            return unableToCast || unableToReact;
        }

        /// <summary>
        /// Returns true if we have effects that inhibit us
        /// from taking any kind of action.
        /// </summary>
        /// <returns></returns>
        public bool IsUnable()
        {
            StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Petrification, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Stun, 
                StatusEffect.Chocobo, StatusEffect.Terror, 
            };

            bool IsPlayerUnable = effectsThatBlock
                .Intersect(GameState.FFInstance.Instance.Player.StatusEffects)
                .Count() != 0;

            return IsPlayerUnable;
        }

        public bool IsAbilitiesBlocked()
        {
            StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Amnesia
            };

            bool IsAbilitiesBlocked = effectsThatBlock
                .Intersect(GameState.FFInstance.Instance.Player.StatusEffects)
                .Count() != 0;

            return IsAbilitiesBlocked || IsUnable();
        }

        /// <summary>
        /// Determines low hp status.
        /// </summary>
        /// <returns></returns>
        public bool IsInjured()
        {
            return GameState.FFInstance.Instance.Player.HPPCurrent <= GameState.Config.RestingValue;
        }

        /// <summary>
        /// Does our character have aggro
        /// </summary>
        /// <returns></returns>
        public bool IsAggroed()
        {
            return GameState.Units.HasAggro;
        }

        /// <summary>
        /// Does our player have a status effect that prevents him
        /// </summary>
        /// <param name="playerStatusEffects"></param>
        /// <returns></returns>
        public bool IsRestingBlocked()
        {
            var RestBlockingDebuffs = new List<StatusEffect>() 
            { 
                StatusEffect.Poison, StatusEffect.Bio, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Poison, StatusEffect.Petrification,
                StatusEffect.Stun, StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Terror, StatusEffect.Frost, StatusEffect.Burn, 
                StatusEffect.Choke, StatusEffect.Rasp, StatusEffect.Shock, 
                StatusEffect.Drown, StatusEffect.Dia, StatusEffect.Requiem, 
                StatusEffect.Lullaby
            };

            return RestBlockingDebuffs.Intersect(GameState.FFInstance.Instance.Player.StatusEffects).Count() != 0;
        }

        /// <summary>
        /// Returns true if our player is able to
        /// safely rest (/heal).
        /// </summary>
        /// <returns></returns>
        public bool CanPlayerRest()
        {
            return (GameState.FFInstance.Instance.Player.HPPCurrent < GameState.Config.StandUPValue && !IsAggroed() && HasHitpoints() && !IsRestingBlocked());
        }

        /// <summary>
        /// Same as IsFighting. Returns true if we are fighting the target.
        /// </summary>
        /// <returns></returns>
        public bool IsEngaged()
        {
            return GameState.FFInstance.Instance.Player.Status == Status.Fighting;
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
                Recast = GameState.FFInstance.Instance.Timer.GetSpellRecast((SpellList)value.Index);
            }
            else
            {
                Recast = GameState.FFInstance.Instance.Timer.GetAbilityRecast((AbilityList)value.Index);
            }

            return 0 == Recast;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Target Info
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Can we pull the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitPullable()
        {
            return IsTargetUnit() && !IsUnitFighting() && GameState.Config.Actions[ActionType.Enter].Count > 0;
        }

        /// <summary>
        /// Is our current target our target unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsTargetUnit()
        {
            return GameState.FFInstance.Instance.Target.ID == TargetUnit.ID;
        }

        /// <summary>
        /// Is the targets status == fighting
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitFighting()
        {
            return TargetUnit.Status == Status.Fighting;
        }

        /// <summary>
        /// Returns true if the units health is 0%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitDead()
        {
            return TargetUnit.HPPCurrent <= 0;
        }

        /// <summary>
        /// Can we battle the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitBattleReady()
        {
            return GameState.Units.IsValid(TargetUnit);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Commands
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void RestingOff()
        {
            GameState.FFInstance.Instance.Windower.SendString(RESTING_OFF);
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void RestingOn()
        {
            GameState.FFInstance.Instance.Windower.SendString(RESTING_ON);
        }

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit()
        {
            GameState.FFInstance.Instance.Navigator.GotoTarget();
        }

        public void Target()
        {
            if (!IsTargetUnit())
            {
                GameState.FFInstance.Instance.Windower.SendKeyPress(FFACETools.KeyCode.TabKey);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull()
        {
            if (IsUnitPullable())
            {
                ExecuteActions(GameState.Config.Actions[ActionType.Enter]);
            }
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage()
        {
            // if we have a correct target and our player isn't currently fighting...
            if (IsTargetUnit() && !IsFighting())
            {
                GameState.FFInstance.Instance.Windower.SendString(ATTACK_ON);
            }

            // Wrong Target Attacked
            else if (!IsTargetUnit() && IsFighting())
            {
                Disengage();
            }
        }

        /// <summary>
        /// Peforms our rotation to kill our
        /// target unit.
        /// </summary>
        /// <param name="unit"></param>
        public void Kill()
        {
            if (IsFighting())
            {
                if (GameState.Config.Actions[ActionType.Battle].Count > 0)
                {
                    ExecuteActions(GameState.Config.Actions[ActionType.Battle]);
                }

                else if (PlayerCanWeaponskill())
                {
                    GameState.FFInstance.Instance.Windower.SendString(GameState.Config.Weaponskill.ToString());
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            GameState.FFInstance.Instance.Navigator.FaceHeading(TargetUnit.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            GameState.FFInstance.Instance.Windower.SendString(ATTACK_OFF);
        }

        /// <summary>
        /// Fights a unit from start to finish.
        /// </summary>
        /// <param name="unit"></param>
        public void Battle()
        {
            MoveToUnit();
            MaintainHeading();

            Target();
            Pull();
            Engage();
            MoveToUnit();
            Kill();
        }

        /// <summary>
        /// Performs a list of actions. 
        /// Could be spells or job abilities.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="unit"></param>
        public void ExecuteActions(List<Ability> actions)
        {
            var ValidActions = actions
                .Where(x => AbilityRecastable(x))
                .Where(x => GameState.FFInstance.Instance.Player.TPCurrent >= x.TPCost)
                .Where(x => GameState.FFInstance.Instance.Player.MPCurrent >= x.MPCost)
                .Where(x => (x.IsSpell && !IsCastingBlocked()) || (x.IsAbility && !IsUnable()));

            foreach (var act in ValidActions)
            {
                int SleepDuration = act.IsSpell ? (int)GameState.FFInstance.Instance.Player.CastMax + 500 : 50;
                MaintainHeading();
                GameState.FFInstance.Instance.Windower.SendString(act.ToString());
            }
        }

        /// <summary>
        /// Starts up the bot for combat
        /// </summary>
        /// <param name="unit"></param>
        public void Enter()
        {
            GameState.FFInstance.Instance.Navigator.DistanceTolerance = DIST_MIN;
            GameState.FFInstance.Instance.Navigator.HeadingTolerance = DIST_MAX;
        }

        /// <summary>
        /// Clean up for path traveling and 
        /// peform any end battle moves
        /// </summary>
        /// <param name="unit"></param>
        public void Exit()
        {
            Disengage();

            if (IsUnitDead() && GameState.Config.Actions[ActionType.Exit].Count > 0)
            {
                ExecuteActions(GameState.Config.Actions[ActionType.Exit]);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // End
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    }
}