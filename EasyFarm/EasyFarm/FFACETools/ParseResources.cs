using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32; // registry keys
using System.IO;

namespace FFACETools
{
    public partial class FFACE
    {
        /// <summary>
        /// This class will parse the resources.xml file to
        /// translate ID <-> Names of spells/abilities/items
        /// </summary>
        public class ParseResources
        {
            #region DELETE THIS...
            internal static bool DeleteInstance ()
            {
                if (_instance != null)
                {
                    lock (stateLock)
                    {
                        if (_instance != null)
                        {
                            _instance.ResourcesCache.Clear();
                            _instance = null;
                        }
                    }
                }
                return true;
            }
            #endregion

            #region Enums

            /// <summary>
            /// Static class of Languages for use with FFXIPhraseLoader.
            /// </summary>
            public enum Languages : int
            {
                First = 1,
                Last = 4,
                Japanese = 1,
                English = 2,
                Deutsch = 3,
                French = 4,
            }

            /// <summary>
            /// Private static class of ints containing information for use with _fileNumberArray.
            /// Because enums suck.
            /// </summary>
            private enum FileTypes
            {
                Items = 0,
                Usable_Objects = 1,
                Weapons = 2,
                Armor = 3,
                Puppet_Items = 4,
                Gil = 5,
                Slips_And_MMM = 6,
                Zones = 7,
                StatusEffects = 8,
                JobNames_Short = 9,
                JobNames_Long = 10,
                SpellNames = 11,
                AbilityNames = 12,
                WeatherTypes = 13,
                Weekdays = 14,
                MoonPhases = 15,
                Nations = 16,
                RaceNames = 17,
            }

            [Flags]
            private enum ItemFlags
            {
                None = 0x0000,
                Rare = 0x8000, // Rare 
                Inscribable = 0x0020, // Can be synthed using guild crystals (i.e. can be inscribed)
                NoAuction = 0x0040, // Cannot be sold at the AH
                Scroll = 0x0080, // Is a scroll
                Linkshell = 0x0100, // Is a linkshell
                CanUse = 0x0200, // Can be used
                CanTrade = 0x0400, // Can be traded to an NPC
                CanEquip = 0x0800, // Can be equipped
                NoSale = 0x1000, // Cannot be sold to NPC
                NoDelivery = 0x2000, // Cannot be sent to mog house
                NoTrade = 0x4000,  // Cannot be traded to a player (includes bazaar)
                Ex = ( NoAuction | NoDelivery | NoTrade ), //0x6040, // NoAuction, NoDelivery, NoTrade
                Nothing = ( Linkshell | NoSale | Ex | Rare )
            }

            /// <summary>
            /// Enum for keeping multiple types of information in a single Dictionary (unique keys required)
            /// </summary>
            [Flags]
            private enum ResourceBit : int
            {
                Item = 0,
                Area = 0x010000,
                Status = 0x010200,	// Allow for over 256 areas
                Spell = 0x020000,	// 
                Abils = 0x040000,	// allow for 65536 Abilities
                JobShort = 0x080000,
                JobLong = 0x080100,  // Special case.
                Weather = 0x080200,  // Weather, Jobs, Days, MoonPhases, Nation/Regions, and RaceNames are bytes
                Day = 0x080400,
                Moon = 0x080800,
                Nation = 0x081000,
                RaceNames = 0x082000,
            };

            #endregion

            #region Constants
            private const string RESOURCES_FILE_NAME = "resources.xml";

            private const string RESOURCES_ITEMS_GENERAL_FILE_NAME = "items_general.xml";
            private const string RESOURCES_ITEM_ARMOR_FILE_NAME = "items_armor.xml";
            private const string RESOURCES_ITEM_WEAPONS_FILE_NAME = "items_weapons.xml";
            private const string RESOURCES_AREAS_FILE_NAME = "areas.xml";
            private const string RESOURCES_STATUS_FILE_NAME = "status.xml";
            private const string RESOURCES_SPELLS_FILE_NAME = "spells.xml";
            private const string RESOURCES_ABILS_FILE_NAME = "abils.xml";
            private const string FILENOTFOUND_MSG = "Ensure WindowerPath is set to an absolute path to Windower plugins folder and that the file stated is present.";
            private const string WINDOWERPATH_MSG = "WindowerPath has not been set!";
            #endregion

            #region Private Properties (GET OFF MY LAWN! lulz -- Yekyaa)
            internal static ParseResources Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (stateLock)
                        {
                            if (_instance == null)
                            {
                                _instance = new ParseResources();
                            }
                        }
                    }
                    return _instance;
                }
            }
            #endregion

            #region Members

            #region Private Members
            private static volatile ParseResources _instance;
            private readonly static object stateLock = new Object();


            /// <summary>
            /// Just a list of valid languages in their text format.
            /// </summary>
            private static readonly String[] LanguageStrings = { "All", "Japanese", "English", "German", "French", "Load All" };
            private static readonly String[] LanguageStringsShort = { "all", "jp", "en", "de", "fr", "load all" };

            /// <summary>
            /// Stores the directory location for every file in _fileNumberArray.
            /// </summary>
            private Dictionary<int, string> FileList = new Dictionary<int, string>(40);

            /// <summary>
            /// Current array of exact file numbers that contain the information for each file type.
            /// </summary>
            private int[] _fileNumberArray = { -1,                               // Nil, ignore
				// To access, do (NUM_LANG_MAX * file_type) + Pref.Language
				// General Items                                            = 0
				4,	73,	55815,  56235,
				// Usable Objects                                           = 1
				5,	74,	55816,	56236,
				// Weapons                                                  = 2
				6,	75,	55817,	56237,
				// Armor                                                    = 3
				7,	76,	55818,	56238,
				// Puppet                                                   = 4
				8,	77,	55819,	56239,
				// Gil                                                      = 5
				9,	91,	55820,	56240,
				// Storage Slips and other stuff? (JP, EN, DE, FR)			= 6
				93, 94, 55787, 56207,
				// AREA Lists (JP, EN, DE, FR)                              = 7
				55535, 55465, 55775, 56195,
				// STATUS Lists (JP, EN, DE, FR)							= 8
				55605, 55725, 55852, 56272,
				// JOB Lists SHORT NAME										= 9
				55468, 55468, 55468, 55468,
				// JOB Lists in their native language, JP, EN, DE, FR order = 10
				55536, 55467, 55776, 56196,
				// SPELL Info (JP, EN, DE, FR)                              = 11
				55582, 55702, 55822, 56242,
				// ABILITY Info (JP, EN, DE, FR)                            = 12
				55581, 55701, 55821, 56241, // Use String 2 for French(?)
				// Weather Types (JP, EN, DE, FR)							= 13
				55537, 55657, 55777, 56197,
				// Day Names												= 14
				55538, 55658, 55778, 56198,
				// Moon Phases												= 15
				55540, 55660, 55780, 56200,
				// Region Names
				55534, 55654, 55774, 56194,
				// Race Names	 JP uses EN I guess... It's seriously NOT in the DATs
				55469, 55469, 55783, 56203,
			};

            // 55472 are Einherjar Chamber Names

            #endregion

            #region Public Members
            public SortedDictionary<int, string> ResourcesCache = new SortedDictionary<int, string>();

            /// <summary>
            /// Property for indicating preferred language for data to be retreived in.
            /// </summary>
            public static Languages LanguagePreference = Languages.English;

            /// <summary>
            /// Property for indicating whether to parse xml files or FFXI DAT Files
            /// </summary>
            public static bool UseFFXIDatFiles = true;

            #endregion

            #endregion

            #region Classes

            #region d_msg specific classes (HeaderFormat, EntryFormat, FileFormat)
            private class d_msgHeaderFormat : Object
            {
                #region d_msgHeaderFormat Variables
                private String _marker;
                private UInt16 _unknown_1; // (always 1)
                private UInt16 _unknown_2; // either 1 or 0
                private UInt32 _unknown_3; // should always be 3
                private UInt32 _unknown_4; // should always be 3

                // Version 1 (Flipped Bits)    Version 2 = Unflipped Bits
                // Common Variables
                private UInt32 _file_size;
                private UInt32 _header_size; // always 64
                private UInt32 _toc_size;           // Version 1 only
                private UInt32 _entry_size;         // Version 2 only
                // Common Variables again
                private UInt32 _data_size; // Version 1 (DataSize = FileSize - TocSize - Header Size) & 2 (Data Size = Entry Count * Entry Size) 
                private UInt32 _entry_count; // Version 1 (ToC Entry Count = ToC Size / 8) & 2 (actual Entry Counts)
                private UInt32 _unknown_6; // Unknown (always 1)
                private UInt64 _unknown_7; // Unknown (always 0)
                private UInt64 _unknown_8; // Unknown (always 0)
                #endregion

                #region d_msgHeaderFormat Properties
                public String Marker
                {
                    get { return _marker; }
                    set { _marker = value; }
                }
                public Boolean AreBitsFlipped
                {
                    get { if (_unknown_2 == 1) return true; else return false; }
                }
                public UInt16 Unknown_1
                {
                    get { return _unknown_1; }
                    set { _unknown_1 = value; }
                }
                public UInt16 Unknown_2
                {
                    get { return _unknown_2; }
                    set { _unknown_2 = value; }
                }
                public UInt32 Unknown_3
                {
                    get { return _unknown_3; }
                    set { _unknown_3 = value; }
                }
                public UInt32 Unknown_4
                {
                    get { return _unknown_4; }
                    set { _unknown_4 = value; }
                }

                public UInt32 Unknown_6
                {
                    get { return _unknown_6; }
                    set { _unknown_6 = value; }
                }
                public UInt64 Unknown_7
                {
                    get { return _unknown_7; }
                    set { _unknown_7 = value; }
                }
                public UInt64 Unknown_8
                {
                    get { return _unknown_8; }
                    set { _unknown_8 = value; }
                }
                public UInt32 FileSize
                {
                    get { return _file_size; }
                    set { _file_size = value; }
                }
                public UInt32 HeaderSize
                {
                    get { return _header_size; }
                    set { _header_size = value; }
                }
                public UInt32 ToCSize
                {
                    get { return _toc_size; }
                    set { _toc_size = value; }
                }
                public UInt32 EntrySize
                {
                    get { return _entry_size; }
                    set { _entry_size = value; }
                }
                public UInt32 DataSize
                {
                    get { return _data_size; }
                    set { _data_size = value; }
                }
                public UInt32 EntryCount
                {
                    get { return _entry_count; }
                    set { _entry_count = value; }
                }
                #endregion

                #region d_msgHeaderFormat Methods
                public override string ToString ()
                {
                    return String.Format("Ver{0} EntrSz: {1}b Num:{4} HdrSz {2}",
                                this.Unknown_2, HeaderSize, ( this.AreBitsFlipped ) ? ToCSize : EntrySize);
                }
                #endregion

                #region d_msgHeaderFormat Constructor
                public d_msgHeaderFormat (BinaryReader _bfile)
                {
                    _marker = String.Empty;
                    byte[] b = _bfile.ReadBytes(8);
                    foreach (byte a in b)
                        _marker += (char)a;
                    _marker = _marker.Trim('\0').Trim();
                    _unknown_1 = _bfile.ReadUInt16();
                    _unknown_2 = _bfile.ReadUInt16();
                    _unknown_3 = _bfile.ReadUInt32();
                    _unknown_4 = _bfile.ReadUInt32();
                    _file_size = _bfile.ReadUInt32();
                    _header_size = _bfile.ReadUInt32();
                    //_header.Unknown_5 = _bfile.ReadUInt32();
                    _toc_size = _bfile.ReadUInt32();
                    _entry_size = _bfile.ReadUInt32();
                    _data_size = _bfile.ReadUInt32();
                    _entry_count = _bfile.ReadUInt32();
                    _unknown_6 = _bfile.ReadUInt32();
                    _unknown_7 = _bfile.ReadUInt64();
                    _unknown_8 = _bfile.ReadUInt64();
                }
                #endregion
            }
            private class d_msgEntryFormat : Object
            {
                #region d_msgEntryFormat Variables
                public UInt32 offset;
                public UInt32 length;
                public String[] data;
                public Byte MessageID;
                public Byte GroupID;
                #endregion

                #region d_msgEntryFormat Methods
                public override string ToString ()
                {
                    int data_cnt = 0;
                    for (; data_cnt < data.Length; data_cnt++)
                    {
                        if (( data[data_cnt] == null ) ||
                            ( data[data_cnt] == String.Empty ) ||
                            ( data[data_cnt].Length <= 0 ))
                            continue;
                        else
                            break;
                    }
                    return String.Format("'{0}'", ( data_cnt >= data.Length ) ? "(Empty)" : data[data_cnt]);
                }

                // this is necessary as of the Dec 18th, 2006 patch
                // which causes all job/ability/area lists to be NOT'd bitwise
                // as part of the cryptography.
                private byte[] Fix (byte[] b)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = (byte)( ~( (uint)b[i] ) );
                    }
                    return b;
                }
                #endregion

                #region d_msgEntryFormat Constructor
                public d_msgEntryFormat (BinaryReader _bfile, d_msgHeaderFormat _header)
                {
                    if (_bfile.BaseStream.Position == _bfile.BaseStream.Length)
                        return;

                    // Loader for the new way as of April 2007
                    long start_of_data_block = _bfile.BaseStream.Position;
                    long saved_pos = _bfile.BaseStream.Position;
                    UInt32 string_count;

                    if (_header.ToCSize == 0) // no ToC, use EntrySize
                    {
                        #region if _header.ToCSize == 0 (Use EntrySize)
                        start_of_data_block = _bfile.BaseStream.Position;
                        if (_header.AreBitsFlipped)
                            string_count = ~( _bfile.ReadUInt32() );
                        else
                            string_count = _bfile.ReadUInt32();
                        if (( string_count < 1 ) || ( string_count > 100 ))
                        {
                            _bfile.BaseStream.Position = start_of_data_block + _header.EntrySize;
                            return;
                        }
                        data = new String[string_count];
                        byte[] b;
                        UInt32 type = 0;
                        for (int str_cnt = 0; str_cnt < string_count; str_cnt++)
                        {
                            data[str_cnt] = String.Empty;
                            if (_header.AreBitsFlipped)
                            {
                                offset = ~( _bfile.ReadUInt32() );
                                length = ~( _bfile.ReadUInt32() ); // Use "length" for flags
                            }
                            else
                            {
                                offset = _bfile.ReadUInt32();
                                length = _bfile.ReadUInt32(); // Use "length" for flags
                            }
                            saved_pos = _bfile.BaseStream.Position;
                            _bfile.BaseStream.Position = start_of_data_block + offset;
                            type = _bfile.ReadUInt32();
                            if (_header.AreBitsFlipped)
                                type = ~type;
                            b = new byte[4];
                            if (( length == 1 ) && ( str_cnt == 0 ) && ( string_count > 3 ))  // It's not a String.
                            {
                                // Hack right now for Key Items.
                                MessageID = (byte)( type & 0x00FF );  // already converted.
                                GroupID = (byte)( ( type & 0xFF00 ) >> 8 );
                                _bfile.BaseStream.Position = saved_pos;
                                continue;
                            }
                            else if (length == 1)
                            {
                                _bfile.BaseStream.Position = saved_pos;
                                continue;
                            }

                            _bfile.BaseStream.Position += 24; // add 24 bytes to get to actual text.
                            byte[] b_xfer = null;
                            do
                            {
                                b[0] = _bfile.ReadByte();
                                b[1] = _bfile.ReadByte();
                                b[2] = _bfile.ReadByte();
                                b[3] = _bfile.ReadByte();
                                if (_header.AreBitsFlipped)
                                {
                                    b[0] = (byte)( ~( (uint)b[0] ) );
                                    b[1] = (byte)( ~( (uint)b[1] ) );
                                    b[2] = (byte)( ~( (uint)b[2] ) );
                                    b[3] = (byte)( ~( (uint)b[3] ) );
                                }
                                if (b_xfer == null)
                                    b_xfer = new byte[4];
                                else
                                    Array.Resize(ref b_xfer, b_xfer.Length + 4);
                                b_xfer[b_xfer.Length - 4] = b[0];
                                b_xfer[b_xfer.Length - 3] = b[1];
                                b_xfer[b_xfer.Length - 2] = b[2];
                                b_xfer[b_xfer.Length - 1] = b[3];
                                if (b[3] == 0x00)
                                    break;
                            } while (true);

                            data[str_cnt] = System.Text.Encoding.GetEncoding(932).GetString(b_xfer).Trim('\0');

                            _bfile.BaseStream.Position = saved_pos;
                        }
                        _bfile.BaseStream.Position = start_of_data_block + _header.EntrySize;
                        #endregion
                    }
                    else if (_header.EntrySize == 0) // use ToCSize
                    {
                        #region if _header.EntrySize == 0 (Use ToCSize)
                        start_of_data_block = _header.HeaderSize + _header.ToCSize;

                        offset = _bfile.ReadUInt32();
                        length = _bfile.ReadUInt32();
                        if (_header.AreBitsFlipped)
                        {
                            offset = ~offset;
                            length = ~length;
                        }
                        saved_pos = _bfile.BaseStream.Position;
                        _bfile.BaseStream.Position = start_of_data_block + offset + 40;
                        data = new String[1];

                        if (_header.AreBitsFlipped)
                            data[0] = System.Text.Encoding.GetEncoding(932).GetString(Fix(_bfile.ReadBytes((int)length - 40))).Trim('\0');
                        else
                            data[0] = System.Text.Encoding.GetEncoding(932).GetString(_bfile.ReadBytes((int)length - 40)).Trim('\0');
                        _bfile.BaseStream.Position = saved_pos;
                        #endregion
                    }
                    else
                        return;
                }
                #endregion
            }
            private class d_msgFile : Object
            {
                #region d_msgFile Variables
                private d_msgHeaderFormat _header;
                private d_msgEntryFormat[] _entry_list;
                #endregion

                #region d_msgFile Properties
                public d_msgHeaderFormat Header
                {
                    get { return _header; }
                }
                public d_msgEntryFormat[] EntryList
                {
                    get { return _entry_list; }
                }
                #endregion

                #region d_msgFile Methods
                public override string ToString ()
                {
                    if (_header == null)
                        return "Header Not Loaded";
                    return String.Format("Ver{0} {1} {2} Entries at {3}b each",
                                _header.Unknown_2, ( this._header.AreBitsFlipped ) ? "Flipped Bits" : "Non-Flipped", _header.EntryCount,
                                ( _header.ToCSize != 0 ) ? _header.ToCSize : _header.EntrySize);
                }

                private static BinaryReader GetBinaryReader (String filename)
                {
                    // Get a File Info on the Spell Info Files, en ROM\119\56.DAT jp ROM\0\11.DAT
                    BinaryReader fi_br = null;
                    if (File.Exists(filename))
                    {
                        FileStream fs = null;
                        try
                        {
                            fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                            if (fs != null)
                            {
                                fi_br = new BinaryReader(fs);
                            }
                        }
                        catch
                        {
                            // ignore errors and return null
                        }
                    }
                    return fi_br;
                }
                #endregion

                #region d_msgFile Constructors
                public d_msgFile (String fi) : this(GetBinaryReader(fi)) { }
                public d_msgFile (BinaryReader _bfile)
                    : this()
                {
                    if (_bfile == null)
                        return;

                    _header = new d_msgHeaderFormat(_bfile);

                    if (_bfile.BaseStream.Length != _header.FileSize || !_header.Marker.StartsWith("d_msg"))
                    {
                        _header = null;
                        return;
                    }

                    _entry_list = new d_msgEntryFormat[_header.EntryCount];

                    for (int counter = 0; counter < _header.EntryCount; counter++)
                        _entry_list[counter] = new d_msgEntryFormat(_bfile, _header);

                    _bfile.Close();
                    _bfile = null;
                }
                public d_msgFile () { _header = null; _entry_list = null; }
                #endregion
            }
            #endregion

            #region item specific classes (itemHeader, itemFormat)
            private class itemHeaderFormat : Object
            {
                #region itemHeaderFormat Variables
                public UInt32 ID;
                public UInt16 Flags;
                public UInt16 StackSize;
                public UInt16 Type;
                public UInt16 ResourceID;
                public UInt16 ValidTargets;
                public UInt16 HeaderSize;
                #endregion

                #region itemHeaderFormat Constructor
                public itemHeaderFormat (BinaryReader br)
                {
                    ID = br.ReadUInt32();
                    Flags = br.ReadUInt16();
                    StackSize = br.ReadUInt16();
                    Type = br.ReadUInt16();
                    ResourceID = br.ReadUInt16();
                    ValidTargets = br.ReadUInt16();
                    HeaderSize = 0x0E;  // 14 bytes, Common
                }
                #endregion
            }

            private class itemFormat : Object
            {
                #region itemFormat Variables
                public itemHeaderFormat itemHeader;
                public String text;
                public String logtext;
                #endregion

                #region itemFormat Constructor
                public itemFormat (BinaryReader br)
                {
                    String[] ItemTexts = new String[9];

                    itemHeader = new itemHeaderFormat(br);
                    long data_pos = 0;
                    UInt32 num_strings = 0, offset = 0, flags = 0;

                    // Objects (General Items)  skip 6 bytes
                    //if (( itemHeader.ID <= 0x08BC ) && ( itemHeader.ID >= 0x0000 ))
                    //    br.BaseStream.Position = itemHeader.HeaderSize + 6;

                    // FIX: (10-7-2013) update
                    // Usable Items
                    if (( itemHeader.ID <= 0x21FF ) && ( itemHeader.ID >= 0x0000 ))
                        br.BaseStream.Position = itemHeader.HeaderSize + 10;  // Unknown is 0x04 bytes not 0x02

                    /* Pre 10-07-2013 update
                    // Usable items skip 2 bytes
                    // Usable Items skip 6 bytes as of March 10, 2008 Update (new UINT32)
                    else if (( itemHeader.ID <= 0x1FFF ) && ( itemHeader.ID >= 0x1000 ))
                        br.BaseStream.Position = itemHeader.HeaderSize + 6;
                    */

                    // Gil skip just 2 bytes
                    else if (itemHeader.ID == 0xFFFF)
                        br.BaseStream.Position = itemHeader.HeaderSize + 2;

                    // Puppet Items, skip 8 bytes
                    //else if (( itemHeader.ID <= 0x21FF ) && ( itemHeader.ID >= 0x2000 ))//11263 - 8192
                        //br.BaseStream.Position = itemHeader.HeaderSize + 10;  // Unknown is 0x04 bytes not 0x02

                    // Armor Specific Info, 22 bytes to skip to get to text
                    // 26 in March 10, 2008 Update (new UINT32)
                    else if (( itemHeader.ID <= 0x3FFF ) && ( itemHeader.ID >= 0x2800 ))//16383 - 11264
                        br.BaseStream.Position = itemHeader.HeaderSize + 26;

                    // Weapon Specific Info, 30 bytes to skip
                    // 34 bytes in March 10, 2008 Update (new UINT32)
                    else if (( itemHeader.ID <= 0x6FFF ) && ( itemHeader.ID >= 0x4000 ))
                        br.BaseStream.Position = itemHeader.HeaderSize + 34;

                    // Storage Slips, Vouchers, etc.
                    else if (( itemHeader.ID <= 0x7FFF ) && ( itemHeader.ID >= 0x7000 ))
                        br.BaseStream.Position = itemHeader.HeaderSize + 70;

                    // Unknown, should not have anything in the way...
                    else
                        br.BaseStream.Position = itemHeader.HeaderSize + 2;

                    data_pos = br.BaseStream.Position;
                    num_strings = br.ReadUInt32();

                    long fallback_pos = 0;
                    for (int i = 0; ( i < num_strings ); i++)
                    {
                        //                    if (num_strings >= 5)
                        //{ int x = i; }

                        offset = br.ReadUInt32();
                        flags = br.ReadUInt32();
                        fallback_pos = br.BaseStream.Position;
                        // Indicator (UInt32) + UInt32 x 6 Padding before text
                        br.BaseStream.Position = data_pos + offset + 28;
                        byte[] b = new byte[4];
                        int counter = 0;
                        do
                        {
                            if (br.BaseStream.Position >= br.BaseStream.Length)
                                break;
                            if (b == null)
                                b = new byte[4];
                            else if (( counter + 4 ) > b.Length)
                                Array.Resize(ref b, (int)( counter + 4 ));
                            b[counter++] = br.ReadByte();
                            b[counter++] = br.ReadByte();
                            b[counter++] = br.ReadByte();
                            b[counter++] = br.ReadByte();
                            if (b[counter - 1] == 0x00)
                                break;
                        } while (true);
                        /*if (i > ItemTexts.Length)
                        {
                            i = i+0;
                            break;
                        }*/
                        ItemTexts[i] = System.Text.Encoding.GetEncoding(932).GetString(b).Trim().Trim("\0\u0001.".ToCharArray());
                        br.BaseStream.Position = fallback_pos;
                    }
                    text = ItemTexts[0];
                    if (num_strings <= 4) // Japanese (no log name, same as shortname)
                        logtext = text;
                    else if (num_strings <= 5) // English (shortname, logname is position 3)
                        logtext = ItemTexts[2];
                    else if (num_strings <= 6) // French (shortname, logname is position 4)
                        logtext = ItemTexts[3];
                    else if (num_strings <= 9)
                        logtext = ItemTexts[4];
                    else
                        logtext = text;
                }
                #endregion
            }
            #endregion

            #endregion

            #region Methods

            private void Initialize ()
            {
                if (UseFFXIDatFiles)
                    ParseActualFiles();
                else
                    ParseResourceFiles();
            }

            #region Load*File()

            private void LoadDMSGFile (FileTypes type, Languages language, ResourceBit rb)
            {
                String Path = GetFilePath(type, language);
                //int index = GetFileNumber(type, language);

                if (!String.IsNullOrEmpty(Path))
                {
                    d_msgFile en_d_msg = new d_msgFile(Path);

                    if (( en_d_msg == null ) || ( en_d_msg.EntryList == null ) || ( en_d_msg.Header == null ))
                        return;

                    int Length = (int)en_d_msg.Header.EntryCount;

                    // for each entry
                    for (int i = 0; i < Length; i++)
                    {
                        if (( en_d_msg.EntryList[i] == null ) ||
                            ( en_d_msg.EntryList[i].data == null ) ||
                            ( en_d_msg.EntryList[i].data[0] == null ) ||
                            ( en_d_msg.EntryList[i].data[0].Length <= 0 ))
                            continue;

                        //if (String.IsNullOrEmpty(en_d_msg.EntryList[i].data[0]) || String.IsNullOrEmpty(en_d_msg.EntryList[i].data[0].Trim('.')))
                        //continue;

                        if (en_d_msg.EntryList[i].data[0].Length > 0)
                        {
                            // SpellNames are special case. To make them match up with FFACETools, we need to double the index.
                            String s = en_d_msg.EntryList[i].data[0];
                            if (type == FileTypes.SpellNames)
                                ResourcesCache.Add((int)( (uint)( i * 2 ) | (uint)rb ), s);
                            else
                                if (!ResourcesCache.ContainsKey((int) ((uint) i | (uint) rb)))
                                    ResourcesCache.Add((int) ((uint) i | (uint) rb), s);
                        }
                    }
                }
            }

            /// <summary>
            /// Generic loader for loading Item names
            /// </summary>
            /// <param name="filetype">FileTypes enum to indicate which file to load.</param>
            /// <param name="language">Preferred language as an Languages enum.</param>
            private void LoadItemFile (FileTypes filetype, Languages language)
            {
                String Path = GetFilePath(filetype, language);
                if (!String.IsNullOrEmpty(Path))
                {
                    FileInfo fi = new FileInfo(Path);
                    Boolean file_error = false;
                    if (!fi.Exists)
                        return;
                    if (( fi.Length % 0xC00 ) != 0)
                        return;

                    int items_in_file = (int)( fi.Length / 0xC00 );
                    BinaryReader iteminfo = null;
                    try
                    {
                        iteminfo = new BinaryReader(File.Open(Path, FileMode.Open, FileAccess.Read));
                    }
                    catch (IOException e)
                    {
                        // this line isn't necessary, but it avoids the annoying (e is assigned but never used)
                        Path = e.Message;
                        file_error = true;
                    }
                    if (file_error == true)
                        return;  // Attempt a Sanity Check
                    for (int item_counter = 0; item_counter < items_in_file; item_counter++)
                    {
                        iteminfo.BaseStream.Position = 0xC00 * item_counter;
                        byte[] readbytes = DecodeBlock(iteminfo.ReadBytes(0x200), 5);
                        BinaryReader data = new BinaryReader(new MemoryStream(readbytes, false));
                        BitConverter.ToInt32(readbytes.Skip(0x18).Take(4).ToArray(), 0);
                        itemFormat itemObjects = new itemFormat(data);
                        // INSERT ITEM CHECK DATA HERE
                        data.Close();
                        //if ((itemObjects.itemHeader.ID < 0xFFFF) && (itemObjects.itemHeader.ID > 0x6FFF))
                        //    continue;
                        if (String.IsNullOrEmpty(itemObjects.text) || String.IsNullOrEmpty(itemObjects.text.Trim('.')))
                            continue;
                        if (( itemObjects.itemHeader.Flags & (ushort)ItemFlags.Nothing ) == (ushort)ItemFlags.Nothing)
                            continue;
                        if (itemObjects.itemHeader.ID == 0x00)
                            continue;

                        // 0x0100 0x0040 0x1000
                        /* UINT32 ID
                         * UINT16 Flags
                         * UINT16 Stack Size
                         * UINT16 Type
                         * UINT16 ResourceID
                         * UINT16 ValidTargets 
                         * 14 Bytes - Common Header Size
                         */

                        if (String.IsNullOrEmpty(itemObjects.text))
                            continue;
                        else if (Char.IsSymbol(itemObjects.text[0]))
                            continue;
                        ResourcesCache.Add((int)itemObjects.itemHeader.ID, itemObjects.text);
                        // or itemObjects.logtext

                    }
                    iteminfo.Close();
                }
            }

            #endregion

            #region LoadFileIDs (must be called before any other Load*File() functions.

            /// <summary>
            /// Fills the FileList using the numbers in _fileNumberArray and FTABLE.DAT and VTABLE.DAT as reference.
            /// </summary>
            /// <param name="s">The path to FTABLE.DAT and VTABLE.DAT</param>
            private void LoadFileIDs (string s)
            {
                if (String.IsNullOrEmpty(s))
                    return;

                String vtablePath = s.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "VTABLE.DAT";
                String ftablePath = s.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "FTABLE.DAT";
                if (!File.Exists(vtablePath))
                    throw new FileNotFoundException("File does not exist!", vtablePath);
                if (!File.Exists(ftablePath))
                    throw new FileNotFoundException("File does not exist!", ftablePath);

                String filenameToCheck = String.Empty;

                BinaryReader vtable = new BinaryReader(File.Open(vtablePath, FileMode.Open, FileAccess.Read));
                BinaryReader ftable = new BinaryReader(File.Open(ftablePath, FileMode.Open, FileAccess.Read));
                foreach (int num in _fileNumberArray)
                {
                    if (num > 0)
                    {
                        vtable.BaseStream.Position = num;
                        if (vtable.ReadByte() == 0x00)
                        {
                            continue;
                        }
                        ftable.BaseStream.Position = num * 2;
                        filenameToCheck = s + InterpretPath(ftable.ReadUInt16());

                        if (File.Exists(filenameToCheck) && !FileList.ContainsKey(num))
                            FileList.Add(num, filenameToCheck);
                    }
                }
                vtable.Close();
                ftable.Close();
            }

            #endregion

            #region File Parsing Methods
            /// <summary>
            /// Entry point into the whole thing.
            /// </summary>
            private void ParseActualFiles ()
            {
                String s;
                // Load autotranslate phrases here.
                s = GetRegistryKey();
                if (String.IsNullOrEmpty(s))
                    return;

                LoadFileIDs(s);
                LoadItemFile(FileTypes.Gil, LanguagePreference);
                LoadItemFile(FileTypes.Armor, LanguagePreference);
                LoadItemFile(FileTypes.Weapons, LanguagePreference);
                LoadItemFile(FileTypes.Puppet_Items, LanguagePreference);
                LoadItemFile(FileTypes.Usable_Objects, LanguagePreference);
                LoadItemFile(FileTypes.Items, LanguagePreference);
                LoadItemFile(FileTypes.Slips_And_MMM, LanguagePreference);
                LoadDMSGFile(FileTypes.Zones, LanguagePreference, ResourceBit.Area);
                LoadDMSGFile(FileTypes.StatusEffects, LanguagePreference, ResourceBit.Status);
                LoadDMSGFile(FileTypes.JobNames_Short, LanguagePreference, ResourceBit.JobShort);
                LoadDMSGFile(FileTypes.JobNames_Long, LanguagePreference, ResourceBit.JobLong);
                LoadDMSGFile(FileTypes.SpellNames, LanguagePreference, ResourceBit.Spell);
                LoadDMSGFile(FileTypes.AbilityNames, LanguagePreference, ResourceBit.Abils);
                LoadDMSGFile(FileTypes.WeatherTypes, LanguagePreference, ResourceBit.Weather);
                LoadDMSGFile(FileTypes.Weekdays, LanguagePreference, ResourceBit.Day);
                LoadDMSGFile(FileTypes.MoonPhases, LanguagePreference, ResourceBit.Moon);
                LoadDMSGFile(FileTypes.Nations, LanguagePreference, ResourceBit.Nation);
                LoadDMSGFile(FileTypes.RaceNames, LanguagePreference, ResourceBit.RaceNames);
            }

            /// <summary>
            /// Entry point for testing the old way
            /// </summary>
            private void ParseResourceFiles ()
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlAttribute x;
                XmlAttribute id;

                #region Items -- Armor
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_ITEM_ARMOR_FILE_NAME));
                XmlNodeList itemsList = xmlDoc.SelectNodes("/items/i");

                if (LanguagePreference == Languages.English)
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (i.InnerText != string.Empty)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, i.InnerText);
                        }
                    }
                }
                else
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (( x = i.Attributes[LanguageStringsShort[(int)LanguagePreference]] ) != null && ( id = i.Attributes["id"] ) != null)
                        {
                            int identity = (int)( uint.Parse(id.Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, x.Value);
                        }
                    }
                }
                #endregion

                #region Items -- General
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_ITEMS_GENERAL_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/items/i");
                if (LanguagePreference == Languages.English)
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (i.InnerText != string.Empty)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, i.InnerText);
                        }
                    }
                }
                else
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (( x = i.Attributes[LanguageStringsShort[(int)LanguagePreference]] ) != null && ( id = i.Attributes["id"] ) != null)
                        {
                            int identity = (int)( uint.Parse(id.Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, x.Value);
                        }
                    }
                }
                #endregion

                #region Items -- Weapons
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_ITEM_WEAPONS_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/items/i");
                if (LanguagePreference == Languages.English)
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (i.InnerText != string.Empty)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, i.InnerText);
                        }
                    }
                }
                else
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (( x = i.Attributes[LanguageStringsShort[(int)LanguagePreference]] ) != null && ( id = i.Attributes["id"] ) != null)
                        {
                            int identity = (int)( uint.Parse(id.Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Item );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, x.Value);
                        }
                    }
                }
                #endregion

                #region Abilities
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_ABILS_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/abils/a");
                foreach (XmlNode i in itemsList)
                {
                    if (( x = i.Attributes[LanguageStrings[(int)LanguagePreference].ToLower()] ) != null && ( id = i.Attributes["id"] ) != null)
                    {
                        int idValue = int.Parse(id.Value, CultureInfo.InvariantCulture);
                        if (idValue < 0)
                            continue;
                        int identity = (int)( (uint)idValue | (uint)ResourceBit.Abils );
                        if (!ResourcesCache.ContainsKey(identity))
                            ResourcesCache.Add(identity, x.Value);
                    }
                }
                #endregion

                #region StatusEffects
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_STATUS_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/status/b");
                if (LanguagePreference == Languages.English)
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (i.InnerText != string.Empty)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Status );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, i.InnerText);
                        }
                    }
                }
                else
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (( x = i.Attributes[LanguageStringsShort[(int)LanguagePreference]] ) != null && ( id = i.Attributes["id"] ) != null)
                        {
                            int identity = (int)( uint.Parse(id.Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Status );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, x.Value);
                        }
                    }
                }
                #endregion

                #region SpellNames
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_SPELLS_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/spells/s");
                foreach (XmlNode i in itemsList)
                {
                    if (( x = i.Attributes[LanguageStrings[(int)LanguagePreference].ToLower()] ) != null && ( id = i.Attributes["id"] ) != null)
                    {
                        int identity = (int)( uint.Parse(id.Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Spell );
                        if (!ResourcesCache.ContainsKey(identity))
                            ResourcesCache.Add(identity, x.Value);
                    }
                }
                #endregion

                #region Areas/Zones
                xmlDoc.Load(String.Format(@"{0}{1}resources{1}{2}", WindowerPath.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar, RESOURCES_AREAS_FILE_NAME));
                itemsList = xmlDoc.SelectNodes("/areas/a");
                if (LanguagePreference == Languages.English)
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (i.InnerText != string.Empty)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Area );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, i.InnerText);
                        }
                    }
                }
                else
                {
                    foreach (XmlNode i in itemsList)
                    {
                        if (( x = i.Attributes[LanguageStringsShort[(int)LanguagePreference]] ) != null && ( id = i.Attributes["id"] ) != null)
                        {
                            int identity = (int)( uint.Parse(i.Attributes["id"].Value, CultureInfo.InvariantCulture) | (uint)ResourceBit.Area );
                            if (!ResourcesCache.ContainsKey(identity))
                                ResourcesCache.Add(identity, x.Value);
                        }
                    }
                }
                #endregion

                //if (dupes.Count > 0)
                //{
                //    String s = String.Format("{0} dupes found", dupes.Count);

                //    foreach (var dupevar in dupes)
                //    {
                //        s += String.Format("{0} - {1} ({2})", dupevar.Key, dupevar.Value, Store[dupevar.Key]);
                //    }
                //    System.Windows.Forms.MessageBox.Show(s);
                //}
            }

            #endregion

            #region private string GetRegistryKey

            /// <summary>
            /// Locates the Registry Key for the FFXI Installation on this computer and pulls the installation folder from it.
            /// </summary>
            /// <returns>Directory to the FFXI Installation with a trailing Path.DirectorySeparatorChar.</returns>
            private string GetRegistryKey ()
            {
                string s = String.Empty;
                // Attempt to open the key
                RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE{0}PlayOnlineUS{0}InstallFolder", Path.DirectorySeparatorChar));
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE{0}PlayOnlineEU{0}InstallFolder", Path.DirectorySeparatorChar));
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE{0}PlayOnline{0}InstallFolder", Path.DirectorySeparatorChar));
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(String.Format(@"Software{0}Wow6432Node{0}PlayOnlineUS{0}InstallFolder", Path.DirectorySeparatorChar));
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(String.Format(@"Software{0}Wow6432Node{0}PlayOnlineEU{0}InstallFolder", Path.DirectorySeparatorChar));
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(String.Format(@"Software{0}Wow6432Node{0}PlayOnline{0}InstallFolder", Path.DirectorySeparatorChar));

                // Attempt to retrieve the value "0001"; if null is returned, the value
                // doesn't exist in the registry.
                Object x = null;
                if (( key != null ) && ( ( x = key.GetValue("0001") ) != null ))
                {
                    s = x as String;
                    return ( String.Format("{0}{1}", s.TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar) );
                }
                if (key == null || x == null)
                {
                    System.Diagnostics.Process[] pol = System.Diagnostics.Process.GetProcessesByName("pol");
                    if (pol.Length > 0)
                    {
                        //System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(pol[0].Id);
                        foreach (System.Diagnostics.ProcessModule module in pol[0].Modules)
                        {
                            if (module.ModuleName.ToLower().Equals("ffxi.dll"))
                                return String.Format("{0}{1}", Path.GetDirectoryName(module.FileName).TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar);// .FileName.Substring(0, module.FileName.Length - 8);
                        }
                    }
                }
                return String.Empty;
            }

            #endregion

            #region Decoding Methods and related.

            /// <summary>Rotates the bits to the right by a set number of places.</summary>
            /// <param name="b">The byte whose bits we want to shift with rotation (preserving all set bits).</param>
            /// <param name="count">The number of places we want to shift the byte by.</param>
            /// <returns>The newly rotated byte.</returns>
            private byte RotateRight (byte b, int count)
            {
                for (; count > 0; count--)
                {
                    if (( b & 0x01 ) == 0x01)
                    {
                        b >>= 1; // if the last bit is 1 (ex. 00000001, it needs to be dropped
                        b |= 0x80; // and then set as the first bit (ex. 10000000)
                    }
                    else
                        b >>= 1; // if the last bit is not 1 (set), just rotate as normal.
                }
                return b;
            }

            /// <summary>
            /// Decodes a data/text block by reversing the set bit rotation.
            /// </summary>
            /// <param name="bytes">Array of bytes to decode.</param>
            /// <param name="shiftcount">The number of bits to shift right by.</param>
            /// <returns>The modified bytes array.</returns>
            private byte[] DecodeBlock (byte[] bytes, int shiftcount)
            {
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = RotateRight(bytes[i], shiftcount);
                return bytes;
            }

            #endregion

            #region Path generation and verification

            private bool LanguageIsValid (Languages language)
            {
                return ( ( language <= Languages.Last ) && ( language >= Languages.First ) );
            }

            private bool IndexIsValid (int index)
            {
                return ( ( index >= 0 ) && ( index < _fileNumberArray.Length ) );
            }

            private String GetFilePath (FileTypes filetype, Languages language)
            {
                return GetFilePath((int)filetype, (int)language);
            }

            private String GetFilePath (int filetype, int language)
            {
                int index = (int)( ( filetype * (int)Languages.Last ) + language );
                if (LanguageIsValid((Languages)language) && IndexIsValid(index))//(language >= Languages.NUM_LANG_MIN) && (language <= Languages.NUM_LANG_MAX))
                {
                    foreach (var kvp in FileList)
                        if (kvp.Key == _fileNumberArray[index])
                            return kvp.Value;
                }
                return String.Empty;
            }

            /// <summary>
            /// Returns a string in the format ROM\dir\file.DAT given a UInt16 fileID.
            /// </summary>
            /// <param name="fileID">The file ID found in FTABLE.DAT and VTABLE.DAT</param>
            /// <returns>String location of the file that fileID references.</returns>
            private string InterpretPath (UInt16 fileID)
            {
                // all files for the FINAL FANTASY XI\FTABLE & VTABLE.DAT files are in ROM\
                // further files in the ROM2, ROM3, ROM4 folders have a separate FTABLE/VTABLE file
                // in their subdirectory.
                return String.Format("ROM\\{0}\\{1}.DAT", fileID >> 7, fileID & 0x007F);
            }

            #endregion

            #endregion

            #region PRIVATE Constructor
            private ParseResources ()
            {
                Initialize();
            }
            #endregion

            #region ParseResources Specific Functions

            #region Get*Name Functions

            /// <summary>
            /// Will return the Short Job name for the passed Job enum (WAR, MNK, WHM, BLM etc)
            /// </summary>
            /// <param name="job">Job enum indicating job string requested.</param>
            /// <returns>String containg the short job name of the passed Job enum, String.Empty on error</returns>
            public static String GetShortJobName (Job job)
            {
                if (Instance == null)
                    return String.Empty;

                int cacheHash = ( (int)job | (int)ResourceBit.JobShort );
                String sResult;
                if (Instance.ResourcesCache.TryGetValue(cacheHash, out sResult))
                {
                    return sResult;
                }
                return String.Empty;
            }

            /// <summary>
            /// Will return the Long Job name for the passed Job enum (Warrior, Monk, White Mage, Black Mage, etc)
            /// </summary>
            /// <param name="job">Job enum indicating job string requested.</param>
            /// <returns>String containg the long job name of the passed Job enum, String.Empty on error</returns>
            public static String GetLongJobName (Job job)
            {
                if (Instance == null)
                    return String.Empty;

                int cacheHash = ( (int)job | (int)ResourceBit.JobLong );
                String sResult;
                if (Instance.ResourcesCache.TryGetValue(cacheHash, out sResult))
                {
                    return sResult;
                }
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the passed status effect
            /// </summary>
            /// <param name="statusEffect">StatusEffect that you want the in-game name for.</param>
            /// <returns>String of the passed StatusEffect, String.Empty on error</returns>
            public static String GetStatusEffectName (StatusEffect statusEffect)
            {
                if (Instance == null)
                    return String.Empty;

                int cacheHash = ( (int)( (ushort)statusEffect ) | (int)( ResourceBit.Status ) );
                String sResult;
                if (( Instance.ResourcesCache.Count > 0 ) && Instance.ResourcesCache.TryGetValue(cacheHash, out sResult))
                    return sResult;
                return String.Empty;
            } // @ public static string GetStatusEffectName(StatusEffect statusEffect)

            /// <summary>
            /// Will get the name of the passed area ID
            /// </summary>
            /// <param name="id">ID of the area to get the name for</param>
            /// <returns>String containing the in-game Zone name for the passed Zone, String.Empty on error</returns>
            public static String GetAreaName (Zone id)
            {
                if (Instance == null)
                    return String.Empty;

                int cacheHash = (int)( (ushort)id ) | (int)ResourceBit.Area;
                String sResult;
                if (Instance.ResourcesCache.TryGetValue(cacheHash, out sResult))
                    return sResult;
                return String.Empty;
            } // @ public static string GetAreaName(int id)

            /// <summary>
            /// Will get the name of the passed item ID
            /// </summary>
            /// <param name="id">ID of the item to get the name for</param>
            /// <returns>Item name matching the passed id, String.Empty on error</returns>
            public static String GetItemName (int id)
            {
                if (Instance == null)
                    return String.Empty;

                int key = id | (int)ResourceBit.Item;
                String result;

                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;

                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the passed ability ID
            /// </summary>
            /// <param name="abil">AbilityList enum indicating the Ability to get the name of.</param>
            /// <returns>In-game name of the ability passed, String.Empty on error</returns>
            public static String GetAbilityName (AbilityList abil)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)abil | (int)ResourceBit.Abils;
                String result;

                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the passed spell ID
            /// </summary>
            /// <param name="spell">SpellList enum indicating the Spell to get the name of.</param>
            /// <returns>In-game name of the spell passed, String.Empty on error</returns>
            public static String GetSpellName (SpellList spell)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)( (ushort)spell ) | (int)ResourceBit.Spell;
                String result;

                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the Moon Phase passed as a String
            /// </summary>
            /// <param name="phase">MoonPhase enum indicating the String to return</param>
            /// <returns>In-game name of the moon phase passed, String.Empty on error</returns>
            public static String GetMoonPhaseName (MoonPhase phase)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)phase | (int)ResourceBit.Moon;
                String result;
                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the Weekday passed
            /// </summary>
            /// <param name="day">Weekday enum indicating the String to return</param>
            /// <returns>In-game name of the moon phase passed, String.Empty on error</returns>
            public static String GetDayName (Weekday day)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)day | (int)ResourceBit.Day;
                String result;
                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the Nation passed
            /// </summary>
            /// <param name="nation">Nation enum indicating the String to return</param>
            /// <returns>In-game name of the Nation passed, String.Empty on error</returns>
            /// <remarks>Protip: This actually returns the REGION NAME, but it happens to match the Nation enum as well.</remarks>
            public static String GetNationName (Nation nation)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)nation | (int)ResourceBit.Nation;
                String result;
                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the Race passed
            /// </summary>
            /// <param name="race">RaceNames enum indicating the String to return</param>
            /// <returns>Returns a String containing the Race and Gender of the passed Race (Mithra Female, Galka Male, etc)</returns>
            public static String GetRaceName (Race race)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)race | (int)ResourceBit.RaceNames;
                String result;
                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }

            /// <summary>
            /// Will get the name of the Weather passed
            /// </summary>
            /// <param name="weather">Weather enum indicating the String to return</param>
            /// <returns>Returns a String containing the in-game text for the passed weather type (gloomy, stellar glare, duststorms, etc)</returns>
            public static String GetWeatherName (Weather weather)
            {
                if (Instance == null)
                    return String.Empty;

                int key = (int)weather | (int)ResourceBit.Weather;
                String result;
                if (Instance.ResourcesCache.TryGetValue(key, out result))
                    return result;
                return String.Empty;
            }
            #endregion

            #region Get*Id Functions
            /// <summary>
            /// Internal and private FindKey function for finding a key by value in a Dictionary
            /// </summary>
            /// <param name="lookup">Dictionary to look through.</param>
            /// <param name="value">Value to locate.</param>
            /// <returns>Key if value is found in Dictionary, null otherwise.</returns>
            private static int? FindKey (IDictionary<int, String> lookup, String value)
            {
                foreach (var pair in lookup)
                {
                    if (pair.Value.ToLower() == value.ToLower())
                        return pair.Key;
                }
                return null;
            }

            /// <summary>
            /// Internal and private FindKey function for finding a key by value in a Dictionary
            /// </summary>
            /// <param name="lookup">Dictionary to look through.</param>
            /// <param name="value">Value to locate.</param>
            /// <returns>Key if value is found in Dictionary, null otherwise.</returns>
            private static int? FindKeyStartsWith (IDictionary<int, String> lookup, String value)
            {
                foreach (var pair in lookup)
                {
                    if (pair.Value.ToLower().StartsWith(value.ToLower()))
                        return pair.Key;
                }
                return null;
            }

            /// <summary>
            /// Returns MoonPhase id based on string passed. (NOTE: Will return *Gibbous if waxing/waning, never *Gibbous2)
            /// </summary>
            /// <param name="phase">String to return ID for. (Exact case insensitive match)</param>
            /// <returns>MoonPhase or MoonPhase.Unknown</returns>
            public static MoonPhase GetMoonId (String phase)
            {
                if (Instance == null)
                    return MoonPhase.Unknown;
                int? key = FindKey(Instance.ResourcesCache, phase);
                if (key == null)
                    return MoonPhase.Unknown;
                return (MoonPhase)( key & ~( (int)ResourceBit.Moon ) );
            }

            /// <summary>
            /// Returns Nation enum indicating the nation passed (Exact case insensitive match)
            /// </summary>
            /// <param name="nation">String containing nation to get enum for.</param>
            /// <returns>255 on error (-1 in a byte), Nation enum otherwise</returns>
            public static Nation GetNationId (String nation)
            {
                if (Instance == null)
                    return (Nation)255;
                int? key = FindKey(Instance.ResourcesCache, nation);
                if (key == null)
                    return (Nation)255;
                return (Nation)( key & ~( (int)ResourceBit.Nation ) );
            }

            /// <summary>
            /// Returns Job ID matching string passed or 255 (byte -1) (Exact case insensitive match)
            /// </summary>
            /// <param name="job">String with either the short or long name of the Job to find.</param>
            /// <returns>255 (byte -1) on error/not found, otherwise Job enum.</returns>
            public static Job GetJobId (String job)
            {
                if (Instance == null)
                    return (Job)255;
                int? key = FindKey(Instance.ResourcesCache, job);
                if (key == null)
                    return (Job)255;
                // Remove either bits if they're set.  Allows us to use a single function to return either IDs based on long or short name
                return (Job)( key & ~( (int)( ResourceBit.JobLong | ResourceBit.JobShort ) ) );
            }

            /// <summary>
            /// Returns StatusEffect for passed String
            /// </summary>
            /// <param name="status">String containing status (Exact case insensitive match)</param>
            /// <returns>StatusEffect.Unknown on error, StatusEffect enum otherwise.</returns>
            public static StatusEffect GetStatusId (String status)
            {
                if (Instance == null)
                    return StatusEffect.Unknown;
                int? key = FindKey(Instance.ResourcesCache, status);
                if (key == null)
                    return StatusEffect.Unknown;
                return (StatusEffect)( key & ~( (int)ResourceBit.Status ) );
            }

            /// <summary>
            /// Returns AbilityList for passed String
            /// </summary>
            /// <param name="abil">String containing ability name (Exact case insensitive match)</param>
            /// <returns>AbilityList.Unknown on error, AbilityList enum otherwise.</returns>
            public static AbilityList GetAbilityId (String abil)
            {
                if (Instance == null)
                    return AbilityList.Two_Hour;
                int? key = FindKey(Instance.ResourcesCache, abil);
                if (key == null)
                    return AbilityList.Two_Hour;
                return (AbilityList)( key & ~( (int)ResourceBit.Abils ) );
            }

            /// <summary>
            /// Returns SpellList for passed String
            /// </summary>
            /// <param name="spell">String containing spell name (Exact case insensitive match)</param>
            /// <returns>SpellList.Unknown on error, SpellList enum otherwise.</returns>
            public static SpellList GetSpellId (String spell)
            {
                if (Instance == null)
                    return SpellList.Unknown;
                int? key = FindKey(Instance.ResourcesCache, spell);
                if (key == null)
                    return SpellList.Unknown;
                return (SpellList)( key & ~( (int)ResourceBit.Spell ) );
            }

            /// <summary>
            /// Returns Weather for passed String
            /// </summary>
            /// <param name="weather">String containing weather (gloomy, stellar glare, etc) -- (gloom matches gloomy in this case)</param>
            /// <returns>Weather enum indicating the equivalent (stellar will return Weather.Light_Double)</returns>
            public static Weather GetWeatherId (String weather)
            {
                if (Instance == null)
                    return Weather.Clear;

                int? key = FindKeyStartsWith(Instance.ResourcesCache, weather);
                if (key == null)
                    return Weather.Clear;
                return (Weather)( key & ~( (int)ResourceBit.Weather ) );
            }

            /// <summary>
            /// Returns Day for passed String
            /// </summary>
            /// <param name="day">String containing day (fire = firesday, light will bring up Lightningsday, lights = Lightsday)</param>
            /// <returns>First match on the string, Weekday.Unknown otherwise.</returns>
            public static Weekday GetDayId (String day)
            {
                if (Instance == null)
                    return Weekday.Unknown;
                int? key = FindKeyStartsWith(Instance.ResourcesCache, day);
                if (key == null)
                    return Weekday.Unknown;
                return (Weekday)( key & ~( (int)ResourceBit.Day ) );
            }

            /// <summary>
            /// Matches Race enum to beginning of string passed ('mithra female' and 'mithra' are the same)
            /// </summary>
            /// <param name="race">String containing race to get enum for.</param>
            /// <returns>Race enum or Race.Unknown</returns>
            public static Race GetRaceId (String race)
            {
                if (Instance == null)
                    return Race.Unknown;
                int? key = FindKeyStartsWith(Instance.ResourcesCache, race);
                if (key == null)
                    return Race.Unknown;
                return (Race)( key & ~( (int)ResourceBit.RaceNames ) );
            }

            /// <summary>
            /// Gets Zone ID for string passed (Exact case insensitive match)
            /// </summary>
            /// <param name="zone">String for zone</param>
            /// <returns>Zone enum or Zone.Unknown</returns>
            public static Zone GetAreaId (String zone)
            {
                if (Instance == null)
                    return Zone.Unknown;

                int? key = FindKey(Instance.ResourcesCache, zone);
                if (key == null)
                    return Zone.Unknown;
                return (Zone)( key & ~( (int)ResourceBit.Area ) );
            }

            public static int GetItemID (String item)
            {
                return GetItemId(item);
            }

            /// <summary>
            /// Will get the id of the passed item name
            /// </summary>
            /// <param name="id">name of the item to get the id for</param>
            /// <returns>0 if no matching item was found.</returns>
            public static int GetItemId (String item)
            {
                if (Instance == null)
                    return 0;

                int? key = FindKey(Instance.ResourcesCache, item);
                if (key == null)
                    return 0;
                return (int)( key & ~( (int)ResourceBit.Item ) );
            }

            /// <summary>
            /// Returns a list of matching ids for the string passed. (Twashtar will return a list of 3 different IDs)
            /// </summary>
            /// <param name="item">String to match the ID for (Exact case insensitive match)</param>
            /// <returns>List of id numbers, empty List if no match.</returns>
            public static List<int> GetItemIds (String item)
            {
                List<int> results = new List<int>();

                if (Instance == null)
                    return results;

                foreach (var kvp in Instance.ResourcesCache)
                {
                    if (kvp.Value.ToLower() == item.ToLower())
                        results.Add(kvp.Key & ~( (int)ResourceBit.Item ));
                }

                return results;
            }
            #endregion

            #endregion

            private static string CapitalizeWords (string value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.Length == 0)
                    return value;

                System.Text.StringBuilder sb = new System.Text.StringBuilder(value.Length);
                // Upper the first char.
                sb.Append(char.ToUpper(value[0]));
                for (int i = 1; i < value.Length; i++)
                {
                    // Get the current char.
                    char c = value[i];

                    // Upper if after a space or if after a period.
                    if (char.IsWhiteSpace(value[i - 1]) || ( value[i - 1] == '.' ) || ( value[i - 1] == '(' ))
                        c = char.ToUpper(c);
                    else
                        c = char.ToLower(c);

                    sb.Append(c);
                }

                return sb.ToString();
            }
        } // @ public class ParseResources
    } // @ public partial class FFACE
}
