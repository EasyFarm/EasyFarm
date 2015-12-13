using FFACETools;
using MemoryAPI;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm
{
    public class FFACEWrapper : MemoryWrapper
    {
        private readonly FFACE FFACE;

        public FFACEWrapper(int pid) : base(pid)
        {
            FFACE = new FFACE(pid);
            Navigator = new NavigationTools(FFACE);
            NPC = new NPCTools(FFACE);
            PartyMember = new Dictionary<byte, IPartyMemberTools>();
            Player = new PlayerTools(FFACE);
            Target = new TargetTools(FFACE);
            Timer = new TimerTools(FFACE);
            Windower = new WindowerTools(FFACE);

            for (byte i = 0; i < 16; i++)
            {
                PartyMember.Add(i, new PartyMemberTools(FFACE, i));
            }
        }

        public class NavigationTools : INavigatorTools
        {
            private readonly FFACE api;

            public double DistanceTolerance
            {
                get { return api.Navigator.DistanceTolerance; }
                set { this.api.Navigator.DistanceTolerance = value; }
            }

            public NavigationTools(FFACE api)
            {
                this.api = api;
            }

            public bool FaceHeading(IPosition position)
            {
                return api.Navigator.FaceHeading(ToPosition(position));
            }

            public double DistanceTo(IPosition position)
            {
                return api.Navigator.DistanceTo(ToPosition(position));
            }

            public void Goto(IPosition position, bool KeepRunning)
            {
                api.Navigator.Goto(ToPosition(position), KeepRunning);
            }

            public void GotoNPC(int ID)
            {
                api.Navigator.GotoNPC(ID);
            }

            public void Reset()
            {
                api.Navigator.Reset();
            }

            private FFACE.Position ToPosition(IPosition position)
            {
                FFACE.Position pos = new FFACE.Position();

                pos.X = position.X;
                pos.Y = position.Y;
                pos.Z = position.Z;
                pos.H = position.H;

                return pos;
            }
        }

        public class NPCTools : INPCTools
        {
            private readonly FFACE api;

            public NPCTools(FFACE api)
            {
                this.api = api;
            }

            public int ClaimedID(int id) { return api.NPC.ClaimedID(id); }

            public double Distance(int id) { return api.NPC.Distance(id); }

            public IPosition GetPosition(int id)
            {
                var position = api.NPC.GetPosition(id);
                return Helpers.CreatePosition(position.X, position.Y, position.Z, position.H);
            }

            public short HPPCurrent(int id) { return api.NPC.HPPCurrent(id); }

            public bool IsActive(int id) { return api.NPC.IsActive(id); }

            public bool IsClaimed(int id) { return api.NPC.IsClaimed(id); }

            public bool IsRendered(int id) { return api.NPC.IsRendered(id); }

            public string Name(int id) { return api.NPC.Name(id); }

            public MemoryAPI.NPCType NPCType(int id) { return (MemoryAPI.NPCType)api.NPC.NPCType(id); }

            public float PosX(int id) { return api.NPC.PosX(id); }

            public float PosY(int id) { return api.NPC.PosY(id); }

            public float PosZ(int id) { return api.NPC.PosZ(id); }

            public MemoryAPI.Status Status(int id) { return (MemoryAPI.Status)api.NPC.Status(id); }
        }

        public class PartyMemberTools : IPartyMemberTools
        {
            private readonly FFACE api;
            private readonly int index;

            public PartyMemberTools(FFACE api, int index)
            {
                this.api = api;
                this.index = index;
            }

            public int ServerID
            {
                get { return api.PartyMember[(byte)index].ServerID; }
            }
        }

        public class PlayerTools : IPlayerTools
        {
            private readonly FFACE api;

            public PlayerTools(FFACE api)
            {
                this.api = api;
            }

            public short CastPercentEx
            {
                get { return api.Player.CastPercentEx; }
            }

            public int HPPCurrent
            {
                get { return api.Player.HPPCurrent; }
            }

            public int ID
            {
                get { return api.Player.ID; }
            }

            public int MPCurrent
            {
                get { return api.Player.MPCurrent; }
            }

            public int MPPCurrent
            {
                get { return api.Player.MPPCurrent; }
            }

            public string Name
            {
                get { return api.Player.Name; }
            }

            public IPosition Position
            {
                get
                {
                    var x = api.Player.PosX;
                    var y = api.Player.PosY;
                    var z = api.Player.PosZ;
                    var h = api.Player.PosH;

                    return Helpers.CreatePosition(x, y, z, h);
                }
            }

            public float PosX
            {
                get { return Position.X; }
            }

            public float PosY
            {
                get { return Position.Y; }
            }

            public float PosZ
            {
                get { return Position.Z; }
            }

            public Structures.PlayerStats Stats
            {
                get
                {
                    var stats = api.Player.Stats;

                    return new Structures.PlayerStats()
                    {
                        Agi = stats.Agi,
                        Chr = stats.Chr,
                        Dex = stats.Dex,
                        Int = stats.Int,
                        Mnd = stats.Mnd,
                        Str = stats.Str,
                        Vit = stats.Vit
                    };
                }
            }

            public MemoryAPI.Status Status
            {
                get { return (MemoryAPI.Status)api.Player.Status; }
            }

            public MemoryAPI.StatusEffect[] StatusEffects
            {
                get
                {
                    return api.Player.StatusEffects.Select(x => (MemoryAPI.StatusEffect)x).ToArray();
                }
            }

            public int TPCurrent
            {
                get { return api.Player.TPCurrent; }
            }

            public MemoryAPI.Zone Zone
            {
                get { return (MemoryAPI.Zone)api.Player.Zone; }
            }
        }

        public class TargetTools : ITargetTools
        {
            private readonly FFACE api;

            public TargetTools(FFACE api)
            {
                this.api = api;
            }

            public int ID
            {
                get { return api.Target.ID; }
            }

            public bool SetNPCTarget(int index)
            {
                return api.Target.SetNPCTarget(index);
            }
        }

        public class TimerTools : ITimerTools
        {
            private readonly FFACE api;

            public TimerTools(FFACE api)
            {
                this.api = api;
            }

            public int GetAbilityRecast(MemoryAPI.AbilityList abil)
            {
                return api.Timer.GetAbilityRecast((byte)abil);
            }

            public short GetSpellRecast(MemoryAPI.SpellList spell)
            {
                return api.Timer.GetSpellRecast((short)spell);
            }
        }

        public class WindowerTools : IWindowerTools
        {
            private readonly FFACE api;

            public WindowerTools(FFACE api)
            {
                this.api = api;
            }

            public void SendString(string stringToSend)
            {
                api.Windower.SendString(stringToSend);
            }
        }
    }
}
