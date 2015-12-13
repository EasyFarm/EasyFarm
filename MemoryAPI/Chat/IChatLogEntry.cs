using System;
using System.Drawing;

namespace MemoryAPI
{
    internal interface IChatLogEntry
    {
        Color ActualLineColor { get; set; }
        int Index { get; set; }
        string LineColor { get; set; }
        string LineText { get; set; }
        DateTime LineTime { get; set; }
        string LineTimeString { get; set; }
        ChatMode LineType { get; set; }
        string[] RawString { get; set; }

        object Clone();
        int CompareTo(object obj);
        bool Equals(object o);
        int GetHashCode();
    }
}