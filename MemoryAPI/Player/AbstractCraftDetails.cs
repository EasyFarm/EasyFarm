namespace MemoryAPI
{
    public abstract class AbstractCraftDetails : ICraftDetails
    {
        public virtual bool Capped { get; set; }
        public virtual int Level { get; set; }
        public virtual CraftRank Rank { get; set; }
        public virtual Craft Skill { get; set; }

        public abstract string ToString(string format);
    }
}