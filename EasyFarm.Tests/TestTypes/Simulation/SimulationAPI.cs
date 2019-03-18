using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Memory;
using MemoryAPI.Resources;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes.Simulation
{
    public class SimulationAPI : IMemoryAPI
    {
        public SimulationAPI(ISimulation simulation)
        {
            Chat = new SimulationChatTools(simulation);
            Windower = new SimulationWindowerTools(simulation);
            Timer = new SimulationTimerTools(simulation);
            Target = new SimulationTargetTools(simulation);
            Player = new SimulationPlayerTools(simulation);
            NPC = new SimulationNPC(simulation);
            Navigator = new SimulationNavigator(simulation);
            PartyMember = new Dictionary<byte, IPartyMemberTools>();

            for (byte i = 0; i < 16; i++)
            {
                PartyMember.Add(i, new SimulationPartyMemberTools(i, simulation));
            }
        }

        public INavigatorTools Navigator { get; set; }
        public INPCTools NPC { get; set; }
        public Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public IPlayerTools Player { get; set; }
        public ITargetTools Target { get; set; }
        public ITimerTools Timer { get; set; }
        public IWindowerTools Windower { get; set; }
        public IChatTools Chat { get; set; }

        public IResourcesTools Resource { get; set; }
    }
}