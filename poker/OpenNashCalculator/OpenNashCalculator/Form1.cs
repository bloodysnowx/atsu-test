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

        private int level = 0;
        public int Level
        {
            get { return level; }
            set
            {
                if (value < 0) level = 0;
                else
                {
                    level = System.Math.Min(value, BB.Length - 1);
                    level = System.Math.Min(level, SB.Length - 1);
                    level = System.Math.Min(level, Ante.Length - 1);
                    SetBBSBAnte();
                }
            }

        }
        int bb_pos = 8;

        string[] SB   = { "10", "15", "20", "30", "40",  "50",  "60",  "75",  "90", "105", "125", "150", "175", "200", "225", "250" };
        string[] BB   = { "20", "30", "40", "60", "80", "100", "120", "150", "180", "210", "250", "300", "350", "400", "450", "500" };
        string[] Ante = {  "2",  "3",  "4",  "6",  "8",  "10",  "12",  "15",  "18",  "21",  "25",  "30",  "35",  "40",  "45",  "50" };
        string[] Position = { "BB", "SB", "BU", "CO", "UTG+4", "UTG+3", "UTG+2", "UTG+1", "UTG" };
        string[] Seat = { "Seat1", "Seat2", "Seat3", "Seat4", "Seat5", "Seat6", "Seat7", "Seat8", "Seat9" };
        RadioButton[] positionRadioButtons;
        TextBox[] chipTextBoxes;
        Label[] ICMLabels;
        Label[] SeatLabels;
        Button[] ClearButtons;

        private void buttonCalc_Click(object sender, EventArgs e)
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

        private void SetBBSBAnte()
        {
            textBoxBB.Text = BB[level];
            textBoxSB.Text = SB[level];
            textBoxAnte.Text = Ante[level];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxBB.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxSB.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxAnte.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);

            level = Properties.Settings.Default.DefaultLevel - 1;
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

            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ChipMouseWheel);

            for (int i = 0; i < 9; ++i)
            {
                if(Properties.Settings.Default.PlayerNum - i > 0)
                    chipTextBoxes[8 - i].Text = Properties.Settings.Default.StartingChip;
            }

            positionRadioButtons = new RadioButton[] { radioButton1, radioButton2, radioButton3,
                radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };

            ICMLabels = new Label[] { labelICM0, labelICM1, labelICM2, labelICM3, labelICM4, labelICM5,
                labelICM6, labelICM7, labelICM8 };
            SeatLabels = new Label[] { label7, label8, label9, label10, label11,
              label12, label13, label14, label15 };
            foreach (Label seatLabel in SeatLabels)
                seatLabel.DoubleClick += new System.EventHandler(this.SeatLabel_DoubleClick);
            ClearButtons = new Button[] { button2, button3, button4, button5, button6, button7, button8, button9, button10 };

            EnabledPositionRadioButton();
        }

        private void buttonBBUP_Click(object sender, EventArgs e)
        {
            if (level < BB.Length - 1)
                ++level;
            SetBBSBAnte();
        }

        private void buttonBBDown_Click(object sender, EventArgs e)
        {
            if (level > 0)
                --level;
            SetBBSBAnte();
        }

        private void checkBoxGearSB_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSB.Enabled = !checkBoxGearSB.Checked;
        }

        private void textBoxBB_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxGearSB.Checked)
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
                {
                    positionRadioButtons[i].Enabled = false;
                    SeatLabels[i].Enabled = false;
                }
                else
                {
                    positionRadioButtons[i].Enabled = true;
                    SeatLabels[i].Enabled = true;
                }
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
            int j;
            positionRadioButtons[bb_pos].Text = Position[label++];
            for(j = 8; j > 0　&& label < 4; --j)
            {
                if (positionRadioButtons[(bb_pos + j) % 9].Enabled)
                    positionRadioButtons[(bb_pos + j) % 9].Text = Position[label++];
                else
                    positionRadioButtons[(bb_pos + j) % 9].Text = "";
            }

            label = 8;
            for (int i = 1; i <= j; ++i)
            {
                if (positionRadioButtons[(bb_pos + i) % 9].Enabled)
                    positionRadioButtons[(bb_pos + i) % 9].Text = Position[label--];
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
            if (checkBoxICM.Checked) CalcICM();
        }

        private void CalcICM()
        {
            ICMCalculator.ICMCalculator calculator = new ICMCalculator.ICMCalculator();

            int[] chips = new int[9];

            string[] structure_str = textBoxStructure.Text.Split(',');
            double[] structure = new double[structure_str.Length];

            for (int i = 0; i < 9; ++i)
            {
                if (structure_str.Length > i)
                    System.Double.TryParse(structure_str[i], out structure[i]);

                System.Int32.TryParse(chipTextBoxes[i].Text, out chips[i]);
            }

            double[] EV = calculator.CalcEV(structure, chips);

            for (int i = 0; i < 9; ++i)
                ICMLabels[i].Text = (EV[i] * chips.Sum() / structure.Sum()).ToString("f2");
        }

        private void checkBoxICM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxICM.Checked) CalcICM();
        }

        private void BBMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int numberOfTextLinesToMove = e.Delta / 120;
            Level = Level + numberOfTextLinesToMove;
        }

        private void ChipMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int numberOfTextLinesToMove = e.Delta / 120;

            try
            {
                int digit = 0;
                System.Int32.TryParse(textBoxAnte.Text, out digit);
                if (digit == 0)
                    digit = System.Convert.ToInt32(textBoxSB.Text);

                int val = System.Math.Max(System.Convert.ToInt32(((TextBox)sender).Text) + digit * numberOfTextLinesToMove, 0);
                ((TextBox)sender).Text = System.Convert.ToString(val);
            }
            catch (FormatException)
            {
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; ++i)
            {
                if (sender == ClearButtons[i])
                    chipTextBoxes[i].Clear();
            }
            SetPosition();
        }

        private void SeatLabel_DoubleClick(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; ++i)
            {
                SeatLabels[i].Text = Seat[i];
                SeatLabels[i].Font = new Font("MS UI Gothic", 9);
            }

            ((Label)sender).Text = "Hero";
            ((Label)sender).Font = new Font("MS UI Gothic", 9, FontStyle.Bold);
        }
        
    }
}
