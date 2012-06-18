using System;
using System.Collections.Generic;
using System.Text;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public class Card
    {
        public enum Rank
        {
            ace = 1,
            deuce = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9,
            ten = 10,
            jack = 11,
            queen = 12,
            king = 13,
        }

        public enum Suit
        {
            club = 0,
            diamond = 1,
            heart = 2,
            spade = 3,
        }

        public Suit suit;
        public Rank rank;
        public bool isPicked;
        public bool isPaired;
        public int cardValue;
        public int deckValue;
        

        public Card(Suit newSuit, Rank newRank)
        {
            isPicked = false;
            isPaired = false;
            suit = newSuit;
            rank = newRank;

            switch (rank)
            {
                case Rank.ace:
                    cardValue = 14;
                    break;

                case Rank.deuce:
                    cardValue = 2;
                    break;

                case Rank.three:
                    cardValue = 3;
                    break;

                case Rank.four:
                    cardValue = 4;
                    break;

                case Rank.five:
                    cardValue = 5;
                    break;

                case Rank.six:
                    cardValue = 6;
                    break;

                case Rank.seven:
                    cardValue = 7;
                    break;

                case Rank.eight:
                    cardValue = 8;
                    break;

                case Rank.nine:
                    cardValue = 9;
                    break;

                case Rank.ten:
                    cardValue = 10;
                    break;

                case Rank.jack:
                    cardValue = 11;
                    break;

                case Rank.queen:
                    cardValue = 12;
                    break;

                case Rank.king:
                    cardValue = 13;
                    break;
            }

        }

        public Card()
        {
           
        }

        public Card ShallowCopy()
        {
            return (Card)this.MemberwiseClone();
        }



        ~Card() { }
        
    
        
    }

    
    
}
