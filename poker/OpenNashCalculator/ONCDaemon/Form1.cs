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
        int count = 0;

        System.IO.FileSystemWatcher watcher;
        Regex regexPS = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T[0-9]+" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
                + "Limit" + Regex.Escape(" ") + "Hold");
        Regex regexFT = new Regex("FT[0-9]+" + Regex.Escape(" ") + ".+" + Regex.Escape("(") 
            + "[0-9]+" + Regex.Escape("), No Limit Hold'em"));

        public Form1()
        {
            InitializeComponent();
            this.labelFolder.Text = Properties.Settings.Default.HandHistoryFolder;
            this.labelCount.Text = count.ToString();
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

            foreach (string file in files)
            {
                if (regexPS.IsMatch(file) || regexFT.IsMatch(file))
                {
                    if (System.IO.File.GetLastWriteTime(file) > DateTime.Now.AddMinutes(-10))
                    {
                        string tmp_file = "\"" + file + "\"";
                        System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/スタンド.exe", tmp_file);
                        this.labelCount.Text = (++count).ToString();
                    }
                }
            }

            watcher.EnableRaisingEvents = true;
        }

        private void watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    if (regexPS.IsMatch(e.FullPath) || regexFT.IsMatch(e.FullPath))
                    {
                        string tmp_file = "\"" + e.FullPath + "\"";
                        System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/スタンド.exe", tmp_file);
                        this.labelCount.Text = (++count).ToString();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
