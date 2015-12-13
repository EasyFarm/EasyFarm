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

using EasyFarm.Components;
using Prism.Mvvm;

namespace EasyFarm.Mvvm
{
    public class ViewModelBase : BindableBase, IViewModel
    {
        /// <summary>
        ///     View Model name for header in tab control item.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        ///     Solo FFACE instance for current player.
        /// </summary>
        public static MemoryWrapper FFACE { get; set; }

        /// <summary>
        ///     Global game engine controlling the player.
        /// </summary>
        public static GameEngine GameEngine;

        public static void SetSession(MemoryWrapper fface)
        {
            if (fface == null) return;

            // Save FFACE Instance
            FFACE = fface;

            // Create a new game engine to control our character. 
            GameEngine = new GameEngine(FFACE);

            if (OnSessionSet != null)
            {
                OnSessionSet(fface);
            }
        }

        public delegate void SessionSet(MemoryWrapper fface);

        public static event SessionSet OnSessionSet;
    }
}