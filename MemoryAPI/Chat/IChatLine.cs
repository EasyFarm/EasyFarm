using System;
using System.Drawing;

namespace MemoryAPI
{
    public interface IChatLine
    {
        Color Color { get; set; }
        int Index { get; set; }
        string Now { get; set; }
        DateTime NowDate { get; set; }
        string[] RawString { get; set; }
        string Text { get; set; }
        ChatMode Type { get; set; }

        bool Equals(object o);
        int GetHashCode();
    }
}