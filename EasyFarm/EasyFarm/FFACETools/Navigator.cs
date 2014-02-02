using System;
using System.Collections.Generic;

namespace FFACETools
{
    public partial class FFACE
    {
        /// <summary>
        /// Class container to impliment Pyrolol's navigation system
        /// </summary>
        public class NavigatorTools
        {

            #region Members

            /// <summary>
            /// Our link to FFACE
            /// </summary>
            private FFACE _FFACE { get; set; }

            /// <summary>
            /// Our FFACE ID
            /// </summary>
            private int _InstanceID { get; set; }

            /// <summary>
            /// Whether or not we're currently moving.
            /// </summary>
            private bool _IsRunning = false;

            /// <summary>
            /// Internal HeadingTolerance value
            /// </summary>
            private uint _HeadingTolerance;

            /// <summary>
            /// Internal DistanceTolerance value
            /// </summary>
            private double _DistanceTolerance;
            /// <summary>
            /// How close to the target heading we're allowed to be before we modify our heading. Range (5-175
            /// </summary>
            public uint HeadingTolerance
            {
                get
                {
                    return _HeadingTolerance;
                }
                set
                {
                    // Valid values 5 minimum (5 degrees error allowed), Maximum 175 degrees
                    _HeadingTolerance = Math.Max(5, Math.Min(175, value));
                }
            }

            public bool UseArrowKeysForTurning { get; set; }

            /// <summary>
            /// How close we need to get to the target position. (Range: 0.25 - 49)
            /// </summary>
            public double DistanceTolerance
            {
                get
                {
                    return _DistanceTolerance;
                }
                set
                {
                    _DistanceTolerance = Math.Max((double)0.25, Math.Min(49, value));
                }
            }

            /// <summary>
            /// How long before we should check our current distance (in ms)
            /// </summary>
            public int SpeedDelay { get; set; }

            /// <summary>
            /// How long to stay running for
            /// </summary>
            [Obsolete("No longer used since we don't need to stop to adjust heading.")]
            public float StayRunningAmount { get; set; }

            /// <summary>
            /// Delegate option 
            /// </summary>
            public delegate float dPoint ();

            /// <summary>
            /// How long before checking current position and adjusting	if necessary
            /// </summary>
            public int GotoDelay { get; set; }

            #endregion

            #region Constructor

            /// <summary>
            /// Contructor to start navigation system
            /// </summary>
            /// <param name="player">Current player</param>
            public NavigatorTools (FFACE fface)
            {
                _FFACE = fface;
                _InstanceID = fface._InstanceID;
                _HeadingTolerance = 40;
                _DistanceTolerance = 3f;
                SpeedDelay = 40;
                GotoDelay = 40;

            } // @ public NavigatorTools(PlayerTools player)

            ~NavigatorTools ()
            {
                if (_IsRunning)
                    StopRunning();
            }

            #endregion

            #region Methods

            #region Goto* methods

            #region GotoTarget overloads  (Uses Goto)

            /// <summary>
            /// Will go to the current target's location (will not stop trying)
            /// </summary>
            public void GotoTarget ()
            {
                Goto(() => _FFACE.Target.PosX, () => _FFACE.Target.PosY, () => _FFACE.Target.PosZ, false, -1);
            } // @ public void GotoTarget()

            /// <summary>
            /// Will go to the current target's location or stop within a specified time.
            /// </summary>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void GotoTarget (int timeOut)
            {
                Goto(() => _FFACE.Target.PosX, () => _FFACE.Target.PosY, () => _FFACE.Target.PosZ, false, timeOut);
            } // @ public void GotoTarget(int timeOut)

            #endregion

            #region GotoNPC overloads (Uses Goto)

            /// <summary>
            /// Will go to the passed NPC's location (will not stop trying)
            /// </summary>
            /// <param name="ID">ID of the NPC</param>
            public void GotoNPC (int ID)
            {
                Goto(() => _FFACE.NPC.PosX(ID), () => _FFACE.NPC.PosY(ID), () => _FFACE.NPC.PosZ(ID), false, -1);
            } // @ public void GotoNPC(short ID)

            /// <summary>
            /// Will go to the passed NPC's location or stop within a specified time.
            /// </summary>
            /// <param name="ID">ID of the NPC</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void GotoNPC (int ID, int timeOut)
            {
                Goto(() => _FFACE.NPC.PosX(ID), () => _FFACE.NPC.PosY(ID), () => _FFACE.NPC.PosZ(ID), false, timeOut);
            } // @ public void GotoNPC(int ID, int timeOut)

            #endregion

            #region Goto overloads

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination (Will not stop trying)
            /// </summary>
            /// <param name="x">X coordinate of the destination (will not live update as it runs)</param>
            /// <param name="z">Z coordinate of the destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            public void Goto (float X, float Z, bool KeepRunning)
            {
                Goto(() => X, () => _FFACE.Player.PosY, () => Z, KeepRunning, -1);
            }

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination or stop within a specified time.
            /// </summary>
            /// <param name="x">X coordinate of the destination (will not live update as it runs)</param>
            /// <param name="z">Z coordinate of the destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void Goto (float X, float Z, bool KeepRunning, int timeOut)
            {
                Goto(() => X, () => _FFACE.Player.PosY, () => Z, KeepRunning, timeOut);
            }

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination. (Will not stop trying)
            /// </summary>
            /// <param name="x">X coordinate of the destination (will not live update as it runs)</param>
            /// <param name="y">Y coordinate of the destination (will not live update as it runs)</param>
            /// <param name="z">Z coordinate of the destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            public void Goto (float X, float Y, float Z, bool KeepRunning)
            {
                Goto(() => X, () => Y, () => Z, KeepRunning, -1);
            }

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination or stop within a specified time.
            /// </summary>
            /// <param name="x">X coordinate of the destination (will not live update as it runs)</param>
            /// <param name="y">Y coordinate of the destination (will not live update as it runs)</param>
            /// <param name="z">Z coordinate of the destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void Goto (float X, float Y, float Z, bool KeepRunning, int timeOut)
            {
                Goto(() => X, () => Y, () => Z, KeepRunning, timeOut);
            }

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination. (will not stop trying)
            /// </summary>
            /// <param name="position">Position of destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            public void Goto (Position position, bool KeepRunning)
            {
                Goto(() => position.X, () => position.Y, () => position.Z, KeepRunning, -1);
            }

            /// <summary>
            /// Will move the player to the passed STATIC ONLY destination or stop within a specified time.
            /// </summary>
            /// <param name="position">Position of destination (will not live update as it runs)</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void Goto (Position position, bool KeepRunning, int timeOut)
            {
                Goto(() => position.X, () => position.Y, () => position.Z, KeepRunning, timeOut);
            }

            /// <summary>
            /// Will move the player to the passed destination (will not stop trying)
            /// </summary>
            /// <param name="x">Function returning X coordinate of the destination</param>
            /// <param name="z">Function returning Z coordinate of the destination</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            public void Goto (dPoint x, dPoint z, bool KeepRunning)
            {
                Goto(x, () => _FFACE.Player.PosY, z, KeepRunning, -1);
            } // @ public void GotoXZ(dPoint x, dPoint z, bool KeepRunning)

            /// <summary>
            /// Will move the player to the passed destination or stop within a specified time.
            /// </summary>
            /// <param name="x">Function returning X coordinate of the destination</param>
            /// <param name="z">Function returning Z coordinate of the destination</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void Goto (dPoint x, dPoint z, bool KeepRunning, int timeOut)
            {
                Goto(x, () => _FFACE.Player.PosY, z, KeepRunning, timeOut);
            } // @ public void GotoXZ(dPoint x, dPoint z, bool KeepRunning, int timeOut)

            /// <summary>
            /// Will move the player to the passed destination (will not stop trying)
            /// </summary>
            /// <param name="x">Function returning X coordinate of the destination</param>
            /// <param name="y">Function returning Y coordinate of the destination</param>
            /// <param name="z">Function returning Z coordinate of the destination</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            public void Goto (dPoint x, dPoint y, dPoint z, bool KeepRunning)
            {
                Goto(x, y, z, KeepRunning, -1);
            } // @ public void GotoXYZ(dPoint x, dPoint y, dPoint z, bool KeepRunning)

            /// <summary>
            /// Will move the player to the passed destination or stop within a specified time.
            /// </summary>
            /// <param name="x">Function returning X coordinate of the destination</param>
            /// <param name="y">Function returning Y coordinate of the destination</param>
            /// <param name="z">Function returning Z coordinate of the destination</param>
            /// <param name="KeepRunning">Whether to keep moving after reaching the destination</param>
            /// <param name="timeOut">Time out in milliseconds</param>
            public void Goto (dPoint x, dPoint y, dPoint z, bool KeepRunning, int timeOut)
            {
                float X = x();
                float Y = y();
                float Z = z();

                // if passed values are all 0's we won't even bother.
                // because a target we're running to shouldn't be the player WHILE HE'S ZONING
                if (X == 0.0f && Y == 0.0f && Z == 0.0f)
                {
                    return;
                }

                double Heading = 0.0f; // = HeadingTo(X, Y, Z, HeadingType.Degrees);
                double PlayerHeading = 0.0f; // = GetPlayerPosHInDegrees();
                double Herror = 0.0f; // = HeadingError(PlayerHeading, Heading);

                DateTime Start = DateTime.Now;

                // while we're not within our distance tolerance
                // and
                // either timeOut is <= 0 (unlimited) or timeOut > 0 and Time since Goto called < timeOut
                while (( DistanceTo(X, Y, Z) > DistanceTolerance ) &&
                    ( ( timeOut <= 0 ) || ( ( timeOut > 0 ) && ( ( DateTime.Now - Start ).TotalMilliseconds ) < timeOut ) ))
                {
                    // Update X, Y, and Z values
                    X = x();
                    Y = y();
                    Z = z();

                    // if ANY of the values are NOT zero, then we aren't zoning, do something
                    if (X != 0.0f || Y != 0.0f || Z != 0.0f)
                    {
                        // Force ViewMode to first person otherwise this doesn't make sense
                        SetViewMode(ViewMode.FirstPerson);

                        // Check Heading Error
                        Heading = HeadingTo(X, Y, Z, HeadingType.Degrees);
                        PlayerHeading = GetPlayerPosHInDegrees();
                        Herror = HeadingError(PlayerHeading, Heading);

                        // if we're out of our heading tolerance
                        if (Math.Abs(Herror) > HeadingTolerance)
                        {
                            // Face proper direction
                            FaceHeading(X, Y, Z);
                        }
                        else if (UseArrowKeysForTurning && ( Herror < -( HeadingTolerance / 2.0f ) ))
                        {
                            _FFACE.Windower.SendKeyPress(KeyCode.NP_Number4);
                        }
                        else if (UseArrowKeysForTurning && ( Herror > ( HeadingTolerance / 2.0f ) ))
                        {
                            _FFACE.Windower.SendKeyPress(KeyCode.NP_Number6);
                        }

                        // Moved StartRunning to AFTER the Distance check
                        // to avoid tap-tap-tapping when we're already within distance
                        if (!IsRunning())
                            StartRunning();
                    }

                    // Sleep(GotoDelay) milliseconds before next loop
                    System.Threading.Thread.Sleep(GotoDelay);

                } // @ while (DistanceToPosXZ(X, Z) > DistanceTolerance)

                if (!KeepRunning)
                    StopRunning();

            } // @ public void GotoXYZ(dPoint x, dPoint y, dPoint z, bool KeepRunning, int timeOut)

            #endregion

            #endregion

            #region GetPlayerPosHInDegrees

            /// <summary>
            /// Will get the players heading (as radians) in degrees, and north as 0
            /// </summary>
            /// <returns>PosH heading of player converted to Degrees</returns>
            public double GetPlayerPosHInDegrees ()
            {
                return PosHToDegrees(_FFACE.Player.PosH);
            } // @ private double GetPlayerPosHInDegrees()

            #endregion

            #region Set* Methods (all private)

            #region SetPlayerPosH, SetNPCPosH (using FFXI Radians)

            /// <summary>
            /// Sets the Player PosH (directly calls FFACE)
            /// </summary>
            /// <param name="value">FFXI Radians to set PosH to.</param>
            /// <returns>true on success, false otherwise</returns>
            private bool SetPlayerPosH (float value)
            {
                if (FFACE.SetNPCPosH(_InstanceID, _FFACE.Player.ID, value) != 0.0f)
                    return true;
                return false;
            } // @ public bool SetPlayerPosH(float value)

            /// <summary>
            /// Set NPCs PosH as Radians (calls FFACE SetNPCPosH directly)
            /// </summary>
            /// <param name="index">index of NPC to modify</param>
            /// <param name="value">FFXI Radians value to set</param>
            /// <returns>true on success, false otherwise</returns>
            private bool SetNPCPosH (int index, float value)
            {
                if (FFACE.SetNPCPosH(_InstanceID, index, value) != 0.0f)
                    return true;
                return false;
            } // @ private bool SetNPCPosH(int index, float value)

            #endregion

            #region SetPlayerDegrees, SetNPCDegrees methods

            /// <summary>
            /// Set the Player PosH (directly calls FFACE)
            /// </summary>
            /// <param name="degrees">Degrees to set the PosH to (0/360 North), Negative valuees valid as well.</param>
            /// <returns>true on success, false otherwise</returns>
            private bool SetPlayerDegrees (double degrees)
            {
                if (FFACE.SetNPCPosH(_InstanceID, _FFACE.Player.ID, DegreesToPosH(degrees)) != 0.0f)
                    return true;
                return false;
            } // @ public bool SetPlayerDegrees(double degrees)

            /// <summary>
            /// Set NPCs PosH as Degrees (calls FFACE SetNPCPosH directly after converting Degrees to Radians)
            /// </summary>
            /// <param name="index">index of NPC to modify</param>
            /// <param name="degrees">Degrees to set to, will be converted to be within 0-360, negatives are valid too</param>
            /// <returns>true on success, false otherwise</returns>
            private bool SetNPCDegrees (int index, double degrees)
            {
                if (FFACE.SetNPCPosH(_InstanceID, index, DegreesToPosH(degrees)) != 0.0f)
                    return true;
                return false;
            } // @ private bool SetNPCDegrees(int index, double degrees)

            #endregion

            #endregion

            #region FaceHeading overloads

            /// <summary>
            /// Faces NPC/PC with matching ID
            /// </summary>
            /// <param name="ID">ID of NPC/PC to face</param>
            /// <returns>true on success, false if ID == 0 or if there was an error</returns>
            public bool FaceHeading (int ID)
            {
                if (ID > 0)
                    return FaceHeading(HeadingTo(ID, HeadingType.Radians), HeadingType.Radians);
                return false;
            } // @ public bool FaceHeading(short ID)

            /// <summary>
            /// Faces to destination as given by position
            /// </summary>
            /// <param name="position">Destination to face as a Position class.</param>
            public bool FaceHeading (Position position)
            {
                return FaceHeading(HeadingTo(position.X, position.Y, position.Z, HeadingType.Radians), HeadingType.Radians);
            } // @ public void FacePosXYZ(Position position)

            /// <summary>
            /// Faces to destination as given by X,Z
            /// </summary>
            /// <param name="X">X coordinate of destination</param>
            /// <param name="Z">Z coordinate of destination</param>
            public bool FaceHeading (double X, double Z)
            {
                return FaceHeading(HeadingTo(X, 0, Z, HeadingType.Radians), HeadingType.Radians);
            } // @ public bool FaceHeading(double X, double Z)

            /// <summary>
            /// Faces to destination as given by X,Y,Z
            /// </summary>
            /// <param name="X">X coordinate of destination</param>
            /// <param name="Y">Y coordinate of destination (not used, here for consistency)</param>
            /// <param name="Z">Z coordinate of destination</param>
            public bool FaceHeading (double X, double Y, double Z)
            {
                return FaceHeading(HeadingTo(X, Y, Z, HeadingType.Radians), HeadingType.Radians);
            } // @ public bool FaceHeading(double X, double Y, double Z)

            /// <summary>
            /// Faces the given heading in the passed HeadingType
            /// </summary>
            /// <param name="PosH">Heading to set on player.</param>
            /// <param name="headingType">HeadingType indicating the type of the value.</param>
            /// <returns>true on success, false otherwise</returns>
            public bool FaceHeading (float PosH, HeadingType headingType)
            {
                if (headingType == HeadingType.Degrees)
                    PosH = DegreesToPosH(PosH);

                if (FFACE.SetNPCPosH(_InstanceID, _FFACE.Player.ID, PosH) != 0.0f)
                    return true;
                return false;
            } // @ public bool FaceHeading(float PosH, HeadingType headingType)

            #endregion

            #region (Utility) *Running, SetViewMode, Distance*, Heading*, And Degree<->PosH conversions

            #region MathmaticaModulo overloads (because C# modulus returns remainder matching dividend's sign)

            /// <summary>
            /// Returns modulo of a to b with sign matching divisor (C# Modulo returns modulo with sign matching dividend)
            /// </summary>
            /// <param name="a">Left-hand operand</param>
            /// <param name="b">Right-hand operand</param>
            /// <returns>Result "wrapped" somewhere between 0 and b.</returns>
            /// <example>-5 % 3 = -2, MathMod(-5, 3) = 1; -270 % 360 = -270, MathMod(-270, 360) = 90</example>
            private double MathMod (double a, double b)
            {
                // 4 known ways to do it here
                // (a - (b * Math.Floor(a / b)))
                // (a - (b * (Math.Sign(b) * Math.Floor(a / Math.Abs(b)))))
                // (Math.Abs(a * b) + a) % b
                // ((a % b) + b) % b					// <- THIS ONE IS THE FASTEST!
                return ( ( a % b ) + b ) % b;
            } // @ private double MathMod(double a, double b)

            /// <summary>
            /// Returns modulo of a to b with sign matching divisor (C# Modulo returns modulo with sign matching dividend)
            /// </summary>
            /// <param name="a">Left-hand operand</param>
            /// <param name="b">Right-hand operand</param>
            /// <returns>Result "wrapped" somewhere between 0 and b.</returns>
            /// <example>-5 % 3 = -2, MathMod(-5, 3) = 1; -270 % 360 = -270, MathMod(-270, 360) = 90</example>
            private int MathMod (int a, int b)
            {
                return ( ( a % b ) + b ) % b;
            }

            /// <summary>
            /// Returns modulo of a to b with sign matching divisor (C# Modulo returns modulo with sign matching dividend)
            /// </summary>
            /// <param name="a">Left-hand operand</param>
            /// <param name="b">Right-hand operand</param>
            /// <returns>Result "wrapped" somewhere between 0 and b.</returns>
            /// <example>-5 % 3 = -2, MathMod(-5, 3) = 1; -270 % 360 = -270, MathMod(-270, 360) = 90</example>
            private float MathMod (float a, float b)
            {
                return ( ( a % b ) + b ) % b;
            }

            #endregion

            #region HeadingError method

            /// <summary>
            /// Calculate Heading Error between Origin and Target relative to Origin (how many degrees to adjust, negative is left, positive is right)
            /// </summary>
            /// <param name="Origin">Origin's (typically player's) current heading in degrees (can use negative values)</param>
            /// <param name="Target">Target heading we're finding we may be off from (can use negative values)</param>
            /// <returns>Heading in degrees of deviation (-## is we need to turn left, +## is need to turn right)</returns>
            public double HeadingError (double Origin, double Target)
            {
                // Normalize input values to be within range of 0-359.99999
                Origin = MathMod(Origin, (double)360.0000);
                Target = MathMod(Target, (double)360.0000);

                // Function: HeadingError = (((Target - Origin) + 180) % 360) - 180;

                // Find the difference.
                double diff = Target - Origin;

                // Add 180 to prepare for modulo
                diff = diff + (double)180.0;

                // Modulo to put value between 0-360
                diff = MathMod(diff, (double)360.0000);

                // Adjust by 180 to put value between -180 and 180
                diff = diff - (double)180.0;

                /* If you're printing this value you may want to account for -180
                 * I don't care though since I only want the Math.Abs() value for
                 * HeadingTolerance anyway
                 */

                /*
                double absDiff = Math.Abs(diff);

                // if value is straight up equal to 180 SOMEHOW, return the absolute value
                // because -180 is 180.
                if (absDiff == 180)
                    return absDiff;				
                */

                // otherwise return the modulated number
                return ( diff == -180.0f ) ? -diff : diff;
            } // @ public double HeadingError(double Origin, double Target)

            #endregion

            #region IsRunning, StartRunning, StopRunning, Reset Methods

            /// <summary>
            /// Resets status of running and sends the KeyUp command to Windower
            /// </summary>
            public void Reset ()
            {
                _IsRunning = true;
                StopRunning();
            } // @ public void Reset()

            /// <summary>
            /// Whether we're currently moving or not
            /// </summary>
            public bool IsRunning ()
            {
                return _IsRunning;
            } // @ public bool IsRunning()

            /// <summary>
            /// Will start moving the player to the destination
            /// </summary>
            private void StartRunning ()
            {
                if (!_IsRunning)
                {
                    _IsRunning = true;
                    _FFACE.Windower.SendKey(KeyCode.NP_Number8, true);
                    System.Threading.Thread.Sleep(SpeedDelay);
                } // @ if (!_IsRunning)
            } // @ private void StartRunning()

            /// <summary>
            /// Will stop moving the player
            /// </summary>
            private void StopRunning ()
            {
                if (_IsRunning)
                {
                    _IsRunning = false;
                    _FFACE.Windower.SendKey(KeyCode.NP_Number8, false);
                    System.Threading.Thread.Sleep(SpeedDelay);
                } // @ if (_IsRunning)
            } // @ private void StopRunning()

            #endregion

            #region SetViewMode

            /// <summary>
            /// Sets players ViewMode by pressing Numpad 5 if passed ViewMode is not already set.
            /// </summary>
            /// <param name="newMode">ViewMode to set it to.</param>
            public void SetViewMode (ViewMode newMode)
            {
                if (_FFACE.Player.ViewMode != newMode)
                {
                    _FFACE.Windower.SendKeyPress(KeyCode.NP_Number5);
                    System.Threading.Thread.Sleep(SpeedDelay);
                }
            } // @ public void SetViewMode(ViewMode newMode)

            #endregion

            #region DistanceTo Overloads

            /// <summary>
            /// Calculates the distance to the passed NPC
            /// </summary>
            /// <param name="ID">ID of the NPC/PC</param>
            /// <returns>Distance to NPC with matching ID, 0.0f if ID == 0</returns>
            public double DistanceTo (int ID)
            {
                if (ID > 0)
                    return DistanceTo(_FFACE.NPC.PosX(ID), _FFACE.NPC.PosY(ID), _FFACE.NPC.PosZ(ID));
                else
                    return 0.0f;
            } // @ public double DistanceTo(short ID)

            /// <summary>
            /// Calculates the distance to the passed destination
            /// </summary>
            /// <param name="position">Coordinates of destination as a Position class</param>
            /// <returns>Distance to position.</returns>
            public double DistanceTo (Position position)
            {
                return DistanceTo(position.X, position.Y, position.Z);
            } // @ public double DistanceToPos(Position position)

            /// <summary>
            /// Calculates the distance to the passed destination
            /// </summary>
            /// <param name="X">X coordinate of destination</param>
            /// <param name="Z">Z coordinate of destination</param>
            /// <returns>Distance to X,Z coordinates.</returns>
            public double DistanceTo (double X, double Z)
            {
                // use DistanceToPosXYZ because 0^2 == 0 (Player.PosY - Player.PosY)^2 = 0
                return DistanceTo(X, _FFACE.Player.PosY, Z);
            } // @ public double DistanceToPosXZ(double X, double Z)

            /// <summary>
            /// Calculates the distance to the passed destination taking height into account.
            /// </summary>
            /// <param name="X">X coordinate of destination</param>
            /// <param name="Y">Y coordinate of destination</param>
            /// <param name="Z">Z coordinate of destination</param>
            /// <returns>Distance to X, Y, Z coordinates.</returns>
            public double DistanceTo (double X, double Y, double Z)
            {
                return Math.Sqrt(Math.Pow(( _FFACE.Player.PosX - X ), 2) + Math.Pow(( _FFACE.Player.PosY - Y ), 2) + Math.Pow(( Z - _FFACE.Player.PosZ ), 2));
            } // @ public double DistanceToPosXYZ(double X, double Y, double Z)

            #endregion

            #region HeadingTo overloads

            /// <summary>
            /// Gets heading from player to NPC/PC as selected HeadingType
            /// </summary>
            /// <param name="ID">ID of NPC/PC</param>
            /// <param name="headingType">Heading type for the return value.</param>
            /// <returns>Heading to NPC with matching ID as headingType</returns>
            public float HeadingTo (int ID, HeadingType headingType)
            {
                if (ID > 0)
                    return HeadingTo(_FFACE.NPC.PosX(ID), _FFACE.NPC.PosY(ID), _FFACE.NPC.PosZ(ID), headingType);
                else
                    throw new IndexOutOfRangeException("NPC ID should be greater than 0 when passing to HeadingTo");
            } // @ public float HeadingTo(short ID, HeadingType headingType)

            /// <summary>
            /// Gets heading from player to Position as selected HeadingType
            /// </summary>
            /// <param name="position">Position containing coordinates to get heading to.</param>
            /// <param name="headingType">Heading type for the return value.</param>
            /// <returns>Heading to position as headingType</returns>
            public float HeadingTo (Position position, HeadingType headingType)
            {
                return HeadingTo(position.X, position.Y, position.Z, headingType);
            } // @ public float HeadingTo(Position position, HeadingType headingType)

            /// <summary>
            /// Gets heading from player to Position as selected HeadingType
            /// </summary>
            /// <param name="X">X coordinate of Position to get heading to.</param>
            /// <param name="Z">Z coordinate of Position to get heading to.</param>
            /// <param name="headingType">Heading type for the return value.</param>
            /// <returns>Heading to X, Z coords as headingType</returns>
            public float HeadingTo (double X, double Z, HeadingType headingType)
            {
                return HeadingTo(X, 0, Z, headingType);
            } // @ public float HeadingTo(double X, double Z, HeadingType headingType)

            /// <summary>
            /// Gets heading from player to Position as selected HeadingType
            /// </summary>
            /// <param name="X">X coordinate of Position to get heading to.</param>
            /// <param name="Y">Y coordinate of Position to get heading to.</param>
            /// <param name="Z">Z coordinate of Position to get heading to.</param>
            /// <param name="headingType">Heading type for the return value.</param>
            /// <returns>Heading to X, Y, Z coords as headingType</returns>
            public float HeadingTo (double X, double Y, double Z, HeadingType headingType)
            {
                X = X - _FFACE.Player.PosX;
                Z = Z - _FFACE.Player.PosZ;
                double H = -Math.Atan2(Z, X);
                if (headingType == HeadingType.Degrees)
                {
                    H = PosHToDegrees((float)H);
                }
                return (float)H;
            } // @ public float HeadingTo(double X, double Y, double Z, HeadingType headingType)

            #endregion

            #region PosHToDegrees and DegreesToPosH functions
            /// <summary>
            /// Converts FFXI PosH Radians headings into degrees (0-North, 270-West, 90-East, 180-South
            /// </summary>
            /// <param name="PosH">FFXI PosH Radians to convert to degrees</param>
            /// <returns>Degrees of heading 0N 90E 180S 270W (Range: 0 -> 359.9999)</returns>
            public double PosHToDegrees (float PosH)
            {
                // Formula: d = (((PosH * 180) / Math.PI) + 90) % 360;
                double d;

                // Convert from Degrees to Radians
                d = ( ( PosH * 180.0 ) / Math.PI );

                // Translate to Degree(Start) from FFXIRadians(Start) (Add 90deg) and Normalize
                return MathMod(( d + (double)90.0 ), (double)360.0000);
            }

            /// <summary>
            /// Converts degrees to FFXI PosH Radians heading.
            /// </summary>
            /// <param name="Degrees">Degrees (can be any value, will be forced into 0-359.999 range)</param>
            /// <returns>FFXI PosH compatible Radians heading (Range: Pi -> -Pi )</returns>
            public float DegreesToPosH (double Degrees)
            {
                // Formula: r = (((((Degrees + 90) % 360) * Math.PI) / 180) % (2 * Math.PI)) - Math.PI;
                // (float)(MathMod((((Degrees + 90) * Math.PI) / (double)180.0), 2 * Math.PI) - Math.PI);
                double r;

                // FFXI Radians Range -Pi > 0 > Pi starting @ our 270 Degrees, 
                // but still working in a 2Pi circle.  Since we have to subtract Pi at the end to normalize,
                // we need to account for this here by translating Degree(Start) to FFXIRadians(Start) + Pi
                // Degree(Start) is 0deg, FFXIRadians(Start) is 270deg, FFXIRadians(Start + Pi) is 90deg
                // Degree(Start) +	x = FFXIRadians(Start + Pi)
                //		0		 +	x = 90
                // The math is right, we subtract 180 at the end,
                // so we add 90 first to get the eventual -90 at the end
                // to move Degree(Start) backwards to FFXIRadians(Start)
                // Took me forever to figure out why this was right... -- Yekyaa
                r = ( Degrees + 90 );

                // Convert "regular" Degrees to Radians
                r = ( r * Math.PI ) / (double)180.0;

                // Normalize output (circle is 2PI, but FFXI uses Pi -> -Pi
                r = MathMod(r, 2 * Math.PI) - Math.PI;

                return (float)r;
            }
            #endregion

            #endregion

            #endregion

        } // @ public class NavigatorTools

        //X : E=+ve ; W=-ve
        //Y : Up=-ve ; Down=+ve
        //Z : N=+ve ; S=-ve
        //H : N=0 ; E=90; S=180; W=270

    } // @ public partial class FFACE
}
