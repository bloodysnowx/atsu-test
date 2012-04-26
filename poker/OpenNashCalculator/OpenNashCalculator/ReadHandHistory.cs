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
        void ReadHandHistory()
        {
            if (updateDate >= System.IO.File.GetLastWriteTime(openHandHistoryDialog.FileName))
                return;

            updateDate = System.IO.File.GetLastWriteTime(openHandHistoryDialog.FileName);
            string[] hh = System.IO.File.ReadAllLines(openHandHistoryDialog.FileName);

            int line = 0;
            for (int i = 0; i < hh.Length; ++i)
            {
                if (hh[i].StartsWith("PokerStars Hand"))
                    line = i;
            }

            // bbとsbを取得
            Regex regex = new Regex(Regex.Escape("(") + "([0-9]+)" + Regex.Escape("/") + "([0-9]+)" + Regex.Escape(")"));
            MatchCollection matchCol = regex.Matches(hh[line]);
            int bb = System.Convert.ToInt32(matchCol[0].Groups[1].Value);
            int sb = System.Convert.ToInt32(matchCol[0].Groups[2].Value);

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
                        chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
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
                        chips[j] -= System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                        pot += System.Convert.ToInt32(matchCol[0].Groups[2].Value);
                    }
                }
            }
            else
            {
                line -= 1;
            }
        }
    }
}