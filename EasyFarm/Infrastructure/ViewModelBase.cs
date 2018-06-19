// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System;
using System.IO;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.Persistence;
using EasyFarm.States;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Infrastructure
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, IViewModel
    {
        /// <summary>
        ///     Global game engine controlling the player.
        /// </summary>
        protected static GameEngine GameEngine { get; set; }

        /// <summary>
        ///     Solo EliteApi instance for current player.
        /// </summary>
        public static IMemoryAPI FFACE { get; set; }

        /// <summary>
        ///     View Model name for header in tab control item.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Path record to let users record waypoint paths
        /// </summary>
        protected static PathRecorder PathRecorder { get; set; }

        public static AbilityService AbilityService { get; set; } = new AbilityService(null);

        /// <summary>
        /// Set up session from given EliteApi session.
        /// </summary>
        /// <param name="fface"></param>
        public static void SetSession(IMemoryAPI fface)
        {
            if (fface == null) return;

            // Save EliteApi Write
            FFACE = fface;

            // Create a new game engine to control our character. 
            GameEngine = new GameEngine(FFACE);

            // Create path record for navigation
            PathRecorder = new PathRecorder(FFACE);

            AbilityService = new AbilityService(FFACE);

            AutoLoadSettings();
        }

        private static void AutoLoadSettings()
        {
            var persister = new Persister();
            var characterName = FFACE?.Player?.Name;
            var fileName = $"{characterName}.eup";
            if (String.IsNullOrWhiteSpace(fileName)) return;
            if (!File.Exists(fileName)) return;
            var config = persister.Deserialize<Config>(fileName);
            Config.Instance = config;
            AppServices.SendConfigLoaded();
        }
    }
}