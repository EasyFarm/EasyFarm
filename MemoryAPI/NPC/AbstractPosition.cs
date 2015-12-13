namespace MemoryAPI
{
    public interface IPosition
    {
        float H { get; set; }
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }

        bool Equals(object obj);
        int GetHashCode();
    }
}