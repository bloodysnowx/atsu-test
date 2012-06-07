using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;

using System.Text.RegularExpressions;

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
        string[] Seat = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        RadioButton[] positionRadioButtons;
        TextBox[] chipTextBoxes;
        Label[] SeatLabels;
        Button[] ClearButtons;
        DateTime updateDate;
        TextBox[] rangeTextBoxes;
        Label[] PlayerNameLabels;
        string tourney_ID;
        CheckBox[] AllinCheckBoxes;
        bool close_flg = false;
        string recent_web_page;

        int retry_num = 0;

        string currentSB;

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
            for (j = 8; j > 0 && label < 4; --j)
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

        private int getHeroNum()
        {
            int hero_num = 0;
            for (int i = 0; i < 9; ++i)
                if (SeatLabels[i].Text == "H") hero_num = i;
            return hero_num;
        }

        private void SetBBSBAnte()
        {
            textBoxBB.Text = BB[level];
            currentSB = SB[level];
            textBoxAnte.Text = Ante[level];
        }

        private void Reset()
        {
            level = Properties.Settings.Default.DefaultLevel - 1;
            SetBBSBAnte();

            textBoxStructure.Text = Properties.Settings.Default.DefaultStructure;

            for (int i = 0; i < 9; ++i)
            {
                if (Properties.Settings.Default.PlayerNum - i > 0)
                    chipTextBoxes[8 - i].Text = Properties.Settings.Default.StartingChip;
            }

            SetPosition();
        }

        private void Calc()
        {
            if (textBoxAnte.Text.Trim() == "")
                textBoxAnte.Text = "0";

            string URL = "http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&bb=";
            URL += textBoxBB.Text;
            URL += "&sb=" + currentSB.Trim();
            URL += "&ante=" + textBoxAnte.Text.Trim();
            textBoxStructure.Text = textBoxStructure.Text.Replace('+', ',').Replace(' ', ',');
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

            foreach (TextBox rangeTextBox in rangeTextBoxes)
                rangeTextBox.Text = "";

            System.Net.WebClient client = new System.Net.WebClient();

            int hero_num = getHeroNum();
            string hero_pos = "";
            hero_pos = positionRadioButtons[hero_num].Text;

            recent_web_page = client.DownloadString(URL);
            Regex regex = new Regex("<td>" + Regex.Escape(hero_pos) + "</td><td /><td>(.*?)</td>");
            MatchCollection matchCol = regex.Matches(recent_web_page);
            int callRangeCount = matchCol.Count - 1;

            for (int i = 1; i < 9 && callRangeCount >= 0; ++i)
            {
                if (positionRadioButtons[(hero_num - i + 9) % 9].Enabled)
                {
                    rangeTextBoxes[(hero_num - i + 9) % 9].Text = matchCol[callRangeCount--].Groups[1].Value;
                }
            }

#if false
            if (hero_pos == Position[0])
            {
                rangeTextBoxes[(hero_num + 1) % 9].Text = matchCol[callRangeCount--].Groups[1].Value;
            }
#endif

            regex = new Regex("<td>" + Regex.Escape(hero_pos) + "</td><td /><td /><td>(.*?)</td>");
            matchCol = regex.Matches(recent_web_page);
            string pushRange = "";
            if (matchCol.Count > 0) pushRange = matchCol[0].Groups[1].Value;

            rangeTextBoxes[hero_num].Text = pushRange;

            if (checkBoxWeb.Checked == false)
                System.Diagnostics.Process.Start(URL);
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxBB.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxAnte.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxBB.Click += new System.EventHandler(textBox_Click);
            textBoxAnte.Click += new System.EventHandler(textBox_Click);
            textBoxStructure.Click += new System.EventHandler(textBox_Click);
            labelChips.ContextMenuStrip = chipContextMenuStrip;

            if (Properties.Settings.Default.BB.Split(',').Length < 2 ||
                Properties.Settings.Default.SB.Split(',').Length < 2 ||
                Properties.Settings.Default.Ante.Split(',').Length < 2)
            {
                MessageBox.Show("Please config BB, SS, Ante");
            }
            else
            {
                BB = Properties.Settings.Default.BB.Split(',');
                SB = Properties.Settings.Default.SB.Split(',');
                Ante = Properties.Settings.Default.Ante.Split(',');
            }

            PlayerNameLabels = new Label[] { label10, label11, label12, label13, label14,
                label15, label16, label17, label18 };

            chipTextBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4,
                textBox5, textBox6, textBox7, textBox8, textBox9 };
            foreach (TextBox chipTextBox in chipTextBoxes)
            {
                chipTextBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ChipMouseWheel);
                chipTextBox.Click +=  new System.EventHandler(textBox_Click);
                chipTextBox.DoubleClick += new System.EventHandler(chipTextBoxes_DoubleClick);
                chipTextBox.ContextMenuStrip = chipContextMenuStrip;
            }

            positionRadioButtons = new RadioButton[] { radioButton1, radioButton2, radioButton3,
                radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };
            foreach (RadioButton positionRadioButton in positionRadioButtons)
                positionRadioButton.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);

            SeatLabels = new Label[] { label1, label2, label3, label4, label5,
              label6, label7, label8, label9 };
            foreach (Label seatLabel in SeatLabels)
                seatLabel.Click += new System.EventHandler(this.SeatLabel_DoubleClick);
            ClearButtons = new Button[] { button2, button3, button4, button5, button6, button7, button8, button9, button10 };

            rangeTextBoxes = new TextBox[] { textBoxRange1, textBoxRange2, textBoxRange3, textBoxRange4, textBoxRange5,
                textBoxRange6, textBoxRange7, textBoxRange8, textBoxRange9 };

            chipTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
            rangeTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);

            AllinCheckBoxes = new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, 
                checkBox7, checkBox8, checkBox9 };
            foreach (CheckBox checkBox in AllinCheckBoxes)
                checkBox.CheckedChanged += new System.EventHandler(this.AllinCheckBox_CheckedChanged);

            Reset();

            string[] args = System.Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                openHandHistoryDialog.FileName = args[1];
                tourney_ID = getTourneyID(openHandHistoryDialog.FileName);
                checkBoxClose.Checked = true;
                openHandHistory();
                FindWindow();
            }
        }

        private string getTourneyID(string fileName)
        {
            Regex regex = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T([0-9]+)" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
                + "Limit" + Regex.Escape(" ") + "Hold");
            MatchCollection matchCol = regex.Matches(fileName);
            string tourney = matchCol[0].Groups[1].Value;
            return tourney;
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

        private void textBoxBB_TextChanged(object sender, EventArgs e)
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

            currentSB = sb.ToString();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void postionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPosition();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            SetPosition();
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
                digit = System.Convert.ToInt32(currentSB);
                int chip = 0;
                System.Int32.TryParse(((TextBox)sender).Text, out chip);

                if (chip == 0 && numberOfTextLinesToMove < 0)
                {
                    ((TextBox)sender).Text = "";
                }
                else
                {
                    int val = System.Math.Max(chip + digit * numberOfTextLinesToMove, 0);
                    ((TextBox)sender).Text = System.Convert.ToString(val);
                }
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

        private void SetHeroSeat(Label sender)
        {
            for (int i = 0; i < 9; ++i)
            {
                SeatLabels[i].Text = Seat[i];
                SeatLabels[i].Font = new Font("MS UI Gothic", 9);

                if (sender == SeatLabels[i])
                {
                    chipTextBoxes[i].BackColor = rangeTextBoxes[i].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
                }
                else
                {
                    chipTextBoxes[i].BackColor = rangeTextBoxes[i].BackColor = Color.White;
                }
            }

            sender.Text = "H";
            sender.Font = new Font("MS UI Gothic", 9, FontStyle.Bold);
        }

        private void SeatLabel_DoubleClick(object sender, EventArgs e)
        {
            SetHeroSeat((Label)sender);
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void chipTextBoxes_DoubleClick(object sender, EventArgs e)
        {
            int chips = 0;
            Int32.TryParse(((TextBox)sender).Text, out chips);
            chips += Properties.Settings.Default.AddonChip;
            ((TextBox)sender).Text = chips.ToString();

            textBoxBB.Text = Properties.Settings.Default.AddonBB.ToString();
            textBoxAnte.Text = Properties.Settings.Default.AddonAnte.ToString();
        }

        private void labelChips_DoubleClick(object sender, EventArgs e)
        {
            /*
            int hero_num = getHeroNum();
            string hero_chips = chipTextBoxes[hero_num].Text;
            for (int i = 0; i < 9; ++i)
                if(positionRadioButtons[i].Enabled) chipTextBoxes[i].Text = hero_chips;
            */
            int chips = 0;

            foreach (TextBox chipTextBox in chipTextBoxes)
            {
                if (Int32.TryParse(chipTextBox.Text, out chips))
                {
                    chips += Properties.Settings.Default.AddonChip;
                    chipTextBox.Text = chips.ToString();
                }
            }

            textBoxBB.Text = Properties.Settings.Default.AddonBB.ToString();
            textBoxAnte.Text = Properties.Settings.Default.AddonAnte.ToString();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void ReadHandHistoryWithRetry()
        {
            for (retry_num = 0; retry_num < 3; ++retry_num)
            {
                try
                {
                    ReadHandHistory();
                    retry_num = 0;
                    break;
                }
                catch
                {
                    updateDate = new DateTime();
                    System.Threading.Thread.Sleep(10);
                }
            }

            if (retry_num > 0)
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
                MessageBox.Show(System.IO.Path.GetFileName(openHandHistoryDialog.FileName) + " may be broken."
                    + System.Environment.NewLine + "This program cannot read this hand history."
                    + System.Environment.NewLine + "Please choose another file.");
            }
            else
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = true;
            }
        }

        private void openHandHistory()
        {
            startingChip = 0;
            updateDate = new DateTime();

            this.Text = System.IO.Path.GetFileName(openHandHistoryDialog.FileName).Split('.')[0];
            this.Text = this.Text.Substring(this.Text.IndexOf('T'));

            ReadHandHistoryWithRetry();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openHandHistoryDialog.ShowDialog() != DialogResult.OK)
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
                return;
            }

            tourney_ID = getTourneyID(openHandHistoryDialog.FileName);
            openHandHistory();
        }

        private void checkBoxRefresh_CheckedChanged(object sender, EventArgs e)
        {
            refreshTimer.Enabled = ((CheckBox)sender).Checked;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            FindTournamentWindow();

            try
            {
                ReadHandHistory();
                retry_num = 0;
            }
            catch
            {
                updateDate = new DateTime();
                retry_num++;
            }

            if (retry_num > 3)
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
                MessageBox.Show(System.IO.Path.GetFileName(openHandHistoryDialog.FileName) + " may be broken."
                    + System.Environment.NewLine + "This program cannot read this hand history."
                    + System.Environment.NewLine + "Please choose another file.");
            }
        }

        private void chipContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Control source = (sender as ContextMenuStrip).SourceControl;
            TextBox[] textBoxes = chipTextBoxes;
            if (source.GetType() == typeof(TextBox))
            {
                textBoxes = new TextBox[] { source as TextBox };
            }
            foreach (TextBox textBox in textBoxes)
            {
                int chips = 0;
                if (Int32.TryParse(textBox.Text, out chips) == false)
                    continue;

                switch (e.ClickedItem.Name)
                {
                    case "Addon30kToolStripMenuItem":
                        chips += 30000;
                        break;
                    case "Addon50kToolStripMenuItem":
                        chips += 50000;
                        break;
                    default:
                        break;
                }
                textBox.Text = chips.ToString();
            }
            textBoxBB.Text = Properties.Settings.Default.AddonBB.ToString();
            textBoxAnte.Text = Properties.Settings.Default.AddonAnte.ToString();
        }

        private void buttonReread_Click(object sender, EventArgs e)
        {
            if (openHandHistoryDialog.FileName != String.Empty)
            {
                openHandHistory();
            }
        }

        private void AllinCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int count = 0;
            foreach (CheckBox checkBox in AllinCheckBoxes)
            {
                if (checkBox.Checked == true)
                    count++;
            }

            if(count == 2)
                Help.ShowPopup(this, "こんにちは。", Control.MousePosition);
        }

#if false
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
#endif
    }
}
