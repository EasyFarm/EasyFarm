
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
using EasyFarm.Classes;

namespace EasyFarm.Classes
{
    public class CombatService
    {               
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
        private GameEngine _engine;

        public CombatService(ref GameEngine engine)
        {
            this._engine = engine;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Info`
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Are we near the target unit?
        /// </summary>
        /// <returns>True if we are near our target unit</returns>
        public bool IsNear
        {
            get
            {
                return _engine.Session.Instance.Navigator
                    .DistanceTo(_engine.TargetData.TargetUnit.Position) <= DIST_MAX;
            }
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
            var NavTools = _engine.Session.Instance.Navigator;
            var TargetData = _engine.TargetData;

            // Save the old tolerance
            var OldTolerance = NavTools.DistanceTolerance;

            // Use the new one
            NavTools.DistanceTolerance = DIST_MIN;

            // Run to the unit while we are out of distance. 
            if (NavTools.DistanceTo(TargetData.Position) >= DIST_MIN)
                NavTools.GotoNPC(TargetData.TargetUnit.ID, 10);

            // Restore the old tolerance.
            NavTools.DistanceTolerance = OldTolerance;
        }

        /// <summary>
        /// Places the cursor on the enemy
        /// </summary>
        public void Target()
        {
            var TargetTools = _engine.Session.Instance.Target;
            var TargetData = _engine.TargetData;

            if (!TargetData.IsTarget)
            {
                TargetTools.SetNPCTarget(TargetData.TargetUnit.ID);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull()
        {
            var TargetData = _engine.TargetData;
            var AbilityExecutor = _engine.AbilityExecutor;
            var PlayerActions = _engine.PlayerActions;

            if (TargetData.IsPullable) 
            { 
                AbilityExecutor.ExecuteActions(PlayerActions.StartList, MaintainHeading); 
            }
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage()
        {
            var TargetData = _engine.TargetData;
            var WindowerTools = _engine.Session.Instance.Windower;
            var PlayerData = _engine.PlayerData;

            // if we have a correct target and our player isn't currently fighting...
            if (TargetData.IsTarget && !PlayerData.IsFighting)
            {
                WindowerTools.SendString(ATTACK_TARGET);
            }

            // Wrong Target Attacked
            else if (!TargetData.IsTarget && PlayerData.IsFighting)
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
            var TargetData = _engine.TargetData;
            var AbilityExecutor = _engine.AbilityExecutor;
            var PlayerActions = _engine.PlayerActions;
            var PlayerData = _engine.PlayerData;
            var Config = _engine.UserSettings;

            if (PlayerData.IsFighting && TargetData.IsTarget)
            {
                // Execute all the battle moves
                if (PlayerActions.HasBattleMoves) {
                    AbilityExecutor.ExecuteActions(PlayerActions.BattleList, MaintainHeading); 
                }

                // Execute the weaponskill
                else if (PlayerData.CanWeaponskill) {
                    AbilityExecutor.ExecuteActions(new List<Ability>() { Config.WeaponInfo.Ability }, MaintainHeading);
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            var TargetData = _engine.TargetData;
            var NavTools = _engine.Session.Instance.Navigator;
            NavTools.FaceHeading(TargetData.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            var WindowerTools = _engine.Session.Instance.Windower;
            var PlayerData = _engine.PlayerData;

            if (PlayerData.IsEngaged) {
                WindowerTools.SendString(ATTACK_OFF);
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

        public void ExecuteActions(List<Ability> actions)
        {
            var AbilityExecutor = _engine.AbilityExecutor;
            AbilityExecutor.ExecuteActions(actions, MaintainHeading);
        }
    }
}