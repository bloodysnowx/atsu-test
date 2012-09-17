using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ONCDaemon
{
    public partial class Form1 : Form
    {
        System.IO.FileSystemWatcher watcher;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            watcher = new System.IO.FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.HandHistoryFolder;
            watcher.Filter = "*.txt";
            watcher.NotifyFilter = System.IO.NotifyFilters.FileName;
            watcher.IncludeSubdirectories = false;
            watcher.SynchronizingObject = this;
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);

            string[] files = System.IO.Directory.GetFiles(Properties.Settings.Default.HandHistoryFolder);
            Regex regex = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
                + "Limit" + Regex.Escape(" ") + "Hold");
            foreach (string file in files)
            {
                if (regex.IsMatch(file))
                {
                    if (System.IO.File.GetLastWriteTime(file) > DateTime.Now.AddMinutes(-10))
                    {
                        string tmp_file = "\"" + file + "\"";
                        System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/スタンド.exe", tmp_file);
                    }
                }
            }

            watcher.EnableRaisingEvents = true;
        }

        private void watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e)
        {
            Regex regex = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
               + "Limit" + Regex.Escape(" ") + "Hold");

            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    if (regex.IsMatch(e.FullPath))
                    {
                        string tmp_file = "\"" + e.FullPath + "\"";
                        System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/スタンド.exe", tmp_file);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
