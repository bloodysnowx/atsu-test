using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using UserValidatorLib;

using System.Text.RegularExpressions;

namespace OpenNashCalculator
{
    public partial class Form1 : Form
    {
        HandHistoryReader reader = new PSHHReader();
        int startingChip = 0;
        UserValidator validator = new UserValidator();

        void ReadHandHistoryNative()
        {
            updateDate = System.IO.File.GetLastWriteTime(openHandHistoryDialog.FileName);
            string[] hh = System.IO.File.ReadAllLines(openHandHistoryDialog.FileName);
            Regex regex;
            MatchCollection matchCol;
            string hero_name = "";

            List<int> hh_line_list = new List<int>();
            for (int i = 0; i < hh.Length; ++i)
            {
                if (hh[i].StartsWith("PokerStars Hand"))
                    hh_line_list.Add(i);
            }
            hh_line_list.Add(hh.Length - 1);

            List<string> now_hh = new List<string>();
            if (hh_back_num >= hh_line_list.Count) hh_back_num = hh_line_list.Count - 1;
            for (int i = hh_line_list[hh_line_list.Count - 1 - hh_back_num]; i < hh_line_list[hh_line_list.Count - hh_back_num]; ++i)
                now_hh.Add(hh[i]);

            int line = 0;
            // bbとsbを取得
            regex = new Regex(Regex.Escape("(") + "([0-9]+)" + Regex.Escape("/") + "([0-9]+)" + Regex.Escape(")"));
            matchCol = regex.Matches(now_hh[line]);
            int sb = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
            int bb = System.Convert.ToInt32(matchCol[0].Groups[2].Value);

            // ボタンの位置を取得
            line += 1;
            regex = new Regex("Seat" + Regex.Escape(" #") + "([0-9]+)" + Regex.Escape(" ") + "is" + Regex.Escape(" ")
                + "the" + Regex.Escape(" ") + "button");
            matchCol = regex.Matches(now_hh[line]);
            int button = System.Convert.ToInt32(matchCol[0].Groups[1].Value);

            // ハンド開始状況を取得
            int[] seats = new int[9];
            string[] names = new string[9];
            int[] chips = new int[9];
            string[] position = new string[9];
            int[] posted = new int[9];
            int ante = 0;

            regex = new Regex("Seat" + Regex.Escape(" ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                + Regex.Escape("(") + "([0-9]+)" + Regex.Escape(" ") + "in" + Regex.Escape(" ") + "chips" + Regex.Escape(")"));
            for (int i = 0; i < 9; ++i)
            {
                matchCol = regex.Matches(now_hh[++line]);
                if (matchCol.Count > 0)
                {
                    seats[i] = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                    names[i] = matchCol[0].Groups[2].Value;
                    chips[i] = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                }
                else
                {
                    line -= 1;
                    break;
                }
            }

            regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + "the" + Regex.Escape(" ") +
                "ante" + Regex.Escape(" ") + "([0-9]+)");
            for (int i = 0; i < 9; ++i)
            {
                matchCol = regex.Matches(now_hh[++line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            if (ante < System.Convert.ToInt32(matchCol[0].Groups[2].Value))
                                ante = System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        }
                    }
                }
                else
                {
                    line -= 1;
                    break;
                }
            }

            // Heroの名前を取得
            for (; line < now_hh.Count; ++line)
            {
                if (now_hh[line].StartsWith("*** HOLE CARDS ***"))
                {
                    break;
                }
            }
            regex = new Regex("Dealt" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" [")
                + "[2-9TJQKA][shdc]" + Regex.Escape(" ") + "[2-9TJQKA][shdc]" + Regex.Escape("]"));
            matchCol = regex.Matches(now_hh[++line]);
            hero_name = matchCol[0].Groups[1].Value;
            int hero_index = 0;
            for (int i = 0; i < 9; ++i)
            {
                if (hero_name.Equals(names[i]))
                {
                    hero_index = i;
                    SetHeroSeat(SeatLabels[seats[i] - 1]);
                    break;
                }
            }

            // 設定
            textBoxBB.Text = bb.ToString();
            currentSB = sb.ToString();
            textBoxAnte.Text = ante.ToString();

            // チップと名前入力
            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.Text = "";
            for (int i = 0; i < 9; ++i)
            {
                if (chips[i] <= 0 && names[i] != string.Empty && seats[i] > 0 && checkBoxRebuy.Checked)
                    chips[i] = startingChip;

                if (chips[i] > 0)
                {
                    chipTextBoxes[seats[i] - 1].Text = chips[i].ToString();
                    PlayerNameLabels[seats[i] - 1].Text = names[i];
                }
            }

            // ボタンの位置を決定
            int player_num = 0;
            for (int i = 0; i < 9; ++i)
            {
                if (chips[i] > 0) player_num++;
            }

            for (int i = 0; i < 9; ++i)
            {
                if (button == seats[i])
                {
                    if (player_num < 3)
                    {
                        positionRadioButtons[seats[1 - i] - 1].Checked = true;
                    }
                    else
                    {
                        for (int j = 1, count = 0; j < 10; ++j)
                        {
                            if (chips[(i + j) % 9] > 0)
                            {
                                if (++count == 2)
                                {
                                    positionRadioButtons[seats[(i + j) % 9] - 1].Checked = true;
                                    break;
                                }
                            }
                        }

                    }
                    break;
                }
            }

            if (Properties.Settings.Default.SetBBLast)
            {
                while (positionRadioButtons[8].Text != Position[0])
                {
                    SeatRotateF();
                    seats[hero_index]++;
                }
                if (seats[hero_index] > 9) seats[hero_index] = seats[hero_index] % 9;
                SetHeroSeat(SeatLabels[seats[hero_index] - 1]);
            }
            else if (0 < Properties.Settings.Default.PreferredSeat && Properties.Settings.Default.PreferredSeat < 10)
            {
                for (int i = 0; i < (Properties.Settings.Default.PreferredSeat - seats[hero_index] + 9) % 9; ++i)
                    SeatRotateF();

                SetHeroSeat(SeatLabels[Properties.Settings.Default.PreferredSeat - 1]);
            }

            SetPosition();

            foreach (CheckBox checkBox in AllinCheckBoxes)
                checkBox.Checked = false;

            if (chips[hero_index] > 0 && player_num > 1 && checkBoxCalc.Checked)
            {
                Calc();
            }
            else
            {
                recent_web_page = String.Empty;
                foreach (TextBox x in rangeTextBoxes)
                    x.Clear();
            }
        }

        void ReadHandHistory()
        {
            if (openHandHistoryDialog.FileName == String.Empty) return;
            DateTime lastWriteTime =reader.GetLastWriteTime(openHandHistoryDialog.FileName);
            if(checkBoxClose.Checked)
            {
                if(DateTime.Now.Minute <= 5 || DateTime.Now.Minute >= 55)
                {
                    if(lastWriteTime.AddMinutes(5 + Properties.Settings.Default.AutoCloseMin) < DateTime.Now)
                        Application.Exit();
                }
                else if (lastWriteTime.AddMinutes(Properties.Settings.Default.AutoCloseMin) < DateTime.Now)
                    Application.Exit();
            }

            if (hh_back_num == 0 && updateDate >= lastWriteTime) return;

            updateDate = lastWriteTime;
            TableData result = reader.read(openHandHistoryDialog.FileName, hh_back_num);
            if(!validator.validate(result.heroName, encryptedUserName)) {
                System.Windows.Forms.MessageBox.Show("You cannot use this application");
                Application.Exit();
            }
            if (checkBoxClose.Checked && result.getLivePlayerCount() <= 1) Application.Exit();

            // 設定
            textBoxBB.Text = result.BB.ToString();
            currentSB = result.SB.ToString();
            textBoxAnte.Text = result.Ante.ToString();
            SetHeroSeat(SeatLabels[result.getHeroSeat() - 1]);

            // チップと名前入力
            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.Text = "";
            for (int i = 0; i < result.MaxSeatNum; ++i)
            {
                if (result.chips[i] <= 0 && result.playerNames[i] != string.Empty && result.seats[i] > 0 && checkBoxRebuy.Checked)
                    result.chips[i] = result.StartingChip;

                if (result.chips[i] > 0)
                {
                    chipTextBoxes[result.seats[i] - 1].Text = result.chips[i].ToString();
                    PlayerNameLabels[result.seats[i] - 1].Text = result.playerNames[i];
                }
            }

            // ボタンの位置を決定
            int player_num = 0;
            for (int i = 0; i < result.MaxSeatNum; ++i)
            {
                if (result.chips[i] > 0) player_num++;
            }

            for (int i = 0; i < result.MaxSeatNum; ++i)
            {
                if (result.buttonPos == result.seats[i])
                {
                    if (player_num < 3)
                    {
                        for (int j = 1, count = 0; j < result.MaxSeatNum + 1; ++j)
                        {
                            if (result.chips[(i + j) % result.MaxSeatNum] > 0)
                            {
                                if (++count == 1)
                                {
                                    positionRadioButtons[result.seats[(i + j) % result.MaxSeatNum] - 1].Checked = true;
                                    break;
                                }
                            }
                        }
                        // positionRadioButtons[result.buttonPos - 1].Checked = true;
                    }
                    else
                    {
                        for (int j = 1, count = 0; j < result.MaxSeatNum + 1; ++j)
                        {
                            if (result.chips[(i + j) % result.MaxSeatNum] > 0)
                            {
                                if (++count == 2)
                                {
                                    positionRadioButtons[result.seats[(i + j) % result.MaxSeatNum] - 1].Checked = true;
                                    break;
                                }
                            }
                        }

                    }
                    break;
                }
            }

            if (Properties.Settings.Default.SetBBLast)
            {
                while (positionRadioButtons[8].Text != Position[0])
                {
                    SeatRotateF();
                    result.seats[result.getHeroIndex()]++;
                }
                if(result.seats[result.getHeroIndex()] > result.MaxSeatNum) result.seats[result.getHeroIndex()] = result.seats[result.getHeroIndex()] % result.MaxSeatNum;
                SetHeroSeat(SeatLabels[result.getHeroSeat() - 1]);
            }
            else if (0 < Properties.Settings.Default.PreferredSeat && Properties.Settings.Default.PreferredSeat < result.MaxSeatNum + 1)
            {
                for (int i = 0; i < (Properties.Settings.Default.PreferredSeat -  result.seats[result.getHeroIndex()] + result.MaxSeatNum) % result.MaxSeatNum; ++i)
                    SeatRotateF();

                SetHeroSeat(SeatLabels[Properties.Settings.Default.PreferredSeat - 1]);
            }

            SetPosition();

            foreach (CheckBox checkBox in AllinCheckBoxes)
                checkBox.Checked = false;

            if (result.chips[result.getHeroIndex()] > 0 && player_num > 1 && checkBoxCalc.Checked)
            {
                Calc();
            }
            else
            {
                recent_web_page = String.Empty;
                foreach (TextBox x in rangeTextBoxes)
                    x.Clear();
            }
        }

        void SeatRotateF()
        {
            string tmp_seat = SeatLabels[8].Text;
            string tmp_chip = chipTextBoxes[8].Text;
            bool tmp_checked = positionRadioButtons[8].Checked;
            string tmp_name = PlayerNameLabels[8].Text;

            for (int i = 7; i >= 0; --i)
            {
                SeatLabels[i + 1].Text = SeatLabels[i].Text;
                chipTextBoxes[i + 1].Text = chipTextBoxes[i].Text;
                positionRadioButtons[i + 1].Checked = positionRadioButtons[i].Checked;
                PlayerNameLabels[i + 1].Text = PlayerNameLabels[i].Text;
            }

            SeatLabels[0].Text = tmp_seat;
            chipTextBoxes[0].Text = tmp_chip;
            positionRadioButtons[0].Checked = tmp_checked;
            PlayerNameLabels[0].Text = tmp_name;
        }

        void SeatRotateR()
        {
        }
    }
}