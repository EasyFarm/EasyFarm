namespace MemoryAPI
{
    public abstract class AbstractSkillDetails
    {
        public virtual bool Capped { get; set; }
        public virtual int Level { get; set; }
        public virtual int Skill { get; set; }
        public virtual MagicOrCombat SkillType { get; set; }

        public abstract string ToString(string format);
    }
}