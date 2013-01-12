using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace OpenNashCalculator
{
    class FTHHReader : HandHistoryReader
    {
        private int maxSeatNum = 10;

        public FTHHReader()
        {
            maxSeatNum = Properties.Settings.Default.MaxSeatNum;
        }

        private string[] ReadAllLines(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            List<string> hhList = new List<string>();
            while (sr.Peek() >= 0)
            {
                hhList.Add(sr.ReadLine());
            }
            return hhList.ToArray();
        }

        private string[] removeSitsDown(string[] hh)
        {
            List<string> hhList = hh.ToList();
            hhList.RemoveAll(line => line.Contains("sits down"));
            return hhList.ToArray();
        }

        public TableData read(string fileName, int backNum)
        {
            TableData result = new TableData();
            
            // string[] hh = System.IO.File.ReadAllLines(fileName);
            string[] hh = ReadAllLines(fileName);
            hh = removeSitsDown(hh);

            result.heroName = getHeroName(hh);
            result.StartingChip = getStartingChip(result.heroName, hh, System.Convert.ToInt32(Properties.Settings.Default.StartingChip));
            List<int> hhLineList = getHHLineList(hh);
            string[] now_hh = getNowHH(hh, hhLineList, backNum);

            int line = 0;
            setBBSB(ref result, now_hh[line]);
            getStartSituation(ref result, now_hh, ref line);
            calcAntePayment(ref result, now_hh, ref line);
            calcBlindsPayment(ref result, now_hh, ref line);
            result.buttonPos = getButtonPos(now_hh[++line]);

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

        private string getHeroName(string[] hh)
        {
            Regex regex = new Regex(Regex.Escape("Dealt to ") + "(.+)" + Regex.Escape(" [")
            + "[2-9TJQKA][shdc]" + Regex.Escape(" ") + "[2-9TJQKA][shdc]" + Regex.Escape("]"));
            foreach (string line in hh)
            {
                MatchCollection matchCol = regex.Matches(line);
                if (matchCol.Count > 0)
                    return matchCol[0].Groups[1].Value;
            }
            return string.Empty;
        }

        private int getStartingChip(string hero_name, string[] hh, int defaultValue)
        {
            int ret = defaultValue;
            Regex regex = new Regex(Regex.Escape("Seat ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" (")
                    + "([0-9,]+)" + Regex.Escape(")"));
            
            for (int i = 0; i < hh.Length; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[i]);
                if (matchCol.Count > 0 && matchCol[0].Groups[2].Value == hero_name)
                {
                    ret = System.Convert.ToInt32(matchCol[0].Groups[3].Value.Replace(",", string.Empty));
                    break;
                }
            }

            return ret;
        }
       
        private List<int> getHHLineList(string[] hh)
        {
            List<int> hh_line_list = new List<int>();
            for (int i = 0; i < hh.Length; ++i)
            {
                if (hh[i].StartsWith("Full Tilt Poker Game"))
                    hh_line_list.Add(i);
            }
            hh_line_list.Add(hh.Length - 1);

            return hh_line_list;
        }

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

        private void setBBSB(ref TableData data, string line)
        {
            Regex regex = new Regex("([0-9,]+)" + Regex.Escape("/") + "([0-9,]+)");
            MatchCollection matchCol = regex.Matches(line);
            data.SB = System.Convert.ToInt32(matchCol[0].Groups[1].Value.Replace(",", string.Empty));
            data.BB = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
        }

        private void getStartSituation(ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex(Regex.Escape("Seat ") + "([0-9]+):" + Regex.Escape(" ") + "(.+)" + Regex.Escape(" ")
                + Regex.Escape("(") + "([0-9,]+)" + Regex.Escape(")"));
            for (int i = 0; i < maxSeatNum; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[++line]);
                if (matchCol.Count > 0)
                {
                    result.seats[i] = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
                    result.playerNames[i] = matchCol[0].Groups[2].Value;
                    result.chips[i] = System.Convert.ToInt32(matchCol[0].Groups[3].Value.Replace(",", string.Empty));
                }
                else
                {
                    line -= 1;
                    break;
                }
            }
        }

        private void calcAntePayment(ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex("(.+)" + Regex.Escape(" antes ") + "([0-9,]+)");
            for (int i = 0; i < maxSeatNum; ++i)
            {
                MatchCollection matchCol = regex.Matches(hh[++line]);
                if (matchCol.Count > 0)
                {
                    for (int j = 0; j < maxSeatNum; ++j)
                    {
                        if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                        {
                            int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                            result.Ante = value;
                            result.chips[j] -= value;
                            result.pot += value;
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

        private void calcBlindPayment(Blind blind, ref TableData result, string[] hh, ref int line)
        {
            Regex regex = new Regex("(.+)" + Regex.Escape(" posts the ") + blind + Regex.Escape(" blind of ") 
                + "([0-9,]+)");
            MatchCollection matchCol = regex.Matches(hh[++line]);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                        result.posted[j] = value;
                        result.chips[j] -= value;
                        result.pot += value;
                    }
                }
            }
            else
            {
                line -= 1;
            }
        }

        private void calcBlindsPayment(ref TableData result, string[] hh, ref int line)
        {
            calcBlindPayment(Blind.small, ref result, hh, ref line);
            calcBlindPayment(Blind.big, ref result, hh, ref line);
        }

        private int getButtonPos(string line)
        {
            Regex regex = new Regex(Regex.Escape("The button is in seat #") + "([0-9]+)");
            MatchCollection matchCol = regex.Matches(line);
            return System.Convert.ToInt32(matchCol[0].Groups[1].Value);
        }

        private bool calcBetPayment(ref TableData result, string line)
        {
            Regex bets_regex = new Regex("(.+)" + Regex.Escape(" bets ") + "([0-9,]+)");
            MatchCollection matchCol = bets_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                        result.chips[j] -= value;
                        result.pot += value;
                        result.posted[j] += value;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcCallPayment(ref TableData result, string line)
        {
            Regex calls_regex = new Regex("(.+)" + Regex.Escape(" calls ") + "([0-9,]+)");
            MatchCollection matchCol = calls_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                        result.chips[j] -= value;
                        result.pot += value;
                        result.posted[j] += value;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcRaisePayment(ref TableData result, string line)
        {
            Regex raises_regex = new Regex("(.+)" + Regex.Escape(" raises to ") + "([0-9,]+)");

            MatchCollection matchCol = raises_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                        result.chips[j] -= value - result.posted[j];
                        result.pot += value - result.posted[j];
                        result.posted[j] = value;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcUncalledPayment(ref TableData result, string line)
        {
            Regex uncalled_regex = new Regex(Regex.Escape("Uncalled bet of ") + "([0-9,]+)"
                    + Regex.Escape(" returned to ") + "(.+)");
            MatchCollection matchCol = uncalled_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[2].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[1].Value.Replace(",", string.Empty));
                        result.chips[j] += value;
                        result.pot -= value;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool calcCollectedPayment(ref TableData result, string line)
        {
            Regex collected_regex = new Regex("(.+)" + Regex.Escape(" wins the pot (") + "([0-9,]+)"
                    + Regex.Escape(")"));
            MatchCollection matchCol = collected_regex.Matches(line);
            if (matchCol.Count > 0)
            {
                for (int j = 0; j < maxSeatNum; ++j)
                {
                    if (matchCol[0].Groups[1].Value.Equals(result.playerNames[j]))
                    {
                        int value = System.Convert.ToInt32(matchCol[0].Groups[2].Value.Replace(",", string.Empty));
                        result.chips[j] += value;
                        result.pot -= value;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        public string getTourneyID(string fileName)
        {
            Regex regex = new Regex(Regex.Escape("(") + "([0-9]+)" + Regex.Escape("), No Limit Hold'em"));
            MatchCollection matchCol = regex.Matches(fileName);
            return matchCol[0].Groups[1].Value;
        }

        public DateTime GetLastWriteTime(string fileName)
        {
            Regex regex = new Regex(Regex.Escape(" - ") + "([0-9]+):([0-9]+):([0-9]+)" + Regex.Escape(" JST - ")
                + "([0-9]+)/([0-9]+)/([0-9]+)");
            string[] hh = ReadAllLines(fileName);
            DateTime lastWriteTime = System.IO.File.GetLastWriteTime(fileName);
            foreach (string line in hh)
            {
                MatchCollection matchCol = regex.Matches(line);
                if (matchCol.Count > 0)
                {
                    lastWriteTime = new DateTime(System.Convert.ToInt32(matchCol[0].Groups[4].Value), 
                        System.Convert.ToInt32(matchCol[0].Groups[5].Value),
                        System.Convert.ToInt32(matchCol[0].Groups[6].Value),
                        System.Convert.ToInt32(matchCol[0].Groups[1].Value), 
                        System.Convert.ToInt32(matchCol[0].Groups[2].Value), 
                        System.Convert.ToInt32(matchCol[0].Groups[3].Value));
                }
            }
            return lastWriteTime;
        }
    }
}
