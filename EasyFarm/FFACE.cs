using System;
using MemoryAPI;
using EliteMMO.API;

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

            public override float DegreesToPosH(double Degrees)
            {
                throw new NotImplementedException();
            }

            public override double DistanceTo(int ID)
            {
                throw new NotImplementedException();
            }

            public override double DistanceTo(IPosition position)
            {
                throw new NotImplementedException();
            }

            public override double DistanceTo(double X, double Z)
            {
                throw new NotImplementedException();
            }

            public override double DistanceTo(double X, double Y, double Z)
            {
                throw new NotImplementedException();
            }

            public override bool FaceHeading(int ID)
            {
                throw new NotImplementedException();
            }

            public override bool FaceHeading(IPosition position)
            {
                throw new NotImplementedException();
            }

            public override bool FaceHeading(double X, double Z)
            {
                throw new NotImplementedException();
            }

            public override bool FaceHeading(float PosH, HeadingType headingType)
            {
                throw new NotImplementedException();
            }

            public override bool FaceHeading(double X, double Y, double Z)
            {
                throw new NotImplementedException();
            }

            public override double GetPlayerPosHInDegrees()
            {
                throw new NotImplementedException();
            }

            public override void Goto(IPosition position, bool KeepRunning)
            {
                api.AutoFollow.SetAutoFollowCoords(position.X, position.Y, position.Z);
            }

            public override void Goto(float X, float Z, bool KeepRunning)
            {
                throw new NotImplementedException();
            }

            public override void Goto(dPoint x, dPoint z, bool KeepRunning)
            {
                throw new NotImplementedException();
            }

            public override void Goto(IPosition position, bool KeepRunning, Func<bool> stopCondition)
            {
                throw new NotImplementedException();
            }

            public override void Goto(IPosition position, bool KeepRunning, int timeOut)
            {
                throw new NotImplementedException();
            }

            public override void Goto(dPoint x, dPoint z, bool KeepRunning, int timeOut)
            {
                throw new NotImplementedException();
            }

            public override void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning)
            {
                throw new NotImplementedException();
            }

            public override void Goto(float X, float Y, float Z, bool KeepRunning)
            {
                throw new NotImplementedException();
            }

            public override void Goto(float X, float Z, bool KeepRunning, int timeOut)
            {
                throw new NotImplementedException();
            }

            public override void Goto(float X, float Y, float Z, bool KeepRunning, int timeOut)
            {
                throw new NotImplementedException();
            }

            public override void Goto(dPoint x, dPoint y, dPoint z, bool KeepRunning, int timeOut, Func<bool> stopCondition = null)
            {
                throw new NotImplementedException();
            }

            public override void GotoNPC(int ID)
            {
                throw new NotImplementedException();
            }

            public override void GotoNPC(int ID, int timeOut)
            {
                throw new NotImplementedException();
            }

            public override void GotoTarget()
            {
                throw new NotImplementedException();
            }

            public override void GotoTarget(int timeOut)
            {
                throw new NotImplementedException();
            }

            public override double HeadingError(double Origin, double Target)
            {
                throw new NotImplementedException();
            }

            public override float HeadingTo(int ID, HeadingType headingType)
            {
                throw new NotImplementedException();
            }

            public override float HeadingTo(IPosition position, HeadingType headingType)
            {
                throw new NotImplementedException();
            }

            public override float HeadingTo(double X, double Z, HeadingType headingType)
            {
                throw new NotImplementedException();
            }

            public override float HeadingTo(double X, double Y, double Z, HeadingType headingType)
            {
                throw new NotImplementedException();
            }

            public override bool IsRunning()
            {
                throw new NotImplementedException();
            }

            public override double PosHToDegrees(float PosH)
            {
                throw new NotImplementedException();
            }

            public override void Reset()
            {
                api.AutoFollow.SetAutoFollowCoords(api.Player.X, api.Player.Y, api.Player.Z);
            }

            public override bool SetPlayerDegrees(double degrees)
            {
                throw new NotImplementedException();
            }

            public override bool SetPlayerPosH(float value)
            {
                throw new NotImplementedException();
            }

            public override void SetViewMode(ViewMode newMode)
            {
                throw new NotImplementedException();
            }
        }
    }
}
