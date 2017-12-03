using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NLog;

namespace EasyFarm.Classes
{
    public class LibraryUpdater
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const string LibraryPath = "http://ext.elitemmonetwork.com/downloads/eliteapi/EliteAPI.dll";
        private const string LibraryPage = "http://ext.elitemmonetwork.com/downloads/eliteapi/";

        public void Update()
        {
            try
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "EliteAPI.dll");
                FileVersionInfo fileInfo = GetFileInfo(filePath);

                string latestVersion = GetLatestVersion();
                if (string.IsNullOrEmpty(latestVersion)) return;

                if (fileInfo == null || latestVersion != fileInfo.FileVersion)
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
