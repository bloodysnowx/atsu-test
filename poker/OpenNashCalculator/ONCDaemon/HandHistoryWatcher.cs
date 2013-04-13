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

        public void addFolder(String path, System.ComponentModel.ISynchronizeInvoke synchroObj)
        {
            var watcher = createWatcher(path, synchroObj);
            System.IO.Directory.GetFiles(path).Where(file => System.IO.File.GetLastWriteTime(file) > DateTime.Now.AddMinutes(-10)).All(
                file => { watcher_Changed(this, new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Created, path, System.IO.Path.GetFileName(file)));
                    return true; });
            watchers.Add(watcher);
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

        Regex regexPS = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
                + "Limit" + Regex.Escape(" ") + "Hold");
        Regex regexFT = new Regex("FT[0-9]+" + Regex.Escape(" ") + ".+" + Regex.Escape("(")
            + "[0-9]+" + Regex.Escape("), No Limit Hold'em"));
        private bool isHandHistory(string fileName)
        {
            return regexPS.IsMatch(fileName) || regexFT.IsMatch(fileName);
        }
    }
}
