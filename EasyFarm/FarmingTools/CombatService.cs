
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
using EasyFarm.Classes.Services;

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
        /// Max time used to spend running to a mob before timing out.
        /// </summary>
        const int RUN_DURATION = 3;

        private FFACE _fface;

        public CombatService(FFACE fface)
        {
            this._fface = fface;
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
                return _fface.Navigator
                    .DistanceTo(FarmingTools.GetInstance(_fface).TargetData.TargetUnit.Position) <= 
                    FarmingTools.GetInstance(_fface).UserSettings.MiscSettings.MaxMeleeDistance;
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
            var OldTolerance = _fface.Navigator.DistanceTolerance;

            // Use the new one
            _fface.Navigator.DistanceTolerance = FarmingTools.GetInstance(_fface)
                .UserSettings.MiscSettings.MinMeleeDistance;

            // Run to the unit while we are out of distance. 
            if (_fface.Navigator.DistanceTo(FarmingTools.GetInstance(_fface).TargetData.Position) >=
                FarmingTools.GetInstance(_fface).UserSettings.MiscSettings.MinMeleeDistance)
            {
                _fface.Navigator.GotoNPC(FarmingTools.GetInstance(_fface).TargetData.TargetUnit.ID, 10);
            }

            // Restore the old tolerance.
            _fface.Navigator.DistanceTolerance = OldTolerance;
        }

        /// <summary>
        /// Places the cursor on the enemy
        /// </summary>
        public void Target()
        {
            if (!FarmingTools.GetInstance(_fface).TargetData.IsTarget)
            {
                _fface.Target.SetNPCTarget(FarmingTools.GetInstance(_fface).TargetData.TargetUnit.ID);
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Performs all starting actions
        /// </summary>
        /// <param name="unit"></param>
        public void Pull()
        {
            if (FarmingTools.GetInstance(_fface).TargetData.IsPullable) 
            { 
                FarmingTools.GetInstance(_fface).AbilityExecutor.ExecuteActions(
                        FarmingTools.GetInstance(_fface).PlayerActions.StartList, 
                        MaintainHeading
                    ); 
            }
        }

        /// <summary>
        /// Switches the player to attack mode on the current unit
        /// </summary>
        /// <param name="unit"></param>
        public void Engage()
        {
            // if we have a correct target and our player isn't currently fighting...
            if (FarmingTools.GetInstance(_fface).TargetData.IsTarget && 
                !FarmingTools.GetInstance(_fface).PlayerData.IsFighting) 
            {
                
                _fface.Windower.SendString(ATTACK_TARGET);
            }

            // Wrong Target Attacked
            else if (!FarmingTools.GetInstance(_fface).TargetData.IsTarget && 
                FarmingTools.GetInstance(_fface).PlayerData.IsFighting)
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
            if (FarmingTools.GetInstance(_fface).PlayerData.IsFighting && 
                FarmingTools.GetInstance(_fface).TargetData.IsTarget)
            {
                // Execute all the battle moves
                if (FarmingTools.GetInstance(_fface).PlayerActions.HasBattleMoves) {
                    FarmingTools.GetInstance(_fface).AbilityExecutor.ExecuteActions(
                        FarmingTools.GetInstance(_fface).PlayerActions.BattleList, 
                        MaintainHeading); 
                }

                // Execute the weaponskill
                else if (FarmingTools.GetInstance(_fface).PlayerData.CanWeaponskill) {
                    FarmingTools.GetInstance(_fface).AbilityExecutor.ExecuteActions(new List<Ability>() 
                    { 
                        FarmingTools.GetInstance(_fface).UserSettings.WeaponInfo.Ability 
                    },
                    MaintainHeading);
                }
            }
        }

        /// <summary>
        /// Face character towards opponent.
        /// </summary>
        /// <param name="unit"></param>
        public void MaintainHeading()
        {
            _fface.Navigator.FaceHeading(FarmingTools.GetInstance(_fface).TargetData.Position);
        }

        /// <summary>
        /// Stop the character from fight the target
        /// </summary>
        public void Disengage()
        {
            if (FarmingTools.GetInstance(_fface).PlayerData.IsEngaged) {
                _fface.Windower.SendString(ATTACK_OFF);
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
            FarmingTools.GetInstance(_fface).AbilityExecutor.ExecuteActions(actions, MaintainHeading);
        }
    }
}