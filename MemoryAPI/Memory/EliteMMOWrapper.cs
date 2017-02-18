using EliteMMO.API;
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MemoryAPI.Memory;
using MemoryAPI.Navigation;
using MemoryAPI.Windower;

namespace MemoryAPI
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
            private const double TooCloseDistance = 1.5;
            private readonly EliteAPI api;

            public double DistanceTolerance { get; set; } = 3;

            public NavigationTools(EliteAPI api)
            {
                this.api = api;
            }

            public bool FaceHeading(Position position)
            {
                var player = api.Entity.GetLocalPlayer();
                var angle = (byte)(Math.Atan((position.Z - player.Z) / (position.X - player.X)) * -(128.0f / Math.PI));
                if (player.X > position.X) angle += 128;
                var radian = (float)angle / 255 * 2 * Math.PI;
                return api.Entity.SetEntityHPosition(api.Entity.LocalPlayerIndex, (float)radian);
            }

            public double DistanceTo(Position position)
            {
                var player = api.Entity.GetLocalPlayer();

                return Math.Sqrt(
                    Math.Pow(position.X - player.X, 2) +
                    Math.Pow(position.Y - player.Y, 2) +
                    Math.Pow(position.Z - player.Z, 2));
            }

            public void Goto(Position position, bool useObjectAvoidance)
            {
                MoveTowardPosition(position, useObjectAvoidance);
                KeepOneYalmBack(position);
            }

            private void MoveTowardPosition(Position targetPosition, bool useObjectAvoidance)
            {
                if (!(DistanceTo(targetPosition) > DistanceTolerance)) return;

                DateTime duration = DateTime.Now.AddSeconds(5);

                while (DistanceTo(targetPosition) > DistanceTolerance && DateTime.Now < duration)
                {
                    var player = api.Player;

                    SetViewMode(ViewMode.FirstPerson);
                    FaceHeading(targetPosition);

                    api.AutoFollow.SetAutoFollowCoords(
                        targetPosition.X - player.X,
                        targetPosition.Y - player.Y,
                        targetPosition.Z - player.Z);

                    api.AutoFollow.IsAutoFollowing = true;

                    if (useObjectAvoidance) AvoidObstacles();

                    Thread.Sleep(30);
                }

                api.AutoFollow.IsAutoFollowing = false;
            }

            private void KeepOneYalmBack(Position position)
            {
                if (DistanceTo(position) > TooCloseDistance) return;

                DateTime duration = DateTime.Now.AddSeconds(5);
                api.ThirdParty.KeyDown(Keys.NUMPAD2);

                while (DistanceTo(position) > TooCloseDistance && DateTime.Now < duration)
                {
                    SetViewMode(ViewMode.FirstPerson);
                    FaceHeading(position);
                    Thread.Sleep(30);
                }

                api.ThirdParty.KeyUp(Keys.NUMPAD2);
            }

            private void SetViewMode(ViewMode viewMode)
            {
                if ((ViewMode) api.Player.ViewMode != viewMode)
                {
                    api.Player.ViewMode = (int) viewMode;
                }
            }

            /// <summary>
            /// Attempts to get a stuck player moving again.
            /// </summary>
            private void AvoidObstacles()
            {
                if (IsStuck())
                {
                    if (IsEngaged()) Disengage();
                    WiggleCharacter(attempts: 3);
                }
            }

            /// <summary>
            /// Determines if the player has become stuck.
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// Author: dlsmd
            /// http://www.elitemmonetwork.com/forums/viewtopic.php?p=4627#p4627
            /// </remarks>
            private bool IsStuck()
            {
                var firstX = api.Player.X;
                var firstZ = api.Player.Z;
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                var dchange = Math.Pow(firstX - api.Player.X, 2) + Math.Pow(firstZ - api.Player.Z, 2);
                return Math.Abs(dchange) < 1;
            }

            /// <summary>
            /// If the player is in fighting stance.
            /// </summary>
            /// <returns></returns>
            private bool IsEngaged()
            {
                return api.Player.Status == (ulong) Status.Fighting;
            }

            /// <summary>
            /// Stop fighting the current target.
            /// </summary>
            private void Disengage()
            {
                api.ThirdParty.SendString("/attack off");
                Thread.Sleep(30);
            }

            /// <summary>
            /// Wiggles the character left and right to become unstuck when stuck on an object.
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// Author: dlsmd
            /// http://www.elitemmonetwork.com/forums/viewtopic.php?p=4627#p4627
            /// </remarks>
            private void WiggleCharacter(int attempts)
            {
                int count = 0;
                float dir = -45;
                while (IsStuck() && attempts-- > 0)
                {
                    api.Entity.GetLocalPlayer().H = api.Player.H + (float)(Math.PI / 180 * dir);
                    api.ThirdParty.KeyDown(Keys.NUMPAD8);
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    api.ThirdParty.KeyUp(Keys.NUMPAD8);
                    count++;
                    if (count == 4)
                    {
                        dir = (Math.Abs(dir - -45) < .001 ? 45 : -45);
                        count = 0;
                    }
                }
                api.ThirdParty.KeyUp(Keys.NUMPAD8);
            }

            public void GotoNPC(int ID, bool useObjectAvoidance)
            {
                var entity = api.Entity.GetEntity(ID);
                Goto(Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H), useObjectAvoidance);
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

            public Position GetPosition(int id)
            {
                var entity = api.Entity.GetEntity(id);
                return Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H);
            }

            public short HPPCurrent(int id) { return api.Entity.GetEntity(id).HealthPercent; }

            public bool IsActive(int id) { return true; }

            public bool IsClaimed(int id) { return api.Entity.GetEntity(id).ClaimID != 0; }

            public int PetID(int id) => api.Entity.GetEntity(id).PetIndex;

            /// <summary>
            /// Checks to see if the object is rendered.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            /// Author: SG1234567
            /// https://github.com/SG1234567
            public bool IsRendered(int id)
            {
                return (api.Entity.GetEntity(id).Render0000 & 0x200) == 0x200;
            }

            public string Name(int id) { return api.Entity.GetEntity(id).Name; }

            public NpcType NPCType(int id)
            {
                var entity = api.Entity.GetEntity(id);
                if (entity.WarpPointer == 0) return NpcType.InanimateObject;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.Mob)) return NpcType.Mob;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.NPC)) return NpcType.NPC;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.PC)) return NpcType.PC;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.Self)) return NpcType.Self;
                return NpcType.InanimateObject;
            }

            private bool IsOfType(int one, int other)
            {
                return (one & other) == other;
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

            public float CastPercentEx
            {
                get { return (api.CastBar.Percent * 100); }
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

            public Position Position
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

            public void SendKeyPress(Keys keys)
            {
                api.ThirdParty.KeyPress(keys);
            }
        }
    }
}
