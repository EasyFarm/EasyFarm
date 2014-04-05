
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
        private PlayerData PlayerData;
        private TargetData TargetData;
        private FFACE.NavigatorTools NavTools;
        private Config Config;
        private FFACE.TargetTools TargetTools;
        private FFACE.WindowerTools WindowerTools;
        private FFACE.TimerTools TimerTools;
        private PlayerActions PlayerActions;
        private AbilityExecutor AbilityExecutor;
        private GameEngine m_gameEngine;

        public CombatService(ref GameEngine Engine)
        {
            this.m_gameEngine = Engine;
            this.PlayerData = Engine.PlayerData;
            this.TargetData = Engine.TargetData;            
            this.Config = Engine.Config;            
            this.PlayerActions = Engine.PlayerActions;
            this.AbilityExecutor = Engine.AbilityExecutor;
            this.NavTools = Engine.FFInstance.Instance.Navigator;
            this.TargetTools = Engine.FFInstance.Instance.Target;
            this.WindowerTools = Engine.FFInstance.Instance.Windower;
            this.TimerTools = Engine.FFInstance.Instance.Timer;
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
                return NavTools.DistanceTo(TargetData.TargetUnit.Position) <= DIST_MAX;
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
            // Save the old tolerance
            var OldTolerance = NavTools.DistanceTolerance;

            // Use the new one
            NavTools.DistanceTolerance = DIST_MAX;

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
            if (PlayerData.IsFighting && TargetData.IsTarget)
            {
                // Execute all the battle moves
                if (PlayerActions.HasBattleMoves) {
                    AbilityExecutor.ExecuteActions(PlayerActions.BattleList, MaintainHeading); 
                }

                // Execute the weaponskill
                else if (PlayerData.CanWeaponskill) {
                    AbilityExecutor.ExecuteActions(new List<Ability>() { Config.WeaponInfo.WeaponSkill.Ability }, MaintainHeading);
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            NavTools.FaceHeading(TargetData.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
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
            AbilityExecutor.ExecuteActions(actions, MaintainHeading);
        }
    }
}