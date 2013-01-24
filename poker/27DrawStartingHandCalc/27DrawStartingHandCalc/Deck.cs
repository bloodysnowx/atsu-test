using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27DrawStartingHandCalc
{
    public class Deck
    {
        Random rand;
        List<int> dealt;
        public Deck()
        {
            rand = new System.Random();
            dealt = new List<int>();
        }

        public void Reset()
        {
            dealt = new List<int>();
        }

        public int DrawA()
        {
            int newCard;
            do
            {
                newCard = rand.Next(52);
            }
            while(dealt.Contains(newCard));

            return newCard;
        }

        public int DrawB()
        {
            dealt.Sort();
            int newCard = rand.Next(52 - dealt.Count);

            for (int i = 0; i < dealt.Count; ++i)
            {
                if (dealt[i] < newCard) ++newCard;
                else break;
            }

            dealt.Add(newCard);

            return newCard;
        }
    }
}
