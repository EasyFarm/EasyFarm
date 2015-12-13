namespace MemoryAPI
{
    public interface ITextObject
    {
        float BGBorderSize { get; set; }
        bool BGVisible { get; set; }
        bool FontVisible { get; set; }
        bool IncludeBackGround { get; set; }
        string Name { get; set; }
        string Text { get; set; }

        void DeleteTextObject();
        void RenderTextObject();
        void SetBGColor(byte transparent, byte red, byte green, byte blue);
        void SetFont(string type, short height);
        void SetFontColor(byte transparent, byte red, byte green, byte blue);
        void SetFontColor(byte transparent, byte red, byte green, byte blue, bool visible, bool bold, bool italic, bool rightJustified);
        void SetLocation(float horizontal, float vertical);
        void UpdateTextObject();
        void UpdateTextObject(string sText);
    }
}