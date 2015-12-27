using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

public class MemoryWrapper : IMemoryAPI
{
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

    public INavigatorTools Navigator { get; set; }

    public INPCTools NPC { get; set; }

    public Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }

    public IPlayerTools Player { get; set; }

    public ITargetTools Target { get; set; }

    public ITimerTools Timer { get; set; }

    public IWindowerTools Windower { get; set; }
}