namespace MemoryAPI
{
    public abstract class AbstractMemoryAPI : IMemoryAPI
    {
        public virtual IChatTools Chat { get; set; }
        public virtual IFishTools Fish { get; set; }
        public virtual IItemTools Item { get; set; }
        public virtual IMenuTools Menu { get; set; }
        public virtual INavigatorTools Navigator { get; set; }
        public virtual INPCTools NPC { get; set; }
        public virtual IPartyTools Party { get; set; }
        public virtual System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public virtual IPlayerTools Player { get; set; }
        public virtual ISearchTools Search { get; set; }
        public virtual ITargetTools Target { get; set; }
        public virtual ITimerTools Timer { get; set; }
        public virtual IWindowerTools Windower { get; set; }
        public virtual int _InstanceID { get; set; }
    }
}