﻿using System;
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
        int bb_pos = 8;

        string[] SB   = { "10", "15", "20", "30", "40",  "50",  "60",  "75",  "90", "105", "125", "150", "175", "200", "225", "250" };
        string[] BB   = { "20", "30", "40", "60", "80", "100", "120", "150", "180", "210", "250", "300", "350", "400", "450", "500" };
        string[] Ante = {  "2",  "3",  "4",  "6",  "8",  "10",  "12",  "15",  "18",  "21",  "25",  "30",  "35",  "40",  "45",  "50" };
        string[] Position = { "BB", "SB", "BU", "CO", "MP+2", "MP+1", "MP", "UTG+1", "UTG" };
        RadioButton[] positionRadioButtons;
        TextBox[] chipTextBoxes;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxAnte.Text.Trim() == "")
                textBoxAnte.Text = "0";

            string URL = "http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&bb=";
            URL += textBoxBB.Text;
            URL += "&sb=" + textBoxSB.Text.Trim();
            URL += "&ante=" + textBoxAnte.Text.Trim();
            URL += "&structure=" + HttpUtility.UrlEncode(textBoxStructure.Text.Trim());
            URL += "&s1=" + chipTextBoxes[(bb_pos + 1) % 9].Text.Trim();
            URL += "&s2=" + chipTextBoxes[(bb_pos + 2) % 9].Text.Trim();
            URL += "&s3=" + chipTextBoxes[(bb_pos + 3) % 9].Text.Trim();
            URL += "&s4=" + chipTextBoxes[(bb_pos + 4) % 9].Text.Trim();
            URL += "&s5=" + chipTextBoxes[(bb_pos + 5) % 9].Text.Trim();
            URL += "&s6=" + chipTextBoxes[(bb_pos + 6) % 9].Text.Trim();
            URL += "&s7=" + chipTextBoxes[(bb_pos + 7) % 9].Text.Trim();
            URL += "&s8=" + chipTextBoxes[(bb_pos + 8) % 9].Text.Trim();
            URL += "&s9=" + chipTextBoxes[(bb_pos + 9) % 9].Text.Trim();
            // http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&
            // bb=200&sb=100&ante=0&structure=0.5%2C0.3%2C0.2&s1=100&s2=100&s3=100&s4=100&s5=100&s6=100&s7=100&s8=100&s9=100
            System.Diagnostics.Process.Start(URL);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tmp = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = textBox3.Text;
            textBox3.Text = textBox4.Text;
            textBox4.Text = textBox5.Text;
            textBox5.Text = textBox6.Text;
            textBox6.Text = textBox7.Text;
            textBox7.Text = textBox8.Text;
            textBox8.Text = textBox9.Text;
            textBox9.Text = tmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BBChange();
        }

        private void BBChange()
        {
            string tmp_str = textBox1.Text;
            textBox1.Text = textBox9.Text;
            textBox9.Text = textBox8.Text;
            textBox8.Text = textBox7.Text;
            textBox7.Text = textBox6.Text;
            textBox6.Text = textBox5.Text;
            textBox5.Text = textBox4.Text;
            textBox4.Text = textBox3.Text;
            textBox3.Text = textBox2.Text;
            textBox2.Text = tmp_str;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" &&
                textBox3.Text == "" &&
                textBox4.Text == "" &&
                textBox5.Text == "" &&
                textBox6.Text == "" &&
                textBox7.Text == "" &&
                textBox8.Text == "" &&
                textBox9.Text == "")
                return;

            while (textBox9.Text == "")
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

            chipTextBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4,
                textBox5, textBox6, textBox7, textBox8, textBox9 };

            for (int i = 0; i < 9; ++i)
            {
                if(Properties.Settings.Default.PlayerNum - i > 0)
                    chipTextBoxes[8 - i].Text = Properties.Settings.Default.StartingChip;
            }

            positionRadioButtons = new RadioButton[] { radioButton1, radioButton2, radioButton3,
                radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };

            EnabledPositionRadioButton();
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

        private void EnabledPositionRadioButton()
        {
            for (int i = 0; i < 9; ++i)
            {
                if (chipTextBoxes[i].Text.Trim() == "")
                    positionRadioButtons[i].Enabled = false;
                else
                    positionRadioButtons[i].Enabled = true;
            }
        }

        private void SetPosition()
        {
            for (int i = 0; i < 9; ++i)
            {
                if (positionRadioButtons[i].Checked == true)
                    bb_pos = i;
            }

            EnabledPositionRadioButton();

            int label = 0;

            positionRadioButtons[bb_pos].Text = Position[label++];
            for(int i = 8; i > 0; --i)
            {
                if (positionRadioButtons[(bb_pos + i) % 9].Enabled)
                    positionRadioButtons[(bb_pos + i) % 9].Text = Position[label++];
                else
                    positionRadioButtons[(bb_pos + i) % 9].Text = "";
            }
        }

        private void postionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPosition();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            SetPosition();
        }
    }
}