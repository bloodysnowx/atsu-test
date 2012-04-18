using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace OpenNashCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int level = 0;

        string[] SB   = { "10", "15", "20", "30", "40",  "50",  "60",  "75",  "90", "105", "125", "150", "175", "200", "225", "250" };
        string[] BB   = { "20", "30", "40", "60", "80", "100", "120", "150", "180", "210", "250", "300", "350", "400", "450", "500" };
        string[] Ante = {  "2",  "3",  "4",  "6",  "8",  "10",  "12",  "15",  "18",  "21",  "25",  "30",  "35",  "40",  "45",  "50" };

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxAnte.Text.Trim() == "")
                textBoxAnte.Text = "0";

            string URL = "http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&bb=";
            URL += textBoxBB.Text;
            URL += "&sb=" + textBoxSB.Text.Trim();
            URL += "&ante=" + textBoxAnte.Text.Trim();
            URL += "&structure=" + HttpUtility.UrlEncode(textBoxStructure.Text.Trim());
            URL += "&s1=" + textBox5.Text.Trim();
            URL += "&s2=" + textBox6.Text.Trim();
            URL += "&s3=" + textBox7.Text.Trim();
            URL += "&s4=" + textBox8.Text.Trim();
            URL += "&s5=" + textBox9.Text.Trim();
            URL += "&s6=" + textBox10.Text.Trim();
            URL += "&s7=" + textBox11.Text.Trim();
            URL += "&s8=" + textBox12.Text.Trim();
            URL += "&s9=" + textBox13.Text.Trim();
            // http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&
            // bb=200&sb=100&ante=0&structure=0.5%2C0.3%2C0.2&s1=100&s2=100&s3=100&s4=100&s5=100&s6=100&s7=100&s8=100&s9=100
            System.Diagnostics.Process.Start(URL);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tmp = textBox5.Text;
            textBox5.Text = textBox6.Text;
            textBox6.Text = textBox7.Text;
            textBox7.Text = textBox8.Text;
            textBox8.Text = textBox9.Text;
            textBox9.Text = textBox10.Text;
            textBox10.Text = textBox11.Text;
            textBox11.Text = textBox12.Text;
            textBox12.Text = textBox13.Text;
            textBox13.Text = tmp;

            bool tmp_check = checkBox1.Checked;
            checkBox1.Checked = checkBox2.Checked;
            checkBox2.Checked = checkBox3.Checked;
            checkBox3.Checked = checkBox4.Checked;
            checkBox4.Checked = checkBox5.Checked;
            checkBox5.Checked = checkBox6.Checked;
            checkBox6.Checked = checkBox7.Checked;
            checkBox7.Checked = checkBox8.Checked;
            checkBox8.Checked = checkBox9.Checked;
            checkBox9.Checked = tmp_check;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BBChange();
        }

        private void BBChange()
        {
            string tmp_str = textBox5.Text;
            textBox5.Text = textBox13.Text;
            textBox13.Text = textBox12.Text;
            textBox12.Text = textBox11.Text;
            textBox11.Text = textBox10.Text;
            textBox10.Text = textBox9.Text;
            textBox9.Text = textBox8.Text;
            textBox8.Text = textBox7.Text;
            textBox7.Text = textBox6.Text;
            textBox6.Text = tmp_str;

            bool tmp_check = checkBox1.Checked;
            checkBox1.Checked = checkBox9.Checked;
            checkBox9.Checked = checkBox8.Checked;
            checkBox8.Checked = checkBox7.Checked;
            checkBox7.Checked = checkBox6.Checked;
            checkBox6.Checked = checkBox5.Checked;
            checkBox5.Checked = checkBox4.Checked;
            checkBox4.Checked = checkBox3.Checked;
            checkBox3.Checked = checkBox2.Checked;
            checkBox2.Checked = tmp_check;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" &&
                textBox7.Text == "" &&
                textBox8.Text == "" &&
                textBox9.Text == "" &&
                textBox10.Text == "" &&
                textBox11.Text == "" &&
                textBox12.Text == "" &&
                textBox13.Text == "")
                return;

            while (textBox13.Text == "")
                BBChange();
            BBChange();
        }

        private void SetBBSBAnte()
        {
            textBoxBB.Text = BB[level];
            textBoxSB.Text = SB[level];
            textBoxAnte.Text = Ante[level];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BB = Properties.Settings.Default.BB.Split(',');
            SB = Properties.Settings.Default.SB.Split(',');
            Ante = Properties.Settings.Default.Ante.Split(',');

            if (BB.Length < 2 || SB.Length < 2 || Ante.Length < 2)
            {
                textBoxBB.Text = Properties.Settings.Default.BB;
                textBoxSB.Text = Properties.Settings.Default.SB;
                textBoxAnte.Text = Properties.Settings.Default.Ante;
                buttonBBDown.Enabled = false;
                buttonBBUP.Enabled = false;
            }
            else
            {
                SetBBSBAnte();
            }

            if (Properties.Settings.Default.PlayerNum > 8)
                textBox5.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 7)
                textBox6.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 6)
                textBox7.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 5)
                textBox8.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 4)
                textBox9.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 3)
                textBox10.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 2)
                textBox11.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 1)
                textBox12.Text = Properties.Settings.Default.StartingChip;
            if (Properties.Settings.Default.PlayerNum > 0)
                textBox13.Text = Properties.Settings.Default.StartingChip;
        }

        private void buttonBBUP_Click(object sender, EventArgs e)
        {
            if (level < BB.Length - 2)
                ++level;
            SetBBSBAnte();
        }

        private void buttonBBDown_Click(object sender, EventArgs e)
        {
            if (level > 0)
                --level;
            SetBBSBAnte();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSB.Enabled = !checkBox10.Checked;
        }

        private void textBoxBB_TextChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                int sb;
                try
                {
                    sb = System.Convert.ToInt32(textBoxBB.Text) / 2;
                }
                catch (System.FormatException)
                {
                    sb = 0;
                }

                textBoxSB.Text = sb.ToString();
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
