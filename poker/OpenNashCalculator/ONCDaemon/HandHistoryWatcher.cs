using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ONCDaemon
{
    public class HandHistoryWatcher
    {
        public delegate void NewHandHistoryEventHandler(object sender, System.IO.FileSystemEventArgs e);
        public event NewHandHistoryEventHandler NewHandHistory;

        List<System.IO.FileSystemWatcher> watchers = new List<System.IO.FileSystemWatcher>();

        private string[] getAllDirs(String path)
        {
            string[] subDirs = System.IO.Directory.GetDirectories(path);
            string[] allDirs = new string[subDirs.Length + 1];
            Array.Copy(subDirs, allDirs, subDirs.Length);
            allDirs[subDirs.Length] = path;
            return allDirs;
        }

        private IEnumerable<System.IO.FileSystemWatcher> createWatchers(String path, System.ComponentModel.ISynchronizeInvoke synchroObj)
        {
            string[] allDirs = getAllDirs(path);

            foreach (string dirPath in allDirs)
            {
                yield return createWatcher(dirPath, synchroObj);
            }
        }

        private System.IO.FileSystemWatcher createWatcher(String path, System.ComponentModel.ISynchronizeInvoke synchroObj)
        {
            var watcher = new System.IO.FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = "*.txt";
            watcher.NotifyFilter = System.IO.NotifyFilters.FileName;
            watcher.IncludeSubdirectories = false;
            watcher.SynchronizingObject = synchroObj;
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true; 
            return watcher;
        }

        private void invokeEventForExistingFiles(string path)
        {
            foreach (var file in System.IO.Directory.GetFiles(path).Where(file => System.IO.File.GetLastWriteTime(file) > DateTime.Now.AddMinutes(-10)))
                watcher_Changed(this, new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Created, path, System.IO.Path.GetFileName(file)));
        }

        public void addFolder(String path, System.ComponentModel.ISynchronizeInvoke synchroObj)
        {
            IEnumerable<System.IO.FileSystemWatcher> newWatchers = createWatchers(path, synchroObj);
            string[] allDirs = getAllDirs(path);
            foreach (string dir in allDirs) invokeEventForExistingFiles(dir);
            watchers.AddRange(newWatchers);
        }

        private void watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    if (isHandHistory(e.Name))
                    {
                        NewHandHistory(this, e);
                    }
                    break;
                default:
                    break;
            }
        }

        public void clear()
        {
            foreach (var watcher in watchers)
                watcher.Created -= new System.IO.FileSystemEventHandler(watcher_Changed);
            watchers.RemoveAll( watcher => true );
        }

        Regex regexPS = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
            + "Limit" + Regex.Escape(" ") + "Hold");
        Regex regexPSJP = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "ノーリミット" + Regex.Escape(" ")
                + "ホールデム");
        Regex regexFT = new Regex("FT[0-9]+" + Regex.Escape(" ") + ".+" + Regex.Escape("(")
            + "[0-9]+" + Regex.Escape("), No Limit Hold'em"));
        private bool isHandHistory(string fileName)
        {
            return regexPS.IsMatch(fileName) || regexFT.IsMatch(fileName) || regexPSJP.IsMatch(fileName);
        }
    }
}
