namespace MemoryAPI
{
    public abstract class AbstractTextObject : ITextObject
    {
        public virtual float BGBorderSize { get; set; }
        public virtual bool BGVisible { get; set; }
        public virtual bool FontVisible { get; set; }
        public virtual bool IncludeBackGround { get; set; }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }

        public abstract void DeleteTextObject();
        public abstract void RenderTextObject();
        public abstract void SetBGColor(byte transparent, byte red, byte green, byte blue);
        public abstract void SetFont(string type, short height);
        public abstract void SetFontColor(byte transparent, byte red, byte green, byte blue);
        public abstract void SetFontColor(byte transparent, byte red, byte green, byte blue, bool visible, bool bold, bool italic, bool rightJustified);
        public abstract void SetLocation(float horizontal, float vertical);
        public abstract void UpdateTextObject();
        public abstract void UpdateTextObject(string sText);
    }
}
