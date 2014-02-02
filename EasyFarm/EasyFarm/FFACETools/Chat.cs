using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FFACETools
{
    public partial class FFACE
    {
        /// <summary>
        /// Class container to impliment Pyrolol's chat system
        /// </summary>
        public class ChatTools
        {
            #region Classes

            /// <summary>
            /// Class container for a chat line returned from GetNextLine()
            /// </summary>
            public class ChatLine
            {
                /// <summary>
                /// Raw String array containing the strings split from one line of the FFXI chatlog.
                /// </summary>
                public String[] RawString { get; set; }
                /// <summary>
                /// The time the message was parsed in
                /// "January 01, 2009 HH:mm:ss AM/PM" format.
                /// </summary>
                public DateTime NowDate { get; set; }

                /// <summary>
                /// The time the message was parsed in "[HH:mm:ss]" format.
                /// </summary>
                public string Now { get; set; }

                /// <summary>
                /// The color code of the message
                /// </summary>
                public Color Color { get; set; }

                /// <summary>
                /// The chat line text
                /// </summary>
                public string Text { get; set; }

                /// <summary>
                /// The type of message
                /// </summary>
                public ChatMode Type { get; set; }

                /// <summary>
                /// The index of the message in FFXI memory.
                /// </summary>
                public Int32 Index { get; set; }

                public static bool operator == (ChatLine item1, ChatLine item2)
                {
                    if ((object)item1 == null && (object)item2 == null)
                        return true;
                    else if ((object)item1 == null || (object)item2 == null)
                        return false;
                    else
                        return item1.Equals(item2);
                }

                public static bool operator != (ChatLine item1, ChatLine item2)
                {
                    if ((object)item1 == null && (object)item2 == null)
                        return false;
                    else if ((object)item1 == null || (object)item2 == null)
                        return true;
                    else
                        return !item1.Equals(item2);
                }

                /// <summary>
                /// Returns a value indicating whether this instance is is equal to a specified instance
                /// </summary>
                /// <param name="o"></param>
                /// <returns></returns>
                public override bool Equals (object o)
                {
                    bool bEquals = false;

                    if (o is ChatLine)
                    {
                        if (this.Text.Equals(( (ChatLine)o ).Text)
                          && this.Type.Equals(( (ChatLine)o ).Type))
                            bEquals = true;
                    }

                    return bEquals;

                } // @ public override bool Equals(object o)

                /// <summary>
                /// Returns the hash code for the ChatLogEntry
                /// </summary>
                public override int GetHashCode ()
                {
                    return ( Text.GetHashCode() ) & ( ~(short)Type );

                } // @ public override int GetHashCode()
            } // @ public class ChatLine

            /// <summary>
            /// Structure to hold a Chat log message and it's type
            /// </summary>
            [Serializable]
            internal class ChatLogEntry : ICloneable, IComparable
            {
                #region Members

                public DateTime LineTime { get; set; }
                public string LineTimeString { get; set; }
                public string LineColor { get; set; }
                public Color ActualLineColor { get; set; }
                public string LineText { get; set; }
                public ChatMode LineType { get; set; }
                public int Index { get; set; }
                public String[] RawString { get; set; }

                #endregion

                #region Methods

                public static bool operator == (ChatLogEntry item1, ChatLogEntry item2)
                {
                    if ((object)item1 == null && (object)item2 == null)
                        return true;
                    else if ((object)item1 == null || (object)item2 == null)
                        return false;
                    else
                        return item1.Equals(item2);
                } // @ public static bool operator ==

                public static bool operator != (ChatLogEntry item1, ChatLogEntry item2)
                {
                    if ((object)item1 == null && (object)item2 == null)
                        return false;
                    else if ((object)item1 == null || (object)item2 == null)
                        return true;
                    else
                        return !item1.Equals(item2);
                } // @ public static bool operator !=

                /// <summary>
                /// Returns a value indicating whether this instance is is equal to a specified instance
                /// </summary>
                /// <param name="o"></param>
                /// <returns></returns>
                public int CompareTo (object obj)
                {
                    if (this.Index > ( (ChatLogEntry)obj ).Index)
                        return 1;
                    else
                        return -1;
                }

                public override bool Equals (object o)
                {
                    bool bEquals = false;

                    if (o is ChatLogEntry)
                    {
                        if (this.LineText.Equals(( (ChatLogEntry)o ).LineText)
                          && this.LineType.Equals(( (ChatLogEntry)o ).LineType)
                          && this.Index.Equals(( (ChatLogEntry)o ).Index))
                            bEquals = true;
                    }

                    return bEquals;

                } // @ public override bool Equals(object o)

                /// <summary>
                /// Returns the hash code for the ChatLogEntry
                /// </summary>
                public override int GetHashCode ()
                {
                    return ( LineText.GetHashCode() ) & ( ~(short)LineType );

                } // @ public override int GetHashCode()

                public object Clone ()
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(ms, this);
                    ms.Position = 0;
                    object obj = bf.Deserialize(ms);
                    ms.Close();

                    return obj;
                } // @ public object Clone()

                #endregion

            } // @ private class ChatLogEntry

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instanceID">Instance ID generated by FFACE</param>
            public ChatTools (int instanceID)
            {
                _InstanceID = instanceID;
                Update();
                Clear();

            } // @ public ChatTools(int instanceID)

            #endregion

            #region Members

            /// <summary>
            /// Instance ID generated by FFACE
            /// </summary>
            private int _InstanceID { get; set; }

            /// <summary>
            /// Queue of Chat Log entries
            /// </summary>
            private Queue<ChatLogEntry> _ChatLog = new Queue<ChatLogEntry>(50);

            /*			/// <summary>
                        /// Last seen chat entry
                        /// </summary>
                        private ChatLogEntry _LastSeenEntry;
                        */
            /// <summary>
            /// Last seen chat index
            /// </summary>
            private int _LastSeenIndex = -1;  //TODO: move initiation to constructor

            /// <summary>
            /// Number of lines FFACE sees in the chat log
            /// 
            /// NOTE: Draws directly from FFACE
            /// </summary>
            public int GetLineCount
            {
                get { return FFACE.GetChatLineCount(_InstanceID); }

            } // @ public int GetLineCount

            /// <summary>
            /// Returns if there is a new line, directly from FFACE
            /// </summary>
            public bool IsNewLine
            {
                get { return FFACE.IsNewLine(_InstanceID); }

            } // @ public bool IsNewLine

            #endregion

            #region Methods

            /// <summary>
            /// Will convert AT Brackets, Element Icons, and strip everything else.
            /// </summary>
            /// <param name="line">line to clean (left intact)</param>
            /// <returns>string containing the cleaned line, original is left unharmed</returns>
            public static String CleanLine (String line)
            {
                return FFACE.ChatTools.CleanLine(line, LineSettings.OldSchool);
            } // @ public static String CleanLine(String line)

            /// <summary>
            /// Will strip/convert requested items based on LineSettings passed.
            /// </summary>
            /// <param name="line">line to clean (left intact)</param>
            /// <param name="lineSettings">LineSettings to apply to the line for cleaning/converting.</param>
            /// <returns>string containing the modified line, original is left unharmed</returns>
            public static String CleanLine (String line, LineSettings lineSettings)
            {
                String cleanedString = line;

                // Duh. If we managed to get here, I'm not bothering.
                if (IsSet(lineSettings, LineSettings.RawText))
                {
                    return cleanedString;
                }

                if (IsSet(lineSettings, LineSettings.CleanTimeStamp))
                {
                    cleanedString = CleanTimeStamp(cleanedString);
                    // if all we're doing is pulling the TimeStamp, just return.
                    if (lineSettings == LineSettings.CleanTimeStamp)
                        return cleanedString;
                }

                Byte[] bytearray1252 = System.Text.Encoding.GetEncoding(1252).GetBytes(cleanedString);
                Int32 i = 0, len = bytearray1252.Length;
                const String sEF = "\xFF\x1F\x20\x21\x22\x23\x24\x25\x26\x27\x28\xFF";
                // 1f 20 21 22 23 24 25 26 27 28
                const String rep = "<FIAETWLD{}>";
                // Turns out \x1E is the start code for color changes/resets in log/dialog text.
                //const String s1E = "\x01\x02\x03\xFC\xFD";
                const String s1F = "\x0E\x0F\x2F\x7F\x79\x7B\x7C\x8D\x88\x8A\xA1\xD0";  //\r\n\x07
                const String sExtra = "\r\n\x07\x7F\x81\x87";

                List<Byte> cleaned = new List<Byte>();
                Int32 ndx = -1;
                bool inItemCode = false;
                bool inKeyItemCode = false;
                bool inObjectCode = false;

                // Sanity checks
                for (Int32 c = 0; c < len; ++c)
                {
                    if (( bytearray1252[c] == '\xEF' ) && ( ( ( c + 1 ) < len ) && ( ( ndx = sEF.IndexOf((Char)bytearray1252[c + 1]) ) >= 0 ) ))
                    {
                        // 3C <  3E >
                        // 7B {  7D }
                        bool isOpenBrace = ( sEF[ndx] == '\x27' );
                        bool isCloseBrace = ( sEF[ndx] == '\x28' );
                        bool isElementIcon = ( !isOpenBrace && !isCloseBrace );

                        if (!isCloseBrace) // Not closing brace? Needs starter char
                        {
                            if (( isOpenBrace && IsSet(lineSettings, LineSettings.ConvertATBrackets) ) ||
                                ( isElementIcon && IsSet(lineSettings, LineSettings.ConvertElementIcons) ))
                                cleaned.Add((Byte)rep[0]);
                        }
                        if (( !isElementIcon && IsSet(lineSettings, LineSettings.ConvertATBrackets) ) ||
                            ( isElementIcon && IsSet(lineSettings, LineSettings.ConvertElementIcons) ))
                            cleaned.Add((Byte)rep[ndx]); // add rep.char based on Index

                        if (!isOpenBrace) // Not opening brace? Needs closer char
                        {
                            if (( isCloseBrace && IsSet(lineSettings, LineSettings.ConvertATBrackets) ) ||
                                ( isElementIcon && IsSet(lineSettings, LineSettings.ConvertElementIcons) ))
                                cleaned.Add((Byte)rep[rep.Length - 1]); // >  Final: <{ and }> for Auto-translate braces
                        }
                        if (IsSet(lineSettings, LineSettings.ConvertATBrackets | LineSettings.ConvertElementIcons | LineSettings.CleanElementIcons | LineSettings.CleanATBrackets))
                            ++c;
                        else
                            cleaned.Add(bytearray1252[c]);
                    }
                    else if (( bytearray1252[c] == '\x1F' ) && ( ( ( c + 1 ) < len ) && ( ( ndx = s1F.IndexOf((char)bytearray1252[c + 1]) ) >= 0 ) ))
                    {
                        if (IsSet(lineSettings, LineSettings.CleanOthers))
                            ++c;
                        else
                            cleaned.Add(bytearray1252[c]);
                    }
                    else if (( bytearray1252[c] == '\x1E' ) && ( ( ( c + 1 ) < len ) ))
                    {
                        byte nextByte = bytearray1252[c + 1];

                        if (( nextByte == '\x03' ) && IsSet(lineSettings, LineSettings.ConvertKIBytes | LineSettings.CleanKIBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertKIBytes))
                            {
                                cleaned.Add((Byte)'[');
                                inKeyItemCode = true;
                            }
                            ++c;
                        }
                        else if (( nextByte == '\x02' ) && IsSet(lineSettings, LineSettings.ConvertItemBytes | LineSettings.CleanItemBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertItemBytes))
                            {
                                cleaned.Add((Byte)'{');
                                inItemCode = true;
                            }
                            ++c;
                        }
                        else if (( nextByte == '\x05' ) && IsSet(lineSettings, LineSettings.ConvertObjectBytes | LineSettings.CleanObjectBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertObjectBytes))
                            {
                                cleaned.Add((Byte)'(');
                                inObjectCode = true;
                            }
                            ++c;
                        }
                        else if (inItemCode && ( nextByte == '\x01' ) && IsSet(lineSettings, LineSettings.ConvertItemBytes | LineSettings.CleanItemBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertItemBytes))
                            {
                                cleaned.Add((Byte)'}');
                                inItemCode = false;
                            }
                            ++c;
                        }
                        else if (inKeyItemCode && ( nextByte == '\x01' ) && IsSet(lineSettings, LineSettings.ConvertKIBytes | LineSettings.CleanKIBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertKIBytes))
                            {
                                cleaned.Add((Byte)']');
                                inKeyItemCode = false;
                            }
                            ++c;
                        }
                        else if (inObjectCode && ( nextByte == '\x01' ) && IsSet(lineSettings, LineSettings.ConvertObjectBytes | LineSettings.CleanObjectBytes))
                        {
                            if (IsSet(lineSettings, LineSettings.ConvertObjectBytes))
                            {
                                cleaned.Add((Byte)')');
                                inObjectCode = false;
                            }
                            ++c;
                        }
                        else if (IsSet(lineSettings, LineSettings.CleanOthers))
                        {
                            ++c;
                        }
                        else
                        {
                            cleaned.Add(bytearray1252[c]);
                        }
                    }
                    else
                    {
                        i = sExtra.IndexOf((char)bytearray1252[c]);
                        if (i >= 3) // \r\n\07 are singles, others are doubles
                        {
                            if (( ( bytearray1252[c] == '\x7F' ) && ( ( ( c + 1 ) < len ) && bytearray1252[c + 1] == '\x31' ) ) ||
                                ( ( bytearray1252[c] == '\x81' ) && ( ( ( c + 1 ) < len ) && bytearray1252[c + 1] == '\xA1' ) ) ||
                                ( ( bytearray1252[c] == '\x81' ) && ( ( ( c + 1 ) < len ) && bytearray1252[c + 1] == '\x40' ) ) ||
                                ( ( bytearray1252[c] == '\x87' ) && ( ( ( c + 1 ) < len ) && bytearray1252[c + 1] == '\xB2' ) ) ||
                                ( ( bytearray1252[c] == '\x87' ) && ( ( ( c + 1 ) < len ) && bytearray1252[c + 1] == '\xB3' ) ))
                            {
                                if (IsSet(lineSettings, LineSettings.CleanOthers))
                                    ++c;
                                else
                                    i = -1;
                            }
                            else
                            {
                                i = -1; // not a target, so "wasn't found"
                            }
                        }
                        else if (i != -1)
                        {
                            // Target character at this point is either a \r\n or \x07
                            if (( sExtra[i] == '\r' ) || ( sExtra[i] == '\n' ))
                            {
                                // ++c; for Double-Byte combinations, this is not one.
                                // if we are NOT doing CleanNewLine
                                // then say we didn't find it.
                                if (!IsSet(lineSettings, LineSettings.CleanNewLine))
                                    i = -1;
                            }
                            else if (!IsSet(lineSettings, LineSettings.CleanOthers))
                            {
                                // At this point, it's a \x07
                                // if we are NOT doing CleanOthers
                                // then say we didn't find it.
                                i = -1;
                                // ++c; for Double-Byte combinations, \x07 is not one
                            }
                        }

                        // If the byte was not in a previous if/else,
                        // and we didn't find it in our "exceptions"
                        // then add it to the list.
                        if (( i < 0 ) && ( bytearray1252[c] != '\0' ))
                        {
                            cleaned.Add(bytearray1252[c]);
                        }
                    }
                }
                // got to end and didn't close an item/keyitem/object code?
                if (inKeyItemCode && IsSet(lineSettings, LineSettings.ConvertKIBytes))
                {
                    cleaned.Add((Byte)']');
                }
                else if (inObjectCode && IsSet(lineSettings, LineSettings.ConvertObjectBytes))
                {
                    cleaned.Add((Byte)')');
                }
                else if (inItemCode && IsSet(lineSettings, LineSettings.ConvertItemBytes))
                {
                    cleaned.Add((Byte)'}');
                }
                cleaned.Add(0);
                #region The above code done with all line.Replace() instead.
                /*
				if (cleanedString.IndexOf('\xEF') >= 0)
				{
				  cleanedString = cleanedString.Replace("\xEF\x1F", "");  // Fire
				  cleanedString = cleanedString.Replace("\xEF\x20", "");  // Ice
				  cleanedString = cleanedString.Replace("\xEF\x21", "");  // Wind (Air)
				  cleanedString = cleanedString.Replace("\xEF\x22", "");  // Earth
				  cleanedString = cleanedString.Replace("\xEF\x23", "");  // Lightning (Thunder)
				  cleanedString = cleanedString.Replace("\xEF\x24", "");  // Water
				  cleanedString = cleanedString.Replace("\xEF\x25", "");  // Light
				  cleanedString = cleanedString.Replace("\xEF\x26", "");  // Darkness
				  cleanedString = cleanedString.Replace("\xEF\x27", "");  // Opening Brace
				  cleanedString = cleanedString.Replace("\xEF\x28", "");  // Closing Brace
				}
				if (cleanedString.IndexOf('\x1E') >= 0)
				{
				  cleanedString = cleanedString.Replace("\x1E\x01", "");
				  cleanedString = cleanedString.Replace("\x1E\x02", "");
				  cleanedString = cleanedString.Replace("\x1E\x03", "");
				  cleanedString = cleanedString.Replace("\x1E\xFC", "");
				  cleanedString = cleanedString.Replace("\x1E\xFD", "");
				}
				if (cleanedString.IndexOf('\x1F') >= 0)
				{
				  cleanedString = cleanedString.Replace("\x1F\x0E", "");
				  cleanedString = cleanedString.Replace("\x1F\x0F", "");
				  cleanedString = cleanedString.Replace("\x1F\x2F", "");
				  cleanedString = cleanedString.Replace("\x1F\x7F", "");
				  cleanedString = cleanedString.Replace("\x1F\x79", "");
				  cleanedString = cleanedString.Replace("\x1F\x7B", "");
				  cleanedString = cleanedString.Replace("\x1F\x7C", "");
				  cleanedString = cleanedString.Replace("\x1F\x8D", "");
				  cleanedString = cleanedString.Replace("\x1F\x88", "");
				  cleanedString = cleanedString.Replace("\x1F\x8A", "");
				  cleanedString = cleanedString.Replace("\x1F\xA1", "");
				  cleanedString = cleanedString.Replace("\x1F\xD0", "");
				  cleanedString = cleanedString.Replace("\x1F\r", "");
				  cleanedString = cleanedString.Replace("\x1F\n", "");
				  cleanedString = cleanedString.Replace("\x1F\x07", "");
				}
				if (cleanedString.IndexOfAny(new char[] { '\r', '\n', '\x7F', '\x81', '\x87', '\x07' }) >= 0) {
				  cleanedString = cleanedString.Replace("\r", "");
				  cleanedString = cleanedString.Replace("\n", "");
				  cleanedString = cleanedString.Replace("\x7F\x31", "");
				  cleanedString = cleanedString.Replace("\x81\xA1", "");
				  cleanedString = cleanedString.Replace("\x87\xB2", "");
				  cleanedString = cleanedString.Replace("\x87\xB3", "");
				  cleanedString = cleanedString.Replace("\x07", "");
				}
			  */
                #endregion
                #region Original code (fail)
                /*
				// change the dot to a [ for start of string
				string startingChars = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x1e, 0xfc });
				if (cleanedString.StartsWith(startingChars))
					cleanedString = "[" + cleanedString.Substring(2);

				cleanedString = cleanedString.Replace("y", String.Empty);

				if (cleanedString.Contains(" "))
				{
					cleanedString = cleanedString.Replace(" ", "**&*&!!@#$@$#$");
					cleanedString = cleanedString.Replace("**&*&!!@#$@$#$", " ");
				}

cleanedString = cleanedString.Replace("1", "");
				cleanedString = cleanedString.Replace(" ", " "); // green start
				cleanedString = cleanedString.Replace("", "");   // green end
				cleanedString = cleanedString.Replace("Ð", "");
				cleanedString = cleanedString.Replace("{", "");
				cleanedString = cleanedString.Replace("ﾐ", "");
				cleanedString = cleanedString.Replace("", "");
				cleanedString = cleanedString.Replace("・", "E");
				cleanedString = cleanedString.Replace("・", "[");
				cleanedString = cleanedString.Replace("・", "");
				cleanedString = cleanedString.Replace("巧", "I"); // Yellow colored lines, JP
				cleanedString = cleanedString.Replace("｡", ""); // Super Kupowers, JP
				cleanedString = cleanedString.Replace("垢", "C"); // Change job text.

				// trim the 1 that occasionally shows up at the end
				if (cleanedString.EndsWith("1"))
					cleanedString = cleanedString.TrimEnd('1');
				
				cleanedString = cleanedString.TrimStart('');
				*/
                #endregion

                if (cleaned[0] != 0)
                {
                    byte[] arr = cleaned.ToArray();
                    cleanedString = System.Text.Encoding.GetEncoding(932).GetString(arr, 0, arr.Length);
                }
                else
                    cleanedString = String.Empty;
                //if (IsSet(ls, LineSettings.CleanTimeStamp) && cleanedString.StartsWith("["))  // Detect and remove Windower Timestamp plugin text.
                //{
                //	cleanedString = CleanTimeStamp(cleanedString);

                // C#/.Net Strings can have \0 at the end and it fucks with Windows RichTextBox
                cleanedString = cleanedString.TrimEnd('\0');

                //} // Detect and remove Windower Timestamp plugin text.


                if (!IsSet(lineSettings, LineSettings.CleanNewLine))
                {
                    // if it doesn't end with \r\n and we weren't supposed to CleanNewLine
                    // add it back on.
                    if (!cleanedString.EndsWith("\r\n"))
                        cleanedString += Environment.NewLine;
                }

                return cleanedString;

            } //  @ public static String CleanLine(String line, LineSettings ls)

            /// <summary>
            /// Cleans Timestamp plugin's addition to the chatlog. (Call before stripping color codes)
            /// </summary>
            /// <param name="s">String to strip Timestamp from (if present).</param>
            /// <returns>String containing the modified line.</returns>
            internal static String CleanTimeStamp (String s)
            {
                String stringToClean = s;
                byte[] textb = System.Text.Encoding.GetEncoding(1252).GetBytes(stringToClean);
                int index = -1, index2 = -1;
                for (index = 0; ( index + 2 ) < textb.Length; index++)
                {
                    // to allow for extra color codes in Timestamp later.			              // '['
                    if (( textb[index] == 0x1E ) && ( textb[index + 1] != 0x01 ) && ( textb[index + 2] == '\x5B' ))
                        break;
                }

                for (index2 = index + 2; ( index2 + 1 ) < textb.Length; index2++)
                {
                    if (( textb[index2] == 0x1E ) && ( textb[index2 + 1] == 0x01 ))
                    {
                        index2++;
						if (textb[index2 + 1] == 0x20)
							index2++;
                        break;
                    }
                }

                // 9 is an arbitrary index, it assumes the color code needed is within the first 9 bytes.
                if (( index <= 9 ) && index2 >= 0 && ( ( index2 + 2 ) < textb.Length ))
                {
                    String txt = String.Empty;
                    try
                    {
                        byte[] textb2 = new byte[textb.Length];

                        Array.ConstrainedCopy(textb, index2 + 1, textb2, 0, textb.Length - ( index2 + 1 ));
                        stringToClean = System.Text.Encoding.GetEncoding(1252).GetString(textb2);
                    }
                    catch
                    {
                        stringToClean = "Error";
                    }
                    return stringToClean;
                }
                // keeping this here in case people try to run CleanTimeStamp after the color byte codes are stripped.
                String text = ( stringToClean.Length >= 9 ) ? stringToClean.Substring(1, 8) : String.Empty;
                string re1 = ".*?";	// Non-greedy match on filler
                string re2 = "((?:(?:[0-1][0-9])|(?:[2][0-3])|(?:[0-9])):(?:[0-5][0-9])(?::[0-5][0-9])?(?:\\s?(?:am|AM|pm|PM))?)";

                Regex r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m = r.Match(text);
                if (m.Success)
                {
                    stringToClean = stringToClean.Remove(0, 11); // this assumes timestamp found is only 10+1 space in length
                    // Better way? : line = line.Remove(0,m.Length+1);
                }
                return stringToClean;
            } // @ internal static String CleanTimeStamp(String s)

            /// <summary>
            /// Will get the raw data of a chat line from FFACE
            /// </summary>
            /// <param name="index">Index of the line to get (0 being most recent)</param>
            /// <returns>null if error, Fully populated ChatLogEntry otherwise. In event of a bad chat line, ChatType == Chat.Error and line contains error message.</returns>
            internal ChatLogEntry GetLineRaw (short index)
            {
                // 210 to make sure it reads to end of string
                // for some reason 200 isn't big enough and it will strip some of the line off if it's long
                int size = 1024;
                byte[] buffer = new byte[size];
                GetChatLineR(_InstanceID, index, buffer, ref size);
                if (size <= 0)
                    return null;
                /*
                new ChatLogEntry() {
                    LineTime = DateTime.Now,
                    LineTimeString = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ",
                    LineColor = String.Empty,
                    ActualLineColor = Color.Empty,
                    LineText = String.Empty,
                    LineType = ChatMode.Error,
                    Index = 0,
                    RawString = new String[0]
                }; */

                // System.Text.Encoding.GetEncoding(932)
                string tempLine = System.Text.Encoding.GetEncoding(1252).GetString(buffer, 0, size - 1);

                string[] sArray = tempLine.Split(new char[1] { ',' }, 12, StringSplitOptions.None);

                if (sArray.Length != 12)
                {
                    return new ChatLogEntry()
                    {
                        LineTime = DateTime.Now,
                        LineTimeString = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ",
                        LineColor = "FFFF0000",
                        ActualLineColor = Color.Red,
                        LineText = "Error: Array length too short, RawString contains hard data.",
                        LineType = ChatMode.Error,
                        RawString = sArray,
                        Index = 0
                    };
                }

                /*
                 * [0] Chat Type
                 * [1] UNKNOWN Observed to be 3 (GuildClosed, Synergy status), 2 (Talking to NPC/Dialogs), 0 (chat messages)
                 * [2] UNKNOWN Observed to be 1 (Possibly when chat lines are shared) 0 (not shared?)
                 * [3] Line Color
                 * [4] Index merging wrapping
                 * [5] Index not merging wrapping (a line wrap gets its own index)
                 * [6] strlen (assume a double-byte special characters and Shift-JIS encoding as String)
                 * [7] UNKNOWN (always zero?)
                 * [8] UNKNOWN (always one?)
                 * [9] UNKNOWN (known values, 00 (echo/syscommands?), 01 (chat?), 02 (spells/abilities/etc?))
                 * [10] UNKNOWN  known values: 1 (Possibly when chat lines are shared) 0 (not shared?) (Duplicate of [2]?)
                 * [11] Actual line text
                 */
                String colorString = String.Empty;
                Color clr = Color.Empty;
                ChatMode linetype = ChatMode.Error;
                String lnTxt = String.Empty;
                String errorMsg = String.Empty;

                int ndx = -1;
                try
                {
                    linetype = (ChatMode)short.Parse(sArray[0], System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                catch (Exception e)
                {
                    linetype = ChatMode.Error;
                    errorMsg += String.Format("{0} ", e.Message);
                }
                colorString = sArray[3].Trim('#');
                try
                {
                    clr = ColorTranslator.FromHtml(String.Format("#{0}", colorString));
                }
                catch
                {
                    clr = Color.Empty;
                }
                //if (colorString == String.Empty)
                //	colorString = "FF0000";
                try
                {
                    ndx = int.Parse(sArray[5], System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                catch (Exception e)
                {
                    ndx = -1;
                    errorMsg += String.Format("{0} ", e.Message);
                }
                lnTxt = sArray[11].TrimEnd('\0');
                if (errorMsg != String.Empty)
                {
                    String temp = errorMsg + lnTxt;
                    lnTxt = temp;
                    linetype = ChatMode.Error;
                }
                return new ChatLogEntry()
                {
                    LineTime = DateTime.Now,
                    LineTimeString = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ",
                    // Original Line:  LineColor = ColorTranslator.FromHtml("#" + sArray[3]),
                    ActualLineColor = clr,
                    RawString = sArray,
                    LineColor = colorString,	//LineColor = sArray[3],
                    LineText = lnTxt,			//sArray[11].Remove(0, 4);
                    LineType = linetype,		//(ChatMode)short.Parse(sArray[0], System.Globalization.NumberStyles.AllowHexSpecifier),
                    Index = ndx					//int.Parse(sArray[5], System.Globalization.NumberStyles.AllowHexSpecifier)
                };
            } // @ internal ChatLogEntry GetLineRaw(short index)

            /// <summary>
            /// Updates the internal ChatTools queue
            /// </summary>
            internal void Update ()
            {
                // create a stack to hold current unparsed messages
                Stack<ChatLogEntry> currentLines = new Stack<ChatLogEntry>();

                // if we don't know our most recent chat line
                // NOTE: This should only happen when the first update is called
                if (_LastSeenIndex < 0) //null == _LastSeenEntry)
                {
                    // iterate over the last 50 chat lines
                    for (short index = 0; index <= 49; index++)
                    {
                        ChatLogEntry currentEntry = GetLineRaw(index);

                        // GetLineRaw() returns null if size <= 0 on the length of string.
                        if (currentEntry == null)
                            continue;

                        // only add the first 3 most recent lines
                        if (index == 0)
                        {
                            _LastSeenIndex = currentEntry.Index;
                            //	REMOVED: _LastSeenEntry = (ChatLogEntry)currentEntry.Clone();
                        }

                        // add the line to the unparsed line list
                        currentLines.Push(currentEntry);

                    } // @ for (short j = 0; j <= 49; j++)
                } // @ if (_LastAdded.Count.Equals(0))

                // we know our most recent chat line
                // (every other time but the first call to this function)
                else
                {
                    // for tracking our most recent chat line
                    //ChatLogEntry mostCurrentEntry = null;
                    //int mostCurrentIndex = 0;

                    // check for new unparsed lines from fface
                    int topindex = 0;
                    bool checkLastBuffer = true;
                    for (short i = 0; i <= 49; i++)
                    {
                        ChatLogEntry currentEntry = GetLineRaw(i);

                        // GetLineRaw() returns null if size <= 0 on the length of string.
                        if (currentEntry != null && currentEntry.LineType != ChatMode.Error)
                        {
                            if (i == 0)
                                topindex = currentEntry.Index;

                            if (currentEntry.Index <= _LastSeenIndex)
                            {
                                checkLastBuffer = false;
                                break;
                            }
                            currentLines.Push(currentEntry);
                        }
                    }
                    _LastSeenIndex = topindex;
                    if (checkLastBuffer)
                    {
                        for (int i = GetLineCount - 50; i < 0; i++)
                        {
                            ChatLogEntry currentEntry = GetLineRaw((short)i);

                            // GetLineRaw() returns null if size <= 0 on the length of string.
                            if (currentEntry != null && currentEntry.LineType != ChatMode.Error)
                            {
                                if (currentEntry.Index <= _LastSeenIndex)
                                    break;
                                currentLines.Push(currentEntry);
                            }
                        }
                    }
                }

                while (0 < currentLines.Count)
                    _ChatLog.Enqueue(currentLines.Pop());

            }

            /// <summary>
            /// Returns the number of unparsed lines in the internal ChatTools queue
            /// </summary>
            internal int NumberOfUnparsedLines ()
            {
                return _ChatLog.Count;

            } // @ internal int NumberOfUnparsedLines()

            /// <summary>
            /// Marks a line as parsed in the internal ChatTools queue
            /// </summary>
            internal void LineParsed ()
            {
                if (_ChatLog.Count != 0)
                    _ChatLog.Dequeue();

            } // @ internal void LineParsed()

            /// <summary>
            /// Clears the internal ChatTools queue
            /// </summary>
            public void Clear ()
            {
                _ChatLog.Clear();

            } // @ public void Clear()

            /// <summary>
            /// Will get the next chat line with default LineSettings.OldSchool
            /// </summary>
            /// <returns>Null if no new line available, otherwise the new line and all assosciated data.</returns>
            public ChatLine GetNextLine ()
            {
                return GetNextLine(LineSettings.OldSchool);
            }
            /// <summary>
            /// Will get the next chat line
            /// </summary>
            /// <param name="lineSettings">LineSettings to apply to cleanline.</param>
            /// <returns>Null if no new line available, otherwise the new line and all assosciated data.</returns>
            public ChatLine GetNextLine (LineSettings lineSettings)
            {
                ChatLine line = null;

                // update our local cache of chat lines
                Update();

                // if we have a new line
                if (NumberOfUnparsedLines() != 0)
                {
                    line = new ChatLine();
                    /*{
                        Color = Color.Empty,
                        NowDate = DateTime.Now,
                        Now = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ",
                        Text = String.Empty,
                        Type = ChatMode.Error
                    };*/
                    // get the next chat line
                    line.Now = _ChatLog.Peek().LineTimeString;
                    line.NowDate = _ChatLog.Peek().LineTime;
                    line.Index = _ChatLog.Peek().Index;
                    line.RawString = _ChatLog.Peek().RawString;

                    try
                    {
                        line.Color = ColorTranslator.FromHtml("#" + _ChatLog.Peek().LineColor.Trim('#'));
                    }
                    catch
                    {
                        line.Color = _ChatLog.Peek().ActualLineColor;
                    }

                    line.Type = _ChatLog.Peek().LineType;

                    // if user wanted to strip off color characters, do so
                    line.Text = FFACE.ChatTools.CleanLine(_ChatLog.Peek().LineText, lineSettings);

                    LineParsed();

                } // @ if (!NumberOfUnparsedLines().Equals(0))

                return line;

            } // @ public String GetNextLine(LineSettings lineSettings)

            #region GetLine, GetLineExtra, GetCurrentLine (Obsoleted and private, but kept for future)

            /// <summary>
            /// Will get a chat line directly from FFACE
            /// </summary>
            /// <param name="index">Index of the line to get (0 being most recent)</param>
            /// <returns>null if error, ChatLogEntry containing raw text of line matching index</returns>
            [Obsolete("Use GetLineRaw(index) instead.")]
            private ChatLogEntry GetLine (short index)
            {
                // 210 to make sure it reads to end of string
                // for some reason 200 isn't big enough and it will strip some of the line off if it's long
                int size = 1024;
                byte[] buffer = new byte[size];
                GetChatLine(_InstanceID, index, buffer, ref size);
                if (size <= 0)
                    return new ChatLogEntry() { LineText = String.Empty, LineType = ChatMode.Error, Index = 0 };

                string tempLine = System.Text.Encoding.GetEncoding(1252).GetString(buffer, 0, size - 1);

                return new ChatLogEntry()
                {
                    LineText = tempLine,
                    Index = index
                };

            } // @ internal ChatLogEntry GetLine(short index)

            /// <summary>
            /// Will get a chat line and type directly from FFACE
            /// </summary>
            /// <param name="index">Index of the line to get (0 being most recent)</param>
            /// <returns>null if error, ChatLogEntry containing Type, index, and raw text of the line matching index.</returns>
            [Obsolete("Use GetLineRaw(index) instead.")]
            private ChatLogEntry GetLineExtra (short index)
            {
                // 210 to make sure it reads to end of string
                // for some reason 200 isn't big enough and it will strip some of the line off if it's long
                int size = 1024;
                byte[] buffer = new byte[size];
                ChatMode mode = new ChatMode();
                GetChatLineEx(_InstanceID, index, buffer, ref size, ref mode);
                if (size <= 0)
                    return new ChatLogEntry() { LineText = String.Empty, LineType = ChatMode.Error, Index = 0 };

                string tempLine = System.Text.Encoding.GetEncoding(1252).GetString(buffer, 0, size - 1);

                return new ChatLogEntry()
                {
                    LineText = tempLine,
                    LineType = mode,
                    Index = index
                };

            } // @ internal ChatLogEntry GetLineExtra(short index)

            /// <summary>
            /// Will get the next chat line
            /// </summary>
            /// <param name="cleanLine">Whether to return a clean text line</param>
            /// <returns>Empty string if no new line available, otherwise the new line</returns>
            [Obsolete("Use GetNextLine() instead.")]
            internal ChatLine GetCurrentLine (LineSettings lineSettings)
            {
                ChatLine line = null;
                /*
                new ChatLine() {
                    Color = Color.Empty,
                    NowDate = DateTime.Now,
                    Now = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ",
                    Text = String.Empty,
                    Type = ChatMode.Error
                };*/

                // update our local cache of chat lines
                Update();

                // if we have a new line
                if (NumberOfUnparsedLines() != 0)
                {
                    line = new ChatLine();
                    // get the next chat line
                    line.Type = _ChatLog.Peek().LineType;
                    line.Text = _ChatLog.Peek().LineText;
                    line.Index = _ChatLog.Peek().Index;
                    line.RawString = _ChatLog.Peek().RawString;

                    try
                    {
                        line.Color = ColorTranslator.FromHtml("#" + _ChatLog.Peek().LineColor.Trim('#'));
                    }
                    catch
                    {
                        line.Color = _ChatLog.Peek().ActualLineColor;
                    }
                    // if user wanted to strip off color characters, do so
                    line.Text = FFACE.ChatTools.CleanLine(line.Text, lineSettings);

                } // @ if (!NumberOfUnparsedLines().Equals(0))

                return line;

            } // @ public string GetCurrentLine(bool cleanLine)

            #endregion

            #endregion

        } // @ public class ChatTools
    } // @ public partial FFACE
}