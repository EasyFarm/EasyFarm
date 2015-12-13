using System;
using MemoryAPI;
using EliteMMO.API;
using MathNet.Numerics;

public class MemoryWrapper : AbstractMemoryAPI
{
    private int id;

    public MemoryWrapper(int id)
    {
        this.id = id;
    }

    public class EliteMMOWrapper : MemoryWrapper
    {
        private readonly EliteAPI EliteAPI;

        public EliteMMOWrapper(int pid) : base(pid)
        {
            this.EliteAPI = new EliteAPI(pid);
            this.Navigator = new NavigationTools(EliteAPI);
        }

        public class NavigationTools : AbstractNavigatorTools
        {
            private readonly EliteAPI api;

            public NavigationTools(EliteAPI api)
            {
                this.api = api;
            }

            public override bool FaceHeading(IPosition position) { return true; }

            public override double DistanceTo(IPosition position) { return 0; }

            public override void Goto(IPosition position, bool KeepRunning) { }

            public override void GotoNPC(int ID) { }

            public override void Reset() { }
        }
    }
}
