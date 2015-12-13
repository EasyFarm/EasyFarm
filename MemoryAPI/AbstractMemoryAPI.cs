namespace MemoryAPI
{
    public abstract class AbstractMemoryAPI : IMemoryAPI
    {
        public virtual INavigatorTools Navigator { get; set; }
        public virtual INPCTools NPC { get; set; }
        public virtual System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public virtual IPlayerTools Player { get; set; }
        public virtual ITargetTools Target { get; set; }
        public virtual ITimerTools Timer { get; set; }
        public virtual IWindowerTools Windower { get; set; }
        public virtual int _InstanceID { get; set; }
    }
}