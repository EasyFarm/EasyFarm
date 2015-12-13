namespace MemoryAPI
{
    public interface IMemoryAPI
    {
        IChatTools Chat { get; set; }
        IFishTools Fish { get; set; }
        IItemTools Item { get; set; }
        IMenuTools Menu { get; set; }
        INavigatorTools Navigator { get; set; }
        INPCTools NPC { get; set; }
        IPartyTools Party { get; set; }
        System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        IPlayerTools Player { get; set; }
        ISearchTools Search { get; set; }
        ITargetTools Target { get; set; }
        ITimerTools Timer { get; set; }
        IWindowerTools Windower { get; set; }
        int _InstanceID { get; set; }
    }
}