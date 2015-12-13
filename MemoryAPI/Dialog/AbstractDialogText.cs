namespace MemoryAPI
{
    public interface IDialogText
    {
        string[] Options { get; }
        string Question { get; }

        IDialogText Clone();
        IDialogText Clone(IDialogText dt);
    }
}