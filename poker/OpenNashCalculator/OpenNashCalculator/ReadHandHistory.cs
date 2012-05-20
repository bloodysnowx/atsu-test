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
        int startingChip = 0;

        void ReadHandHistory()
        {
            if (updateDate >= System.IO.File.GetLastWriteTime(openHandHistoryDialog.FileName))
                return;

            updateDate = System.IO.File.GetLastWriteTime(openHandHistoryDialog.FileName);
            string[] hh = System.IO.File.ReadAllLines(openHandHistoryDialog.FileName);
            Regex regex;
            MatchCollection matchCol;
            string hero_name = "";

            // リバイ時のチップを取得する
            if (startingChip == 0)
            {
                regex = new Regex("Dealt" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" [")
                    + "[2-9TJQKA][shdc]" + Regex.Escape(" ") + "[2-9TJQKA][shdc]" + Regex.Escape("]"));

                for (int i = 0; i < hh.Length; ++i)
                {
                    matchCol = regex.Matches(hh[i]);
                    if(matchCol.Count > 0)
                        hero_name = matchCol[0].Groups[1].Value;
                }

                regex = new Regex("Seat" + Regex.Escape(" ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                    + Regex.Escape("(") + "([0-9]+)" + Regex.Escape(" ") + "in" + Regex.Escape(" ") + "chips" + Regex.Escape(")"));

                for (int i = 0; i < hh.Length; ++i)
                {
                    matchCol = regex.Matches(hh[i]);
                    if (matchCol.Count > 0 && matchCol[0].Groups[2].Value == hero_name)
                    {
                        startingChip = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                        break;
                    }
                }

                if (startingChip == 0)
                {
                    startingChip = System.Convert.ToInt32(Properties.Settings.Default.StartingChip);
                }
            }

            int line = 0;
            for (int i = 0; i < hh.Length; ++i)
            {
                if (hh[i].StartsWith("PokerStars Hand"))
                    line = i;
            }

            // bbとsbを取得
            regex = new Regex(Regex.Escape("(") + "([0-9]+)" + Regex.Escape("/") + "([0-9]+)" + Regex.Escape(")"));
            matchCol = regex.Matches(hh[line]);
            int sb = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
            int bb = System.Convert.ToInt32(matchCol[0].Groups[2].Value);

            // ボタンの位置を取得
            line += 1;
            regex = new Regex("Seat" + Regex.Escape(" #") + "([0-9]+)" + Regex.Escape(" ") + "is" + Regex.Escape(" ") 
                + "the" + Regex.Escape(" ") + "button");
            matchCol = regex.Matches(hh[line]);
            int button = System.Convert.ToInt32(matchCol[0].Groups[1].Value);

            // ハンド開始状況を取得
            int[] seats = new int[9];
            string[] names = new string[9];
            int[] chips = new int[9];
            string[] position = new string[9];
            int pot = 0;
            int[] posted = new int[9];
            int ante = 0;

            regex = new Regex("Seat" + Regex.Escape(" ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                + Regex.Escape("(") + "([0-9]+)" + Regex.Escape(" ") + "in" + Regex.Escape(" ") + "chips" + Regex.Escape(")"));
            for (int i = 0; i < 9; ++i)
            {
                matchCol = regex.Matches(hh[++line]);
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

            // ante
            regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + "the" + Regex.Escape(" ") +
                "ante" + Regex.Escape(" ") + "([0-9]+)");
            for (int i = 0; i < 9; ++i)
            {
                matchCol = regex.Matches(hh[++line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            if(ante < System.Convert.ToInt32(matchCol[0].Groups[2].Value))
                                ante = System.Convert.ToInt32(matchCol[0].Groups[2].Value);

                            chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        }
                    }
                }
                else
                {
                    line -= 1;
                    break;
                }
            }

            // blind
            regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + "small" + Regex.Escape(" ") +
                "blind" + Regex.Escape(" ") + "([0-9]+)");
            matchCol = regex.Matches(hh[++line]);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(names[j]))
                    {
                        posted[j] = System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        chips[j] -= posted[j];
                        pot += posted[j];
                    }
                }
            }
            else
            {
                line -= 1;
            }

            regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + "big" + Regex.Escape(" ") +
                "blind" + Regex.Escape(" ") + "([0-9]+)");
            matchCol = regex.Matches(hh[++line]);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(names[j]))
                    {
                        posted[j] = System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        chips[j] -= posted[j];
                        pot += posted[j];
                    }
                }
            }
            else
            {
                line -= 1;
            }

            // Heroの名前を取得
            for (; line < hh.Length; ++line)
            {
                if (hh[line].StartsWith("*** HOLE CARDS ***"))
                {
                    break;
                }
            }
            regex = new Regex("Dealt" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" [")
                + "[2-9TJQKA][shdc]" + Regex.Escape(" ") + "[2-9TJQKA][shdc]" + Regex.Escape("]"));
            matchCol = regex.Matches(hh[++line]);
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

            // bets, calls, raises, UNCALLED bet, collected
            Regex bets_regex = new Regex("(.+):" + Regex.Escape(" ") + "bets" + Regex.Escape(" ") + "([0-9]+)");
            Regex calls_regex = new Regex("(.+):" + Regex.Escape(" ") + "calls" + Regex.Escape(" ") + "([0-9]+)");
            Regex raises_regex = new Regex("(.+):" + Regex.Escape(" ") + "raises" + Regex.Escape(" ") + "([0-9]+)"
                + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "([0-9]+)");
            Regex uncalled_regex = new Regex("Uncalled" + Regex.Escape(" ") + "bet" + Regex.Escape(" (") + "([0-9]+)"
                + Regex.Escape(") ") + "returned" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)");
            Regex collected_regex = new Regex("(.+)" + Regex.Escape(" ") + "collected" + Regex.Escape(" ") + "([0-9]+)"
                + Regex.Escape(" ") + "from" + Regex.Escape(" "));

            for (; line < hh.Length; ++line)
            {
                matchCol = bets_regex.Matches(hh[line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            posted[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            break;
                        }
                    }
                    continue;
                }
                matchCol = calls_regex.Matches(hh[line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            posted[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            break;
                        }
                    }
                    continue;
                }
                matchCol = raises_regex.Matches(hh[line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[3].Value) - posted[j];
                            pot += System.Convert.ToInt32(matchCol[0].Groups[3].Value) - posted[j];
                            posted[j] = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                            break;
                        }
                    }
                    continue;
                }
                matchCol = uncalled_regex.Matches(hh[line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[2].Value.Equals(names[j]))
                        {
                            chips[j] += System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                            pot -= System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                            break;
                        }
                    }
                    continue;
                }
                matchCol = collected_regex.Matches(hh[line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(names[j]))
                        {
                            chips[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            pot -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            break;
                        }
                    }
                    continue;
                }

                if (hh[line].StartsWith("*** FLOP ***") || hh[line].StartsWith("*** TURN ***") || hh[line].StartsWith("*** RIVER ***"))
                {
                    for (int i = 0; i < 9; ++i)
                        posted[i] = 0;
                }
                else if(hh[line].StartsWith("*** SUMMARY ***"))
                {
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
                if (chips[i] <= 0 && names[i] != string.Empty && checkBoxRebuy.Checked)
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
                        positionRadioButtons[button - 1].Checked = true;
                    }
                    else
                    {
                        for (int j = 1, count = 0; j < 10; ++j)
                        {
                            if (chips[(i + j) % 9] > 0)
                            {
                                if (++count == 3)
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
                if(seats[hero_index] > 9) seats[hero_index] = seats[hero_index] % 9;
                SetHeroSeat(SeatLabels[seats[hero_index] - 1]);
            }
            else if (0 < Properties.Settings.Default.PreferredSeat && Properties.Settings.Default.PreferredSeat < 10)
            {
                for (int i = 0; i < (Properties.Settings.Default.PreferredSeat -  seats[hero_index] + 9) % 9; ++i)
                    SeatRotateF();

                SetHeroSeat(SeatLabels[Properties.Settings.Default.PreferredSeat - 1]);
            }

            SetPosition();

            if (chips[hero_index] > 0 && player_num > 1 && checkBoxCalc.Checked)
            {
                Calc();
            }
            else
            {
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