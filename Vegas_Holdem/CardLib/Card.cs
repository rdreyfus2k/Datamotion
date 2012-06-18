using System;
using System.Collections.Generic;
using System.Text;

namespace CardLib
{
    public class Card
    {
        private readonly CardLib.Suit suit;
        private readonly CardLib.Rank rank;

        public Card(CardLib.Suit newSuit, CardLib.Rank newRank)
        {
            suit = newSuit;
            rank = newRank;
            //throw new System.NotImplementedException();
        }

        private Card()
        {

        }

        //public override 
    }
}
