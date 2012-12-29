using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace OpenNashCalculator
{
    class PSHHReader : HandHistoryReader
    {
        private int startingChip = 0;

        /// <summary>HEROの名前を取得する</summary>
        /// <param name="hh"></param>
        /// <returns>HEROの名前</returns>
        private string getHeroName(string[] hh)
        {
            Regex regex = new Regex("Dealt" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" [")
                + "[2-9TJQKA][shdc]" + Regex.Escape(" ") + "[2-9TJQKA][shdc]" + Regex.Escape("]"));
            for (int i = 0; i < hh.Length; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[i]);
                if (matchCol.Count > 0)
                    return matchCol[0].Groups[1].Value;
            }
            return string.Empty;
        }

        /// <summary>リバイ時のチップを取得する</summary>
        /// <param name="hh"></param>
        /// <param name="defaultValue">リバイ時のチップが決定できなかった場合のデフォルト値</param>
        /// <returns>リバイ時のチップ</returns>
        private int getStartingChip(string[] hh, int defaultValue)
        {
            int ret = 0;
            Regex regex = new Regex("Seat" + Regex.Escape(" ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                    + Regex.Escape("(") + "([0-9]+)" + Regex.Escape(" ") + "in" + Regex.Escape(" ") + "chips" + Regex.Escape(")"));

            for (int i = 0; i < hh.Length; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[i]);
                if (matchCol.Count > 0 && matchCol[0].Groups[2].Value == hero_name)
                {
                    ret = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                    break;
                }
            }

            if (ret == 0)
            {
                ret = defaultValue;
            }

            return ret;
        }

        /// <summary>ハンドヒストリの区切りのラインを取得する</summary>
        /// <param name="hh"></param>
        /// <returns>1つ目の開始行, 2つ目の開始行, ..., 最後の開始行, 最後の終了行</returns>
        private List<int> getHHLineList(string[] hh)
        {
            List<int> hh_line_list = new List<int>();
            for (int i = 0; i < hh.Length; ++i)
            {
                if (hh[i].StartsWith("PokerStars Hand"))
                    hh_line_list.Add(i);
            }
            hh_line_list.Add(hh.Length - 1);

            return hh_line_list;
        }

        /// <summary>今回の処理で使用するハンドヒストリーを抽出する</summary>
        /// <param name="hh"></param>
        /// <param name="lineList"></param>
        /// <param name="backNum"></param>
        /// <returns></returns>
        private List<string> getNowHH(string[] hh, List<int> lineList, int backNum)
        {
            List<string> now_hh = new List<string>();
            if (backNum == 0)
            {
                for (int i = lineList[lineList.Count - 2]; i < lineList[lineList.Count - 1]; ++i)
                {
                    now_hh.Add(hh[i]);
                }
            }
            else
            {
                if (backNum >= lineList.Count) backNum = lineList.Count - 1;
                for (int i = lineList[lineList.Count - 1 - backNum]; i < lineList[lineList.Count - backNum]; ++i)
                {
                    now_hh.Add(hh[i]);
                }
            }
            return now_hh;
        }

        /// <summary>BBとSBを取得し、設定する</summary>
        /// <param name="data"></param>
        /// <param name="line"></param>
        private void setBBSB(ref TableData data, string line)
        {
            Regex regex = new Regex(Regex.Escape("(") + "([0-9]+)" + Regex.Escape("/") + "([0-9]+)" + Regex.Escape(")"));
            MatchCollection matchCol = regex.Matches(line);
            data.SB = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
            data.BB = System.Convert.ToInt32(matchCol[0].Groups[2].Value);
        }

        /// <summary>ボタンの位置を取得</summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private int getButtonPos(string line)
        {
            Regex regex = new Regex("Seat" + Regex.Escape(" #") + "([0-9]+)" + Regex.Escape(" ") + "is" + Regex.Escape(" ")
                + "the" + Regex.Escape(" ") + "button");
            MatchCollection matchCol = regex.Matches(line);
            return System.Convert.ToInt32(matchCol[0].Groups[1].Value);
        }

        TableData HandHistoryReader.read(string fileName, int backNum)
        {
            TableData result = new TableData();
            DateTime updateDate = System.IO.File.GetLastWriteTime(fileName);
            string[] hh = System.IO.File.ReadAllLines(fileName);
            Regex regex;
            MatchCollection matchCol;
            string hero_name = string.Empty;

            if (hero_name == string.Empty) hero_name = getHeroName(hh);

            if (startingChip == 0) startingChip = getStartingChip(hh, System.Convert.ToInt32(Properties.Settings.Default.StartingChip));

            List<int> hhLineList = getHHLineList(hh);

            List<string> now_hh = getNowHH(hh, hhLineList, backNum);

            int line = 0;
            setBBSB(ref result, now_hh[line]);

            line += 1;
            result.buttonPos = getButtonPos(now_hh[line]);

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

            // ante
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
            matchCol = regex.Matches(now_hh[++line]);
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
            matchCol = regex.Matches(now_hh[++line]);
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

            if (hh_back_num == 0)
            {
                // bets, calls, raises, UNCALLED bet, collected
                Regex bets_regex = new Regex("(.+):" + Regex.Escape(" ") + "bets" + Regex.Escape(" ") + "([0-9]+)");
                Regex calls_regex = new Regex("(.+):" + Regex.Escape(" ") + "calls" + Regex.Escape(" ") + "([0-9]+)");
                Regex raises_regex = new Regex("(.+):" + Regex.Escape(" ") + "raises" + Regex.Escape(" ") + "([0-9]+)"
                    + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "([0-9]+)");
                Regex uncalled_regex = new Regex("Uncalled" + Regex.Escape(" ") + "bet" + Regex.Escape(" (") + "([0-9]+)"
                    + Regex.Escape(") ") + "returned" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)");
                Regex collected_regex = new Regex("(.+)" + Regex.Escape(" ") + "collected" + Regex.Escape(" ") + "([0-9]+)"
                    + Regex.Escape(" ") + "from" + Regex.Escape(" "));

                for (; line < now_hh.Count; ++line)
                {
                    matchCol = bets_regex.Matches(now_hh[line]);
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
                    matchCol = calls_regex.Matches(now_hh[line]);
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
                    matchCol = raises_regex.Matches(now_hh[line]);
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
                    matchCol = uncalled_regex.Matches(now_hh[line]);
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
                    matchCol = collected_regex.Matches(now_hh[line]);
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

                    if (now_hh[line].StartsWith("*** FLOP ***") || now_hh[line].StartsWith("*** TURN ***") || now_hh[line].StartsWith("*** RIVER ***"))
                    {
                        for (int i = 0; i < 9; ++i)
                            posted[i] = 0;
                    }
                    else if (now_hh[line].StartsWith("*** SUMMARY ***"))
                    {
                        break;
                    }
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
            throw new NotImplementedException();
        }
    }
}
