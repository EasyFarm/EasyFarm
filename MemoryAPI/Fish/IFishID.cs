namespace MemoryAPI
{
    public interface IFishID
    {
        int ID1 { get; set; }
        int ID2 { get; set; }
        int ID3 { get; set; }
        int ID4 { get; set; }

        bool Equals(object obj);
        int GetHashCode();
    }
}