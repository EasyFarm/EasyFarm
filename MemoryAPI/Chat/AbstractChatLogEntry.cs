using System;
using System.Drawing;

namespace MemoryAPI
{
    public abstract class AbstractChatLogEntry : IChatLogEntry
    {
        public virtual Color ActualLineColor { get; set; }
        public virtual int Index { get; set; }
        public virtual string LineColor { get; set; }
        public virtual string LineText { get; set; }
        public virtual DateTime LineTime { get; set; }
        public virtual string LineTimeString { get; set; }
        public virtual ChatMode LineType { get; set; }
        public virtual string[] RawString { get; set; }

        public abstract object Clone();
        public abstract int CompareTo(object obj);
    }
}
