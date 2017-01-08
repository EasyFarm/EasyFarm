/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System;
using System.IO;
using EasyFarm.Logging;
using Microsoft.Win32;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Manages the saving and loading of game data to
    ///     file under their own extensions.
    /// </summary>
    public class SettingsManager
    {
        private readonly string _extension;
        private readonly string _fileType;
        private readonly string _startPath;

        public SettingsManager(string extension, string fileType)
        {
            _extension = extension;
            _fileType = fileType;
            _startPath = Environment.CurrentDirectory;
        }

        /// <summary>
        ///     Saves settings to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public bool TrySave<T>(T value)
        {
            string path = GetSavePath();

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {                
                Serialization.Serialize(path, value);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(TrySave)} : Failure on serialize settings", ex));
                return false;
            }
        }

        /// <summary>
        ///     Loads settings from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// ///
        /// <exception cref="InvalidOperationException"></exception>
        public T TryLoad<T>()
        {
            string path = GetLoadPath();

            if (!File.Exists(path))
            {
                return default(T);
            }

            try
            {                
                return Serialization.Deserialize<T>(path);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(TrySave)} : Failure on de-serialize settings", ex));
                return default(T);
            }
        }

        private string GetSavePath()
        {
            var sfd = new SaveFileDialog
            {
                OverwritePrompt = true,
                InitialDirectory = _startPath,
                AddExtension = true,
                DefaultExt = _extension,
                Filter = GetFilter()
            };

            sfd.ShowDialog();
            return sfd.FileName;
        }

        private string GetLoadPath()
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = _startPath,
                AddExtension = true,
                DefaultExt = _extension,
                Filter = GetFilter()
            };

            ofd.ShowDialog();
            return ofd.FileName;
        }

        private string GetFilter()
        {
            return string.Format("{0} ({1})|*.{1}", _fileType, _extension);
        }
    }
}