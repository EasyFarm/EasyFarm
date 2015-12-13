namespace MemoryAPI
{
    public interface IPlayerTools
    {
        short AttackPower { get; }
        float CastCountDown { get; }
        float CastMax { get; }
        float CastPercent { get; }
        short CastPercentEx { get; }
        short Defense { get; }
        Structures.PlayerElements Elements { get; }
        ushort EXPForLevel { get; }
        ushort EXPIntoLevel { get; }
        LoginStatus GetLoginStatus { get; }
        Structures.TRADEINFO GetTradeWindowInformation { get; }
        int HomePoint_ID { get; }
        int HPCurrent { get; }
        int HPMax { get; }
        int HPPCurrent { get; }
        int ID { get; }
        bool IsExpMode { get; }
        bool IsMeritMode { get; }
        byte LimitMode { get; }
        ushort LimitPoints { get; }
        Job MainJob { get; }
        short MainJobLevel { get; }
        short MeritPoints { get; }
        int MPCurrent { get; }
        int MPMax { get; }
        int MPPCurrent { get; }
        string Name { get; }
        Nation Nation { get; }
        byte PlayerServerID { get; }
        float PosH { get; }
        IPosition Position { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        short Rank { get; }
        short RankPoints { get; }
        byte Residence_ID { get; }
        Structures.PlayerStats StatModifiers { get; }
        Structures.PlayerStats Stats { get; }
        Status Status { get; }
        StatusEffect[] StatusEffects { get; }
        Job SubJob { get; }
        short SubJobLevel { get; }
        bool Synthing { get; }
        short Title_ID { get; }
        int TPCurrent { get; }
        ViewMode ViewMode { get; }
        Weather Weather { get; }
        Zone Zone { get; }

        System.Collections.Generic.List<ICraftDetails> GetAllCraftDetails();
        ISkillDetails GetCombatSkillDetails(CombatSkill skill);
        ICraftDetails GetCraftDetails(Craft craft);
        ISkillDetails GetMagicSkillDetails(MagicSkill skill);
        bool HasAbility(AbilityList ability);
        bool HasKeyitem(KeyItem item);
        bool HasPetCommand(uint ID);
        bool HasTrait(uint ID);
        bool HasWeaponSkill(uint ID);
        bool IsSynthing();
        bool KnowsSpell(SpellList spell);
    }
}