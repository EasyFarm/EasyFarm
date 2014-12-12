
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

using EasyFarm.BehaviorTree;
using EasyFarm.FarmingTool;
using EasyFarm.ViewModels;
using FFACETools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ZeroLimits.FarmingTool;
using System.Linq;

namespace EasyFarm.Components
{
    /// <summary>
    /// Controls whether or not the bot should run. Basically anything 
    /// that can pause or resume the bot's engine should be here. 
    /// </summary>
    public class GameEngine
    {
        // Notice this matches FilterDelegate from the TreeBase class:
        // We use an empty delegate so later on we don't have to use the
        // "if (OnFilterTree != null)" syntax dozens of times. Slight performance
        // hit? Yes. Willing to trade for code readability? Absolutely.
        public event FilterDelegate OnFilterTree;

        /// <summary>
        /// Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking = false;

        /// <summary>
        /// Monitors for zone changes and allows for pausing / resuming
        /// the program after zoning. 
        /// </summary>
        private ZoneMonitor _zoneMonitor;

        /// <summary>
        /// Monitors for players nearby and allows for pausing / resuming
        /// the program on detection. 
        /// </summary>
        private PlayerMonitor _playerMonitor;

        /// <summary>
        /// Monitors the player's status for dead and shuts down the 
        /// program when he is. 
        /// </summary>
        private DeadMonitor _statusMonitor;

        /// <summary>
        /// Monitors the player tos ee if he's stuck agains a wall and 
        /// if so stops the engine. 
        /// </summary>
        private StuckMonitor _stuckMonitor;

        /// <summary>
        /// Provides information about game data. 
        /// </summary>
        private FFACE _fface;

        public GameEngine(FFACE fface)
        {
            this._fface = fface;
            this._zoneMonitor = new ZoneMonitor(fface);
            this._playerMonitor = new PlayerMonitor(fface);
            this._statusMonitor = new DeadMonitor(fface);
            this._stuckMonitor = new StuckMonitor(fface);
            this.StateMachine = new FiniteStateEngine(fface);

            _zoneMonitor.Changed += ZoneMonitor_ZoneChanged;
            _zoneMonitor.Start();

            _statusMonitor.Changed += StatusMonitor_StatusChanged;
            _statusMonitor.Start();

            _stuckMonitor.Changed += StuckMonitor_StuckChanged;
            _stuckMonitor.Start();
        }

        /// <summary>
        /// The engine that controls player actions. 
        /// </summary>
        public FiniteStateEngine StateMachine { get; set; }

        /// <summary>
        /// Monitors engine status for player being stuck and 
        /// shuts it down when detected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StuckMonitor_StuckChanged(object sender, EventArgs e)
        {
            // Do nothing if the engine is not running already. 
            if (!IsWorking) { return; }

            // Stop program from running to next waypoint. 
            _fface.Navigator.Reset();

            // Tell the use we paused the program. 
            ViewModelBase.InformUser("Program Paused");

            // Stop the engine from running. 
            Stop();
        }

        /// <summary>
        /// Monitors engine stats for player dead status 
        /// and then shuts down the engine. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusMonitor_StatusChanged(object sender, EventArgs e)
        {
            // Do nothing if the engine is not running already. 
            if (!IsWorking) { return; }

            // Stop program from running to next waypoint. 
            _fface.Navigator.Reset();

            // Tell the use we paused the program. 
            ViewModelBase.InformUser("Program Paused");

            // Stop the engine from running. 
            Stop();
        }

        /// <summary>
        /// Stops the engine when another player is detected and 
        /// resumes afterwards. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerMonitor_DetectedChanged(object sender, EventArgs e)
        {
            // If the program is not running then bail out. 
            if (!IsWorking) { return; }

            var args = (e as MonitorArgs<bool>);
            if (args.Status)
            {
                ViewModelBase.InformUser("Program Paused");
                Stop();
            }
            else
            {
                ViewModelBase.InformUser("Program Resumed");
                Start();
            }
        }

        /// <summary>
        /// Pauses the engine when the player zones and
        /// resumes after. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoneMonitor_ZoneChanged(object sender, EventArgs e)
        {
            var args = (e as MonitorArgs<Zone>);

            // If the program is not running then bail out. 
            if (!IsWorking) { return; }

            ViewModelBase.InformUser("Program Paused");

            // Stop the state machine.
            Stop();

            // Set up waiting of 5 seconds to be our current time + 10 seconds. 
            var waitDelay = DateTime.Now.Add(TimeSpan.FromSeconds(10));

            // Wait for five seconds after zoning. 
            while (DateTime.Now < waitDelay) { }

            // Start up the state machine again.
            Start();

            ViewModelBase.InformUser("Program Resumed");
        }

        /// <summary>
        /// Start the bot up
        /// </summary>
        public void Start()
        {
            var WorkingClasses = new List<TreeBase>();

            Assembly.GetExecutingAssembly()
           .GetTypes()
           .Where(x => x.IsClass)
           .Where(x => x.GetCustomAttributes(typeof(BehaviorAttribute), false).Any())
           .ToList().ForEach(item =>
           {
               // Get a constructor inside the class we understand. TreeBase
               // uses (double MyValue, uint behaviormask) so look for that.
               ConstructorInfo CI = item.GetConstructor(new Type[] { typeof(double), typeof(BehaviorType) });
               if (CI != null)
               {
                   // Save the Influences on this particular class.
                   BehaviorType mask = BehaviorType.None;
                   foreach (BehaviorAttribute B in item.GetCustomAttributes(typeof(BehaviorAttribute), false))
                   {
                       mask = B.Influences;
                   }

                   // And build the class.
                   TreeBase check = (TreeBase)CI.Invoke(new object[] { 1.0, mask });
                   if (check != null)
                   {
                       WorkingClasses.Add(check);
                       OnFilterTree += check.FilterSwitch;
                   }
               }
           });

            var world = new GameState(this._fface);

            // StateMachine.Start();
            while (true)
            {
                world.Update();
                WorkingClasses.Sort();
                OnFilterTree(BehaviorType.Curing);

                foreach (var TB in WorkingClasses)
                {
                    if (!TB.Enabled) continue;
                    TB.Execute(ref world);
                }
            }

            // IsWorking = true;
        }

        /// <summary>
        /// Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            StateMachine.Stop();
            IsWorking = false;
        }

        // And this matches the MessageDelegate syntax:
        public void AcceptOutput(string one_message)
        {
            // You could print this message out to a logging file or TextBox if you preferred.
            System.Diagnostics.Debug.Print(one_message);
        }
    }

}
