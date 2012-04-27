﻿using System;
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
            int[] posted = new int[9];

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
                + "[2-9JQKA][shdc]" + Regex.Escape(" ") + "[2-9JQKA][shdc]" + Regex.Escape("]"));
            matchCol = regex.Matches(hh[++line]);
            string hero_name = matchCol[0].Groups[1].Value;

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
        }
    }
}