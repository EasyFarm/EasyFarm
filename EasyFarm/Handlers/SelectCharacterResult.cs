using System;
using System.Diagnostics;

namespace EasyFarm.Handlers
{
    public partial class SelectCharacterRequestHandler
    {
        public class SelectCharacterResult
        {
            public Process Process { get; set; }
            public Boolean IsSelected { get; set; }
        }
    }
}