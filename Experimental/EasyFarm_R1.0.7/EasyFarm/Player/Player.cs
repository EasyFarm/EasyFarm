using System.Collections.Generic;
using System.Linq;
using EasyFarm.Engine;
using EasyFarm.UnitTools;
using FFACETools;

namespace EasyFarm.PlayerTools
{
    public class Player
    {
        private GameEngine Engine;
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
        public Player(ref GameEngine Engine): this()
        {
            this.Engine = Engine;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Info
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool HasBattleMoves
        {
            get { return Engine.Config.BattleList.Count > 0; }
        }

        public bool HasStartMoves
        {
            get { return Engine.Config.StartList.Count > 0; }
        }

        public bool HasEndMoves
        {
            get { return Engine.Config.EndList.Count > 0; }
        }

        /// <summary>
        /// If we have more than zero hp,
        /// return true
        /// </summary>
        /// <returns></returns>
        public bool HasHitpoints
        {
            get
            {
                return Engine.FFInstance.Instance.Player.HPCurrent > 0;
            }
        }

        /// <summary>
        /// Returns true if our player is healing.
        /// </summary>
        /// <returns></returns>
        public bool IsResting
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Healing;
            }
        }

        /// <summary>
        /// Are we near the target unit?
        /// </summary>
        /// <returns>True if we are near our target unit</returns>
        public bool IsNear
        {
            get
            {
                return Engine.FFInstance.Instance.Navigator.DistanceTo(TargetUnit.Position) <= DIST_MAX;
            }
        }

        /// <summary>
        /// Is our status == fighting
        /// </summary>
        /// <returns></returns>
        public bool IsFighting
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Fighting;
            }
        }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool PlayerCanWeaponskill
        {
            get
            {
                return Engine.FFInstance.Instance.Player.TPCurrent >= Engine.Config.Weaponskill.TPCost &&
                                    TargetUnit.HPPCurrent <= Engine.Config.WSHealthThreshold &&
                                    IsFighting &&
                                    TargetUnit.Distance < Engine.Config.Weaponskill.MaxDistance &&
                                    Engine.Config.Weaponskill.IsValidName;
            }
        }

        /// <summary>
        /// Returns true if we can not cast a spell.
        /// </summary>
        /// <returns></returns>
        public bool IsCastingBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Silence,
                StatusEffect.Mute
            };

                // If we have effects that block,
                // return true.
                bool unableToCast = effectsThatBlock
                    .Intersect(this.Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                // 
                bool unableToReact = IsUnable;

                return unableToCast || unableToReact;
            }
        }

        /// <summary>
        /// Returns true if we have effects that inhibit us
        /// from taking any kind of action.
        /// </summary>
        /// <returns></returns>
        public bool IsUnable
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Petrification, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Stun, 
                StatusEffect.Chocobo, StatusEffect.Terror, 
            };

                bool IsPlayerUnable = effectsThatBlock
                    .Intersect(Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsPlayerUnable;
            }
        }

        public bool IsAbilitiesBlocked
        {
            get
            {
                StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Amnesia
            };

                bool IsAbilitiesBlocked = effectsThatBlock
                    .Intersect(Engine.FFInstance.Instance.Player.StatusEffects)
                    .Count() != 0;

                return IsAbilitiesBlocked || IsUnable;
            }
        }

        /// <summary>
        /// Determines low hp status.
        /// </summary>
        /// <returns></returns>
        public bool IsInjured
        {
            get
            {
                return Engine.FFInstance.Instance.Player.HPPCurrent <= Engine.Config.LowHP;
            }
        }

        /// <summary>
        /// Does our character have aggro
        /// </summary>
        /// <returns></returns>
        public bool IsAggroed
        {
            get
            {
                return Engine.Units.HasAggro;
            }
        }

        /// <summary>
        /// Does our player have a status effect that prevents him
        /// </summary>
        /// <param name="playerStatusEffects"></param>
        /// <returns></returns>
        public bool IsRestingBlocked
        {
            get
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

                return RestBlockingDebuffs.Intersect(Engine.FFInstance.Instance.Player.StatusEffects).Count() != 0;
            }
        }

        /// <summary>
        /// Returns true if our player is able to
        /// safely rest (/heal).
        /// </summary>
        /// <returns></returns>
        public bool CanPlayerRest
        {
            get
            {
                return (IsInjured && !IsAggroed && HasHitpoints && !IsRestingBlocked);
            }
        }

        /// <summary>
        /// Same as IsFighting. Returns true if we are fighting the target.
        /// </summary>
        /// <returns></returns>
        public bool IsEngaged
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Fighting;
            }
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
                Recast = Engine.FFInstance.Instance.Timer.GetSpellRecast((SpellList)value.Index);
            }
            else
            {
                Recast = Engine.FFInstance.Instance.Timer.GetAbilityRecast((AbilityList)value.Index);
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
        public bool IsUnitPullable
        {
            get
            {
                return IsTargetUnit && !IsUnitFighting && HasStartMoves;
            }
        }

        /// <summary>
        /// Is our current target our target unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsTargetUnit
        {
            get
            {
                return Engine.FFInstance.Instance.Target.ID == TargetUnit.ID;
            }
        }

        /// <summary>
        /// Is the targets status == fighting
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitFighting
        {
            get
            {
                return TargetUnit.Status == Status.Fighting;
            }
        }

        /// <summary>
        /// Returns true if the units health is 0%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitDead
        {
            get
            {
                return TargetUnit.HPPCurrent <= 0;
            }
        }

        /// <summary>
        /// Can we battle the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitBattleReady
        {
            get
            {
                return Engine.Units.IsValid(TargetUnit);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Commands
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        public void RestingOff()
        {
            if (IsResting)
            {
                Engine.FFInstance.Instance.Windower.SendString(RESTING_OFF);
            }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void RestingOn()
        {
            if (!IsResting)
            {
                Engine.FFInstance.Instance.Windower.SendString(RESTING_ON);
            }
        }

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit()
        {
            Engine.FFInstance.Instance.Navigator.GotoTarget();
        }

        public void Target()
        {
            if (!IsTargetUnit)
            {
                Engine.FFInstance.Instance.Windower.SendKeyPress(FFACETools.KeyCode.TabKey);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull()
        {
            if (IsUnitPullable)
            {
                ExecuteActions(Engine.Config.StartList);
            }
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage()
        {
            // if we have a correct target and our player isn't currently fighting...
            if (IsTargetUnit && !IsFighting)
            {
                Engine.FFInstance.Instance.Windower.SendString(ATTACK_ON);
            }

            // Wrong Target Attacked
            else if (!IsTargetUnit && IsFighting)
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
            if (IsFighting)
            {
                if (HasBattleMoves)
                {
                    ExecuteActions(Engine.Config.BattleList);
                }

                else if (PlayerCanWeaponskill)
                {
                    Engine.FFInstance.Instance.Windower.SendString(Engine.Config.Weaponskill.ToString());
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            Engine.FFInstance.Instance.Navigator.FaceHeading(TargetUnit.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            if (IsEngaged)
            {
                Engine.FFInstance.Instance.Windower.SendString(ATTACK_OFF);
            }
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
        public void ExecuteActions(IList<Ability> actions)
        {
            var ValidActions = actions
                .Where(x => AbilityRecastable(x))
                .Where(x => Engine.FFInstance.Instance.Player.TPCurrent >= x.TPCost)
                .Where(x => Engine.FFInstance.Instance.Player.MPCurrent >= x.MPCost)
                .Where(x => (x.IsSpell && !IsCastingBlocked) || (x.IsAbility && !IsUnable));

            foreach (var act in ValidActions)
            {
                int SleepDuration = act.IsSpell ? (int)Engine.FFInstance.Instance.Player.CastMax + 500 : 50;
                MaintainHeading();
                Engine.FFInstance.Instance.Windower.SendString(act.ToString());
            }
        }

        /// <summary>
        /// Starts up the bot for combat
        /// </summary>
        /// <param name="unit"></param>
        public void Enter()
        {
            Engine.FFInstance.Instance.Navigator.DistanceTolerance = DIST_MIN;
            Engine.FFInstance.Instance.Navigator.HeadingTolerance = DIST_MAX;
        }

        /// <summary>
        /// Clean up for path traveling and 
        /// peform any end battle moves
        /// </summary>
        /// <param name="unit"></param>
        public void Exit()
        {            
            if (IsUnitDead && HasEndMoves)
            {
                ExecuteActions(Engine.Config.EndList);
                Disengage();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // End
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

        public bool IsDead
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Dead1 ||
                    Engine.FFInstance.Instance.Player.Status == Status.Dead2;
            }
        }

        public bool shouldRest
        {
            get
            {
                bool IsHPAtTriggerLevel = Engine.FFInstance.Instance.Player.HPPCurrent <= Engine.Config.LowHP;
                bool IsMPAtTriggerLevel = Engine.FFInstance.Instance.Player.MPPCurrent <= Engine.Config.LowMP;
                bool NeedsMoreHP = IsResting && Engine.FFInstance.Instance.Player.HPPCurrent < Engine.Config.HighHP;
                bool NeedsMoreMP = IsResting && Engine.FFInstance.Instance.Player.MPPCurrent < Engine.Config.HighMP;
                bool RestingPossible = (Engine.Config.IsRestingHPEnabled || Engine.Config.IsRestingMPEnabled) && !IsRestingBlocked && !IsAggroed && !IsFighting && !IsDead;
                return RestingPossible && ((IsHPAtTriggerLevel || NeedsMoreHP) || (IsMPAtTriggerLevel || NeedsMoreMP));
            }
        }

        public bool shouldHeal
        { 
            get 
            {
                return Engine.IsWorking && !IsDead && HealingList.Count > 0;
            }
        }

        public bool shouldTravel
        {
            get
            {
                return Engine.Config.Waypoints.Count > 0 && Engine.Units.Target.ID == 0 && !shouldRest && !shouldHeal && !IsUnable;
            }
        }

        public List<Ability> HealingList
        {
            get 
            { 
                return Engine.Config.HealingList
                    .Where(x => x.Item.IsEnabled)
                    .Where(x => x.Item.TriggerLevel >= Engine.FFInstance.Instance.Player.HPPCurrent)
                    .Select(x => new Ability(x.Item.Name))
                    .Where(x => x.IsValidName)
                    .Where(x => Engine.Player.AbilityRecastable(x))
                    .Where(x => x.MPCost <= Engine.FFInstance.Instance.Player.MPCurrent && x.TPCost <= Engine.FFInstance.Instance.Player.TPCurrent)
                    .Where(x => (x.IsSpell && !IsCastingBlocked) || (x.IsAbility && !IsAbilitiesBlocked)).ToList();
            }
        }
    }
}