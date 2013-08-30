using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using FFACETools;
using System.Diagnostics;
using System.Windows;
using System.IO.IsolatedStorage;
using EasyFarm.Properties;
using EasyFarm.Engine;
using System.Runtime.Serialization;

namespace EasyFarm.UtilityTools
{
    /// <summary>
    /// This class is essential the code graveyard. Any code that I would rather not dispose
    /// of because I found use for it in the past ends up here.
    /// </summary>
    static class Utilities
    {
        /// <summary>
        /// Dynamically resize the app to the size of the shown groupboxes
        /// </summary>
        /// <param name="Elements">Elements to resize to (The visible groupboxes)</param>
        public static void SetWindowSize(MainWindow Form, params GroupBox[] Elements)
        {
            double Width = 0;

            foreach (var Element in Elements)
            {
                Width += Element.Width;
            }

            Form.Width = Width;
        }

        /// <summary>
        /// Controls the visiblity status of groupboxes on my form.
        /// We do not want to overclutter the user with groupboxes
        /// </summary>
        /// <param name="Visible">The groupboxes that we want hidden</param>
        public static void SetHiddenElements(params GroupBox[] Hidden)
        {
            foreach (GroupBox h in Hidden)
            {
                h.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Controls the visiblity status of groupboxes on my form.
        /// We do not want to overclutter the user with groupboxes
        /// </summary>
        /// <param name="Visible">The groupboxes that we want visible</param>
        public static void SetVisibleElements(params GroupBox[] Visible)
        {
            foreach (GroupBox v in Visible)
            {
                v.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Returns the item ids of items passed in.
        /// </summary>
        /// <param name="Items"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetItemIDs(params string[] Items)
        {
            Dictionary<int, string> ItemIDs = new Dictionary<int, string>();

            foreach (var item in Items)
            {
                ItemIDs.Add(FFACE.ParseResources.GetItemId(item), item);
            }

            return ItemIDs;
        }

        /// <summary>
        /// Gets the ability's id
        /// </summary>
        /// <param name="Name">Name of ability</param>
        /// <param name="directory">Path to resource</param>
        /// <returns></returns>
        public static int GetAbilityID(string Name, string resource)
        {
            const string abils = "abils.xml";
            XElement XMLDoc;
            int Result = -1;

            if (File.Exists(resource))
            {
                XMLDoc = XElement.Load(abils);
                var Query = from i in XMLDoc.Elements("a")
                            let Key = (string)i.Attribute("english")
                            let Value = (int)i.Attribute("index")
                            where Name == Key
                            select new { Key, Value };

                if (!(Query.Count() > 0)) { }
                else Result = Query.Single().Value;
            }

            return Result;
        }

        public static int GetSpellID(string Name)
        {
            const string spells = "spells.xml";
            XElement XMLDoc;
            int Result = -1;
            var WorkingDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(FFACE.WindowerPath))
                Directory.SetCurrentDirectory(FFACE.WindowerPath);
            if (Directory.Exists("resources"))
                Directory.SetCurrentDirectory("resources");
            if (File.Exists(spells))
            {
                XMLDoc = XElement.Load(spells);
                var Query = from i in XMLDoc.Elements("s")
                            let Key = (string)i.Attribute("english")
                            let Value = (int)i.Attribute("index")
                            where Name == Key
                            select new { Key, Value };
                if (!(Query.Count() > 0)) { }
                else Result = Query.Single().Value;
            }
            Directory.SetCurrentDirectory(WorkingDirectory);
            return Result;
        }

        public static void Serialize(string FileName, dynamic Object)
        {
            using (Stream fStream = new FileStream(FileName,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(Object.GetType());
                xmlSerializer.Serialize(fStream, Object);
            }
        }

        public static dynamic Deserialize(string FileName, dynamic Object)
        {
            if (System.IO.File.Exists(FileName))
            {
                using (Stream fStream = new FileStream(FileName,
                    FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer xmlDeserializer = new XmlSerializer(Object.GetType());
                    return xmlDeserializer.Deserialize(fStream);
                }
            }

            return Object;
        }

        public static string[] SplitAction(string Action)
        {
            string[] SplitAction = new string[3];

            var split = Action.Split();

            foreach (string s in split)
            {
                if (split.First() == s)
                {
                    SplitAction[0] = s;
                }
                else if (s.StartsWith("<"))
                {
                    SplitAction[2] = s;
                }
                else
                {
                    SplitAction[1] += s + " ";
                }
            }

            SplitAction[1] = SplitAction[1].Replace('"', ' ');
            SplitAction[1] = SplitAction[1].TrimStart();
            SplitAction[1] = SplitAction[1].TrimEnd();

            return SplitAction;
        }

        public static double Distance(FFACE.Position A, FFACE.Position B)
        {
            double Distance = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
            return Distance;
        }

        public static FFACETools.KeyCode ToFFACEKey(this Key KeyPassed)
        {
            FFACETools.KeyCode ReturnKey = new FFACETools.KeyCode();

            switch (KeyPassed)
            {
                case Key.Return:
                    ReturnKey = FFACETools.KeyCode.EnterKey;
                    break;
                case Key.Escape:
                    ReturnKey = KeyCode.EscapeKey;
                    break;
                case Key.Y:
                    ReturnKey = FFACETools.KeyCode.EnterKey;
                    break;
                case Key.N:
                    ReturnKey = FFACETools.KeyCode.EscapeKey;
                    break;
                case Key.Up:
                    ReturnKey = FFACETools.KeyCode.UpArrow;
                    break;
                case Key.Down:
                    ReturnKey = FFACETools.KeyCode.DownArrow;
                    break;
                case Key.Left:
                    ReturnKey = FFACETools.KeyCode.LeftArrow;
                    break;
                case Key.Right:
                    ReturnKey = FFACETools.KeyCode.RightArrow;
                    break;
                case Key.OemMinus:
                    ReturnKey = FFACETools.KeyCode.MinusKey;
                    break;
                default:
                    break;

                #region UnusedKeys
                case Key.A:
                    break;
                case Key.AbntC1:
                    break;
                case Key.AbntC2:
                    break;
                case Key.Add:
                    break;
                case Key.Apps:
                    break;
                case Key.Attn:
                    break;
                case Key.B:
                    break;
                case Key.Back:
                    break;
                case Key.BrowserBack:
                    break;
                case Key.BrowserFavorites:
                    break;
                case Key.BrowserForward:
                    break;
                case Key.BrowserHome:
                    break;
                case Key.BrowserRefresh:
                    break;
                case Key.BrowserSearch:
                    break;
                case Key.BrowserStop:
                    break;
                case Key.C:
                    break;
                case Key.Cancel:
                    break;
                case Key.Capital:
                    break;
                case Key.Clear:
                    break;
                case Key.CrSel:
                    break;
                case Key.D:
                    break;
                case Key.D0:
                    break;
                case Key.D1:
                    break;
                case Key.D2:
                    break;
                case Key.D3:
                    break;
                case Key.D4:
                    break;
                case Key.D5:
                    break;
                case Key.D6:
                    break;
                case Key.D7:
                    break;
                case Key.D8:
                    break;
                case Key.D9:
                    break;
                case Key.DbeAlphanumeric:
                    break;
                case Key.DbeCodeInput:
                    break;
                case Key.DbeDbcsChar:
                    break;
                case Key.DbeDetermineString:
                    break;
                case Key.DbeEnterDialogConversionMode:
                    break;
                case Key.DbeEnterImeConfigureMode:
                    break;
                case Key.DbeFlushString:
                    break;
                case Key.DbeHiragana:
                    break;
                case Key.DbeKatakana:
                    break;
                case Key.DbeNoCodeInput:
                    break;
                case Key.DbeRoman:
                    break;
                case Key.DbeSbcsChar:
                    break;
                case Key.DeadCharProcessed:
                    break;
                case Key.Decimal:
                    break;
                case Key.Delete:
                    break;
                case Key.Divide:
                    break;
                case Key.E:
                    break;
                case Key.End:
                    break;
                case Key.Execute:
                    break;
                case Key.F:
                    break;
                case Key.F1:
                    break;
                case Key.F10:
                    break;
                case Key.F11:
                    break;
                case Key.F12:
                    break;
                case Key.F13:
                    break;
                case Key.F14:
                    break;
                case Key.F15:
                    break;
                case Key.F16:
                    break;
                case Key.F17:
                    break;
                case Key.F18:
                    break;
                case Key.F19:
                    break;
                case Key.F2:
                    break;
                case Key.F20:
                    break;
                case Key.F21:
                    break;
                case Key.F22:
                    break;
                case Key.F23:
                    break;
                case Key.F24:
                    break;
                case Key.F3:
                    break;
                case Key.F4:
                    break;
                case Key.F5:
                    break;
                case Key.F6:
                    break;
                case Key.F7:
                    break;
                case Key.F8:
                    break;
                case Key.F9:
                    break;
                case Key.FinalMode:
                    break;
                case Key.G:
                    break;
                case Key.H:
                    break;
                case Key.HangulMode:
                    break;
                case Key.HanjaMode:
                    break;
                case Key.Help:
                    break;
                case Key.Home:
                    break;
                case Key.I:
                    break;
                case Key.ImeAccept:
                    break;
                case Key.ImeConvert:
                    break;
                case Key.ImeModeChange:
                    break;
                case Key.ImeNonConvert:
                    break;
                case Key.ImeProcessed:
                    break;
                case Key.Insert:
                    break;
                case Key.J:
                    break;
                case Key.JunjaMode:
                    break;
                case Key.K:
                    break;
                case Key.L:
                    break;
                case Key.LWin:
                    break;
                case Key.LaunchApplication1:
                    break;
                case Key.LaunchApplication2:
                    break;
                case Key.LaunchMail:
                    break;
                case Key.LeftAlt:
                    break;
                case Key.LeftCtrl:
                    break;
                case Key.LeftShift:
                    break;
                case Key.LineFeed:
                    break;
                case Key.M:
                    break;
                case Key.MediaNextTrack:
                    break;
                case Key.MediaPlayPause:
                    break;
                case Key.MediaPreviousTrack:
                    break;
                case Key.MediaStop:
                    break;
                case Key.Multiply:
                    break;
                case Key.Next:
                    break;
                case Key.None:
                    break;
                case Key.NumLock:
                    break;
                case Key.NumPad0:
                    break;
                case Key.NumPad1:
                    break;
                case Key.NumPad2:
                    break;
                case Key.NumPad3:
                    break;
                case Key.NumPad4:
                    break;
                case Key.NumPad5:
                    break;
                case Key.NumPad6:
                    break;
                case Key.NumPad7:
                    break;
                case Key.NumPad8:
                    break;
                case Key.NumPad9:
                    break;
                case Key.O:
                    break;
                case Key.Oem1:
                    break;
                case Key.Oem102:
                    break;
                case Key.Oem2:
                    break;
                case Key.Oem3:
                    break;
                case Key.Oem4:
                    break;
                case Key.Oem5:
                    break;
                case Key.Oem6:
                    break;
                case Key.Oem7:
                    break;
                case Key.Oem8:
                    break;
                case Key.OemClear:
                    break;
                case Key.OemComma:
                    break;
                case Key.OemPeriod:
                    break;
                case Key.OemPlus:
                    break;
                case Key.P:
                    break;
                case Key.PageUp:
                    break;
                case Key.Pause:
                    break;
                case Key.Print:
                    break;
                case Key.PrintScreen:
                    break;
                case Key.Q:
                    break;
                case Key.R:
                    break;
                case Key.RWin:
                    break;
                case Key.RightAlt:
                    break;
                case Key.RightCtrl:
                    break;
                case Key.RightShift:
                    break;
                case Key.S:
                    break;
                case Key.Scroll:
                    break;
                case Key.Select:
                    break;
                case Key.SelectMedia:
                    break;
                case Key.Separator:
                    break;
                case Key.Sleep:
                    break;
                case Key.Space:
                    break;
                case Key.Subtract:
                    break;
                case Key.System:
                    break;
                case Key.T:
                    break;
                case Key.Tab:
                    break;
                case Key.U:
                    break;
                case Key.V:
                    break;
                case Key.VolumeDown:
                    break;
                case Key.VolumeMute:
                    break;
                case Key.VolumeUp:
                    break;
                case Key.W:
                    break;
                case Key.X:
                    break;
                case Key.Z:
                    break;
                #endregion
            }

            return ReturnKey;
        }

        private static bool ActionDuration(int Duration, DateTime StartTime)
        {
            bool Result = Duration >= (DateTime.Now - StartTime).TotalSeconds;
            return Result;
        }

        public static FFACE CreateFFACE()
        {
            FFACE Session;
            List<Process> POLProcesses = new List<Process>();
            POLProcesses.AddRange(Process.GetProcessesByName("pol"));
            if (POLProcesses.Count > 0)
            {
                Session = new FFACE(POLProcesses.First().Id);

                if (Session.Player.GetLoginStatus != LoginStatus.LoggedIn)
                {
                    MessageBox.Show("Character Not Logged In", "Shutting Down!",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                    System.Environment.Exit(0);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Final Fantasy Not Loaded", "Shutting Down!",
                    MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                System.Environment.Exit(0);
                return null;
            }

            return Session;
        }

        public static void SetGridVisibility(Grid passedControl, StackPanel Panel)
        {
            if (passedControl != null)
            {
                foreach (Grid Grid in Panel.Children)
                {
                    if (Grid.Equals(passedControl))
                        Grid.Visibility = Visibility.Visible;
                    else
                        Grid.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
