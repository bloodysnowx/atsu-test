using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ONCDaemon
{
    public partial class ONCDaemonViewController : Form
    {
        int count = 0;
        HandHistoryWatcher watcher;

        public ONCDaemonViewController()
        {
            InitializeComponent();
            this.labelFolderPS.Text = Properties.Settings.Default.PSHandHistoryFolder;
            this.labelFolderFT.Text = Properties.Settings.Default.FTHandHistoryFolder;
            this.labelCount.Text = count.ToString();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            watcher = new HandHistoryWatcher();
            watcher.NewHandHistory += new HandHistoryWatcher.NewHandHistoryEventHandler(this.newHandHistoryCreated);
            watcher.addFolder(Properties.Settings.Default.PSHandHistoryFolder, this);
            watcher.addFolder(Properties.Settings.Default.FTHandHistoryFolder, this);
        }

        private void newHandHistoryCreated(object source, System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    string tmp_file = "\"" + e.FullPath + "\"";
                    System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/スタンド.exe", tmp_file);
                    this.labelCount.Text = (++count).ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
