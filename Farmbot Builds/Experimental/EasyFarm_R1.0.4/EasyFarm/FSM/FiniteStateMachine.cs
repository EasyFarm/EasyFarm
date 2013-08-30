using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EasyFarm.Engine;
using EasyFarm.FSM;
using System.Threading.Tasks;

public class FiniteStateEngine
{
    private List<BaseState> Brains = new List<BaseState>();
    private BaseState LastRan = null;

    // Timer loop, check the State list.
    private Timer Heartbeat = new Timer();
    private GameState GameState;

    // Constructor.
    public FiniteStateEngine()
    {
        Heartbeat.Interval = 100; // Check State list ten times per second.
        Heartbeat.Tick += new EventHandler(Heartbeat_Tick);
    }

    public FiniteStateEngine(ref GameState GameState)
        : this()
    {
        this.GameState = GameState;

        AddState(new RestState(ref this.GameState));
        AddState(new AttackState(ref this.GameState));
        AddState(new TravelState(ref this.GameState));
        AddState(new IdleState(ref this.GameState));

        foreach (var b in this.Brains)
            b.Enabled = true;
    }

    // Handles the updating.
    public void Heartbeat_Tick(object sender, EventArgs e)
    {
        new Task(() =>
            {
                lock (Brains)
                {
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
                                return; // We'll pick back up next cycle.
                            }

                            // Run this State and stop.
                            BS.RunState();
                            return;
                        }
                    }
                }
            })
            .Start();
    }

    // Start and stop.
    public void Start() { Heartbeat.Start(); }
    public void Stop() { Heartbeat.Stop(); }

    // Add and remove States.
    public void AddState(BaseState ToAdd) { lock (Brains) { Brains.Add(ToAdd); } }
    public void RemoveState(int index) { lock (Brains) { Brains.RemoveAt(index); } }
}