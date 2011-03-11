namespace HoldemLib
{
    enum HAND_SUIT
    {
        UNKNOWN = 0,
        SPADES = 1, // s
        DIAMONDS, // d
        HEARTS, // h
        CLUBS // c
    };

    enum HAND_CLASS
    {
        NO_PAIR,
        PAIR,
        TWO_PAIR,
        THREE_OF_A_KIND,
        STRAIGHT,
        FLUSH,
        FULL_HOUSE,
        FOUR_OF_A_KIND,
        STRAIGHT_FLUSH
    };
}