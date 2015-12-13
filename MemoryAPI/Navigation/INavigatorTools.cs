using System;

namespace MemoryAPI
{
    public abstract class AbstractNavigatorTools : INavigatorTools
    {
        public virtual double DistanceTolerance { get; set; }
        public virtual int GotoDelay { get; set; }
        public virtual uint HeadingTolerance { get; set; }
        public virtual int SpeedDelay { get; set; }
        public virtual float StayRunningAmount { get; set; }
        public virtual bool UseArrowKeysForTurning { get; set; }

        public abstract float DegreesToPosH(double Degrees);
        public abstract double DistanceTo(IPosition position);
        public abstract double DistanceTo(int ID);
        public abstract double DistanceTo(double X, double Z);
        public abstract double DistanceTo(double X, double Y, double Z);
        public abstract bool FaceHeading(IPosition position);
        public abstract bool FaceHeading(int ID);
        public abstract bool FaceHeading(float PosH, HeadingType headingType);
        public abstract bool FaceHeading(double X, double Z);
        public abstract bool FaceHeading(double X, double Y, double Z);
        public abstract double GetPlayerPosHInDegrees();
        public abstract void Goto(IPosition position, bool KeepRunning);
        public abstract void Goto(IPosition position, bool KeepRunning, int timeOut);
        public abstract void Goto(IPosition position, bool KeepRunning, Func<bool> stopCondition);
        public abstract void Goto(float X, float Z, bool KeepRunning);
        public abstract void Goto(dPoint x, dPoint z, bool KeepRunning);
        public abstract void Goto(float X, float Z, bool KeepRunning, int timeOut);
        public abstract void Goto(float X, float Y, float Z, bool KeepRunning);
        public abstract void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning);
        public abstract void Goto(dPoint x, dPoint z, bool KeepRunning, int timeOut);
        public abstract void Goto(float X, float Y, float Z, bool KeepRunning, int timeOut);
        public abstract void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning, int timeOut, Func<bool> stopCondition = null);
        public abstract void GotoNPC(int ID);
        public abstract void GotoNPC(int ID, int timeOut);
        public abstract void GotoTarget();
        public abstract void GotoTarget(int timeOut);
        public abstract double HeadingError(double Origin, double Target);
        public abstract float HeadingTo(IPosition position, HeadingType headingType);
        public abstract float HeadingTo(int ID, HeadingType headingType);
        public abstract float HeadingTo(double X, double Z, HeadingType headingType);
        public abstract float HeadingTo(double X, double Y, double Z, HeadingType headingType);
        public abstract bool IsRunning();
        public abstract double PosHToDegrees(float PosH);
        public abstract void Reset();
        public abstract bool SetPlayerDegrees(double degrees);
        public abstract bool SetPlayerPosH(float value);
        public abstract void SetViewMode(ViewMode newMode);
    }
}