using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNashCalculator
{
    class TableData
    {
        public int MaxSeatNum { get; private set; }
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
        public int StartingChip { get; set; }
        private int heroIndex = -1;
        public double[] ICMs { get; set; }
        public DateTime startTime { get; set; }
        public DateTime handTime { get; set; }

        public TableData()
        {
            MaxSeatNum = Properties.Settings.Default.MaxSeatNum;
            seats = new int[MaxSeatNum];
            positions = new string[MaxSeatNum];
            playerNames = new string[MaxSeatNum];
            chips = new int[MaxSeatNum];
            posted = new int[MaxSeatNum];
            ICMs = new double[MaxSeatNum];
        }

        public int getHeroIndex()
        {
            if (heroIndex < 0) heroIndex = calcHeroIndex();
            return heroIndex;
        }

        private int calcHeroIndex()
        {
            for (int i = 0; i < playerNames.Length; ++i)
            {
                if (playerNames[i].Equals(heroName))
                    return i;
            }
            return 0;
        }

        public int getHeroSeat()
        {
            return seats[getHeroIndex()];
        }

        public void nextButton()
        {
            // if (isSBAlive() == false) return;
            // 現在のボタンの次に存在するシートにボタンを移動する
            int nowIndex;
            for (nowIndex = 0; nowIndex < MaxSeatNum; ++nowIndex)
                if (seats[nowIndex] == this.buttonPos) break;
            for (int i = 1; i < MaxSeatNum; ++i)
            {
                if (chips[(i + nowIndex) % MaxSeatNum] > 0)
                {
                    this.buttonPos = seats[(i + nowIndex) % MaxSeatNum];
                    break;
                }
            }
        }

        public int getLivePlayerCount()
        {
            int count = 0;
            foreach (int chip in chips)
            {
                if (chip > 0) ++count;
            }
            return count;
        }

        public int getHeroChip()
        {
            return chips[getHeroIndex()];
        }

        public void calcICMs(double[] structures)
        {
            ICM icm = new ICM();
            double[] tmpChips = new double[chips.Length];
            for (int i = 0; i < chips.Length; ++i)
                tmpChips[i] = chips[i];
            icm.calcICM(tmpChips, ICMs, MaxSeatNum, structures, structures.Length);
        }

        private int getButtonIndex()
        {
            int buttonIndex = -1;
            for (int i = 0; i < MaxSeatNum; ++i)
            {
                if (seats[i] == buttonPos) buttonIndex = i;
            }
            return buttonIndex;
        }

        private int getSBIndex()
        {
            int SBIndex = -1;
            for (int i = 1; i <= MaxSeatNum; ++i)
            {
                if (playerNames[(i + getButtonIndex()) % MaxSeatNum] != null && playerNames[(i + getButtonIndex()) % MaxSeatNum].Length > 0) SBIndex = i % MaxSeatNum;
            }
            return SBIndex;
        }

        public bool isSBAlive()
        {
            return chips[getSBIndex()] > 0;
        }

        public void addonAll(int rebuyChip, int addonChip)
        {
            for (int i = 0; i < MaxSeatNum; ++i)
            {
                if (playerNames[i] != null)
                {
                    if (chips[i] <= 0) chips[i] = rebuyChip;
                    chips[i] += addonChip;
                }
            }
        }
    }
}
