
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

using EasyFarm.FarmingTool;
using EasyFarm.States;
using EasyFarm.UserSettings;
using FFACETools;
using System;
using ZeroLimits.XITool.Classes;

namespace ZeroLimits.FarmingTool
{
    public class FTools
    {
        /// <summary>
        /// The current fface instance bound to farming tools. 
        /// </summary>
        private FFACE _fface;

        private XITools XITools;

        public FTools(FFACE fface)
        {
            _fface = fface;

            // Expose XITools fields. 
            this.XITools = new XITools(_fface);
            this.AbilityExecutor = this.XITools.AbilityExecutor;
            this.AbilityService = this.XITools.AbilityService;
            this.CombatService = this.XITools.CombatService;
            this.RestingService = this.XITools.RestingService;
            this.UnitService = this.XITools.UnitService;
            this.ActionBlocked = this.XITools.ActionBlocked;

            // Set up UnitService to use this mob filter instead of its
            // default mob filter. 
            this.UnitService.UnitFilter = UnitFilters.MobFilter(_fface);

            // Allow the ability service to set the distance on abilities
            // on their creation. 
            this.AbilityService.IsDistanceEnabled = true;
        }

        /// <summary>
        /// Provides access to FFACE memory reading api which returns details
        /// about various game environment objects. 
        /// </summary>
        public FFACE FFACE
        {
            get { return _fface; }
            set { _fface = value; }
        }

        /// <summary>
        /// Provides services for acquiring ability/spell data.
        /// </summary>
        public AbilityService AbilityService { get; set; }

        /// <summary>
        /// Provides the ability to executor abilities/spells.
        /// </summary>
        public AbilityExecutor AbilityExecutor { get; set; }

        /// <summary>
        /// Provides methods for performing battle.
        /// </summary>
        public CombatService CombatService { get; set; }

        /// <summary>
        /// Provides methods for resting our character.
        /// </summary>
        public RestingService RestingService { get; set; }

        /// <summary>
        /// Provide details about the units around us. 
        /// </summary>
        public UnitService UnitService { get; set; }

        /// <summary>
        /// Provides methods on whether an ability/spell is usable.
        /// </summary>
        public ActionBlocked ActionBlocked { get; set; }
    }
}
