using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using EasyFarm.PathingTools;
using EasyFarm.UnitTools;
using FFACETools;
using System.Threading;
using System.Xml.Serialization;

namespace EasyFarm.PlayerTools
{
    [Serializable]
    public class Player
    {
        public Pathing Pathing;
        
        public WeaponAbility Weaponskill = new WeaponAbility();

        public Units Units;       

        // Moves
        public List<Ability> StartingResponses = new List<Ability>();
        public List<Ability> CombatResponses = new List<Ability>();
        public List<Ability> EndingResponses = new List<Ability>();
        public List<Ability> HealingResponses = new List<Ability>();
        public List<Ability> DebuffResponses = new List<Ability>();

        // Resting
        public int StandUPHPPValue { get; set; }
        public int SitdownHPPValue { get; set; }
        public int WeaponSkillHP { get; set; }

        public bool KillAggro = true;
        public bool KillPartyClaimed = true;
        public bool KillUnclaimed = true;

         [XmlIgnore()] 
        public bool IsWorking = false;

         [XmlIgnore()] 
        private BackgroundWorker WorkerThread = new BackgroundWorker();

         [XmlIgnore()] 
        private DispatcherTimer Timer = new DispatcherTimer();

         [XmlIgnore()] 
        private FFACE.Position LastPosition = new FFACE.Position();

         [XmlIgnore()] 
        public FFACE Session;

        // Events
        public delegate void Handler();        
        public static event Handler OnFinish;
        public static event Handler OnStart;

        public Player()
        {

        }

        /// <summary>
        /// Sets up pathing, fface and units objs and
        /// initializes timers and threads.
        /// </summary>
        /// <param name="session"></param>
        public Player(FFACE session)
        {
            Session = session;
            Pathing = new Pathing(Session);
            Units = new Units(Session);

            WorkerThread.DoWork += new DoWorkEventHandler(BotThread);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            WorkerThread.WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// Sets the path for our bot and the sequence,
        /// that resting and battle should occur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotThread(object sender, DoWorkEventArgs e)
        {
            var Waypoints = Pathing.GetPath();
            if (Waypoints.Count > 0)
            {
                var NearestPoint = Pathing.GetNearestPoint(Waypoints);

                if (NearestPoint == null)
                {
                    IsWorking = false;
                    return;
                }

                Pathing.GotoWaypoint(NearestPoint);

                var Route = Waypoints.SkipWhile(element => element != NearestPoint).ToList();

                foreach (var NextPosition in Route)
                {
                    if (IsPlayerAlive() && !UserCancelingThread())
                    {
                        var Mob = GetTargetUnit();

                        // Check Health Before Battle
                        if (IsPlayerHPLow() && !PlayerHasAggro())
                            Rest();
                        // Battle if health high
                        else if (Mob.ID != 0)
                            Battle(Mob);
                        else
                            Pathing.GotoWaypoint(NextPosition);
                    }
                }
            }
            else if (IsPlayerAlive() && !UserCancelingThread())
            {
                var Mob = GetTargetUnit();

                // Check Health Before Battle
                if (IsPlayerHPLow() && !PlayerHasAggro())
                    Rest();
                // Battle if health high
                else if (Mob.ID != 0)
                    Battle(Mob);
            }
        }

        /*
        public void Logic()
        {
            var Target = GetTargetUnit();

            if (IsPlayerAlive())
            {
                if (PlayerHasAggro())
                {
                    Battle(Target);
                }
                else if (IsPlayerHPLow())
                {
                    Rest();
                }
                else if (Target.ID != 0)
                {
                    Battle(Target);
                }
                else
                {
                    Pathing.MoveNext();
                }
            }
        }*/

        /// <summary>
        /// If we have more than zero hp,
        /// return true
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerAlive()
        {
            return Session.Player.HPPCurrent > 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!WorkerThread.IsBusy && !UserCancelingThread())
                WorkerThread.RunWorkerAsync();

            if (!IsPlayerAlive() || UserCancelingThread())
                Pause();
        }

        /// <summary>
        /// Makes the character stop resting
        /// </summary>
        private void RestingOff()
        {
            if (IsPlayerHealing())
            {
                Session.Windower.SendString("/heal off");
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Returns true if our player is healing.
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerHealing()
        {
            return Session.Player.Status == Status.Healing;
        }

        /// <summary>
        /// Makes the character rest
        /// </summary>
        private void RestingOn()
        {
            if (IsPlayerHealing())
            {
                Session.Windower.SendString("/heal on");
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Starts up the bot for combat
        /// </summary>
        /// <param name="unit"></param>
        public void Start(Unit unit)
        {
            Session.Navigator.DistanceTolerance = 3;
            Session.Navigator.HeadingTolerance = 5;
            RestingOff();
        }

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit(Unit unit)
        {
            if (IsTargetUnit(unit) && IsPlayerFighting() && !IsNearUnit(unit, 5))
            {
                Session.Navigator.GotoTarget();
            }
        }

        private bool IsNearUnit(Unit unit, double distance)
        {
            return Session.Navigator.DistanceTo(unit.Position) <= distance;
        }


        /// <summary>
        /// Targets the target unit.
        /// </summary>
        /// <param name="unit"></param>
        public void Target(Unit unit)
        {
            if (!IsTargetUnit(unit))
            {
                MaintainHeading(unit);
                Session.Windower.SendKeyPress(FFACETools.KeyCode.TabKey);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull(Unit unit)
        {
            MaintainHeading(unit);

            if (CanPull(unit))
            {
                ExecuteActions(StartingResponses, unit);
            }
        }

        /// <summary>
        /// Can we pull the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private bool CanPull(Unit unit)
        {
            return IsTargetUnit(unit) && !IsTargetFighting(unit) && StartingResponses.Count > 0;
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage(Unit unit)
        {
            // if we have a correct target and our player isn't currently fighting...
            if (IsTargetUnit(unit) && !IsPlayerFighting())
            {
                MaintainHeading(unit);
                Session.Windower.SendString("/attack on");
                System.Threading.Thread.Sleep(50);
            }

            // Wrong Target Attacked
            else if (!IsTargetUnit(unit) && IsPlayerFighting())
            {
                Disengage();
            }
        }

        /// <summary>
        /// Peforms our rotation to kill our
        /// target unit.
        /// </summary>
        /// <param name="unit"></param>
        public void Kill(Unit unit)
        {
            if (IsTargetUnit(unit) && IsPlayerFighting())
            {
                MoveToUnit(unit);
                MaintainHeading(unit);

                if (CombatResponses.Count > 0)
                    ExecuteActions(CombatResponses, unit);

                if (PlayerCanWeaponskill(unit))
                {
                    Session.Windower.SendString(Weaponskill.ToString());
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// Can we perform our weaponskill on the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private bool PlayerCanWeaponskill(Unit unit)
        {
            return Session.Player.TPCurrent >= Weaponskill.TPCost &&
                                unit.HPPCurrent <= WeaponSkillHP &&
                                IsPlayerFighting() &&
                                IsTargetFighting(unit) &&
                                unit.Distance < Weaponskill.MaxDistance &&
                                Weaponskill.IsValidName;
        }

        /// <summary>
        /// Is the targets status == fighting
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static bool IsTargetFighting(Unit unit)
        {
            return unit.Status == Status.Fighting;
        }


        /// <summary>
        /// Is our status == fighting
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerFighting()
        {
            return Session.Player.Status == Status.Fighting;
        }

        /// <summary>
        /// Is our current target our target unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private bool IsTargetUnit(Unit unit)
        {
            return Session.Target.ID == unit.ID;
        }

        /// <summary>
        /// Clean up for path traveling and 
        /// peform any end battle moves
        /// </summary>
        /// <param name="unit"></param>
        public void End(Unit unit)
        {
            Disengage();

            if (IsUnitDead(unit) && EndingResponses.Count > 0)
            {
                ExecuteActions(EndingResponses, unit);
            }

            Session.Navigator.DistanceTolerance = 1;
            Session.Navigator.HeadingTolerance = 1;
        }

        /// <summary>
        /// Returns true if the units health is 0%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static bool IsUnitDead(Unit unit)
        {
            return unit.HPPCurrent <= 0;
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        private void MaintainHeading(Unit unit)
        {
            Session.Navigator.FaceHeading(unit.Position);
            System.Threading.Thread.Sleep(50);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        private void Disengage()
        {
            Session.Windower.SendString("/attack off");
            System.Threading.Thread.Sleep(50);
        }

        /// <summary>
        /// Rests the characters HP
        /// </summary>
        public void Rest()
        {
            while (CanPlayerRest())
            {
                if (!IsPlayerHealing())
                {
                    Session.Windower.SendString("/heal on");
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Fights a unit from start to finish.
        /// </summary>
        /// <param name="unit"></param>
        public void Battle(Unit unit)
        {
            Start(unit);

            while (CanPlayerBattle(unit))
            {
                Target(unit);
                Pull(unit);
                Engage(unit);
                MoveToUnit(unit);
                Kill(unit);
                System.Threading.Thread.Sleep(50);
            }

            End(unit);
        }

        /// <summary>
        /// Can we battle the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private bool CanPlayerBattle(Unit unit)
        {
            return Units.IsValid(unit) && IsPlayerAlive() && !UserCancelingThread();
        }

        /// <summary>
        /// Returns true if the user has decided to stop the bot.
        /// </summary>
        /// <returns></returns>
        private bool UserCancelingThread()
        {
            return !IsWorking || WorkerThread.CancellationPending;
        }

        /// <summary>
        /// Checks to  see if we can cast/use 
        /// a job ability or spell.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool AbilityRecastable(Ability value)
        {
            int AbilityRecast = Session.Timer.GetAbilityRecast((AbilityList)value.Index);
            int SpellRecast = Session.Timer.GetSpellRecast((SpellList)value.Index);

            if (value.Type == "JobAbility")
            {
                return AbilityRecast <= 0;
            }
            else
            {
                return SpellRecast <= 0;
            }
        }

        /// <summary>
        /// Performs a list of actions. 
        /// Could be spells or job abilities.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="unit"></param>
        private void ExecuteActions(List<Ability> actions, Unit unit)
        {
            foreach (var act in actions)
            {
                if (AbilityRecastable(act) && Session.Player.TPCurrent >= act.TPCost && Session.Player.MPCurrent >= act.MPCost)
                {
                    // (Casttime * 1 second) + 1 second or 2 seconds
                    int SleepDuration = act.IsSpell ? ((int)Math.Ceiling(act.CastTime) * 1000) + 1000 : 2000;
                    MaintainHeading(unit);
                    Session.Windower.SendString(act.ToString());
                    Thread.Sleep(SleepDuration);
                }
            }
        }

        /// <summary>
        /// Returns true if we can not cast a spell.
        /// </summary>
        /// <returns></returns>
        private bool IsCastingBlocked()
        {
            StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Silence,
                StatusEffect.Mute
            };

            // If we have effects that block,
            // return true.
            bool unableToCast = effectsThatBlock
                .Intersect(this.Session.Player.StatusEffects)
                .Count() != 0;

            // 
            bool unableToReact = IsPlayerUnable();

            return unableToCast || unableToReact;
        }

        /// <summary>
        /// Returns true if we have effects that inhibit us
        /// from taking any kind of action.
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerUnable()
        {
            StatusEffect[] effectsThatBlock = 
            {
                StatusEffect.Charm1, StatusEffect.Charm2, 
                StatusEffect.Petrification, StatusEffect.Sleep, 
                StatusEffect.Sleep2, StatusEffect.Stun, 
                StatusEffect.Chocobo, StatusEffect.Terror, 
            };

            bool IsPlayerUnable = effectsThatBlock
                .Intersect(Session.Player.StatusEffects)
                .Count() != 0;

            return IsPlayerUnable;
        }

        /// <summary>
        /// Makes the decision to which mob in the area
        /// should get the target unit.
        /// </summary>
        /// <returns></returns>
        public Unit GetTargetUnit()
        {
            var MainTarget = Unit.CreateUnit(0);

            try
            {
                if (KillPartyClaimed && Units.Mobs.Where(mob => mob.PartyClaim).Count() > 0)
                {
                    MainTarget = Units.Mobs.First(mob => mob.PartyClaim);
                }
                else if (Units.Mobs.Where(mob => mob.MyClaim).Count() > 0)
                {
                    MainTarget = Units.Mobs.First(mob => mob.MyClaim);
                }
                else if (KillAggro && Units.Mobs.Where(mob => mob.HasAggroed).Count() > 0)
                {
                    MainTarget = Units.Mobs.First(mob => mob.HasAggroed);
                }
                else if (KillUnclaimed && Units.Mobs.Where(mob => !mob.IsClaimed).Count() > 0)
                {
                    MainTarget = Units.Mobs.Where(mob => !mob.IsClaimed).First();
                }
            }
            catch (InvalidOperationException)
            {
                // Do Nothing, let bot retry
            }

            return MainTarget;
        }

        /// <summary>
        /// Determines low hp status.
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerHPLow()
        {
            return Session.Player.HPPCurrent < SitdownHPPValue;
        }

        /// <summary>
        /// Does our character have aggro
        /// </summary>
        /// <returns></returns>
        public bool PlayerHasAggro()
        {
            return Units.HasAggro;
        }

        /// <summary>
        /// Does our player have a status effect that prevents him
        /// </summary>
        /// <param name="playerStatusEffects"></param>
        /// <returns></returns>
        private bool IsRestingBlocked(params StatusEffect[] playerStatusEffects)
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

            var RestingBlocked = false;

            foreach (var Effect in playerStatusEffects)
            {
                if (RestBlockingDebuffs.Contains(Effect))
                {
                    RestingBlocked = true;
                }
            }

            return RestingBlocked;
        }

        /// <summary>
        /// Runs our bot
        /// </summary>
        public void Run()
        {
            IsWorking = true;
            OnStart.Invoke();
            Timer.Start();
        }

        /// <summary>
        /// Stops the bot.
        /// </summary>
        public void Pause()
        {
            IsWorking = false;
            OnFinish.Invoke();
            Timer.Stop();
            WorkerThread.CancelAsync();
        }

        /// <summary>
        /// Transfers fields from one Player obj,
        /// to another player object.
        /// </summary>
        /// <param name="Temp"></param>
        internal void TransferDeserializedFields(Player Temp)
        {
            this.CombatResponses = Temp.CombatResponses;
            this.DebuffResponses = Temp.DebuffResponses;
            this.EndingResponses = Temp.EndingResponses;
            this.HealingResponses = Temp.HealingResponses;
            this.KillAggro = Temp.KillAggro;
            this.KillPartyClaimed = Temp.KillPartyClaimed;
            this.KillUnclaimed = Temp.KillUnclaimed;
            this.LastPosition = Temp.LastPosition;
            this.Pathing.Waypoints = Temp.Pathing.Waypoints;
            this.SitdownHPPValue = Temp.SitdownHPPValue;
            this.StandUPHPPValue = Temp.StandUPHPPValue;
            this.StartingResponses = Temp.StartingResponses;
            this.Units.IgnoredMobs = Temp.Units.IgnoredMobs;
            this.Units.TargetNames = Temp.Units.TargetNames;
            this.Weaponskill = Temp.Weaponskill;
            this.WeaponSkillHP = Temp.WeaponSkillHP;
        }

        /// <summary>
        /// Sets the desired unit hp to which our 
        /// character should weaponskill at.
        /// </summary>
        /// <param name="p"></param>
        public void SetWeaponSkillHP(int p)
        {
            this.WeaponSkillHP = p;
        }

        /// <summary>
        /// Targets an enemy. Sames as target, 
        /// but exposed to other classes.
        /// </summary>
        /// <param name="unit"></param>
        public static void TargetNPC(Unit unit)
        {
            TargetNPC(unit);
        }

        /// <summary>
        /// Returns true if our player is able to
        /// safely rest (/heal).
        /// </summary>
        /// <returns></returns>
        public bool CanPlayerRest()
        {
            return (Session.Player.HPPCurrent < StandUPHPPValue && !PlayerHasAggro() && !UserCancelingThread() &&
                IsPlayerAlive() && !IsRestingBlocked(Session.Player.StatusEffects));
        }

        /// <summary>
        /// Returns true if our path contains waypoints.
        /// </summary>
        /// <returns></returns>
        public bool HasPath()
        {
            return Pathing.GetPath().Count() > 0;
        }
    }
}