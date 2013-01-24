using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27DrawStartingHandCalc
{
    class Hand
    {
        public List<int> Cards { get; private set; }

        public Hand(string hand)
        {
            Cards = new List<int>();
            for (int i = 0; i < 5; ++i)
            {
                Cards.Add(charToInt(hand[i]));
            }
        }

        public Hand(Deck deck)
        {
            Cards = new List<int>();
            for (int i = 0; i < 5; ++i)
                Cards.Add((deck.DrawB() % 13));

            // Cards.Sort();
        }

        public int charToInt(char c)
        {
            switch (c)
            {
                case '1':
                case 'a':
                case 'A': return 0;
                case '2': return 1;
                case '3': return 2;
                case '4': return 3;
                case '5': return 4;
                case '6': return 5;
                case '7': return 6;
                case '8': return 7;
                case '9': return 8;
                case 't':
                case 'T': return 9;
                case 'j':
                case 'J': return 10;
                case 'q':
                case 'Q': return 11;
                case 'k':
                case 'K': return 12;
                default: return -1;
            }
        }

        public int getCountOf(int card)
        {
            return Cards.FindAll(x => x == card).Count;
        }

        public bool isEqualTo(Hand target)
        {
            foreach (int card in target.Cards)
            {
                if (card >= 0 && target.getCountOf(card) > this.getCountOf(card))
                    return false;
            }
            return true;
        }

        public bool isEqualTo(string target)
        {
            return isEqualTo(new Hand(target));
        }
    }
}
