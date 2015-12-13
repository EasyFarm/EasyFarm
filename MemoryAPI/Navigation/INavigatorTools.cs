using System;

namespace MemoryAPI
{
    public abstract class AbstractNavigatorTools : INavigatorTools
    {
        public virtual double DistanceTolerance { get; set; }
                
        public abstract double DistanceTo(IPosition position);
        public abstract bool FaceHeading(IPosition position);        
        public abstract void Goto(IPosition position, bool KeepRunning);
        public abstract void GotoNPC(int ID);
        public abstract void Reset();        
    }
}