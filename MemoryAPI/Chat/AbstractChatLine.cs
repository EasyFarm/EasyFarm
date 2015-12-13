using System;
using System.Drawing;

namespace MemoryAPI
{
    public class AbstractChatLine : IChatLine
    {
        public virtual Color Color { get; set; }
        public virtual int Index { get; set; }
        public virtual string Now { get; set; }
        public virtual DateTime NowDate { get; set; }
        public virtual string[] RawString { get; set; }
        public virtual string Text { get; set; }
        public virtual ChatMode Type { get; set; }
    }
}
