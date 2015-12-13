using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

public class MemoryWrapper : IMemoryAPI
{
    INavigatorTools navigatorTools;
    INPCTools npcTools;
    Dictionary<byte, IPartyMemberTools> partyMemberTools;
    IPlayerTools playerTools;
    ITargetTools targetTools;
    ITimerTools timerTools;
    IWindowerTools windowerTools;

    public static MemoryWrapper Create(int pid)
    {
        try
        {
            string value = ConfigurationManager.AppSettings["MemoryWrapper"];
            if (string.IsNullOrWhiteSpace(value)) return null;
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetType(value);
            return (MemoryWrapper)Activator.CreateInstance(type, pid);
        }
        catch (Exception)
        {
            return null;
        }
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