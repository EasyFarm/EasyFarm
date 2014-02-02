using System.Collections.Generic;
using System.Linq;
using EasyFarm.Engine;
using EasyFarm.UnitTools;
using FFACETools;
using System;

namespace EasyFarm.PlayerTools
{
    public class Player
    {
        /// <summary>
        /// Reference to the game engine object. 
        /// Modifications to the changes the object for everyone.
        /// </summary>
        private GameEngine Engine;

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit TargetUnit { get { return Engine.Units.Target; } }
        
        /// <summary>
        /// Command for resting
        /// </summary>
        const string RESTING_ON = "/heal on";
        
        /// <summary>
        /// Command for stopping resting
        /// </summary>
        const string RESTING_OFF = "/heal off";
        
        /// <summary>
        /// Command for disengaging
        /// </summary>
        const string ATTACK_OFF = "/attack off";
        
        /// <summary>
        /// Command for engaging
        /// </summary>
        const string ATTACK_TARGET = "/attack <t>";

        /// <summary>
        /// Command for engaging a target
        /// </summary>
        const string ATTACK_ON = "/attack on";
        
        /// <summary>
        /// Min distance we need to maintain from the enemy
        /// </summary>
        const int DIST_MIN = 3;
        
        /// <summary>
        /// Max distance we can be from the enemy
        /// </summary>
        const int DIST_MAX = 5;

        /// <summary>
        /// Use to be used for Serialize and Deserialize methods in
        /// Untilities.cs
        /// </summary>
        private Player() { }

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

        /// <summary>
        /// Is our players status equal to dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
                return Engine.FFInstance.Instance.Player.Status == Status.Dead1 ||
                    Engine.FFInstance.Instance.Player.Status == Status.Dead2;
            }
        }

        /// <summary>
        /// Should we rest by way of /heal?
        /// </summary>
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

        /// <summary>
        /// Should we heal by way of abilities and spells?
        /// </summary>
        public bool shouldHeal
        {
            get
            {
                return Engine.IsWorking && !IsDead && HealingList.Count > 0;
            }
        }

        /// <summary>
        /// Should we move to the next waypoint?
        /// </summary>
        public bool shouldTravel
        {
            get
            {
                return Engine.Config.Waypoints.Count > 0 && Engine.Units.Target.ID == 0 && !shouldRest && !shouldHeal && !IsUnable;
            }
        }

        /// <summary>
        /// Returns the list of usable pulling abilities and spells.
        /// </summary>
        public List<Ability> StartList
        {
            get { return FilterValidActions(Engine.Config.StartList); }
        }

        /// <summary>
        /// Returns the list of usable ending abilities and spells.
        /// </summary>
        public List<Ability> EndList
        {
            get { return FilterValidActions(Engine.Config.EndList); }
        }

        /// <summary>
        /// Returns the list of usable battle abilities and spells.
        /// </summary>
        public List<Ability> BattleList
        {
            get { return FilterValidActions(Engine.Config.BattleList); }
        }

        /// <summary>
        /// Returns the list of currently usuable Healing Abilities and Spells.
        /// </summary>
        public List<Ability> HealingList
        {
            get
            {
                return FilterValidActions(
                        Engine.Config.HealingList
                            .Where(x => x.Item.IsEnabled)
                            .Where(x => x.Item.TriggerLevel >= Engine.FFInstance.Instance.Player.HPPCurrent)
                            .Select(x => new Ability(x.Item.Name)).ToList()
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
                                    IsFighting && TargetUnit.Distance < Engine.Config.Weaponskill.MaxDistance &&
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

        /// <summary>
        /// Can we use job abilities?
        /// </summary>
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
            get { return TargetUnit.Status == Status.Fighting; }
        }

        /// <summary>
        /// Returns true if the units health is 0%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitDead
        {
            get { return TargetUnit.HPPCurrent <= 0; }
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
            if (IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_OFF); }
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        public void RestingOn()
        {
            if (!IsResting) { Engine.FFInstance.Instance.Windower.SendString(RESTING_ON); }
        }

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit()
        {
            Engine.FFInstance.Instance.Navigator.Goto(TargetUnit.Position, false);
        }

        /// <summary>
        /// Places the cursor on the enemy
        /// </summary>
        public void Target()
        {
            if (!IsTargetUnit)
            {
                Engine.FFInstance.Instance.Target.SetNPCTarget(TargetUnit.ID);
                // Engine.FFInstance.Instance.Windower.SendKeyPress(FFACETools.KeyCode.TabKey);
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
                ExecuteActions(StartList); 
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
                Engine.FFInstance.Instance.Windower.SendString(ATTACK_TARGET);
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
            if (IsFighting && IsTargetUnit)
            {
                // Execute all the battle moves
                if (HasBattleMoves) { 
                    ExecuteActions(BattleList); 
                }

                // Execute the weaponskill
                else if (PlayerCanWeaponskill) {
                    ExecuteActions(new List<Ability>() { Engine.Config.Weaponskill });
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
            if (IsEngaged) {
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
                ExecuteActions(EndList);
                Disengage();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Other
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

        /// <summary>
        /// Returns the list of usable abilities
        /// </summary>
        /// <param name="Actions"></param>
        /// <returns></returns>
        public List<Ability> FilterValidActions(IList<Ability> Actions)
        { 
            return Actions
                    .Where(x => x.IsValidName)
                    .Where(x => Engine.Player.AbilityRecastable(x))
                    .Where(x => x.MPCost <= Engine.FFInstance.Instance.Player.MPCurrent && x.TPCost <= Engine.FFInstance.Instance.Player.TPCurrent)
                    .Where(x => (x.IsSpell && !IsCastingBlocked) || (x.IsAbility && !IsAbilitiesBlocked))
                    .ToList();
        }

        /// <summary>
        /// Performs a list of actions. 
        /// Could be spells or job abilities. 
        /// Does not validate actions.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="unit"></param>
        public void ExecuteActions(IList<Ability> actions)
        {
            foreach (var act in actions)
            {
                MaintainHeading();
                UseAbility(act);
            }
        }

        /// <summary>
        /// Attempts to use the passed in ability
        /// </summary>
        /// <param name="Ability"></param>
        public void UseAbility(Ability Ability)
        {
            // Set the duration to spell time or 50 for an ability
            int SleepDuration = Ability.IsSpell ? (int)Engine.FFInstance.Instance.Player.CastMax + 1000 : 50;

            // Send it to the game
            Engine.FFInstance.Instance.Windower.SendString(Ability.ToString());

            // Sleep the duration
            System.Threading.Thread.Sleep(SleepDuration);

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
    }
}