/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Components
{
    /// <summary>
    ///     A class for defeating monsters.
    /// </summary>
    public class AttackContainer : SequenceContainer
    {
        static AttackContainer()
        {
            FightStarted = false;
        }

        public AttackContainer(FFACE fface)
        {
            FFACE = fface;

            // Add components.
            AddComponent(new ApproachComponent(fface) {Priority = 0});
            AddComponent(new BattleComponent(fface) {Priority = 3});
            AddComponent(new WeaponSkillComponent(fface) {Priority = 2});
            AddComponent(new PullComponent(fface) {Priority = 4});
            AddComponent(new StartComponent(fface) {Priority = 5});

            // Enable all attack components. 
            Components.ForEach(x => x.Enabled = true);
        }

        /// <summary>
        ///     Whether the fight has started or not.
        /// </summary>
        public static bool FightStarted { get; set; }

        /// <summary>
        ///     The game session.
        /// </summary>
        public FFACE FFACE { get; set; }

        /// <summary>
        ///     Who we are trying to kill currently
        /// </summary>
        public static Unit TargetUnit { get; set; }

        public override bool CheckComponent()
        {
            // If we're injured. 
            if (new RestComponent(FFACE).CheckComponent()) return false;

            if (TargetUnit != null)
            {
                // Target is out of distance and we should not attack it. 
                if (TargetUnit.Distance > Config.Instance.WanderDistance) return false;
            }

            // Return if other components need to fire. 
            return base.CheckComponent();
        }
    }
}