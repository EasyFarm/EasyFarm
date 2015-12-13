namespace MemoryAPI
{
    public interface ISkillDetails
    {
        bool Capped { get; set; }
        int Level { get; set; }
        int Skill { get; set; }
        MagicOrCombat SkillType { get; set; }

        string ToString();
        string ToString(string format);
    }
}