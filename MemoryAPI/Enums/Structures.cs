using System;
using System.Runtime.InteropServices;

namespace MemoryAPI
{
    public class Structures
    {
        /// <summary>
        /// Stats of the player
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PlayerStats
        {
            public short Str;
            public short Dex;
            public short Vit;
            public short Agi;
            public short Int;
            public short Mnd;
            public short Chr;
        } // @ public struct PlayerStats
    }
}