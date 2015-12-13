namespace MemoryAPI
{
    public interface IChatTools
    {
        int GetLineCount { get; }
        bool IsNewLine { get; }

        void Clear();
        IChatLine GetNextLine();
        IChatLine GetNextLine(LineSettings lineSettings);
    }
}