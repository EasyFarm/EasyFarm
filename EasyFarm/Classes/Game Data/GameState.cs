using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class GameState
    {
        FFACE _session;

        public GameState(FFACE Session)
        {
            this._session = Session;
        }
    }
}
