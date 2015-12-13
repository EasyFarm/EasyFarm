namespace MemoryAPI
{
    public abstract class AbstractDialogText : IDialogText
    {
        public virtual string[] Options { get; }
        public virtual string Question { get; }

        public abstract IDialogText Clone();
        public abstract IDialogText Clone(IDialogText dt);
    }
}