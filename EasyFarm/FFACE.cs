using System;
using MemoryAPI;
using EliteMMO.API;
using MathNet.Numerics;
using System.Collections.Generic;
using EasyFarm.Classes;
using System.Linq;
using static Helpers;

public class MemoryWrapper : IMemoryAPI
{
    private int id;

    MemoryAPI.INavigatorTools navigatorTools;
    MemoryAPI.INPCTools npcTools;
    Dictionary<byte, IPartyMemberTools> partyMemberTools;
    MemoryAPI.IPlayerTools playerTools;
    MemoryAPI.ITargetTools targetTools;
    MemoryAPI.ITimerTools timerTools;
    MemoryAPI.IWindowerTools windowerTools;

    public MemoryWrapper(int id)
    {
        this.id = id;
    }

    public INavigatorTools Navigator
    {
        get { return navigatorTools; }
        set { navigatorTools = value; }
    }

    public INPCTools NPC
    {
        get { return npcTools; }
        set { npcTools = value; }
    }

    public Dictionary<byte, IPartyMemberTools> PartyMember
    {
        get { return partyMemberTools; }
        set { partyMemberTools = value; }
    }

    public IPlayerTools Player
    {
        get { return playerTools; }
        set { playerTools = value; }
    }

    public ITargetTools Target
    {
        get { return targetTools; }
        set { targetTools = value; }
    }

    public ITimerTools Timer
    {
        get { return timerTools; }
        set { timerTools = value; }
    }

    public IWindowerTools Windower
    {
        get { return windowerTools; }
        set { windowerTools = value; }
    }

    public class EliteMMOWrapper : MemoryWrapper
    {
        private readonly EliteAPI EliteAPI;

        public EliteMMOWrapper(int pid) : base(pid)
        {
            this.EliteAPI = new EliteAPI(pid);
        }
    }
}

public class NavigationTools : INavigatorTools
{
    private readonly EliteAPI api;

    public double DistanceTolerance
    {
        get { return 0; }
        set { }
    }

    public NavigationTools(EliteAPI api)
    {
        this.api = api;
    }

    public bool FaceHeading(IPosition position) { return true; }

    public double DistanceTo(IPosition position) { return 0; }

    public void Goto(IPosition position, bool KeepRunning) { }

    public void GotoNPC(int ID) { }

    public void Reset() { }
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
        return CreatePosition(entity.X, entity.Y, entity.Z, entity.H);
    }

    public short HPPCurrent(int id) { return api.Entity.GetEntity(id).HealthPercent; }

    public bool IsActive(int id) { return true; }

    public bool IsClaimed(int id) { return true; }

    public bool IsRendered(int id) { return true; }

    public string Name(int id) { return api.Entity.GetEntity(id).Name; }

    public NPCType NPCType(int id) { return (MemoryAPI.NPCType)api.Entity.GetEntity(id).Type; }

    public float PosX(int id) { return api.Entity.GetEntity(id).X; }

    public float PosY(int id) { return api.Entity.GetEntity(id).Y; }

    public float PosZ(int id) { return api.Entity.GetEntity(id).Z; }

    public Status Status(int id) { return (MemoryAPI.Status)api.Entity.GetEntity(id).Status; }
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

            return CreatePosition(x, y, z, h);
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
        get { return (MemoryAPI.Status)api.Player.Status; }
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
        get { return (int)api.Target.GetTargetInfo().TargetId; }
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

    public int GetAbilityRecast(AbilityList abil)
    {
        return api.Recast.GetAbilityRecast((int)abil);
    }

    public short GetSpellRecast(SpellList spell)
    {
        return (short)api.Recast.GetSpellRecast((int)spell);
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

public class Helpers
{
    public static IPosition CreatePosition(float x, float y, float z, float h)
    {
        var position = new Position();

        position.X = x;
        position.Y = y;
        position.Z = z;
        position.H = h;

        return position;
    }
}