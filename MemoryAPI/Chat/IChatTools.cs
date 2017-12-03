using System.Collections.Generic;
using EliteMMO.API;

namespace MemoryAPI.Chat
{
    public interface IChatTools
    {
        Queue<EliteAPI.ChatEntry> ChatEntries { get; set; }
    }
}
