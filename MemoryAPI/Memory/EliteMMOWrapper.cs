// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EliteMMO.API;
using MemoryAPI.Chat;
using MemoryAPI.Navigation;
using MemoryAPI.Windower;

namespace MemoryAPI.Memory
{
    public class EliteMmoWrapper : MemoryWrapper
    {
        public enum ViewMode
        {
            ThirdPerson = 0,
            FirstPerson
        }

        public EliteMmoWrapper(int pid)
        {
            var eliteApi = new EliteAPI(pid);

            Navigator = new NavigationTools(eliteApi);
            NPC = new NpcTools(eliteApi);
            PartyMember = new Dictionary<byte, IPartyMemberTools>();
            Player = new PlayerTools(eliteApi);
            Target = new TargetTools(eliteApi);
            Timer = new TimerTools(eliteApi);
            Windower = new WindowerTools(eliteApi);
            Chat = new ChatTools(eliteApi);

            for (byte i = 0; i < 16; i++)
            {
                PartyMember.Add(i, new PartyMemberTools(eliteApi, i));
            }
        }

        public class NavigationTools : INavigatorTools
        {
            private const double TooCloseDistance = 1.5;
            private readonly EliteAPI _api;

            public double DistanceTolerance { get; set; } = 3;

            public NavigationTools(EliteAPI api)
            {
                _api = api;
            }

            public void FaceHeading(Position position)
            {
                var player = _api.Entity.GetLocalPlayer();
                var angle = (byte)(Math.Atan((position.Z - player.Z) / (position.X - player.X)) * -(128.0f / Math.PI));
                if (player.X > position.X) angle += 128;
                var radian = (float)angle / 255 * 2 * Math.PI;
                _api.Entity.SetEntityHPosition(_api.Entity.LocalPlayerIndex, (float)radian);
            }

            private double DistanceTo(Position position)
            {
                var player = _api.Entity.GetLocalPlayer();

                return Math.Sqrt(
                    Math.Pow(position.X - player.X, 2) +
                    Math.Pow(position.Y - player.Y, 2) +
                    Math.Pow(position.Z - player.Z, 2));
            }

            public void GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning)
            {
                if (!(DistanceTo(position) > DistanceTolerance)) return;
                MoveForwardTowardsPosition(() => position, useObjectAvoidance);
                if (!keepRunning) Reset();
            }

            public void GotoNPC(int id, bool useObjectAvoidance)
            {
                MoveForwardTowardsPosition(() => GetEntityPosition(id), useObjectAvoidance);
                KeepOneYalmBack(GetEntityPosition(id));
                FaceHeading(GetEntityPosition(id));
                Reset();
            }

            private Position GetEntityPosition(int id)
            {
                var entity = _api.GetCachedEntity(id);
                var position = Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H);
                return position;
            }            

            private void MoveForwardTowardsPosition(
                Func<Position> targetPosition,
                bool useObjectAvoidance)
            {
                if (!(DistanceTo(targetPosition()) > DistanceTolerance)) return;

                DateTime duration = DateTime.Now.AddSeconds(5);

                while (DistanceTo(targetPosition()) > DistanceTolerance && DateTime.Now < duration)
                {
                    SetViewMode(ViewMode.FirstPerson);
                    FaceHeading(targetPosition());
                    _api.ThirdParty.KeyDown(Keys.NUMPAD8);
                    if (useObjectAvoidance) AvoidObstacles();
                    Thread.Sleep(100);
                }
            }

            private void KeepRunningWithKeyboard()
            {
                _api.ThirdParty.KeyDown(Keys.NUMPAD8);
            }

            private void KeepOneYalmBack(Position position)
            {
                if (DistanceTo(position) > TooCloseDistance) return;

                DateTime duration = DateTime.Now.AddSeconds(5);
                _api.ThirdParty.KeyDown(Keys.NUMPAD2);

                while (DistanceTo(position) <= TooCloseDistance && DateTime.Now < duration)
                {
                    SetViewMode(ViewMode.FirstPerson);
                    FaceHeading(position);
                    Thread.Sleep(30);
                }

                _api.ThirdParty.KeyUp(Keys.NUMPAD2);
            }

            private void SetViewMode(ViewMode viewMode)
            {
                if ((ViewMode)_api.Player.ViewMode != viewMode)
                {
                    _api.Player.ViewMode = (int)viewMode;
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
                var firstX = _api.Player.X;
                var firstZ = _api.Player.Z;
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                var dchange = Math.Pow(firstX - _api.Player.X, 2) + Math.Pow(firstZ - _api.Player.Z, 2);
                return Math.Abs(dchange) < 1;
            }

            /// <summary>
            /// If the player is in fighting stance.
            /// </summary>
            /// <returns></returns>
            private bool IsEngaged()
            {
                return _api.Player.Status == (ulong)Status.Fighting;
            }

            /// <summary>
            /// Stop fighting the current target.
            /// </summary>
            private void Disengage()
            {
                _api.ThirdParty.SendString("/attack off");
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
                    _api.Entity.GetLocalPlayer().H = _api.Player.H + (float)(Math.PI / 180 * dir);
                    _api.ThirdParty.KeyDown(Keys.NUMPAD8);
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    _api.ThirdParty.KeyUp(Keys.NUMPAD8);
                    count++;
                    if (count == 4)
                    {
                        dir = (Math.Abs(dir - -45) < .001 ? 45 : -45);
                        count = 0;
                    }
                }
                _api.ThirdParty.KeyUp(Keys.NUMPAD8);
            }

            public void Reset()
            {
                _api.ThirdParty.KeyUp(Keys.NUMPAD8);
                _api.ThirdParty.KeyUp(Keys.NUMPAD2);
            }
        }

        public class NpcTools : INPCTools
        {
            private readonly EliteAPI _api;

            public NpcTools(EliteAPI api)
            {
                _api = api;
            }

            public int ClaimedID(int id) { return (int)_api.GetCachedEntity(id).ClaimID; }

            public double Distance(int id) { return _api.GetCachedEntity(id).Distance; }

            public Position GetPosition(int id)
            {
                var entity = _api.GetCachedEntity(id);
                return Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H);
            }

            public short HPPCurrent(int id) { return _api.GetCachedEntity(id).HealthPercent; }

            public bool IsActive(int id) { return true; }

            public bool IsClaimed(int id) { return _api.GetCachedEntity(id).ClaimID != 0; }

            public int PetID(int id) => _api.GetCachedEntity(id).PetIndex;

            /// <summary>
            /// Checks to see if the object is rendered.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            /// Author: SG1234567
            /// https://github.com/SG1234567
            public bool IsRendered(int id)
            {
                return (_api.GetCachedEntity(id).Render0000 & 0x200) == 0x200;
            }

            public string Name(int id) { return _api.GetCachedEntity(id).Name; }

            public NpcType NPCType(int id)
            {
                var entity = _api.GetCachedEntity(id);
                return Helpers.GetNpcType(entity);
            }

            public float PosX(int id) { return _api.GetCachedEntity(id).X; }

            public float PosY(int id) { return _api.GetCachedEntity(id).Y; }

            public float PosZ(int id) { return _api.GetCachedEntity(id).Z; }

            public Status Status(int id)
            {
                var status = (EntityStatus)_api.GetCachedEntity(id).Status;
                return Helpers.ToStatus(status);
            }
        }

        public class PartyMemberTools : IPartyMemberTools
        {
            private readonly EliteAPI _api;
            private readonly int _index;

            private EliteAPI.PartyMember Unit
            {
                get
                {
                    var member = _api.Party.GetPartyMember(_index);
                    return member;
                }
            }

            public PartyMemberTools(EliteAPI api, int index)
            {
                _api = api;
                _index = index;
            }

            public bool UnitPresent => Convert.ToBoolean(Unit.Active);

            public int ServerID => (int)Unit.ID;

            public string Name => Unit.Name;

            public int HPCurrent => (int)Unit.CurrentHP;

            public int HPPCurrent => Unit.CurrentHPP;

            public int MPCurrent => (int)Unit.CurrentMP;

            public int MPPCurrent => Unit.CurrentMPP;

            public int TPCurrent => (int)Unit.CurrentTP;

            public Job Job => (Job)Unit.MainJob;

            public Job SubJob => (Job)Unit.SubJob;

            public NpcType NpcType
            {
                get
                {
                    var key = $"PartyMember.NpcType.{_index}";
                    var result = RuntimeCache.Get<NpcType?>(key);

                    if (result == null)
                    {
                        var entity = FindEntityByServerId(ServerID);
                        var npcType = Helpers.GetNpcType(entity);
                        RuntimeCache.Set(key, npcType, DateTimeOffset.Now.AddSeconds(3));
                        return npcType;
                    }

                    return result.Value;
                }
            }

            private EliteAPI.EntityEntry FindEntityByServerId(int serverId)
            {
                return Enumerable.Range(0, 4096)
                    .Select(_api.GetCachedEntity)
                    .FirstOrDefault(x => x.ServerID == serverId);
            }
        }

        public class PlayerTools : IPlayerTools
        {
            private readonly EliteAPI _api;

            public PlayerTools(EliteAPI api)
            {
                _api = api;
            }

            public float CastPercentEx => (_api.CastBar.Percent * 100);

            public int HPPCurrent => (int)_api.Player.HPP;

            public int ID => _api.Player.ServerId;

            public int MPCurrent => (int)_api.Player.MP;

            public int MPPCurrent => (int)_api.Player.MPP;

            public string Name => _api.Player.Name;

            public Position Position
            {
                get
                {
                    var x = _api.Player.X;
                    var y = _api.Player.Y;
                    var z = _api.Player.Z;
                    var h = _api.Player.H;

                    return Helpers.ToPosition(x, y, z, h);
                }
            }

            public float PosX => Position.X;

            public float PosY => Position.Y;

            public float PosZ => Position.Z;

            public Structures.PlayerStats Stats
            {
                get
                {
                    var stats = _api.Player.Stats;

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

            public Status Status => Helpers.ToStatus((EntityStatus)_api.Player.Status);

            public StatusEffect[] StatusEffects
            {
                get
                {
                    return _api.Player.Buffs.Select(x => (StatusEffect)x).ToArray();
                }
            }

            public int TPCurrent => (int)_api.Player.TP;

            public Zone Zone => (Zone)_api.Player.ZoneId;

            public Job Job => (Job)_api.Player.MainJob;

            public Job SubJob => (Job)_api.Player.SubJob;
        }

        public class TargetTools : ITargetTools
        {
            private readonly EliteAPI _api;

            public TargetTools(EliteAPI api)
            {
                _api = api;
            }

            public int ID => (int)_api.Target.GetTargetInfo().TargetIndex;

            public bool SetNPCTarget(int index)
            {
                return _api.Target.SetTarget(index);
            }
        }

        public class TimerTools : ITimerTools
        {
            private readonly EliteAPI _api;

            public TimerTools(EliteAPI api)
            {
                _api = api;
            }

            public int GetAbilityRecast(int index)
            {
                var ids = _api.Recast.GetAbilityIds();
                var ability = _api.Resources.GetAbility((uint)index);
                var idx = ids.IndexOf(ability.TimerID);
                return _api.Recast.GetAbilityRecast(idx);
            }

            public int GetSpellRecast(int index)
            {
                return _api.Recast.GetSpellRecast(index);
            }
        }

        public class WindowerTools : IWindowerTools
        {
            private readonly EliteAPI _api;

            public WindowerTools(EliteAPI api)
            {
                _api = api;
            }

            public void SendString(string stringToSend)
            {
                _api.ThirdParty.SendString(stringToSend);
            }

            public void SendKeyPress(Keys keys)
            {
                _api.ThirdParty.KeyPress(keys);
            }
        }

        public class ChatTools : IChatTools
        {
            private readonly EliteAPI _api;
            public Queue<EliteAPI.ChatEntry> ChatEntries { get; set; } = new Queue<EliteAPI.ChatEntry>();

            public ChatTools(EliteAPI api)
            {
                _api = api;
                var timer = new PollingProcessor(QueueChatEntries);
                timer.Start();
            }

            private void QueueChatEntries()
            {
                EliteAPI.ChatEntry chatEntry;
                while ((chatEntry =  _api.Chat.GetNextChatLine()) != null)
                {
                    ChatEntries.Enqueue(chatEntry);
                }
            }
        }
    }
}
