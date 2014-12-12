using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.BehaviorTree
{
    [Flags]
    public enum ContinueType : uint
    {
        Start = 0,
        Finished = 1,
        KeepGoing = 2,
        Error = 4
    }

    public class GameState
    {
        public FFACE FFACE;
        public List<TreeBase> Actors = new List<TreeBase>();
        public ContinueType ContinueMask;

        public GameState(FFACE fface)
        {
            FFACE = fface;
        }

        public void Update()
        {
            Actors.Clear();
            ContinueMask = ContinueType.Start;
        }
    }
}
