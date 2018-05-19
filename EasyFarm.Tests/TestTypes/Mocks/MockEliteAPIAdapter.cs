using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    /// <summary>
    /// Allow <see cref="MockEliteAPI"/> to have more flexibility in using the mock version of the
    /// IMemoryAPI properties.
    /// </summary>
    public class MockEliteAPIAdapter : IMemoryAPI
    {
        public MockEliteAPIAdapter(MockEliteAPI mockEliteAPI)
        {
            Player = mockEliteAPI.Player;
            Windower = mockEliteAPI.Windower;
            Chat = mockEliteAPI.Chat;
            NPC = mockEliteAPI.NPC;
            Navigator = mockEliteAPI.Navigator;
            PartyMember = mockEliteAPI.PartyMember;
            Target = mockEliteAPI.Target;
            Timer = mockEliteAPI.Timer;
        }

        public INavigatorTools Navigator { get; set; }
        public INPCTools NPC { get; set; }
        public Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public IPlayerTools Player { get; set; }
        public ITargetTools Target { get; set; }
        public ITimerTools Timer { get; set; }
        public IWindowerTools Windower { get; set; }
        public IChatTools Chat { get; set; }
    }
}