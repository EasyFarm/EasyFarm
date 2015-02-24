using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public class SettingsManager
    {
        private static string _startPath = Environment.CurrentDirectory;
        private static string _extension = ".xml";

        public static void Save<T>(T value)
        {
            var path = GetSavePath();            
            if (!File.Exists(path)) return;
            Serialization.Serialize<T>(path, value);
        }

        public static T Load<T>()
        {
            var path = GetLoadPath();
            if (!File.Exists(path)) return default(T);
            return Serialization.Deserialize<T>(path);
        }

        private static string GetSavePath()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.InitialDirectory = _startPath;
            sfd.AddExtension = true;
            sfd.DefaultExt = _extension;
            sfd.ShowDialog();
            return sfd.FileName;
        }

        private static string GetLoadPath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = _startPath;
            ofd.AddExtension = true;
            ofd.DefaultExt = _extension;
            ofd.ShowDialog();
            return ofd.FileName;
        }
    }
}
