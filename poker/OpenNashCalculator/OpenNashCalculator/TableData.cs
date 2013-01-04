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

        public TableData()
        {
            MaxSeatNum = Properties.Settings.Default.MaxSeatNum;
            seats = new int[MaxSeatNum];
            positions = new string[MaxSeatNum];
            playerNames = new string[MaxSeatNum];
            chips = new int[MaxSeatNum];
            posted = new int[MaxSeatNum];
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
            // 現在のボタンの次に存在するシートにボタンを移動する
            int nowIndex;
            for (nowIndex = 0; nowIndex < MaxSeatNum; ++nowIndex)
                if (seats[nowIndex] == this.buttonPos) break;
            for (int i = 1; i < MaxSeatNum; ++i)
            {
                if (seats[(i + nowIndex) % MaxSeatNum] > 0)
                {
                    this.buttonPos = seats[(i + nowIndex) % MaxSeatNum];
                    break;
                }
            }
        }
    }
}
