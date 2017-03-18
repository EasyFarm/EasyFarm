using MemoryAPI;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeMemoryAPI : IMemoryAPI
    {
        public INavigatorTools Navigator { get; set; }
        public INPCTools NPC { get; set; }
        public System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public IPlayerTools Player { get; set; }
        public ITargetTools Target { get; set; }
        public ITimerTools Timer { get; set; }
        public IWindowerTools Windower { get; set; }
    }
}
