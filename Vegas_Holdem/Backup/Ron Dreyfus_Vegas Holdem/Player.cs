using System;
using System.Collections.Generic;
using System.Text;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public class Player
    {
        #region members

        public enum betstats
        {
            call = 1,
            check = 2,
            raise = 3,
            fold = 4,
            none = 5,
        }

        public enum winHand
        {
            pair = 1,
            twopair = 2,
            threekind = 3,
            straight = 4,
            flush = 5,
            fullhouse = 6,
            fourkind = 7,
            straightflush = 8,
            royalflush = 9,
            none = 10,
        }

        public betstats betStatus;
        public winHand winHands;

        
        // int
        public int money;
        public int bet;
        public int handScore;
        public int winHandScore;
        public int raiseCount;
        public int handsWon;
        
        
        //string
        public string name;

        //bool
        
        public bool isBluff = false;
        public bool isRaising = false;
        public bool aceLow = false;
        public bool aceHigh = false;
        public bool isBust = false;
        public bool prevPlayerIsChecked = false;
        public bool canRaise = true;
        
        /*
        public bool isPair = false;
        public bool is2Pair = false;
        public bool is3Kind = false;
        public bool isFullHouse = false;
        public bool isStraight = false;
        public bool isFlush = false;
        public bool is4Kind = false;
        public bool isStraightFlush = false;
        public bool isRoyalFlush = false;
         */


        //Hand myHand = new hand();
        public Card[] playerHand = new Card[2];

        Card playerHandTemp = new Card();
        
#endregion
        
       

        //contructors
        public Player(string n)
        {
            money = 5000;
            name = n;
            for (int x = 0; x < 2; x++)
            {
                playerHand[x] = playerHandTemp;
            }
            winHands = winHand.none;
            handScore = 0;
            winHandScore = 0;
            betStatus = betstats.none;
            raiseCount = 0;
            
            

        }

        // decontructor
        ~Player() { }

# region methods

        public void Bet()
        {
        }

        public int checkHand()
        {
            int score = 0;
            return score;
            
        }

        public void Raise()
        {
        }

        public void Bluff()
        {
        }

        public void Fold()
        {
        }

        public void Call()
        {
        }

        public void Check()
        {
        }

        

#endregion

    }

    
}

