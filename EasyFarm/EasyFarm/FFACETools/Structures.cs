using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FFACETools
{
    public partial class FFACE
    {
        /*
         * Structures passed to/from FFACE
         */
        #region PC/NPC

        #region Player Information

        /// <summary>
        /// Stats of the player
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PlayerStats
        {
            public short Str;
            public short Dex;
            public short Vit;
            public short Agi;
            public short Int;
            public short Mnd;
            public short Chr;

        } // @ public struct PlayerStats

        /// <summary>
        /// Elemental resistances of the player
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PlayerElements
        {
            public ushort Fire;
            public ushort Ice;
            public ushort Wind;
            public ushort Earth;
            public ushort Lightning;
            public ushort Water;
            public ushort Light;
            public ushort Dark;

        } // @ public struct PlayerElements

        /// <summary>
        /// Players Combat Skills
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct PlayerCombatSkills
        {
            public ushort HandToHand;
            public ushort Dagger;
            public ushort Sword;
            public ushort GreatSword;
            public ushort Axe;
            public ushort GreatAxe;
            public ushort Scythe;
            public ushort Polearm;
            public ushort Katana;
            public ushort GreatKatana;
            public ushort Club;
            public ushort Staff;
            private ushort unkweap0;
            private ushort unkweap1;
            private ushort unkweap2;
            private ushort unkweap3;
            private ushort unkweap4;
            private ushort unkweap5;
            private ushort unkweap6;
            private ushort unkweap7;
            private ushort unkweap8;
            private ushort unkweap9;
            private ushort unkweap10;
            private ushort unkweap11;
            public ushort Archery;
            public ushort Marksmanship;
            public ushort Throwing;
            public ushort Guarding;
            public ushort Evasion;
            public ushort Shield;
            public ushort Parrying;

        } // @ public struct PlayerCombatSkills

        /// <summary>
        /// Players magic skills
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct PlayerMagicSkills
        {
            public ushort Divine;
            public ushort Healing;
            public ushort Enhancing;
            public ushort Enfeebling;
            public ushort Elemental;
            public ushort Dark;
            public ushort Summon;
            public ushort Ninjitsu;
            public ushort Singing;
            public ushort String;
            public ushort Wind;
            public ushort BlueMagic;
            private ushort unkmagic0;
            private ushort unkmagic1;
            private ushort unkmagic2;
            private ushort unkmagic3;

        } // @ public struct PlayerMagicSkills

        /// <summary>
        /// Players craft skills
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct PlayerCraftLevels
        {
            public ushort Fishing;
            public ushort Woodworking;
            public ushort Smithing;
            public ushort Goldsmithing;
            public ushort Clothcraft;
            public ushort Leathercraft;
            public ushort Bonecraft;
            public ushort Alchemy;
            public ushort Cooking;
            public ushort Synergy;

        } // @ public struct PlayerCraftLevels

        /// <summary>
        /// Structure passed back from FFACE.GetPlayerInfo()
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct PLAYERINFO
        {
            public int HPMax;
            public int MPMax;
            public Job MainJob;
            public byte MainJobLVL;
            public Job SubJob;
            public byte SubJobLVL;
            public ushort EXPIntoLVL;
            public ushort EXPForLVL;
            public PlayerStats Stats;
            public PlayerStats StatModifiers;
            public short Attack;
            public short Defense;
            public PlayerElements Elements;
            public short Title;
            public short Rank;
            public short RankPts;
            public byte Nation;
            public byte Residence;
            public int HomePoint;
            public PlayerCombatSkills CombatSkills;
            public PlayerMagicSkills MagicSkills;
            public PlayerCraftLevels CraftLevels;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
            private byte[] null0;
            public ushort LimitPoints;
            public byte MeritPoints;
            public byte LimitMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 78)]
            private byte[] null1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public StatusEffect[] Buffs;

        } // @ private struct PLAYERINFO

        #endregion

        /// <summary>
        /// FFACE structure for a party member
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct PARTYMEMBER
        {
            public int pad0;
            public byte Index;
            public byte MemberNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Name;
            public int SvrID;
            public int ID;
            private int unknown0;
            public int CurrentHP;
            public int CurrentMP;
            public int CurrentTP;
            public byte CurrentHPP;
            public byte CurrentMPP;
            public short Zone;
            private int pad1;
            public uint FlagMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            private byte[] pad2;
            public int SvrIDDupe;
            public byte CurrentHPPDupe;
            public byte CurrentMPPDupe;
            public byte Active;
            private byte pad3;

        } // @ private struct PARTYMEMBER

        /// <summary>
        /// FFACE Structure for target information
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct TARGETINFO
        {
            public int CurrentID;
            public int SubID;
            public int CurrentSvrID;
            public int SubSrvID;
            public ushort CurrentMask;
            public ushort SubMask;
            public byte IsLocked;
            public byte IsSub;
            public byte HPP;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
            public string Name;

        } // @ public struct TARGETINFO

        /// <summary>
        /// FFACE structure for alliance information
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct ALLIANCEINFO
        {
            public int AllianceLeaderID;
            public int Party0LeaderID;
            public int Party1LeaderID;
            public int Party2LeaderID;
            public byte Party0Visible;
            public byte Party1Visible;
            public byte Party2Visible;
            public byte Party0Count;
            public byte Party1Count;
            public byte Party2Count;
            public byte Invited;
            private byte unknown;

        } // @ private struct ALLIANCEINFO

        #endregion

        #region Items

        /// <summary>
        /// FFACE structure for an inventory item
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct INVENTORYITEM
        {
            public ushort ID;
            public byte Index;
            public uint Count;
            public uint Flag;
            public uint Price;
            public ushort Extra;

        } // @ public struct INVENTORYITEM

        /// <summary>
        /// Structure to hold information about an item in the treasure pool
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct TREASUREITEM
        {
            public byte Flag; //3=no item, 2=item	
            public short ItemID;
            public byte Count;
            public TreasureStatus Status;
            public short MyLot;
            public short WinLot;
            public int WinPlayerSrvID;
            public int WinPlayerID;
            public int TimeStamp; //utc timestamp

        } // @ private struct TREASUREITEM

        /// <summary>
        /// Structure to hold information about a specific item in the trade window
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TRADEITEM
        {
            public ushort ItemID;
            public byte Index;
            public byte Count;

        } // @ private struct TRADEITEM

        /// <summary>
        /// Structure containing information about the trade window
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TRADEINFO
        {
            public uint Gil;
            public int TargetID;
            public byte SelectedBox;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public TRADEITEM[] items;

        } // @ private struct TRADEINFO

        ///<summary>
        ///FFACETools Structure for setTradeItems
        ///</summary>
        public struct NPCTRADEINFO
        {
            public UInt32 Gil;
            public TRADEITEM[] items;
        }


        #endregion

        #region Chat

        /// <summary>
        /// Chat extra info structure
        /// </summary>
        //private struct CHATEXTRAINFO
        //{
        //	public ChatMode MessageType;

        //} // @ private struct CHATEXTRAINFO

        #endregion

        #region Menu
        public class DialogText
        {

            private string _Question;
            private string[] _Options;
            private string _RawText;

            private string[] CleanLine (string[] lines)
            {
                return CleanLine(lines, LineSettings.DialogDefault);
            }

            private string[] CleanLine (string[] lines, LineSettings lineSettings)
            {
                System.Collections.ArrayList ret = new System.Collections.ArrayList();

                for (int i = 0; i < lines.Length; i++)
                {
                    //	CleanLine(lines[i]);
                    string x = FFACE.ChatTools.CleanLine(lines[i], lineSettings);
                    if (x == ".")
                        continue;
                    ret.Add(x);
                }
                string[] returnList = (string[])ret.ToArray(typeof(string));
                return returnList;
            }

            #region Old_Code
            /*
			/// <summary>
			/// Will strip abnormal characters (colors, etc) from the string
			/// </summary>
			/// <param name="line">line to clean (left intact)</param>
			/// <returns>string containing the cleaned line</returns>
			private string CleanLine(string line)
			{
				string cleanedString = line;
				byte[] bytearray1252 = System.Text.Encoding.GetEncoding(1252).GetBytes(cleanedString);
				int i = 0, len = bytearray1252.Length;
				string sEF = "\xFF\x1F\x20\x21\x22\x23\x24\x25\x26\x27\x28\xFF";
				// 1f 20 21 22 23 24 25 26 27 28
				string rep = "<FIAETWLD{}>";
				string s1E = "\x01\x02\x03\xFC\xFD";
				string s1F = "\x0E\x0F\x2F\x7F\x79\x7B\x7C\x8D\x88\x8A\xA1\xD0\r\n\x07";
				string sExtra = "\r\n\x07\x7F\x81\x87";
				System.Collections.Generic.List<Byte> cleaned = new System.Collections.Generic.List<byte>();
				int ndx = -1;

				for (int c = 0; c < len; ++c)
				{

					if ((bytearray1252[c] == '\xEF') && (((c + 1) < len) && ((ndx = sEF.IndexOf((char)bytearray1252[c + 1])) >= 0)))
					{
						// 3C <  3E >
						// 7B {  7D }
						if (sEF[ndx] != '\x28') // Not closing brace? Needs starter char
							cleaned.Add((byte)rep[0]);
						cleaned.Add((byte)rep[ndx]); // add rep.char based on Index
						if (sEF[ndx] != '\x27') // Not opening brace? Needs closer char
							cleaned.Add((byte)rep[rep.Length - 1]); // >  Final: <{ and }> for Auto-translate braces
						++c;
					}
					else if ((bytearray1252[c] == '\x1F') && (((c + 1) < len) && s1F.IndexOf((char)bytearray1252[c + 1]) >= 0))
					{
						++c;
					}
					else if ((bytearray1252[c] == '\x1E') && (((c + 1) < len) && s1E.IndexOf((char)bytearray1252[c + 1]) >= 0))
					{
						++c;
					}
					else
					{
						i = sExtra.IndexOf((char)bytearray1252[c]);
						if (i >= 3) // \r\n\07 are singles, others are doubles
						{
							if (((bytearray1252[c] == '\x7F') && (((c + 1) < len) && bytearray1252[c + 1] == '\x31')) ||
								((bytearray1252[c] == '\x81') && (((c + 1) < len) && bytearray1252[c + 1] == '\xA1')) ||
								((bytearray1252[c] == '\x87') && (((c + 1) < len) && bytearray1252[c + 1] == '\xB2')) ||
								((bytearray1252[c] == '\x87') && (((c + 1) < len) && bytearray1252[c + 1] == '\xB3')))
							{
								++c;
							}
							else
							{
								i = -1; // not a target, so "wasn't found"
							}
						}
						if (i < 0)
						{
							cleaned.Add(bytearray1252[c]);
						}
					}
				}
				cleaned.Add(0);
				if (cleaned[0] != 0)
					cleanedString = System.Text.Encoding.GetEncoding(932).GetString(cleaned.ToArray());
				else
					cleanedString = String.Empty;
				if (cleanedString.StartsWith("["))  // Detect and remove Windower Timestamp plugin text.
				{
					string text = cleanedString.Substring(1, 8);
					string re1 = ".*?";	// Non-greedy match on filler
					string re2 = "((?:(?:[0-1][0-9])|(?:[2][0-3])|(?:[0-9])):(?:[0-5][0-9])(?::[0-5][0-9])?(?:\\s?(?:am|AM|pm|PM))?)";

					Regex r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
					Match m = r.Match(text);
					if (m.Success)
					{
						cleanedString = cleanedString.Remove(0, 11); // this assumes timestamp found is only 10+1 space in length
						// Better way? : line = line.Remove(0,m.Length+1);
					}
				} // Detect and remove Windower Timestamp plugin text.

				return cleanedString;

			} // private CleanLine(string line)
			*/
            #endregion

            public DialogText Clone (DialogText dt)
            {
                if (dt == null)
                    return null;
                DialogText ret = new DialogText(String.Empty);
                ret._Question = dt._Question;
                System.Collections.Generic.List<String> tmp = new System.Collections.Generic.List<string>();
                tmp.AddRange(dt._Options);
                ret._Options = new string[dt._Options.Length];
                for (int i = 0; i < dt._Options.Length; i++)
                {
                    ret._Options[i] = dt._Options[i];
                }
                return ret;
            }

            public DialogText Clone ()
            {
                return Clone(this);
            }

            public DialogText (string RawText) : this(RawText, LineSettings.DialogDefault) { }

            public DialogText (string RawText, LineSettings lineSettings)
            {
                _RawText = RawText;
                if (RawText == String.Empty)
                {
                    _Question = String.Empty;
                    _Options = new string[0];
                }
                else
                {
                    _Question = FFACE.ChatTools.CleanLine(RawText.Substring(0, RawText.IndexOf("\v") - 1), lineSettings);
                    string buffer = RawText.Substring(RawText.IndexOf("\v") + 1);
                    _Options = CleanLine(buffer.Split(new string[] { "\a" }, StringSplitOptions.RemoveEmptyEntries), lineSettings);
                }
            }
            public string Question
            {
                get { return _Question; }
            }
            public string[] Options
            {
                get { return _Options; }
            }
        }
        #endregion

    } // @ public partial class FFACE
}
