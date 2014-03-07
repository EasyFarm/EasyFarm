
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*////////////////////////////////////////////////////////////////////

﻿using System.Collections.Generic;
using System.Linq;
using FFACETools;
using System;
using EasyFarm.Engine;
using EasyFarm.Classes;

namespace EasyFarm.Views.Classes
{
    public class Combat
    {
        /// <summary>
        /// Reference to the game engine object. 
        /// Modifications to the changes the object for everyone.
        /// </summary>
        private GameEngine Engine;
               
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
        /// Max time used to spend running to a mob before timing out.
        /// </summary>
        const int RUN_DURATION = 3;

        public Combat() { }

        /// <summary>
        /// Sets up pathing, fface and units objs and
        /// initializes timers and threads.
        /// </summary>
        /// <param name="session"></param>
        public Combat(ref GameEngine Engine) : this()
        {
            this.Engine = Engine;
        }

        /// <summary>
        /// Details about the player
        /// </summary>
        private PlayerData PlayerData
        {
            get { return Engine.PlayerData; }
        }

        /// <summary>
        /// Details about the target
        /// </summary>
        private TargetData TargetData
        {
            get { return Engine.TargetData; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Info
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Are we near the target unit?
        /// </summary>
        /// <returns>True if we are near our target unit</returns>
        public bool IsNear
        {
            get
            {
                return Engine.FFInstance.Instance.Navigator.DistanceTo(TargetData.TargetUnit.Position) <= DIST_MAX;
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
                            .Select(x => Abilities.CreateAbility(x.Item.Name)).ToList()
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
        

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Commands
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Closes the distance between the character and 
        /// the target unit. 
        /// </summary>
        /// <param name="unit"></param>
        public void MoveToUnit()
        {
            // Run to the unit while we are out of distance. 
            if (Engine.FFInstance.Instance.Navigator.DistanceTo(TargetData.Position) >= DIST_MIN)
                Engine.FFInstance.Instance.Navigator.GotoNPC(TargetData.TargetUnit.ID, 10);
        }

        /// <summary>
        /// Places the cursor on the enemy
        /// </summary>
        public void Target()
        {
            if (!TargetData.IsTargetUnit)
            {
                Engine.FFInstance.Instance.Target.SetNPCTarget(TargetData.TargetUnit.ID);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull()
        {
            if (TargetData.IsUnitPullable) 
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
            if (TargetData.IsTargetUnit && !PlayerData.IsFighting)
            {
                Engine.FFInstance.Instance.Windower.SendString(ATTACK_TARGET);
            }

            // Wrong Target Attacked
            else if (!TargetData.IsTargetUnit && PlayerData.IsFighting)
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
            if (PlayerData.IsFighting && TargetData.IsTargetUnit)
            {
                // Execute all the battle moves
                if (HasBattleMoves) { 
                    ExecuteActions(BattleList); 
                }

                // Execute the weaponskill
                else if (PlayerData.CanWeaponskill) {
                    ExecuteActions(new List<Ability>() { Engine.Config.Weaponskill.Ability });
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            Engine.FFInstance.Instance.Navigator.FaceHeading(TargetData.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            if (PlayerData.IsEngaged) {
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
            if (TargetData.IsDead && HasEndMoves)
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
                    .Where(x => Engine.Combat.AbilityRecastable(x))
                    .Where(x => x.MPCost <= Engine.FFInstance.Instance.Player.MPCurrent && x.TPCost <= Engine.FFInstance.Instance.Player.TPCurrent)
                    .Where(x => (x.IsSpell && !PlayerData.IsCastingBlocked) || (x.IsAbility && !PlayerData.IsAbilitiesBlocked))
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
            int SleepDuration = Ability.IsSpell ? (int)Ability.CastTime + 1500 : 50;

            // Sleep for a second to pause the bots motion
            System.Threading.Thread.Sleep(1000);

            // Send it to the game
            Engine.FFInstance.Instance.Windower.SendString(Ability.ToString());

            // Sleep for the cast duration
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