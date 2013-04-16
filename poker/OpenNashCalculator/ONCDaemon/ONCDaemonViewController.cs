using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ONCDaemon
{
    public partial class ONCDaemonViewController : Form
    {
        int count = 0;
        HandHistoryWatcher watcher;
        object syncObject = new object();
        bool isChanged;
        FilePathManager pathManager = new FilePathManager();

        public ONCDaemonViewController()
        {
            InitializeComponent();
        }

        private void readFromConfig()
        {
            if(!System.IO.Directory.Exists(Properties.Settings.Default.PSHandHistoryFolder))
                Properties.Settings.Default.PSHandHistoryFolder = String.Empty;
            if (!System.IO.Directory.Exists(Properties.Settings.Default.FTHandHistoryFolder))
                Properties.Settings.Default.FTHandHistoryFolder = String.Empty;
        }

        private void setLabels()
        {
            this.labelFolderPS.Text = Properties.Settings.Default.PSHandHistoryFolder;
            this.labelFolderFT.Text = Properties.Settings.Default.FTHandHistoryFolder;
            this.textBoxDefaultStructure.Text = Properties.Settings.Default.DefaultStructure;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            readFromConfig();
            setLabels();
            setupEventHandler();
            textBoxHyperSatBuyinList.Lines = System.IO.File.ReadAllLines(pathManager.getHyperSatBuyinListPath());
            if (Properties.Settings.Default.PSHandHistoryFolder == String.Empty) showPSFolderBrowser();
        }

        private void setupEventHandler()
        {
            createEventHandler();
            registFolders();
        }

        private void createEventHandler()
        {
            watcher = new HandHistoryWatcher();
            watcher.NewHandHistory += new HandHistoryWatcher.NewHandHistoryEventHandler(this.newHandHistoryCreated);
        }

        private void registFolders()
        {
            if (Properties.Settings.Default.PSHandHistoryFolder != String.Empty)
                watcher.addFolder(Properties.Settings.Default.PSHandHistoryFolder, this);
            if (Properties.Settings.Default.FTHandHistoryFolder != String.Empty)
                watcher.addFolder(Properties.Settings.Default.FTHandHistoryFolder, this);
        }

        private void newHandHistoryCreated(object source, System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    syncHyperSatBuyinList();
                    System.Diagnostics.Process.Start(pathManager.getONCPath(), makeHandHistoryArgs(e.FullPath));
                    break;
                default:
                    break;
            }
        }

        private string makeHandHistoryArgs(string hhPath)
        {
            return "\"" + hhPath + "\"" + " \"" + this.textBoxDefaultStructure.Text.Trim() + "\""
                + " \"" + count % 10 * 40 + "," + count % 10 * 20 + "\"";
        }

        private void syncHyperSatBuyinList()
        {
            if (isChanged)
            {
                Monitor.Enter(syncObject);
                isChanged = false;
                System.IO.File.WriteAllText(pathManager.getHyperSatBuyinListPath(), textBoxHyperSatBuyinList.Text);
                Monitor.Exit(syncObject);
            }
        }

        private void ONCDaemonViewController_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            syncHyperSatBuyinList();
        }

        private void labelFolderPS_Click(object sender, EventArgs e)
        {
            showPSFolderBrowser();
        }

        private void showPSFolderBrowser()
        {

            string result = Microsoft.VisualBasic.Interaction.InputBox("ポーカースターズのハンドヒストリフォルダを指定してください", "ポーカースターズのハンドヒストリフォルダ", Properties.Settings.Default.PSHandHistoryFolder, this.Location.X + 20, this.Location.Y + 100);
            if (result == string.Empty) return;
            if (!System.IO.Directory.Exists(result)) {
                System.Windows.Forms.MessageBox.Show("指定されたディレクトリは存在しません");
                return;
            }
            Properties.Settings.Default.PSHandHistoryFolder = result;
            watcher.clear();
            registFolders();
            setLabels();
#if false
            if (PSFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.PSHandHistoryFolder = PSFolderBrowserDialog.SelectedPath;
                watcher.clear();
                registFolders();
                setLabels();
            }
#endif
        }

        private void textBoxHyperSatBuyinList_TextChanged(object sender, EventArgs e)
        {
            if (isChanged) return;
            Monitor.Enter(syncObject);
            isChanged = true;
            Monitor.Exit(syncObject);
        }
    }
}
