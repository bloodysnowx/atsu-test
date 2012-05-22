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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HandHistoryFolderBrowserDialog.SelectedPath = Properties.Settings.Default.HandHistoryFolder;
            System.Diagnostics.Process.Start("./OpenNashCalculator.exe", "\"D:\\src\\atsu-test\\poker\\HH20120418 T546368001 No Limit Hold'em $3 + $0.30.txt\"");
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (HandHistoryFolderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }
    }
}
