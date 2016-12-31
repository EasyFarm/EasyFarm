using MemoryAPI.Windower;

namespace MemoryAPI
{
    public interface IMemoryAPI
    {        
        INavigatorTools Navigator { get; set; }
        INPCTools NPC { get; set; }
        System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        IPlayerTools Player { get; set; }
        ITargetTools Target { get; set; }
        ITimerTools Timer { get; set; }
        IWindowerTools Windower { get; set; }
    }
}