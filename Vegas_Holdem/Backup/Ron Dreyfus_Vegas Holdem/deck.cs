using System;
using System.Collections.Generic;
using System.Text;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public class Deck
    {

        public Card[] cards = new Card[52];
        
        public Deck()
        {
            
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    int val = suitVal * 13 + rankVal - 1;
                    cards[val] = new Card((Card.Suit)suitVal, (Card.Rank)rankVal);
                    //cards[val].cardValue = val+1;

                }
            }
        }

        /*public Card GetCard(int cardNum)
        {
            
            if (cardNum >= 0 && cardNum <= 51)
                return cards[cardNum];
             

        }
         * */


        public void Shuffle()
        {
             /*
            Card[] newDeck = new Card[52];
            bool[] assigned = new bool[52];
            Random sourceGen = new Random();

            

            for (int i = 0; i < 52; i++)
            {
                int destCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    destCard = sourceGen.Next(52);
                    if (assigned[destCard] == false)
                        foundCard = true;
                }
                assigned[destCard] = true;
                newDeck[destCard] = cards[i];
            }

            newDeck.CopyTo(cards, 0);
            */



        }


    }
}



