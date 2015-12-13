using System;

namespace MemoryAPI
{
    /// <summary>
    /// Delegate option 
    /// </summary>
    public delegate float dPoint();

    public interface INavigatorTools
    {
        double DistanceTolerance { get; set; }
        int GotoDelay { get; set; }
        uint HeadingTolerance { get; set; }
        int SpeedDelay { get; set; }
        float StayRunningAmount { get; set; }
        bool UseArrowKeysForTurning { get; set; }

        float DegreesToPosH(double Degrees);
        double DistanceTo(IPosition position);
        double DistanceTo(int ID);
        double DistanceTo(double X, double Z);
        double DistanceTo(double X, double Y, double Z);
        bool FaceHeading(IPosition position);
        bool FaceHeading(int ID);
        bool FaceHeading(float PosH, HeadingType headingType);
        bool FaceHeading(double X, double Z);
        bool FaceHeading(double X, double Y, double Z);
        double GetPlayerPosHInDegrees();
        void Goto(IPosition position, bool KeepRunning);
        void Goto(IPosition position, bool KeepRunning, int timeOut);
        void Goto(IPosition position, bool KeepRunning, Func<bool> stopCondition);
        void Goto(float X, float Z, bool KeepRunning);
        void Goto(dPoint x, dPoint z, bool KeepRunning);
        void Goto(float X, float Z, bool KeepRunning, int timeOut);
        void Goto(float X, float Y, float Z, bool KeepRunning);
        void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning);
        void Goto(dPoint x, dPoint z, bool KeepRunning, int timeOut);
        void Goto(float X, float Y, float Z, bool KeepRunning, int timeOut);
        void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning, int timeOut, Func<bool> stopCondition = null);
        void GotoNPC(int ID);
        void GotoNPC(int ID, int timeOut);
        void GotoTarget();
        void GotoTarget(int timeOut);
        double HeadingError(double Origin, double Target);
        float HeadingTo(IPosition position, HeadingType headingType);
        float HeadingTo(int ID, HeadingType headingType);
        float HeadingTo(double X, double Z, HeadingType headingType);
        float HeadingTo(double X, double Y, double Z, HeadingType headingType);
        bool IsRunning();
        double PosHToDegrees(float PosH);
        void Reset();
        bool SetPlayerDegrees(double degrees);
        bool SetPlayerPosH(float value);
        void SetViewMode(ViewMode newMode);
    }
}