namespace MemoryAPI
{
    public abstract class AbstractPlayerTools : IPlayerTools
    {
        public virtual short AttackPower { get; }
        public virtual float CastCountDown { get; }
        public virtual float CastMax { get; }
        public virtual float CastPercent { get; }
        public virtual short CastPercentEx { get; }
        public virtual short Defense { get; }
        public virtual Structures.PlayerElements Elements { get; }
        public virtual ushort EXPForLevel { get; }
        public virtual ushort EXPIntoLevel { get; }
        public virtual LoginStatus GetLoginStatus { get; }
        public virtual Structures.TRADEINFO GetTradeWindowInformation { get; }
        public virtual int HomePoint_ID { get; }
        public virtual int HPCurrent { get; }
        public virtual int HPMax { get; }
        public virtual int HPPCurrent { get; }
        public virtual int ID { get; }
        public virtual bool IsExpMode { get; }
        public virtual bool IsMeritMode { get; }
        public virtual byte LimitMode { get; }
        public virtual ushort LimitPoints { get; }
        public virtual Job MainJob { get; }
        public virtual short MainJobLevel { get; }
        public virtual short MeritPoints { get; }
        public virtual int MPCurrent { get; }
        public virtual int MPMax { get; }
        public virtual int MPPCurrent { get; }
        public virtual string Name { get; }
        public virtual Nation Nation { get; }
        public virtual byte PlayerServerID { get; }
        public virtual float PosH { get; }
        public virtual IPosition Position { get; }
        public virtual float PosX { get; }
        public virtual float PosY { get; }
        public virtual float PosZ { get; }
        public virtual short Rank { get; }
        public virtual short RankPoints { get; }
        public virtual byte Residence_ID { get; }
        public virtual Structures.PlayerStats StatModifiers { get; }
        public virtual Structures.PlayerStats Stats { get; }
        public virtual Status Status { get; }
        public virtual StatusEffect[] StatusEffects { get; }
        public virtual Job SubJob { get; }
        public virtual short SubJobLevel { get; }
        public virtual bool Synthing { get; }
        public virtual short Title_ID { get; }
        public virtual int TPCurrent { get; }
        public virtual ViewMode ViewMode { get; }
        public virtual Weather Weather { get; }
        public virtual Zone Zone { get; }

        public abstract System.Collections.Generic.List<ICraftDetails> GetAllCraftDetails();
        public abstract ISkillDetails GetCombatSkillDetails(CombatSkill skill);
        public abstract ICraftDetails GetCraftDetails(Craft craft);
        public abstract ISkillDetails GetMagicSkillDetails(MagicSkill skill);
        public abstract bool HasAbility(AbilityList ability);
        public abstract bool HasKeyitem(KeyItem item);
        public abstract bool HasPetCommand(uint ID);
        public abstract bool HasTrait(uint ID);
        public abstract bool HasWeaponSkill(uint ID);
        public abstract bool IsSynthing();
        public abstract bool KnowsSpell(SpellList spell);
    }
}