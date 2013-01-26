using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27DrawStartingHandCalc
{
    class AllHands
    {
        public enum ORDER { ASC, DESC }
        public enum POSITION { ZERO, ONE, TWO, THREE }

        public const int MAX_LENGTH = 2598960;
        int i = 0, j = 1, k = 2, l = 3, m = 3;

        int count = 0;

        public AllHands(ORDER order)
        {
            if (order == ORDER.DESC)
            {
                i = 47; j = 48; k = 49; l = 50; m = 52;
            }
        }

        public AllHands(POSITION pos)
        {
            if (pos == POSITION.ONE)
            {
                i = 2; j = 18; k = 30; l = 38; m = 43;
            }
            else if (pos == POSITION.TWO)
            {
                i = 6; j = 13; k = 22; l = 29; m = 30;
            }
            else if (pos == POSITION.THREE)
            {
                i = 12; j = 13; k = 40; l = 48; m = 51;
            }
        }

        private Hand makeHand()
        {
            return new Hand(new List<int>() { i % 13, j % 13, k % 13, l % 13, m % 13 });
        }

        public Hand Next()
        {
            this.increment();
            return makeHand();
        }

        public Hand Prev()
        {
            this.decrement();
            return makeHand();
        }

        private void increment()
        {
            if (m < 51) ++m;
            else if (l < 50) { ++l; m = l + 1; }
            else if (k < 49) { ++k; l = k + 1; m = l + 1; }
            else if (j < 48) { ++j; k = j + 1; l = k + 1; m = l + 1; }
            else if (i < 47) { ++i; j = i + 1; k = j + 1; l = k + 1; m = l + 1; }
            else throw new IndexOutOfRangeException();
        }

        private void decrement()
        {
            if (l + 1 < m) --m;
            else if (k + 1 < l) { --l; m = 51; }
            else if (j + 1 < k) { --k; l = 50; }
            else if (i + 1 < j) { --j; k = 49; }
            else if (0 < i) { --i; j = 48; }
            else throw new IndexOutOfRangeException();
        }
    }
}
