namespace MemoryAPI
{
    public interface IWindowerTools
    {
        short ArgumentCount();
        void BlockInput(bool block);
        string GetArgument(short index);
        int IsNewCommand();
        void SendKey(KeyCode key, bool down);
        void SendKeyPress(KeyCode key);
        void SendString(string stringToSend);
    }
}