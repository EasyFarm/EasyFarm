using EasyFarm.Classes;
using MemoryAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EasyFarm.Tests.UnitTests.Classes
{
    public class PathRecorderTest
    {
        protected PathRecorder recorder;

        [TestInitialize]
        public void Initialize()
        {
            recorder = new PathRecorder(new MMemoryWrapper());
            recorder.Interval = 5;
        }

        [TestClass]
        public class Start : PathRecorderTest
        {
            /// <summary>
            /// Test whether the position recorder records positions. 
            /// </summary>
            [TestMethod]
            public void Adds_Waypoint_To_Path()
            {                
                var position = RecordNewPosition();
                Assert.IsNotNull(position);
            }

            /// <summary>
            /// Starts recording a new waypoint. 
            /// </summary>
            private IPosition RecordNewPosition()
            {
                var task = new TaskCompletionSource<IPosition>();
                recorder.OnPositionAdded += (pos) => task.SetResult(pos);
                recorder.Interval = 1;
                recorder.Start();
                var position = task.Task.GetAwaiter().GetResult();
                recorder.Stop();
                return position;
            }            
        }        
    }

    public class MMemoryWrapper : MemoryWrapper
    {
        public MMemoryWrapper()
        {
            this.Player = new MPlayerTools();
        }

        public class MPlayerTools : IPlayerTools
        {
            public short CastPercentEx { get; }
            public int HPPCurrent { get; }
            public int ID { get; }
            public int MPCurrent { get; }
            public int MPPCurrent { get; }
            public string Name { get; }

            public IPosition Position
            {
                get
                {
                    return new Position() { H = 1, X = 1, Y = 1, Z = 1 };
                }
            }

            public float PosX { get; }
            public float PosY { get; }
            public float PosZ { get; }
            public Structures.PlayerStats Stats { get; }
            public Status Status { get; }
            public StatusEffect[] StatusEffects { get; }
            public int TPCurrent { get; }
            public Zone Zone { get; }
        }
    }
}