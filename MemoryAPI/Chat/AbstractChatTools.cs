namespace MemoryAPI
{
    public abstract class AbstractChatTools : IChatTools
    {
        public virtual int GetLineCount { get; }
        public virtual bool IsNewLine { get; }

        public abstract void Clear();
        public abstract IChatLine GetNextLine();
        public abstract IChatLine GetNextLine(LineSettings lineSettings);
    }
}
