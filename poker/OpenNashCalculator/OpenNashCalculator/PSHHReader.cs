using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace OpenNashCalculator
{
    class PSHHReader : HandHistoryReader
    {
        private int maxSeatNum = 10;

        public PSHHReader()
        {
            maxSeatNum = Properties.Settings.Default.MaxSeatNum;
        }

        public string getTourneyID(string fileName)
        {
            Regex regex = new Regex("HH[0-9]+" + Regex.Escape(" ") + "T([0-9]+)" + Regex.Escape(" ") + "No" + Regex.Escape(" ")
                + "Limit" + Regex.Escape(" ") + "Hold");
            MatchCollection matchCol = regex.Matches(fileName);
            return matchCol[0].Groups[1].Value;
        }

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
        private int getStartingChip(string hero_name, string[] hh, int defaultValue)
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
        private string[] getNowHH(string[] hh, List<int> lineList, int backNum)
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
            return now_hh.ToArray();
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

        /// <summary>ハンド開始状況を取得</summary>
        /// <param name="result"></param>
        /// <param name="hh"></param>
        /// <param name="line"></param>
        private void getStartSituation(ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex("Seat" + Regex.Escape(" ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                + Regex.Escape("(") + "([0-9]+)" + Regex.Escape(" ") + "in" + Regex.Escape(" ") + "chips" + Regex.Escape(")"));
            for (int i = 0; i < maxSeatNum; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[++line]);
                if (matchCol.Count > 0)
                {
                    result.seats[i] = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                    result.playerNames[i] = matchCol[0].Groups[2].Value;
                    result.chips[i] = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                }
                else
                {
                    line -= 1;
                    break;
                }
            }
        }

        /// <summary>Anteの支払い処理を計算する</summary>
        /// <param name="result"></param>
        /// <param name="hh"></param>
        /// <param name="line"></param>
        private void calcAntePayment(ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + "the" + Regex.Escape(" ") +
                "ante" + Regex.Escape(" ") + "([0-9]+)");
            for (int i = 0; i < maxSeatNum; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[++line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < maxSeatNum; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                        {
                            result.Ante = Math.Min(result.Ante, System.Convert.ToInt32(matchCol[0].Groups[2].Value));

                            result.chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                            result.pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        }
                    }
                }
                else
                {
                    line -= 1;
                    break;
                }
            }
        }

        /// <summary>指定されたBlindの支払い処理を計算する</summary>
        /// <param name="blind"></param>
        /// <param name="result"></param>
        /// <param name="hh"></param>
        /// <param name="line"></param>
        private void calcBlindPayment(Blind blind, ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex("(.+):" + Regex.Escape(" ") + "posts" + Regex.Escape(" ") + blind + Regex.Escape(" ") +
                "blind" + Regex.Escape(" ") + "([0-9]+)");
            MatchCollection matchCol = regex.Matches(hh[++line]);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        result.posted[j] = System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.chips[j] -= result.posted[j];
                        result.pot += result.posted[j];
                    }
                }
            }
            else
            {
                line -= 1;
            }
        }
        
        /// <summary>全Blindの支払い処理を計算する</summary>
        /// <param name="result"></param>
        /// <param name="hh"></param>
        /// <param name="line"></param>
        private void calcBlindsPayment(ref TableData result, string[] hh, ref int line)
        {
            calcBlindPayment(Blind.small, ref result, hh, ref line);
            calcBlindPayment(Blind.big, ref result, hh, ref line);
        }

        private bool calcBetPayment(ref TableData result, string line)
        {
            Regex bets_regex = new Regex("(.+):" + Regex.Escape(" ") + "bets" + Regex.Escape(" ") + "([0-9]+)");
            MatchCollection matchCol = bets_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        result.chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.posted[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcCallPayment(ref TableData result, string line)
        {
            Regex calls_regex = new Regex("(.+):" + Regex.Escape(" ") + "calls" + Regex.Escape(" ") + "([0-9]+)");
            MatchCollection matchCol = calls_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        result.chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.posted[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcRaisePayment(ref TableData result, string line)
        {
            Regex raises_regex = new Regex("(.+):" + Regex.Escape(" ") + "raises" + Regex.Escape(" ") + "([0-9]+)"
                    + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "([0-9]+)");

            MatchCollection matchCol = raises_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        result.chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[3].Value) - result.posted[j];
                        result.pot += System.Convert.ToInt32(matchCol[0].Groups[3].Value) - result.posted[j];
                        result.posted[j] = System.Convert.ToInt32(matchCol[0].Groups[3].Value);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcUncalledPayment(ref TableData result, string line)
        {
            Regex uncalled_regex = new Regex("Uncalled" + Regex.Escape(" ") + "bet" + Regex.Escape(" (") + "([0-9]+)"
                    + Regex.Escape(") ") + "returned" + Regex.Escape(" ") + "to" + Regex.Escape(" ") + "(.+)");
            MatchCollection matchCol = uncalled_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[2].Value.Equals(result.playerNames[j]))
                    {
                        result.chips[j] += System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                        result.pot -= System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcCollectedPayment(ref TableData result, string line)
        {
            Regex collected_regex = new Regex("(.+)" + Regex.Escape(" ") + "collected" + Regex.Escape(" ") + "([0-9]+)"
                    + Regex.Escape(" ") + "from" + Regex.Escape(" "));
            MatchCollection matchCol = collected_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        result.chips[j] += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        result.pot -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        TableData HandHistoryReader.read(string fileName, int backNum)
        {
            TableData result = new TableData();
            string[] hh = System.IO.File.ReadAllLines(fileName);

            result.heroName = getHeroName(hh);
            result.StartingChip = getStartingChip(result.heroName, hh, System.Convert.ToInt32(Properties.Settings.Default.StartingChip));
            List<int> hhLineList = getHHLineList(hh);
            string[] now_hh = getNowHH(hh, hhLineList, backNum);

            int line = 0;
            setBBSB(ref result, now_hh[line++]);
            result.buttonPos = getButtonPos(now_hh[line]);
            getStartSituation(ref result, now_hh, ref line);
            calcAntePayment(ref result, now_hh, ref line);
            calcBlindsPayment(ref result, now_hh, ref line);

            if (backNum == 0)
            {
                // bets, calls, raises, UNCALLED bet, collected
                for (; line < now_hh.Length; ++line)
                {
                    if (calcBetPayment(ref result, now_hh[line])) continue;
                    if (calcCallPayment(ref result, now_hh[line])) continue;
                    if (calcRaisePayment(ref result, now_hh[line])) continue;
                    if (calcUncalledPayment(ref result, now_hh[line])) continue;
                    if (calcCollectedPayment(ref result, now_hh[line])) continue;

                    if (now_hh[line].StartsWith("*** FLOP ***") || now_hh[line].StartsWith("*** TURN ***") || now_hh[line].StartsWith("*** RIVER ***"))
                    {
                        result.posted.Initialize();
                    }
                    else if (now_hh[line].StartsWith("*** SUMMARY ***"))
                    {
                        break;
                    }
                }
                result.nextButton();
            }

            return result;
        }
    }
}
