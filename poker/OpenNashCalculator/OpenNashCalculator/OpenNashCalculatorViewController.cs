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
        Label[] ICMLabels;
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
        }

        private void setViewsToArray()
        {
            PlayerNameLabels = new Label[] { label10, label11, label12, label13, label14,
                label15, label16, label17, label18 };
            chipTextBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4,
                textBox5, textBox6, textBox7, textBox8, textBox9 };
            positionRadioButtons = new RadioButton[] { radioButton1, radioButton2, radioButton3,
                radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };
            SeatLabels = new Label[] { labelSeat1, labelSeat2, labelSeat3, labelSeat4, labelSeat5,
              labelSeat6, labelSeat7, labelSeat8, labelSeat9 };
            ClearButtons = new Button[] { button2, button3, button4, button5, button6, button7, button8, button9, button10 };
            rangeTextBoxes = new TextBox[] { textBoxRange1, textBoxRange2, textBoxRange3, textBoxRange4, textBoxRange5,
                textBoxRange6, textBoxRange7, textBoxRange8, textBoxRange9 };
            AllinCheckBoxes = new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, 
                checkBox7, checkBox8, checkBox9 };
            ICMLabels = new Label[] { labelICM1, labelICM2, labelICM3, labelICM4, labelICM5, labelICM6,
                labelICM7, labelICM8, labelICM9 };
        }

        private void setupViews()
        {
            labelChips.ContextMenuStrip = chipContextMenuStrip;
            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.ContextMenuStrip = chipContextMenuStrip;
            chipTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
            rangeTextBoxes[4].BackColor = Color.FromArgb(0xc3, 0xff, 0x4c);
            this.Size = new Size(this.Size.Width - 38, this.Size.Height);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            var timeLimit = new DateTime(2015, 7, 31);
            if (timeLimit < DateTime.Now)
            {
                System.Windows.Forms.MessageBox.Show("試用期間が終了しました");
                Application.Exit();
            }

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
            if(!System.IO.File.Exists(path)) return new string[0];
            string[] lines = System.IO.File.ReadAllLines(path);
            return lines;

            /*
            foreach (string line in lines)
                yield return line.Trim();
            */
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
                this.reader.setHyperSatBuyinList(readHyperSatBuyinList(System.IO.Directory.GetCurrentDirectory() + "\\" + Properties.Settings.Default.HyperSatBuyinListName));
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

        private void setBBLevel()
        {
            for (int i = BB.Length - 1; i >= 0; --i)
            {
                level = i;
                if (System.Convert.ToInt32(BB[i]) <= System.Convert.ToInt32(textBoxBB.Text))
                {
                    return;
                }
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

        private void setReaderFromFileName()
        {
            this.reader = HandHistoryReaderFactory.create(openHandHistoryDialog.FileName);
            this.reader.setHyperSatBuyinList(readHyperSatBuyinList(System.IO.Directory.GetCurrentDirectory() + "\\" + Properties.Settings.Default.HyperSatBuyinListName));
            this.Text = tourney_ID.Substring(tourney_ID.Length - 4);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openHandHistoryDialog.ShowDialog() != DialogResult.OK)
            {
                checkBoxRefresh.Enabled = checkBoxRefresh.Checked = false;
                return;
            }
            setReaderFromFileName();
            hh_back_num = 0;

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
            switch (e.ClickedItem.Name)
            {
                case "Addon30kToolStripMenuItem":
                    addonAll(30000);
                    break;
                case "Addon50kToolStripMenuItem":
                    addonAll(50000);
                    break;
                default:
                    break;
            }
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
                if (resultXML != null)
                {
                    int pushCount = 0;
                    int ocCount = 1;
                    int heroCount = 2;
                    var spotPct = resultXML.SelectSingleNode(@"//spot[@pu=" + pushCount.ToString() + "][@ca=" + ocCount.ToString() +
                        "][@oc=" + heroCount.ToString() + "]/rangepct");
                    var spotRange = resultXML.SelectSingleNode(@"//spot[@pu=" + pushCount.ToString() + "][@ca=" + ocCount.ToString() + 
                        "][@oc=" + heroCount.ToString() + "]/range");
                    Help.ShowPopup(this, spotPct.InnerText + " " + spotRange.InnerText, Control.MousePosition);
                }
                else if (recent_web_page != null)
                {
                    string opponentPushRange = recent_web_page.Substring(recent_web_page.LastIndexOf(getPushRange(recent_web_page, push_pos)));
                    string callRange = opponentPushRange.Substring(opponentPushRange.LastIndexOf(getCallRange(opponentPushRange, oc_pos)));
                    Regex regex = new Regex(Regex.Escape(hero_pos) + "([0-9]+" + Regex.Escape(".") + "[0-9]+%, .*?)\n");
                    MatchCollection matchCol = regex.Matches(callRange);
                    String overCallRange = matchCol[0].Groups[1].Value;
                    Help.ShowPopup(this, overCallRange, Control.MousePosition);
                }
                else
                {
                    int first = -1;
                    int second = -1;
                    for (int i = 1, j = 0; i < 10; ++i)
                    {
                        if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                        {
                            if (AllinCheckBoxes[(bb_pos + i) % 9].Checked)
                            {
                                if (first < 0)
                                    first = j;
                                else
                                    second = j;
                            }
                            ++j;
                        }
                    }
                    Help.ShowPopup(this, CalcByHRC.getOverCallRange(first, second, currentTableData.getHeroNumber()), Control.MousePosition);
                }
            }
        }

        private int calcCountByPos(string position)
        {
            if(position == "UTG") return 0;
            if(position == "UTG+1") return 1;
            if(position == "UTG+2") return 2;
            if(position == "UTG+3") return 3;
            if(position == "UTG+4") return 4;
            if(position == "CO") {
                return currentTableData.getLivePlayerCount() - 4;
            }
            if(position == "BU") {
                return currentTableData.getLivePlayerCount() - 3;
            }
            if(position == "SB") {
                return currentTableData.getLivePlayerCount() - 2;
            }
            if(position == "BB") {
                return currentTableData.getLivePlayerCount() - 1;
            }
            return -1;
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

        private void labelBB_Click(object sender, EventArgs e)
        {
            setBBLevel();
            buttonBBUP_Click(sender, e);
        }

        private void textBoxStructure_TextChanged(object sender, EventArgs e)
        {
            if (textBoxStructure.Text.Trim() == "1,1")
                this.BackColor = Color.FromArgb(255, 255, 240);
            else this.BackColor = Color.WhiteSmoke;
        }

        private void checkBoxICM_CheckedChanged(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width + (((CheckBox)sender).Checked ? 38 : -38), this.Size.Height);
        }

        private void OpenNashCalculatorViewController_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string d in drags)
                {
                    if (!System.IO.File.Exists(d))
                        return;
                }
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void OpenNashCalculatorViewController_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            openHandHistoryDialog.FileName = files[0];
            hh_back_num = 1;
            setReaderFromFileName();
            openHandHistory();
        }
        private void buttonHRC_Click(object sender, EventArgs e)
        {
            clearCalc();
            setupCurrentTableData();
            StringBuilder chips = new StringBuilder();
            for (int i = 1, j = 1; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    string playerName = PlayerNameLabels[(bb_pos + i) % 9].Text.Equals(string.Empty) ? "Player" + j : PlayerNameLabels[(bb_pos + i) % 9].Text;
                    chips.AppendLine("Seat " + j + ": " + playerName + " (" +  chipTextBoxes[(bb_pos + i) % 9].Text.Trim() + " in chips)");
                    j++;
                }
            }
            string[] ranges = CalcByHRC.Calc(currentTableData, chips.ToString(), isForceAllInList);
            if (ranges == null) return;
            for (int i = 1, j = 0, k = 0; i < 10 && j < isForceAllInList.Count && k < ranges.Length; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    if(isForceAllInList[j++] != true)
                        rangeTextBoxes[(bb_pos + i) % 9].Text = ranges[k++];
                }
            }
        }

        private void webKitBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowserTimer.Enabled = true;
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
