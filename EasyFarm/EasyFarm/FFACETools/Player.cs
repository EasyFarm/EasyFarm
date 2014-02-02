using System;
using System.Collections.Generic;

namespace FFACETools
{
    public partial class FFACE
    {
        /// <summary>
        /// Wrapper class for all player information from FFACE
        /// </summary>
        public class PlayerTools
        {
            #region Classes

            /// <summary>
            /// Details about a specific craft
            /// </summary>
            public class CraftDetails
            {

                /// <summary>
                /// Skill of the craft
                /// </summary>
                public Craft Skill { get; set; }

                /// <summary>
                /// Level of the craft
                /// </summary>
                public int Level { get; set; }

                /// <summary>
                /// Rank of the craft
                /// </summary>
                public CraftRank Rank { get; set; }

                /// <summary>
                /// Whether the craft is capped
                /// </summary>
                public bool Capped { get; set; }

                public CraftDetails (Craft skill, int level, CraftRank rank, bool capped)
                {
                    Skill = skill;
                    Level = level;
                    Rank = rank;
                    Capped = capped;
                }

                /// <summary>
                /// ToString override.
                /// </summary>
                /// <returns>String in the format "Skill Rank (Level) (Capped/Uncapped)"</returns>
                public override string ToString ()
                {
                    string ret = String.Format("{2} {1} ({0}) ({3})",
                    this.Level, this.Rank.ToString("G"),
                    this.Skill.ToString("G"),
                    this.Capped ? "Capped" : "Uncapped");
                    return ret;
                }

                /// <summary>
                /// Private function for determining if a character is a "special" char for formatting
                /// </summary>
                /// <param name="c"></param>
                /// <returns></returns>
                private bool IsSpecial (char c)
                {
                    char x = char.ToLower(c);
                    if (x == 'r' || x == 'c' || x == 's' || x == 'l')
                        return true;
                    return false;
                }

                /// <summary>
                /// ToString overload for passing a custom format. (Extremely rudimentary)
                /// </summary>
                /// <param name="format">String containing any of {S} (Skill), {R} (Rank), {L} (Level), or {C} (Capped/Uncapped)</param>
                /// <returns>String formatted to the provided parameter's requirement.</returns>
                public string ToString (string format)
                {
                    if (String.IsNullOrEmpty(format))
                        return this.ToString();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder(50);
                    for (int i = 0; i < format.Length; i++)
                    {
                        char next = '\0';
                        char next2 = '\0';

                        if (( i + 1 ) < format.Length)
                            next = char.ToLower(format[i + 1]);
                        if (( i + 2 ) < format.Length)
                            next2 = char.ToLower(format[i + 2]);
                        if (( format[i] == '{' ) && this.IsSpecial(next) && ( next2 == '}' ))
                        {
                            if (next == 'r')
                                sb.Append(this.Rank.ToString("G"));
                            else if (next == 'c')
                                sb.Append(this.Capped ? "Capped" : "Uncapped");
                            else if (next == 's')
                                sb.Append(this.Skill.ToString("G"));
                            else if (next == 'l')
                                sb.Append(this.Level);
                            i += 2;
                        }
                        else if (( format[i] == '{' ) && ( next == '{' ))
                        {
                            sb.Append('{');
                            i++;
                        }
                        else if (( format[i] == '}' ) && ( next == '}' ))
                        {
                            sb.Append('}');
                            i++;
                        }
                        else
                            sb.Append(format[i]);
                    }
                    return sb.ToString();
                }

            } // @ public struct CraftDetails

            /// <summary>
            /// Details about a specific skill
            /// </summary>
            public class SkillDetails
            {

                private int _Skill;

                public int Skill
                {
                    get { return _Skill; }
                    set
                    {
                        if (value > 255) { _Skill = value & 0xFF; SkillType = (MagicOrCombat)( value & 0xFF00 ); }
                        else
                            _Skill = value;
                    }
                }

                public MagicOrCombat SkillType { get; set; }

                /// <summary>
                /// Level of the skill
                /// </summary>
                public int Level { get; set; }

                /// <summary>
                /// Whether the skill is capped
                /// </summary>
                public bool Capped { get; set; }

                /// <summary>
                /// ToString override.
                /// </summary>
                /// <returns>String in the format "Skill Level (Capped/Uncapped)"</returns>
                public override string ToString ()
                {
                    string ret = String.Format("{0} {1} ({2})",
                    ( this.SkillType == MagicOrCombat.Combat ) ? ( (CombatSkill)this.Skill ).ToString("G") :
                    ( this.SkillType == MagicOrCombat.Magic ) ? ( (MagicSkill)this.Skill ).ToString("G") : "Unknown-Skill",
                    this.Level,
                    this.Capped ? "Capped" : "Uncapped");
                    return ret;
                }

                /// <summary>
                /// Private function for determining if a character is a "special" char for formatting
                /// </summary>
                /// <param name="c"></param>
                /// <returns></returns>
                private bool IsSpecial (char c)
                {
                    char x = char.ToLower(c);
                    if (x == 'c' || x == 's' || x == 'l')
                        return true;
                    return false;
                }

                /// <summary>
                /// ToString overload for passing a custom format. (Extremely rudimentary)
                /// </summary>
                /// <param name="format">String containing any of {S} (Skill), {L} (Level), or {C} (Capped/Uncapped)</param>
                /// <returns>String formatted to the provided parameter's requirement.</returns>
                public string ToString (string format)
                {
                    if (String.IsNullOrEmpty(format))
                        return this.ToString();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder(50);
                    for (int i = 0; i < format.Length; i++)
                    {
                        char next = '\0';
                        char next2 = '\0';

                        if (( i + 1 ) < format.Length)
                            next = char.ToLower(format[i + 1]);
                        if (( i + 2 ) < format.Length)
                            next2 = char.ToLower(format[i + 2]);
                        if (( format[i] == '{' ) && this.IsSpecial(next) && ( next2 == '}' ))
                        {
                            if (next == 'c')
                                sb.Append(this.Capped ? "Capped" : "Uncapped");
                            else if (next == 's')
                            {
                                if (this.SkillType == MagicOrCombat.Magic)
                                    sb.Append(( (MagicSkill)this.Skill ).ToString("G"));
                                else if (this.SkillType == MagicOrCombat.Combat)
                                    sb.Append(( (CombatSkill)this.Skill ).ToString("G"));
                                else
                                    sb.Append("Unknown-Skill");
                            }
                            else if (next == 'l')
                                sb.Append(this.Level);
                            i += 2;
                        }
                        else if (( format[i] == '{' ) && ( next == '{' ))
                        {
                            sb.Append('{');
                            i++;
                        }
                        else if (( format[i] == '}' ) && ( next == '}' ))
                        {
                            sb.Append('}');
                            i++;
                        }
                        else
                            sb.Append(format[i]);
                    }
                    return sb.ToString();
                }

            } // @ public struct SkillDetails

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instanceID">Instance ID created by FFACE</param>
            public PlayerTools (int instanceID)
            {
                _InstanceID = instanceID;
                _PartyInformation = new PartyMemberTools(instanceID, 0);

            } // @ public Player(int instanceID)

            #endregion

            #region Members

            /// <summary>
            /// Instance ID generate by FFACE
            /// </summary>
            private int _InstanceID { get; set; }

            /// <summary>
            /// PartyMember information for getting things such as name
            /// </summary>
            private PartyMemberTools _PartyInformation { get; set; }

            /// <summary>
            /// Name of the current player
            /// </summary>
            public string Name
            {
                get { return _PartyInformation.Name; }

            } // @ public string Name

            /// <summary>
            /// Player ID as it appears from the server
            /// </summary>
            public int ID
            {
                get { return _PartyInformation.ID; }

            } // @ public short ID

            /// <summary>
            /// ID of the server the player is on. For ServerID of the player check out <see cref="ID"/>
            /// </summary>
            public byte PlayerServerID
            {
                get { return GetPlayerServerID(_InstanceID); }

            } // @ public byte PlayerServerID

            /// <summary>
            /// Current status player is in
            /// </summary>
            public Status Status
            {
                get { return GetPlayerStatus(_InstanceID); }

            } // @ public eStatus Status

            /// <summary>
            /// Players current hit points in percent
            /// </summary>
            public int HPPCurrent
            {
                get { return _PartyInformation.HPPCurrent; }

            } // @ public short HPPCurrent

            /// <summary>
            /// Players current mana in percent
            /// </summary>
            public int MPPCurrent
            {
                get { return _PartyInformation.MPPCurrent; }

            } // @ public short MPPCurrent

            /// <summary>
            /// Players current hit points
            /// </summary>
            public int HPCurrent
            {
                get { return _PartyInformation.HPCurrent; }

            } // @ public int HPCurrent

            /// <summary>
            /// Players current mana
            /// </summary>
            public int MPCurrent
            {
                get { return _PartyInformation.MPCurrent; }

            } // @ public int MPCurrent

            /// <summary>
            /// Players current view mode
            /// </summary>
            public ViewMode ViewMode
            {
                get { return GetViewMode(_InstanceID); }

            } // @ public byte ViewMode

            /// <summary>
            /// UNKNOWN
            /// </summary>
            public float CastMax
            {
                get { return GetCastMax(_InstanceID); }

            } // @ public decimal CastMax

            /// <summary>
            /// A Countdown until the casting bar will reach 100%
            /// Notes: 
            /// - Value seems to be about 0.625 * number of seconds left
            /// - Value will stop higher than 0 if player zones while casting,
            ///   and will not count down further until another spell is cast
            /// </summary>
            public float CastCountDown
            {
                get { return GetCastCountDown(_InstanceID); }

            } // @ public decimal CastCountDown

            /// <summary>
            /// Current count on the casting progress bar, as a percentage between 0 and 1
            /// </summary>
            public float CastPercent
            {
                get { return GetCastPercent(_InstanceID); }

            } // @ public decimal CastPercent

            /// <summary>
            /// Current count on the casting progress bar, between 0 and 100
            /// </summary>
            public short CastPercentEx
            {
                // return GetCastPercentEx either only returned 0 or 100, this is a better way.
                get { return (short)( GetCastPercent(_InstanceID) * 100.0f ); }

            } // @ public decimal CastPercentEx

            /// <summary>
            /// Zone player is in
            /// </summary>
            public Zone Zone
            {
                get { return (Zone)_PartyInformation.Zone; }

            } // @ public eZone Zone

            /// <summary>
            /// Players current TP
            /// </summary>
            public int TPCurrent
            {
                get { return _PartyInformation.TPCurrent; }

            } // @ public short TPCurrent

            /// <summary>
            /// Players X position in relation to the zone
            /// </summary>
            public float PosX
            {
                get { return GetNPCPosX(_InstanceID, ID); }

            } // @ public double PoxX

            /// <summary>
            /// Players Y position in relation to the zone
            /// </summary>
            public float PosY
            {
                get { return GetNPCPosY(_InstanceID, ID); }

            } // @ public double PosY

            /// <summary>
            /// Players Z position in relation to the zone
            /// </summary>
            public float PosZ
            {
                get { return GetNPCPosZ(_InstanceID, ID); }

            } // @ public double PosZ

            /// <summary>
            /// Players Heading Direction
            /// </summary>
            public float PosH
            {
                get { return GetNPCPosH(_InstanceID, ID); }

            } // @ public double PosH

            /// <summary>
            /// Players position and heading
            /// </summary>
            public Position Position
            {
                get
                {
                    return new Position
                    {
                        X = GetNPCPosX(_InstanceID, ID),
                        Y = GetNPCPosY(_InstanceID, ID),
                        Z = GetNPCPosZ(_InstanceID, ID),
                        H = GetNPCPosH(_InstanceID, ID)
                    };
                }
            } // @ public Position Position

            /// <summary>
            /// Players maximum hit points
            /// </summary>
            public int HPMax
            {
                get { return GetPlayerInformation().HPMax; }

            } // @ public ushort MaxHP

            /// <summary>
            /// Players maximum mana points
            /// </summary>
            public int MPMax
            {
                get { return GetPlayerInformation().MPMax; }

            } // @ public int MPMax

            /// <summary>
            /// Players current main job
            /// </summary>
            public Job MainJob
            {
                get { return GetPlayerInformation().MainJob; }

            } // @ public eJob MainJob

            /// <summary>
            /// Players current main job level
            /// </summary>
            public short MainJobLevel
            {
                get { return GetPlayerInformation().MainJobLVL; }

            } // @ public short MainJobLVL

            /// <summary>
            /// Players current sub job
            /// </summary>
            public Job SubJob
            {
                get { return GetPlayerInformation().SubJob; }

            } // @ public eJob SubJob

            /// <summary>
            /// Players current sub job level
            /// </summary>
            public short SubJobLevel
            {
                get { return GetPlayerInformation().SubJobLVL; }

            } // @ public ushort SubJobLVL

            /// <summary>
            /// Players experience into current level
            /// </summary>
            public ushort EXPIntoLevel
            {
                get { return GetPlayerInformation().EXPIntoLVL; }

            } // @ public ushort EXPIntoLVL

            /// <summary>
            /// Total experience needed for current level
            /// </summary>
            public ushort EXPForLevel
            {
                get { return GetPlayerInformation().EXPForLVL; }

            } // @ public ushort EXPForLVL

            /// <summary>
            /// Players current attack power
            /// </summary>
            public short AttackPower
            {
                get { return GetPlayerInformation().Attack; }

            } // @ public short AttackPower

            /// <summary>
            /// Players current defense
            /// </summary>
            public short Defense
            {
                get { return GetPlayerInformation().Defense; }

            } // @ public short Defense

            /// <summary>
            /// Players current rank with their campaign
            /// </summary>
            public short Rank
            {
                get { return GetPlayerInformation().Rank; }

            } // @ public short Rank

            /// <summary>
            /// Points into players current rank
            /// </summary>
            public short RankPoints
            {
                get { return GetPlayerInformation().RankPts; }

            } // @ public short RankPoints

            /// <summary>
            /// Current amount of limit points
            /// </summary>
            public ushort LimitPoints
            {
                get { return GetPlayerInformation().LimitPoints; }

            } // @ public ushort LimitPoints

            /// <summary>
            /// Current limit mode as the byte value from memory.
            /// </summary>
            public byte LimitMode
            {
                get { return GetPlayerInformation().LimitMode; }
            } // @ public byte LimitMode

            /// <summary>
            /// Is 'Limit Points' mode set in Status/Merit Points/Mode Switch
            /// </summary>
            public bool IsMeritMode
            {
                get { return IsSet((uint)LimitMode, 0x80); }
            } // @ public bool IsMeritMode

            /// <summary>
            /// Is 'EXP' mode set in Status/Merit Points/Mode Switch
            /// </summary>
            public bool IsExpMode
            {
                get { return !IsSet((uint)LimitMode, 0x80); }
            } // @ public bool IsExpMode

            /// <summary>
            /// Current amount of merit points
            /// </summary>
            public short MeritPoints
            {
                get { return GetPlayerInformation().MeritPoints; }

            } // @ public short MeritPoints

            /// <summary>
            /// Status effects on the player
            /// </summary>
            public StatusEffect[] StatusEffects
            {
                get { return GetPlayerInformation().Buffs; }

            } // @ public eStatusEffect[] StatusEffects

            /// <summary>
            /// Players current stats
            /// </summary>
            public PlayerStats Stats
            {
                get { return GetPlayerInformation().Stats; }

            } // @ public PlayerStats Stats

            /// <summary>
            /// Players current stat modifiers
            /// </summary>
            public PlayerStats StatModifiers
            {
                get { return GetPlayerInformation().StatModifiers; }

            } // @ public PlayerStats StatModifiers

            /// <summary>
            /// Players current elements
            /// </summary>
            public PlayerElements Elements
            {
                get { return GetPlayerInformation().Elements; }

            } // @ public PlayerElements Elements

            /// <summary>
            /// Players current title ID
            /// 
            /// UNKNOWN (flagged for enum)
            /// NOTE: Enum should be created for title listing (good luck!)
            /// </summary>
            public short Title_ID
            {
                get { return GetPlayerInformation().Title; }

            } // @ public short Title

            /// <summary>
            /// Players current nation
            /// </summary>
            public Nation Nation
            {
                get { return (Nation)GetPlayerInformation().Nation; }

            } // @ public byte Nation

            /// <summary>
            /// Players current residence ID
            /// 
            /// UNKNOWN (flagged for enum)
            /// NOTE: enum should be created for residence listing
            /// </summary>
            public byte Residence_ID
            {
                get { return GetPlayerInformation().Residence; }

            } // @ public byte Residence_ID

            /// <summary>
            /// Player current home point ID
            /// 
            /// UNKNOWN (flagged for enum)
            /// NOTE: enum should be created for home point listing (good luck!)
            /// </summary>
            public int HomePoint_ID
            {
                get { return GetPlayerInformation().HomePoint; }

            } // @ public int HomePoint_ID

            /// <summary>
            /// Get Login Status
            /// </summary>
            public LoginStatus GetLoginStatus
            {
                get { return (LoginStatus)GetLoginStatus(_InstanceID); }
            }

            /// <summary>
            /// Will get information about the trade window
            /// </summary>
            public TRADEINFO GetTradeWindowInformation
            {
                get
                {
                    TRADEINFO information = new TRADEINFO();
                    GetNPCTradeInfo(_InstanceID, ref information);
                    return information;
                }
            } // @ public TRADEINFO GetTradeWindowInformation

            /// <summary>
            /// Weather of the Zone player is in
            /// </summary>
            public Weather Weather
            {
                get { return (Weather)GetWeatherType(_InstanceID); }

            } // @ public Weather Weather

            /// <summary>

            /// <summary>
            /// Returns true if you are in the middle of a synth
            /// </summary>
            public bool Synthing
            {
                get { return IsSynthesis(_InstanceID); }
            }

            #endregion

            #region Methods
            /// <summary>
            /// Returns true if you are in the middle of a synth
            /// </summary>
            public bool IsSynthing ()
            {
                return IsSynthesis(_InstanceID);

            } // @ public bool Synthing()


            /// <summary>
            /// Gets the PLAYERINFO struct from FFACE
            /// </summary>
            private PLAYERINFO GetPlayerInformation ()
            {
                PLAYERINFO playerInfo = new PLAYERINFO();
                GetPlayerInfo(_InstanceID, ref playerInfo);

                return playerInfo;

            } // @ private PLAYERINFO GetPlayerInfo()

            /// <summary>
            /// Will get all craft details as a List<>
            /// </summary>
            /// <returns>List<> of CraftDetails populated with all crafting information.</returns>
            public List<CraftDetails> GetAllCraftDetails ()
            {
                List<CraftDetails> ret = new List<CraftDetails>();
                ushort value;
                foreach (byte val in Enum.GetValues(typeof(Craft)))
                {
                    PlayerCraftLevels craftLevels = GetPlayerInformation().CraftLevels;
                    switch ((Craft)val)
                    {
                        case Craft.Alchemy:
                            value = craftLevels.Alchemy;
                            break;
                        case Craft.Bonecrafting:
                            value = craftLevels.Bonecraft;
                            break;
                        case Craft.Clothcraft:
                            value = craftLevels.Clothcraft;
                            break;
                        case Craft.Cooking:
                            value = craftLevels.Cooking;
                            break;
                        case Craft.Fishing:
                            value = craftLevels.Fishing;
                            break;
                        case Craft.Goldsmithing:
                            value = craftLevels.Goldsmithing;
                            break;
                        case Craft.Leathercraft:
                            value = craftLevels.Leathercraft;
                            break;
                        case Craft.Smithing:
                            value = craftLevels.Smithing;
                            break;
                        case Craft.Woodworking:
                            value = craftLevels.Woodworking;
                            break;
                        case Craft.Synergy:
                            value = craftLevels.Synergy;
                            break;
                        default:
                            continue;
                    }
                    CraftDetails details = new CraftDetails((Craft)val, ( ( value & 0x1FE0 ) >> 5 ), (CraftRank)( value & 0x1F ), Convert.ToBoolean(( ( value & 0x8000 ) >> 15 )));
                    //CraftDetails tmp = GetCraftDetails((Craft)val);
                    ret.Add(details);
                }
                return ret;
            }

            /// <summary>
            /// Will get craft details about the passed craft
            /// </summary>
            /// <param name="craft">Craft to get details about</param>
            public CraftDetails GetCraftDetails (Craft craft)
            {
                // Get craft information from fface
                PlayerCraftLevels craftLevels = GetPlayerInformation().CraftLevels;

                // value of specific craft
                int value = 0;

                // See which craft is selected
                switch (craft)
                {
                    case Craft.Alchemy:
                        value = craftLevels.Alchemy;
                        break;
                    case Craft.Bonecrafting:
                        value = craftLevels.Bonecraft;
                        break;
                    case Craft.Clothcraft:
                        value = craftLevels.Clothcraft;
                        break;
                    case Craft.Cooking:
                        value = craftLevels.Cooking;
                        break;
                    case Craft.Fishing:
                        value = craftLevels.Fishing;
                        break;
                    case Craft.Goldsmithing:
                        value = craftLevels.Goldsmithing;
                        break;
                    case Craft.Leathercraft:
                        value = craftLevels.Leathercraft;
                        break;
                    case Craft.Smithing:
                        value = craftLevels.Smithing;
                        break;
                    case Craft.Woodworking:
                        value = craftLevels.Woodworking;
                        break;
                    case Craft.Synergy:
                        value = craftLevels.Synergy;
                        break;
                    default:
                        throw new ArgumentException("Unknown craft passed to GetCraftDetails()");

                } // @ switch (craft)

                // create return result
                CraftDetails details = new CraftDetails(craft, ( ( value & 0x1FE0 ) >> 5 ), (CraftRank)( value & 0x1F ), Convert.ToBoolean(( ( value & 0x8000 ) >> 15 )));
                //details.Capped = Convert.ToBoolean(((value & 0x8000) >> 15));
                //details.Level = ((value & 0x1FE0) >> 5);
                //details.Rank = (CraftRank)(value & 0x1F);
                //details.Skill = craft;

                return details;

            } // @ public CraftDetails GetCraftDetails(Craft craft)

            /// <summary>
            /// Will get magic skill details about the passed magic skill
            /// </summary>
            /// <param name="skill">Magic skill to get details about</param>
            /// <returns></returns>
            public SkillDetails GetMagicSkillDetails (MagicSkill skill)
            {
                // Get magic skill information from fface
                PlayerMagicSkills magicSkills = GetPlayerInformation().MagicSkills;

                // value of specific magic skill
                int value = 0;

                // see which skill is selected
                switch (skill)
                {
                    case MagicSkill.BlueMagic:
                        value = magicSkills.BlueMagic;
                        break;
                    case MagicSkill.Dark:
                        value = magicSkills.Dark;
                        break;
                    case MagicSkill.Divine:
                        value = magicSkills.Divine;
                        break;
                    case MagicSkill.Elemental:
                        value = magicSkills.Elemental;
                        break;
                    case MagicSkill.Enfeebling:
                        value = magicSkills.Enfeebling;
                        break;
                    case MagicSkill.Enhancing:
                        value = magicSkills.Enhancing;
                        break;
                    case MagicSkill.Healing:
                        value = magicSkills.Healing;
                        break;
                    case MagicSkill.Ninjitsu:
                        value = magicSkills.Ninjitsu;
                        break;
                    case MagicSkill.Singing:
                        value = magicSkills.Singing;
                        break;
                    case MagicSkill.String:
                        value = magicSkills.String;
                        break;
                    case MagicSkill.Summoning:
                        value = magicSkills.Summon;
                        break;
                    case MagicSkill.Wind:
                        value = magicSkills.Wind;
                        break;
                    default:
                        throw new ArgumentException("Unknown magic skill passed to GetMagicSkillDetails()");

                } // @ switch (skill)

                SkillDetails details = new SkillDetails();
                details.Level = ( value & 0xFFF );
                details.Capped = Convert.ToBoolean(( ( value & 0x8000 ) >> 15 ));
                details.SkillType = MagicOrCombat.Magic;
                details.Skill = (int)skill;
                return details;

            } // @ public SkillDetails GetMagicSkillDetail(MagicSkill skill)

            /// <summary>
            /// Will get combat skill details about the passed combat skill
            /// </summary>
            /// <param name="skill">Combat skill to get details about</param>
            /// <returns></returns>
            public SkillDetails GetCombatSkillDetails (CombatSkill skill)
            {
                // Get combat skill information from fface
                PlayerCombatSkills combatSkills = GetPlayerInformation().CombatSkills;

                // value of specific combat skill
                int value = 0;

                // see which skill is selected
                switch (skill)
                {
                    case CombatSkill.Archery:
                        value = combatSkills.Archery;
                        break;
                    case CombatSkill.Axe:
                        value = combatSkills.Axe;
                        break;
                    case CombatSkill.Club:
                        value = combatSkills.Club;
                        break;
                    case CombatSkill.Dagger:
                        value = combatSkills.Dagger;
                        break;
                    case CombatSkill.Evasion:
                        value = combatSkills.Evasion;
                        break;
                    case CombatSkill.GreatAxe:
                        value = combatSkills.GreatAxe;
                        break;
                    case CombatSkill.GreatKatana:
                        value = combatSkills.GreatKatana;
                        break;
                    case CombatSkill.GreatSword:
                        value = combatSkills.GreatSword;
                        break;
                    case CombatSkill.Guarding:
                        value = combatSkills.Guarding;
                        break;
                    case CombatSkill.HandToHand:
                        value = combatSkills.HandToHand;
                        break;
                    case CombatSkill.Katana:
                        value = combatSkills.Katana;
                        break;
                    case CombatSkill.Marksmanship:
                        value = combatSkills.Marksmanship;
                        break;
                    case CombatSkill.Parrying:
                        value = combatSkills.Parrying;
                        break;
                    case CombatSkill.Polearm:
                        value = combatSkills.Polearm;
                        break;
                    case CombatSkill.Scythe:
                        value = combatSkills.Scythe;
                        break;
                    case CombatSkill.Shield:
                        value = combatSkills.Shield;
                        break;
                    case CombatSkill.Staff:
                        value = combatSkills.Staff;
                        break;
                    case CombatSkill.Sword:
                        value = combatSkills.Sword;
                        break;
                    case CombatSkill.Throwing:
                        value = combatSkills.Throwing;
                        break;
                    default:
                        throw new ArgumentException("Unknown combat skill passed to GetCombatSkillDetails()");

                } // @ switch (skill)

                SkillDetails details = new SkillDetails();
                details.Level = ( value & 0xFFF );
                details.Capped = Convert.ToBoolean(( ( value & 0x8000 ) >> 15 ));
                details.SkillType = MagicOrCombat.Combat;
                details.Skill = (int)skill;

                return details;

            } // @ public SkillDetails GetCombatSkillDetail(CombatSkill skill)

            public bool HasKeyitem (KeyItem item)
            {
                return HasKeyItem(_InstanceID, (uint)item);
            }

            ///<summary>
            ///Spells you have learned.
            ///</summary>
            public bool KnowsSpell (SpellList spell)
            {
                return HasSpell(_InstanceID, (uint)( (short)spell / 2 ));
            }

            /// <summary>
            /// Checks that you have access to a given ability
            /// </summary>
            /// <param name="ability"></param>
            /// <returns></returns>
            public bool HasAbility (AbilityList ability)
            {
                return GetAbilityAvailable(_InstanceID, (uint)ability);
            }

            /// <summary>
            /// Checks that you have a given job trait
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public bool HasTrait (uint ID)
            {
                return GetTraitAvailable(_InstanceID, ID);
            }

            /// <summary>
            /// Check that you have a given pet command
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public bool HasPetCommand (uint ID)
            {
                return GetPetCommandAvailable(_InstanceID, ID);
            }

            /// <summary>
            /// Check that you has a given weaponskill
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public bool HasWeaponSkill (uint ID)
            {
                return GetWeaponSkillAvailable(_InstanceID, ID);
            }
            #endregion

        } // @ public class PlayerTools
    }
}
