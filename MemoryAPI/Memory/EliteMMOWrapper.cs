using EliteMMO.API;
using MemoryAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm
{
    public class EliteMMOWrapper : MemoryWrapper
    {
        public enum ViewMode : int
        {
            ThirdPerson = 0,
            FirstPerson
        }

        private readonly EliteAPI EliteAPI;

        public EliteMMOWrapper(int pid)
        {
            EliteAPI = new EliteAPI(pid);
            Navigator = new NavigationTools(EliteAPI);
            NPC = new NPCTools(EliteAPI);
            PartyMember = new Dictionary<byte, IPartyMemberTools>();
            Player = new PlayerTools(EliteAPI);
            Target = new TargetTools(EliteAPI);
            Timer = new TimerTools(EliteAPI);
            Windower = new WindowerTools(EliteAPI);

            for (byte i = 0; i < 16; i++)
            {
                PartyMember.Add(i, new PartyMemberTools(EliteAPI, i));
            }
        }

        public class NavigationTools : INavigatorTools
        {
            private readonly EliteAPI api;

            public double DistanceTolerance { get; set; } = 3;

            public NavigationTools(EliteAPI api)
            {
                this.api = api;
            }

            /// <summary>
            /// Makes the player look at the specified position. 
            /// </summary>            
            /// Author: SMD111
            /// https://github.com/smd111/EliteMMO.Scripted
            public bool FaceHeading(IPosition position)
            {
                var player = api.Entity.GetLocalPlayer();
                var angle = (byte)(Math.Atan((position.Z - player.Z) / (position.X - player.X)) * -(128.0f / Math.PI));
                if (player.X > position.X) angle += 128;
                var radian = (((float)angle) / 255) * 2 * Math.PI;
                return api.Entity.SetEntityHPosition(api.Entity.LocalPlayerIndex, (float)radian);
            }

            public double DistanceTo(IPosition position)
            {
                var player = api.Entity.GetLocalPlayer();

                return Math.Sqrt(
                    Math.Pow(position.X - player.X, 2) +
                    Math.Pow(position.Y - player.Y, 2) +
                    Math.Pow(position.Z - player.Z, 2));
            }

            public void Goto(IPosition position, bool KeepRunning)
            {
                var distance = DistanceTo(position);

                if (DistanceTo(position) > DistanceTolerance)
                {
                    DateTime duration = DateTime.Now.AddSeconds(5);
                    var player = api.Entity.GetLocalPlayer();
                    api.ThirdParty.KeyDown(Keys.NUMPAD8);

                    while (DistanceTo(position) > DistanceTolerance && DateTime.Now < duration)
                    {
                        if ((ViewMode)api.Player.ViewMode != ViewMode.FirstPerson)
                        {
                            api.Player.ViewMode = (int)ViewMode.FirstPerson;
                        }

                        FaceHeading(position);

                        System.Threading.Thread.Sleep(30);
                    }

                    api.ThirdParty.KeyUp(Keys.NUMPAD8);
                }
            }

            public void GotoNPC(int ID)
            {
                var entity = api.Entity.GetEntity(ID);
                Goto(Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H), false);
            }

            public void Reset()
            {
                api.ThirdParty.KeyUp(Keys.NUMPAD8);
            }
        }

        public class NPCTools : INPCTools
        {
            private readonly EliteAPI api;

            public NPCTools(EliteAPI api)
            {
                this.api = api;
            }

            public int ClaimedID(int id) { return (int)api.Entity.GetEntity(id).ClaimID; }

            public double Distance(int id) { return api.Entity.GetEntity(id).Distance; }

            public IPosition GetPosition(int id)
            {
                var entity = api.Entity.GetEntity(id);
                return Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H);
            }

            public short HPPCurrent(int id) { return api.Entity.GetEntity(id).HealthPercent; }

            public bool IsActive(int id) { return true; }

            public bool IsClaimed(int id) { return api.Entity.GetEntity(id).ClaimID != 0; }

            public bool IsPet(int id)
            {
                return api.Entity.GetLocalPlayer().PetIndex == id;
            }

            /// <summary>
            /// Checks to see if the object is rendered. 
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            /// Author: SG1234567
            /// https://github.com/SG1234567
            public bool IsRendered(int id)
            {
                //Render0000 specificaly uses 0x200 to check if the object is rendered
                if ((api.Entity.GetEntity(id).Render0000 & 0x200) != 0x200)
                    return false;
                return true;
            }

            public string Name(int id) { return api.Entity.GetEntity(id).Name; }

            public NPCType NPCType(int id)
            {
                var entity = api.Entity.GetEntity(id);
                if (entity.Type == 2) return MemoryAPI.NPCType.Mob;
                else if (entity.Type == 0) return MemoryAPI.NPCType.PC;
                else return MemoryAPI.NPCType.NPC;
            }

            public float PosX(int id) { return api.Entity.GetEntity(id).X; }

            public float PosY(int id) { return api.Entity.GetEntity(id).Y; }

            public float PosZ(int id) { return api.Entity.GetEntity(id).Z; }

            public Status Status(int id)
            {
                var status = (EntityStatus)api.Entity.GetEntity(id).Status;
                return Helpers.ToStatus(status);
            }
        }

        public class PartyMemberTools : IPartyMemberTools
        {
            private readonly EliteAPI api;
            private readonly int index;

            public PartyMemberTools(EliteAPI api, int index)
            {
                this.api = api;
                this.index = index;
            }

            public int ServerID
            {
                get { return (int)api.Party.GetPartyMember(index).ID; }
            }
        }

        public class PlayerTools : IPlayerTools
        {
            private readonly EliteAPI api;

            public PlayerTools(EliteAPI api)
            {
                this.api = api;
            }

            public short CastPercentEx
            {
                get { return (short)(api.CastBar.Percent * 100); }
            }

            public int HPPCurrent
            {
                get { return (int)api.Player.HPP; }
            }

            public int ID
            {
                get { return api.Player.ServerId; }
            }

            public int MPCurrent
            {
                get { return (int)api.Player.MP; }
            }

            public int MPPCurrent
            {
                get { return (int)api.Player.MPP; }
            }

            public string Name
            {
                get { return api.Player.Name; }
            }

            public IPosition Position
            {
                get
                {
                    var x = api.Player.X;
                    var y = api.Player.Y;
                    var z = api.Player.Z;
                    var h = api.Player.H;

                    return Helpers.ToPosition(x, y, z, h);
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
                        Agi = stats.Agility,
                        Chr = stats.Charisma,
                        Dex = stats.Dexterity,
                        Int = stats.Intelligence,
                        Mnd = stats.Mind,
                        Str = stats.Strength,
                        Vit = stats.Vitality
                    };
                }
            }

            public Status Status
            {
                get { return Helpers.ToStatus((EntityStatus)api.Player.Status); }
            }

            public MemoryAPI.StatusEffect[] StatusEffects
            {
                get
                {
                    return api.Player.Buffs.Select(x => (MemoryAPI.StatusEffect)x).ToArray();
                }
            }

            public int TPCurrent
            {
                get { return (int)api.Player.TP; }
            }

            public Zone Zone
            {
                get { return (MemoryAPI.Zone)api.Player.ZoneId; }
            }
        }

        public class TargetTools : ITargetTools
        {
            private readonly EliteAPI api;

            public TargetTools(EliteAPI api)
            {
                this.api = api;
            }

            public int ID
            {
                get { return (int)api.Target.GetTargetInfo().TargetIndex; }
            }

            public bool SetNPCTarget(int index)
            {
                return api.Target.SetTarget(index);
            }
        }

        public class TimerTools : ITimerTools
        {
            private readonly EliteAPI api;

            public TimerTools(EliteAPI api)
            {
                this.api = api;
            }

            public int GetAbilityRecast(int index)
            {
                var ids = api.Recast.GetAbilityIds();
                var ability = api.Resources.GetAbility((uint)index);
                var idx = ids.IndexOf(ability.TimerID);
                return api.Recast.GetAbilityRecast(idx);
            }

            public int GetSpellRecast(int index)
            {
                return api.Recast.GetSpellRecast(index);
            }
        }

        public class WindowerTools : IWindowerTools
        {
            private readonly EliteAPI api;

            public WindowerTools(EliteAPI api)
            {
                this.api = api;
            }

            public void SendString(string stringToSend)
            {
                api.ThirdParty.SendString(stringToSend);
            }
        }
    }
}
