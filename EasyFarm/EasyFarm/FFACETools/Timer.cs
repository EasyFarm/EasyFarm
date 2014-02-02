using System;

namespace FFACETools
{
    public partial class FFACE
    {
        /// <summary>
        /// Class wrapper to consume timer methods
        /// </summary>
        public class TimerTools
        {
            #region Classes

            /// <summary>
            /// Structure containing 
            /// </summary>
            public struct VanaTime
            {
                #region Members

                /// <summary>
                /// The current day of the week (ie: earthsday)
                /// </summary>
                public Weekday DayType { get; set; }

                /// <summary>
                /// Current game day
                /// </summary>
                public byte Day { get; set; }

                /// <summary>
                /// Current game month
                /// </summary>
                public byte Month { get; set; }

                /// <summary>
                /// Current game year
                /// </summary>
                public short Year { get; set; }

                /// <summary>
                /// Current game hour
                /// </summary>
                public byte Hour { get; set; }

                /// <summary>
                /// Current game minute
                /// </summary>
                public byte Minute { get; set; }

                /// <summary>
                /// Current game second
                /// </summary>
                public byte Second { get; set; }

                /// <summary>
                /// Current moon percent
                /// </summary>
                public byte MoonPercent { get; set; }

                /// <summary>
                /// Current moon phase
                /// </summary>
                public MoonPhase MoonPhase { get; set; }

                /// <summary>
                /// If true, Moon phase is waxing, otherwise false
                /// </summary>
                public bool Waxing { get; set; }

                #endregion

                /// <summary>
                /// Returns a string representation of the Vana'Diel time details
                /// </summary>
                /// <returns></returns>
                public override string ToString ()
                {
                    string vTime = Month + "/" + Day + "/" + Year + ", "
                                 + GetDayOfWeekName(DayType) + ", "
                                 + Hour + ":";

                    if (10 > Minute)
                        vTime += "0" + Minute + ", "
                         + GetMoonPhaseName(MoonPhase) + " (" + MoonPercent + "%)";
                    else
                        vTime += Minute + ", "
                         + GetMoonPhaseName(MoonPhase) + " (" + MoonPercent + "%)";

                    return vTime;

                } // @ public override string ToString()

                /// <summary>
                /// Will get a string representation of the current day of the week
                /// </summary>
                /// <param name="day">Weekday to convert to string</param>
                public string GetDayOfWeekName (Weekday day)
                {
                    string dayweek = String.Empty;
                    switch (day)
                    {
                        case Weekday.Darksday:
                            dayweek = "Darksday";
                            break;
                        case Weekday.Earthsday:
                            dayweek = "Earthsday";
                            break;
                        case Weekday.Firesday:
                            dayweek = "Firesday";
                            break;
                        case Weekday.Iceday:
                            dayweek = "Iceday";
                            break;
                        case Weekday.Lightningday:
                            dayweek = "Lightningday";
                            break;
                        case Weekday.Lightsday:
                            dayweek = "Lightsday";
                            break;
                        case Weekday.Watersday:
                            dayweek = "Watersday";
                            break;
                        case Weekday.Windsday:
                            dayweek = "Windsday";
                            break;

                    } // @ switch (day)

                    return dayweek;

                } // @ public string GetDayOfWeekName(Weekday day)

                /// <summary>
                /// Will get a string representation of the current moon phase
                /// </summary>
                /// <param name="phase">MoonPhase to get a proper String equivalent of.</param>
                /// <returns></returns>
                public string GetMoonPhaseName (MoonPhase phase)
                {
                    string phaseName = String.Empty;
                    switch (phase)
                    {
                        case MoonPhase.FirstQuarter:
                            phaseName = "First Quarter";
                            break;
                        case MoonPhase.Full:
                            phaseName = "Full";
                            break;
                        case MoonPhase.LastQuarter:
                            phaseName = "Last Quarter";
                            break;
                        case MoonPhase.New:
                            phaseName = "New";
                            break;
                        case MoonPhase.WaningCrescent:
                        case MoonPhase.WaningCrescent2:
                            phaseName = "Waning Crescent";
                            break;
                        case MoonPhase.WaningGibbous:
                        case MoonPhase.WaningGibbous2:
                            phaseName = "Waning Gibbous";
                            break;
                        case MoonPhase.WaxingCrescent:
                        case MoonPhase.WaxingCrescent2:
                            phaseName = "Waxing Crescent";
                            break;
                        case MoonPhase.WaxingGibbous:
                        case MoonPhase.WaxingGibbous2:
                            phaseName = "Waxing Gibbous";
                            break;

                    } // @ switch (phase)

                    return phaseName;

                } // @ public string GetMoonPhaseName(MoonPhase phase)
            } // @ public struct VanaTime

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instanceID">Instance ID generated by FFACE</param>
            public TimerTools (int instanceID)
            {
                _InstanceID = instanceID;

            } // @ public TimerWrapper(int instance)

            #endregion

            #region Members

            /// <summary>
            /// Instance ID generated by FFACE
            /// </summary>
            private int _InstanceID { get; set; }

            /// <summary>
            /// Maximum Ability Index for use with GetAbilityRecast/GetAbilityID
            /// </summary>
            static readonly public byte MAX_ABILITY_INDEX = 32;

            /// <summary>
            /// Current server time in UTC
            /// 
            /// NOTE: Japan is +9 hours from UTC (ie: UTC midnight is 9am JST)
            /// </summary>
            public DateTime ServerTimeUTC
            {
                get
                {
                    // get UTC start time
                    TimeSpan tsUTCStart = new TimeSpan(new DateTime(1970, 1, 1).Ticks);

                    // get the server time (in seconds) and add the UTC start (in seconds)
                    long baseTime = (long)GetVanaUTC(_InstanceID) + (long)tsUTCStart.TotalSeconds;

                    // calculate how many hours there are since too many seconds to fit in an int
                    int hours = Convert.ToInt32(baseTime / 3600);
                    // calculate our seconds minusing the hours
                    int seconds = Convert.ToInt32(baseTime % ( baseTime / 3600 ));

                    // return the results
                    return new DateTime(new TimeSpan(hours, 0, seconds).Ticks);
                }

            } // @ public DateTime ServerTimeUTC

            public int GetVanaUTC
            {
                get { return GetVanaUTC(_InstanceID); }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Will get the time left in seconds before being able to recast a spell
            /// </summary>
            /// <param name="spell">Spell to check recast timer on</param>
            public short GetSpellRecast (short id)
            {
                // get recast time from fface
                short time = FFACE.GetSpellRecast(_InstanceID, (SpellList)id);

                // FFACE seems to return the recast 1 second shorter then it is
                if (0 < time)
                    time += 60;

                return (short)( time / 60 );
            } // @ public short GetSpellRecast(short id)

            /// <summary>
            /// Will get the time left in seconds before being able to recast a spell
            /// </summary>
            /// <param name="spell">Spell to check recast timer on</param>
            public short GetSpellRecast (SpellList spell)
            {
                // get recast time from fface
                short time = FFACE.GetSpellRecast(_InstanceID, spell);

                // FFACE seems to return the recast 1 second shorter then it is
                if (0 < time)
                    time += 60;

                return (short)( time / 60 );

            } // @ public TimeSpan GetSpellRecast(eSpellList spell)

            /// <summary>
            /// Gets the time left in seconds before being able to reuse an ability (use sparingly)
            /// </summary>
            /// <param name="abil">Ability List</param>
            /// <returns>-1 if Ability is not "equipped", Recast of the ability otherwise</returns>
            public int GetAbilityRecast (AbilityList abil)
            {
                for (byte i = 0; i < MAX_ABILITY_INDEX; i++)
                    if (GetAbilityID(i) == abil)
                        return FFACE.GetAbilityRecast(_InstanceID, i) / 60;
                // not found.
                return -1;
            }

            /// <summary>
            /// Gets the time left in seconds before being able to reuse an ability
            /// </summary>
            /// <param name="index">Index of the ability</param>
            public int GetAbilityRecast (byte index)
            {
                if (index >= 0 && index <= MAX_ABILITY_INDEX)
                    return FFACE.GetAbilityRecast(_InstanceID, index) / 60;
                else
                    throw new ArgumentOutOfRangeException("Must be within 0 to MAX_ABILITY_INDEX");
            } // @ public int GetAbilityRecast(byte index)

            /// <summary>
            /// Gets the ID of an ability by the index
            /// </summary>
            /// <param name="index">Index of the ability</param>
            public AbilityList GetAbilityID (byte index)
            {
                if (index >= 0 && index <= MAX_ABILITY_INDEX)
                    return FFACE.GetAbilityID(_InstanceID, index);
                else
                    throw new ArgumentOutOfRangeException("Must be within 0 to MAX_ABILITY_INDEX");

            } // @ public byte GetAbilityID(byte index)

            /// <summary>
            /// Gets the current Vana'Diel time information
            /// </summary>
            public VanaTime GetVanaTime ()
            {
                // get the server time (in seconds)
                long baseTime = (long)GetVanaUTC(_InstanceID);

                // calculate the difference between server time and 
                // 1/1/1970 00:00:00 unix time -> "Vana'Diel time in seconds"
                long timeInSeconds = ( (long)baseTime + 92514960 ) * 25;

                // how many days
                decimal dayOfYear = Math.Floor((decimal)( timeInSeconds / (decimal)86400 ));

                VanaTime vanaTime = new VanaTime();
                vanaTime.DayType = (Weekday)( dayOfYear % 8 );
                vanaTime.Day = (byte)( ( dayOfYear % 30 ) + 1 );
                vanaTime.Month = (byte)( ( ( dayOfYear % 360 ) / 30 ) + 1 );
                vanaTime.Year = (short)( dayOfYear / 360 );
                vanaTime.Hour = (byte)( ( timeInSeconds / 3600 ) % 24 );
                vanaTime.Minute = (byte)( ( timeInSeconds / 60 ) % 60 );
                // can't floor on a long, so need to shrink it first
                vanaTime.Second = (byte)( ( timeInSeconds - ( System.Math.Floor((decimal)( timeInSeconds / 60 )) * 60 ) ) );

                // calculate moon phase/percent
                decimal moonPhase = ( dayOfYear + (decimal)26 ) % 84;

                // calculate moon percent
                decimal moonPercent = ( ( ( 42 - moonPhase ) * 100 ) / 42 );
                if (0 > moonPercent)
                    moonPercent = Math.Abs(moonPercent);

                // get final moon percent
                vanaTime.MoonPercent = (byte)Math.Floor(( moonPercent + (decimal)0.5 ));

                // get final moon phase
                if (38 <= moonPhase)
                    vanaTime.MoonPhase = (MoonPhase)Math.Floor(( moonPhase - (decimal)38 ) / (decimal)7);
                else
                    vanaTime.MoonPhase = (MoonPhase)Math.Floor(( moonPhase + (decimal)46 ) / (decimal)7);

                return vanaTime;

            } // @ public VanaTime GetVanaTime()

            #endregion

        } // @ public class TimerTools
    } // @ public partial class FFACE
}
