// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace EasyFarm.Classes
{
    public class LibraryUpdater
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const string LibraryPath = "http://ext.elitemmonetwork.com/downloads/eliteapi/EliteAPI.dll";
        private const string LibraryPage = "http://ext.elitemmonetwork.com/downloads/eliteapi/";

        public bool HasUpdate()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "EliteAPI.dll");
            FileVersionInfo fileInfo = GetFileInfo(filePath);

            string latestVersion = GetLatestVersion();
            if (string.IsNullOrEmpty(latestVersion)) return false;

            return fileInfo == null || latestVersion != fileInfo.FileVersion;
        }

        public void Update()
        {
            try
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "EliteAPI.dll");
                FileVersionInfo fileInfo = GetFileInfo(filePath);               

                if (HasUpdate())
                {
                    BackupLibrary(fileInfo);
                    DownloadLibrary(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update EliteAPI.dll");
            }
        }

        private static FileVersionInfo GetFileInfo(string filePath)
        {
            FileVersionInfo fileInfo = null;

            if (File.Exists(filePath))
            {
                fileInfo = FileVersionInfo.GetVersionInfo(filePath);
            }

            return fileInfo;
        }

        private void BackupLibrary(FileVersionInfo fileVersionInfo)
        {
            if (fileVersionInfo == null) return;

            var backupPath = Path.Combine(
                Environment.CurrentDirectory,
                "backups",
                fileVersionInfo.FileVersion);

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            var backupFilePath = Path.Combine(backupPath, fileVersionInfo.OriginalFilename);
            if (File.Exists(fileVersionInfo.FileName) && !File.Exists(backupFilePath))
            {
                File.Move(fileVersionInfo.FileName, backupFilePath);
            }
        }

        private static string GetLatestVersion()
        {
            var response = new WebClient().DownloadString(new Uri(LibraryPage));

            var document = new HtmlDocument();
            document.LoadHtml(response);

            var download = document.DocumentNode
                .SelectNodes("//a[@id=\"download\"]")
                .FirstOrDefault();
            if (download == null) return null;

            var versionMatches = Regex.Match(download.InnerText, "v([\\d\\.]+)");
            if (!versionMatches.Success) return null;

            var latestVersion = versionMatches.Groups[1].Value;
            return latestVersion;
        }

        private static void DownloadLibrary(string filePath)
        {
            var client = new WebClient();
            client.DownloadFile(new Uri(LibraryPath), filePath);
        }
    }
}
