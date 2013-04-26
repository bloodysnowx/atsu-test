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
    public partial class OpenNashCalculatorViewController : Form
    {
        const int MAX_SEAT_NUM = 9;

        public OpenNashCalculatorViewController()
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
        string[] Position = { "BB", "SB", "BU", "CO", /*"UTG+5",*/ "UTG+4", "UTG+3", "UTG+2", "UTG+1", "UTG" };
        string[] Seat = { "1", "2", "3", "4", "5", "6", "7", "8", "9", /*"10"*/ };
        RadioButton[] positionRadioButtons;
        TextBox[] chipTextBoxes;
        Label[] SeatLabels;
        Button[] ClearButtons;
        DateTime updateDate;
        TextBox[] rangeTextBoxes;
        Label[] PlayerNameLabels;
        string tourney_ID = "xxxxxxxxxxxxxxxxxxxxx";
        CheckBox[] AllinCheckBoxes;
        // bool close_flg = false;
        string recent_web_page;
        string last_web_page = "";
        // RECT tourneyWindowRect = new RECT();
        int hero_num;
        string hero_pos;
        int hh_back_num = 0;
        int retry_num = 0;
        string currentSB;
        string encryptedUserName = "";
        string defaultStructure = "";
        Point formOrigin = new Point(0, 0);
        IEnumerable<string> hyperSatBuyinList;
        IEnumerable<int> indexes = Enumerable.Range(0, MAX_SEAT_NUM);

        private void EnabledPositionRadioButton()
        {
            for (int i = 0; i < 9; ++i)
            {
                if (chipTextBoxes[i].Text.Trim() == "")
                {
                    positionRadioButtons[i].Enabled = false;
                    SeatLabels[i].Enabled = false;
                    AllinCheckBoxes[i].Checked = AllinCheckBoxes[i].Enabled = false;
                }
                else
                {
                    positionRadioButtons[i].Enabled = true;
                    SeatLabels[i].Enabled = true;
                    AllinCheckBoxes[i].Enabled = true;
                }
            }
        }

        private void SetPosition()
        {
            bb_pos = indexes.First(i => positionRadioButtons[i].Checked);

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
            return indexes.First(i => SeatLabels[i].Text == "H");
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

            textBoxStructure.Text = defaultStructure;

            foreach (var i in indexes.Where(i => Properties.Settings.Default.PlayerNum - i > 0))
            {
                chipTextBoxes[8 - i].Text = Properties.Settings.Default.StartingChip;
            }

            SetPosition();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void setupEventHandler()
        {
            textBoxBB.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxAnte.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BBMouseWheel);
            textBoxBB.Click += new System.EventHandler(textBox_Click);
            textBoxAnte.Click += new System.EventHandler(textBox_Click);
            // textBoxStructure.Click += new System.EventHandler(textBox_Click);
            foreach (TextBox chipTextBox in chipTextBoxes)
            {
                chipTextBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ChipMouseWheel);
                chipTextBox.Click += new System.EventHandler(textBox_Click);
                chipTextBox.DoubleClick += new System.EventHandler(chipTextBoxes_DoubleClick);
            }
            foreach (RadioButton positionRadioButton in positionRadioButtons)
                positionRadioButton.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            foreach (Label seatLabel in SeatLabels)
                seatLabel.Click += new System.EventHandler(this.SeatLabel_DoubleClick);
            foreach (CheckBox checkBox in AllinCheckBoxes)
                checkBox.CheckedChanged += new System.EventHandler(this.AllinCheckBox_CheckedChanged);
        }

        private bool checkConfig()
        {
            return Properties.Settings.Default.BB.Split(',').Length >= 2
                && Properties.Settings.Default.SB.Split(',').Length >= 2
                && Properties.Settings.Default.Ante.Split(',').Length >= 2;
        }

        private void readFromConfig()
        {
            BB = Properties.Settings.Default.BB.Split(',');
            SB = Properties.Settings.Default.SB.Split(',');
            Ante = Properties.Settings.Default.Ante.Split(',');
            defaultStructure = Properties.Settings.Default.DefaultStructure;
            hyperSatBuyinList = readHyperSatBuyinList(System.IO.Directory.GetCurrentDirectory() + "\\" + Properties.Settings.Default.HyperSatBuyinListName);
        }

        private void setViewsToArray()
        {
            PlayerNameLabels = new Label[] { label10, label11, label12, label13, label14,
                label15, label16, label17, label18 };
            chipTextBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4,
                textBox5, textBox6, textBox7, textBox8, textBox9 };
            positionRadioButtons = new RadioButton[] { radioButton1, radioButton2, radioButton3,
                radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };
            SeatLabels = new Label[] { label1, label2, label3, label4, label5,
              label6, label7, label8, label9 };
            ClearButtons = new Button[] { button2, button3, button4, button5, button6, button7, button8, button9, button10 };
            rangeTextBoxes = new TextBox[] { textBoxRange1, textBoxRange2, textBoxRange3, textBoxRange4, textBoxRange5,
                textBoxRange6, textBoxRange7, textBoxRange8, textBoxRange9 };
            AllinCheckBoxes = new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, 
                checkBox7, checkBox8, checkBox9 };
        }

        private void setupViews()
        {
            labelChips.ContextMenuStrip = chipContextMenuStrip;
            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.ContextMenuStrip = chipContextMenuStrip;
            chipTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
            rangeTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            setViewsToArray();
            setupEventHandler();
            setupViews();

            if (checkConfig() == false)
                MessageBox.Show("Please config BB, SS, Ante");
            else
                readFromConfig();

            string[] args = System.Environment.GetCommandLineArgs();
            readArgs(args);
            Reset();
            this.SetDesktopLocation(formOrigin.X, formOrigin.Y);
            encryptedUserName = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\" + Properties.Settings.Default.WhiteListNme);
            if (isRunFromDaemon(args)) openHandHistory();
        }

        private IEnumerable<string> readHyperSatBuyinList(string path)
        {
            if(!System.IO.File.Exists(path)) yield break;
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
                yield return line.Trim();
        }

        private bool isRunFromDaemon(string[] args)
        {
            return args.Length > 1;
        }

        private void readArgs(string[] args)
        {
            if (isRunFromDaemon(args))
            {
                openHandHistoryDialog.FileName = args[1];
                this.reader = HandHistoryReaderFactory.create(openHandHistoryDialog.FileName);
                tourney_ID = reader.getTourneyID(openHandHistoryDialog.FileName);
#if !DEBUG
                checkBoxClose.Checked = true;
#endif
                // FindTournamentWindow();
                // GoBack();
                defaultStructure = args[2];
                string[] formOriginStr = args[3].Split(',');
                formOrigin = new Point(System.Int32.Parse(formOriginStr[0]), System.Int32.Parse(formOriginStr[1]));
            }
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
            chipTextBoxes[indexes.First(i => sender == ClearButtons[i])].Clear();
            SetPosition();
        }

        private void resetSeats()
        {
            for (int i = 0; i < MAX_SEAT_NUM; ++i)
            {
                SeatLabels[i].Text = Seat[i];
                SeatLabels[i].Font = new Font("MS UI Gothic", 9);
                chipTextBoxes[i].BackColor = rangeTextBoxes[i].BackColor = Color.White;
            }
        }

        private void SetHeroSeat(Label sender)
        {
            resetSeats();
            int index = indexes.First(i => sender == SeatLabels[i]);
            chipTextBoxes[index].BackColor = rangeTextBoxes[index].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
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
                    hh_back_num = 0;
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
                reportHandHistoryReadError();
            }
            else
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = true;
            }
        }

        private void reportHandHistoryReadError()
        {
            checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
            MessageBox.Show(System.IO.Path.GetFileName(openHandHistoryDialog.FileName) + " may be broken."
                + System.Environment.NewLine + "This program cannot read this hand history."
                + System.Environment.NewLine + "Please choose another file.");
        }

        private void openHandHistory()
        {
            updateDate = new DateTime();
            this.Text = tourney_ID.Substring(tourney_ID.Length - 4);

            ReadHandHistoryWithRetry();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openHandHistoryDialog.ShowDialog() != DialogResult.OK)
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
                return;
            }
            this.reader = HandHistoryReaderFactory.create(openHandHistoryDialog.FileName);
            this.Text = tourney_ID.Substring(tourney_ID.Length - 4);

            // FindTournamentWindow();
            // GoBack();
            openHandHistory();
        }

        private void checkBoxRefresh_CheckedChanged(object sender, EventArgs e)
        {
            refreshTimer.Enabled = ((CheckBox)sender).Checked;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                hh_back_num = 0;
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
                reportHandHistoryReadError();
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
            string push_pos = string.Empty;
            string oc_pos = string.Empty;
            string hero_pos = positionRadioButtons[getHeroNum()].Text;

            foreach (var i in indexes.Where(i => AllinCheckBoxes[i].Checked))
            {
                if (count == 0) push_pos = positionRadioButtons[i].Text;
                else if (count == 1)
                {
                    oc_pos = positionRadioButtons[i].Text;
                    if (Position.ToList().IndexOf(push_pos) < Position.ToList().IndexOf(oc_pos))
                    {
                        oc_pos = push_pos;
                        push_pos = positionRadioButtons[i].Text;
                    }
                }
                count++;
            }

            if (count == 2 && Position.ToList().IndexOf(hero_pos) < Position.ToList().IndexOf(oc_pos))
            {
                string tmp = recent_web_page.Substring(recent_web_page.IndexOf("</TR>\r\n<TR>\r\n<TD>" + push_pos + "</TD>\r\n<TD>\r\n<TD>\r\n<TD>"));
                tmp = tmp.Substring(tmp.IndexOf("</TR>\r\n<TR>\r\n<TD>\r\n<TD>" + oc_pos + "</TD>\r\n<TD>\r\n<TD>"));
                Regex regex = new Regex("</TR>\r\n<TR>\r\n<TD>\r\n<TD>\r\n<TD>" + Regex.Escape(hero_pos) + "</TD>\r\n<TD>(.*?)</TD></TR>");
                MatchCollection matchCol = regex.Matches(tmp);
                Help.ShowPopup(this, matchCol[0].Groups[1].Value, Control.MousePosition);
            }
        }

        private void findTimer_Tick(object sender, EventArgs e)
        {
            // FindTournamentWindow();
        }

        private void checkBoxClose_CheckedChanged(object sender, EventArgs e)
        {
            findTimer.Enabled = (sender as CheckBox).Checked;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            ++hh_back_num;
            checkBoxRefresh.Checked = false;
            ReadHandHistory();
        }

        private void button_fore_Click(object sender, EventArgs e)
        {
            if(hh_back_num > 0) --hh_back_num;
            ReadHandHistory();
        }

        private Point mousePoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.SetDesktopLocation(this.Left - mousePoint.X + e.X, this.Top - mousePoint.Y + e.Y);
            }
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
