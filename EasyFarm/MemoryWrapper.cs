using MemoryAPI;
using System.Collections.Generic;

public class MemoryWrapper : IMemoryAPI
{
    private int id;

    INavigatorTools navigatorTools;
    INPCTools npcTools;
    Dictionary<byte, IPartyMemberTools> partyMemberTools;
    IPlayerTools playerTools;
    ITargetTools targetTools;
    ITimerTools timerTools;
    IWindowerTools windowerTools;

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
}