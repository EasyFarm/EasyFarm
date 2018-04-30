using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.UserSettings;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Navigation;
using MemoryAPI.Windower;
using StatusEffect = MemoryAPI.StatusEffect;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    /// <summary>
    /// A fake implementation of the EliteAPI which switches state based on messages passed to it.
    /// </summary>
    /// <remarks>
    /// We'll use this class to more accurately test the game state. The idea here is that we don't
    /// want to run the full game for our testing since the tests would take forever to run, and
    /// would break with the slightest changes in the game client.
    ///
    /// Instead, we'll use a mock object that switches state based on how the client interacts with
    /// this object. If the program sends the /attack on command, we'll update the mock's state for
    /// the player to Status.Fighting.
    ///
    /// This way, we can test the states from end to end, without bootstraping the game and all of
    /// its dependencies.
    /// </remarks>
    public class MockEliteAPI
    {
        public MockEliteAPI()
        {
            Player = new MockPlayerTools();
            NPC = new MockNPCTools();
            Windower = new MockWindowerTools(this);
            Navigator = new MockNavigatorTools();
            Timer = new MockTimerTools();
            Target = new MockTargetTools();
        }

        public MockNavigatorTools Navigator { get; set; }
        public MockNPCTools NPC { get; set; }
        public Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        public MockPlayerTools Player { get; set; }
        public ITargetTools Target { get; set; }
        public MockTimerTools Timer { get; set; }
        public MockWindowerTools Windower { get; set; }
        public IChatTools Chat { get; set; }

        public static MockEliteAPI Create()
        {
            return new MockEliteAPI();
        }
    }

    public class MockTargetTools : ITargetTools
    {
        public int ID { get; }
        public bool SetNPCTarget(int index)
        {
            return true;
        }
    }

    public class MockTimerTools : ITimerTools
    {
        public int RecastTime { get; set; }

        public int GetAbilityRecast(int index)
        {
            return RecastTime;
        }

        public int GetSpellRecast(int index)
        {
            return RecastTime;
        }
    }

    public class MockNavigatorTools : INavigatorTools
    {
        public double DistanceTolerance { get; set; }
        public bool IsRunning { get; set; }

        public void FaceHeading(Position position)
        {
        }

        public void GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning)
        {
            throw new System.NotImplementedException();
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            IsRunning = false;
        }
    }

    public class MockNPC
    {
        public int ClaimID { get; set; }
        public double Distance { get; set; }
        public Position Position { get; set; } = new Position();
        public short HealthPercent { get; set; }
        public bool IsActive { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRendered { get; set; }
        public string Name { get; set; }
        public NpcType NPCType { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Status Status { get; set; }
        public int PetID { get; set; }
    }

    public class MockNPCTools : INPCTools
    {
        public MockNPCTools()
        {
            InitializeEntities();
        }

        private void InitializeEntities()
        {
            Entities = Enumerable.Range(0, Constants.UnitArrayMax)
                .Select(x => new KeyValuePair<int, MockNPC>(x, new MockNPC()))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void AddOrUpdateEntity(int index, MockNPC entity)
        {
            if (Entities.ContainsKey(index))
                Entities[index] = entity;
            else
                Entities.Add(index, entity);
        }

        public Dictionary<int, MockNPC> Entities { get; set; } = new Dictionary<int, MockNPC>();

        public int ClaimedID(int id)
        {
            return Entities[id].ClaimID;
        }

        public double Distance(int id)
        {
            return Entities[id].Distance;
        }

        public Position GetPosition(int id)
        {
            return Entities[id].Position;
        }

        public short HPPCurrent(int id)
        {
            return Entities[id].HealthPercent;
        }

        public bool IsActive(int id)
        {
            return Entities[id].IsActive;
        }

        public bool IsClaimed(int id)
        {
            return Entities[id].IsClaimed;
        }

        public bool IsRendered(int id)
        {
            return Entities[id].IsRendered;
        }

        public string Name(int id)
        {
            return Entities[id].Name;
        }

        public NpcType NPCType(int id)
        {
            return Entities[id].NPCType;
        }

        public float PosX(int id)
        {
            return Entities[id].PosX;
        }

        public float PosY(int id)
        {
            return Entities[id].PosY;
        }

        public float PosZ(int id)
        {
            return Entities[id].PosZ;
        }

        public Status Status(int id)
        {
            return Entities[id].Status;
        }

        public int PetID(int id)
        {
            return Entities[id].PetID;
        }
    }

    public class MockPlayerTools : IPlayerTools
    {
        public float CastPercentEx { get; set; }
        public int HPPCurrent { get; set; }
        public int ID { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Structures.PlayerStats Stats { get; set; }
        public Status Status { get; set; }
        public StatusEffect[] StatusEffects { get; set; } = new StatusEffect[0];
        public int TPCurrent { get; set; }
        public Zone Zone { get; set; }
        public Job Job { get; set; }
        public Job SubJob { get; set; }
    }

    public class MockWindowerTools : IWindowerTools
    {
        private readonly MockEliteAPI _eliteAPI;

        public string LastCommand { get; set; }
        public IList<Keys> KeyPresses { get; set; } = new List<Keys>();
        public Keys LastKeyPress { get; set; }

        public MockWindowerTools(MockEliteAPI eliteAPI)
        {
            _eliteAPI = eliteAPI;
        }

        public void SendString(string stringToSend)
        {
            LastCommand = stringToSend;

            if (stringToSend == Constants.AttackTarget)
            {
                _eliteAPI.Player.Status = Status.Fighting;
            }

            if (stringToSend == Constants.RestingOn)
            {
                _eliteAPI.Player.Status = Status.Healing;
            }

            if (stringToSend == Constants.RestingOff)
            {
                _eliteAPI.Player.Status = Status.Standing;
            }
        }

        public void SendKeyPress(Keys key)
        {
            LastKeyPress = key;
            KeyPresses.Add(key);
        }
    }
}
