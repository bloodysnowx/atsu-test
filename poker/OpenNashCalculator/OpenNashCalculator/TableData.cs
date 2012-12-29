using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNashCalculator
{
    class TableData
    {
        private int maxSeatNum;
        public int BB { get; set; }
        public int SB { get; set; }
        public int Ante { get; set; }
        public string Structure { get; set; }
        public int hero_pos { get; set; }
        public string heroName { get; set; }
        public int[] seats { get; set; }
        public string[] positions { get; set; }
        public string[] playerNames { get; set; }
        public int[] chips { get; set; }
        public int buttonPos { get; set; }
        public int pot { get; set; }
        public int[] posted { get; set; }

        public TableData()
        {
            maxSeatNum = Properties.Settings.Default.MaxSeatNum;
            seats = new int[maxSeatNum];
            positions = new string[maxSeatNum];
            playerNames = new string[maxSeatNum];
            chips = new int[maxSeatNum];
            posted = new int[maxSeatNum];
        }

        public int getHeroIndex()
        {
            for (int i = 0; i < playerNames.Length; ++i)
            {
                if (playerNames[i].Equals(heroName))
                    return i;
            }
            return 0;
        }
    }
}
