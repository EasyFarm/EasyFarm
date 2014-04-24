
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

﻿// Author: Myrmidon
// Site: FFEVO.net
// All credit to him!

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EasyFarm.Classes;
using EasyFarm.FSM;
using System.Threading.Tasks;
using EasyFarm.Decision.FSM;
using EasyFarm.Decision;

public class FiniteStateEngine
{
    private List<BaseState> Brains = new List<BaseState>();
    private BaseState LastRan = null;

    // Timer loop, check the State list.
    private Timer Heartbeat = new Timer();
    private GameEngine _engine;
    private Task m_thread;
    
    // private BehaviorTree _behaviorTree;
    
    public Task Thread
    {
        get
        {
            return 
                m_thread == null || !m_thread.Status.Equals(TaskStatus.Running) ?
                m_thread = new Task(() => Heartbeat_Tick(null, null)) : m_thread;
        }    
    }

    // Constructor.
    public FiniteStateEngine()
    {
        Heartbeat.Interval = 100; // Check State list ten times per second.
        Heartbeat.Tick += new EventHandler((X, Y) => 
        {
            if(Thread.Status != TaskStatus.Running) 
                Thread.Start(); 
        });
    }

    public FiniteStateEngine(ref GameEngine engine)
        : this()
    {
        this._engine = engine;

        //Create the states
        AddState(new RestState(ref this._engine));
        AddState(new AttackState(ref this._engine));
        AddState(new TravelState(ref this._engine));
        AddState(new HealingState(ref this._engine));
        AddState(new DeadState(ref this._engine));

        foreach (var b in this.Brains)
            b.Enabled = true;

        // _behaviorTree = new BehaviorTree(ref _engine);
    }

    // Handles the updating.
    public void Heartbeat_Tick(object sender, EventArgs e)
    {
        lock (Brains)
        {
            /*if (_behaviorTree.CanExecute())
                _behaviorTree.Execute();*/
                       
            // Sort the List, States may have updated Priorities.
            Brains.Sort();

            // Find a State that says it needs to run.
            foreach (BaseState BS in Brains)
            {
                if (BS.Enabled == false) { continue; } // Skip disabled States.
                if (BS.CheckState() == true)
                {
                    // Says it needs to run. Same State as before?
                    if (LastRan == null) { LastRan = BS; }
                    if (LastRan != BS)
                    {
                        // Make the previous State clean up and exit.
                        LastRan.ExitState();
                        LastRan = BS;
                        BS.EnterState();
                    }

                    // Run this State and stop.
                    BS.RunState();
                    return;
                }
            }
        }
    }

    // Start and stop.
    public void Start() { Heartbeat.Start(); }
    public void Stop() { Heartbeat.Stop(); }

    // Add and remove States.
    public void AddState(BaseState ToAdd) { lock (Brains) { Brains.Add(ToAdd); } }
    public void RemoveState(int index) { lock (Brains) { Brains.RemoveAt(index); } }
}