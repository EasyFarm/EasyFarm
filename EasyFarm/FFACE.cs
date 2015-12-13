using System;
using MemoryAPI;
using EliteMMO.API;
using MathNet.Numerics;
using System.Collections.Generic;
using EasyFarm.Classes;

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

        var position = new Position();

        position.X = entity.X;
        position.Y = entity.Y;
        position.Z = entity.Z;
        position.H = entity.H;

        return position;
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

    public PartyMemberTools(EliteAPI api)
    {
        this.api = api;
    }

    public int ServerID
    {
        get { return 0; }
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
        get { return 0; }
    }

    public int HPPCurrent
    {
        get { return 100; }
    }

    public int ID
    {
        get { return 1000; }
    }

    public int MPCurrent
    {
        get { return 1000; }
    }

    public int MPPCurrent
    {
        get { return 100; }
    }

    public string Name
    {
        get { return "Mykezero"; }
    }

    public IPosition Position
    {
        get { return default(IPosition); }
    }

    public float PosX
    {
        get { return 0; }
    }

    public float PosY
    {
        get { return 0; }
    }

    public float PosZ
    {
        get { return 0; }
    }

    public Structures.PlayerStats Stats
    {
        get { return default(Structures.PlayerStats); }
    }

    public Status Status
    {
        get { return Status.Fighting; }
    }

    public MemoryAPI.StatusEffect[] StatusEffects
    {
        get { return new MemoryAPI.StatusEffect[0]; }
    }

    public int TPCurrent
    {
        get { return 1000; }
    }

    public Zone Zone
    {
        get { return Zone.Rolanberry_Fields; }
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
        get { return 0; }
    }

    public bool SetNPCTarget(int index) { return true; }
}

public class TimerTools : ITimerTools
{
    private readonly EliteAPI api;

    public TimerTools(EliteAPI api)
    {
        this.api = api;
    }

    public int GetAbilityRecast(AbilityList abil) { return 0; }

    public short GetSpellRecast(SpellList spell) { return 0; }
}

public class WindowerTools : IWindowerTools
{
    private readonly EliteAPI api;

    public WindowerTools(EliteAPI api)
    {
        this.api = api;
    }

    public void SendString(string stringToSend) { }
}

