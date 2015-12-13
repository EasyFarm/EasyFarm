namespace MemoryAPI
{
    public class AbstractPosition : IPosition
    {
        public virtual float H { get; set; }
        public virtual float X { get; set; }
        public virtual float Y { get; set; }
        public virtual float Z { get; set; }
    }
}