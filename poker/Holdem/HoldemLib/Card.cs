using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoldemLib
{
    /// <summary>
    /// カード(スートとランク)
    /// </summary>
    class Card
    {
        /// <summary>
        /// スート
        /// </summary>
        public HAND_SUIT suit { private set; get; }
        /// <summary>
        /// ランク
        /// </summary>
        public uint rank { private set; get; }

        /// <summary>
        /// スートとランクを初期化する
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="rank"></param>
        public Card(HAND_SUIT suit, uint rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        /// <summary>
        /// スートとランクを初期化する
        /// </summary>
        /// <param name="card_str"></param>
        public Card(string card_str)
        {
            switch (card_str[0])
            {
                case '2': rank =  2; break;
                case '3': rank =  3; break;
                case '4': rank =  4; break;
                case '5': rank =  5; break;
                case '6': rank =  6; break;
                case '7': rank =  7; break;
                case '8': rank =  8; break;
                case '9': rank =  9; break;
                case 'T':
                case 't': rank = 10; break;
                case 'J':
                case 'j': rank = 11; break;
                case 'Q':
                case 'q': rank = 12; break;
                case 'K':
                case 'k': rank = 13; break;
                case '1':
                case 'A':
                case 'a': rank = 14; break;
                default : rank =  0;
                    throw new ArgumentException("unknown rank", "card_str");
            }

            switch (card_str[1])
            {
                case 's': suit = HAND_SUIT.SPADES;		break;
                case 'd': suit = HAND_SUIT.DIAMONDS;	break;
                case 'h': suit = HAND_SUIT.HEARTS;		break;
                case 'c': suit = HAND_SUIT.CLUBS;		break;
                default : suit = HAND_SUIT.UNKNOWN;
                    throw new ArgumentException("unknown suit", "card_str");
	        }
        }
    }
}
