namespace MemoryAPI
{
    public interface ICraftDetails
    {
        bool Capped { get; set; }
        int Level { get; set; }
        CraftRank Rank { get; set; }
        Craft Skill { get; set; }

        string ToString();
        string ToString(string format);
    }
}