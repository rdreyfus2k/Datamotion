using System;
using System.Collections.Generic;
using System.Text;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public class Deck
    {

        public Card[] cards = new Card[52];
        
        
        //constructor
        public Deck()
        {
            
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    int val = suitVal * 13 + rankVal - 1;
                    cards[val] = new Card((Card.Suit)suitVal, (Card.Rank)rankVal);
                    cards[val].deckValue = val;

                }
            }
        }

        
        // deconstructor
        ~Deck() { }


    }
}



