using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ron_Dreyfus_Vegas_Holdem;
using System.Threading;

namespace Ron_Dreyfus_Vegas_Holdem
{

    public partial class Form1 : Form
    {
        
        #region global variables and objects

        // constants

        // int
        int ante = 5;
        int currentBet = 0;
        int pot = 0;
        int betRound;

        // strings
        
        //........array that stores names of players
        string[] playerNames = new string[60]{
                "Gus Hansen", "Marcel Luske", "Amarillo Slim",
                "Brett Jungblut", "Annie Duke", "Mike Caro",
                "Doyle Brunson", "Antonio Esfandari", "Jamie Gold",
                "Phil Helmuth", "Phil Ivey", "Chris Ferguson",
                "Phil Laak", "Chris Moneymaker", "Daniel Negreanu",
                "Greg Raymer", "Chip Reese", "Scotty Nguyen",
                "Robert Williamson III", "Barry Greenstein", "TJ Cloutier",
                "Shannon Elizabeth", "Liz Lieu", "Jennifier Tilly",
                "Kip Patterson", "Mike Tolliver", "Edward Mulligan",
                "Cindy Sykes", "Oscar Davis", "Al Burrows",
                "Al Bundy", "John Doe", "Archie Bunker", 
                "Jack Shepard", "James Ford", "Kate Austin",
                "Hugo Reyes", "John Locke", "Ben Linus",
                "Jack Bauer", "Elliot Stabler", "Dean Winchester",
                "Michael Schofield", "John Connor", "Bruce Wayne",
                "Clark Kent", "Oliver Queen", "Jon Jones",
                "Wally West", "Diana Prince", "Peter Parker",
                "Bruce Banner", "Steve Rogers", "Luke Skywalker",
                "Han Solo", "James T. Kirk", "Jean Luc Picard",
                "Henry Jones", "Fox Muldar", "Homer Simpson"};

        // bool
        bool newGame = true;
        bool qualified = false;
        bool cardUpdate = false;
        bool isChecked = false;
        bool firstBet = true;
        bool mainPlayerBetDone = false;
        bool finishedHand = false;
        bool finishedAIBetRound = false;
        bool endRound = false;
        

        // declare objects
        Player mainPlayer = new Player("Ron Dreyfus");  // creates the main player
        Player[] AIplayer = new Player[4];          // creates a collection of AI Players
        Dealer Dealer = new Dealer();           // creates the dealer
        
       
        // misc objects
        public Random random1 = new Random();
        //Thread thread1 = new Thread();

        #endregion variables


        // initialize form
        public Form1()
        {
            
            InitializeComponent();
        }


        #region methods

        #region events

        private void Form1_Load(object sender, EventArgs e)
        {
            StartNewGame();
        }


        private void btnFold_Click(object sender, EventArgs e)
        {
            mainPlayer.betStatus = Player.betstats.fold;
            PlayerActions();

        }

        private void btnCallCheck_Click(object sender, EventArgs e)
        {
            mainPlayer.betStatus = Player.betstats.fold;
            PlayerActions();
        }

        private void btnAllIn_Click(object sender, EventArgs e)
        {

        }

        private void pbChipOne_Click(object sender, EventArgs e)
        {

        }

        private void pbChipFive_Click(object sender, EventArgs e)
        {

        }

        private void pbChipTen_Click(object sender, EventArgs e)
        {

        }


        #endregion events


        #region methods: Game

        // sets up new game
        public void StartNewGame()
        {
                     // intiialize Game user interface

            StartQualification();       // starts qualification round
            
            //StartTournament();          // starts tournament mode 
        }

        // Qualification round
        public void StartQualification()
        {
            CreateAIPlayers(31);  // creates the AI players

            // test to see if player wins more than $25,000 or the AI players go bust
            
            //while ((mainPlayer.money <= 25000 && mainPlayer.money > 0) ||
            //    (!AIplayer[0].isBust && !AIplayer[1].isBust &&
            //    !AIplayer[2].isBust && !AIplayer[3].isBust))
                 
            //{
                  // 
           // }


            InitializeGameUI();
            
            //StartHand();

            Qualification();
        }

        public void Qualification()
        {
            if (mainPlayer.money >= 25000 || (AIplayer[0].isBust && AIplayer[1].isBust &&
                AIplayer[2].isBust && AIplayer[3].isBust))
            {
                StartTournament();
            }
            
            StartHand();

            
        }

        // Tournament mode
        public void StartTournament()
        {
            
        }


        // begin new round of tournament
        public void StartRound()
        {
            // update UI

            ante = ante + 5;
            StartHand();
        }

          
        // begin hand
        public void StartHand()
        {
            //declare objects and variables
            Deck cardDeck = new Deck();         // generate a new deck of cards
            pot = 0;                        // reset pot to $0
            
            Dealer.flopDeal = 0;

            




            // AI players ante up and reset values
            for (int x = 0; x < 4; x++)
            {
                pot = pot + ante;
                
                AIplayer[x].money = AIplayer[x].money - ante;
                /*
                AIplayer[x].aceHigh = false;
                AIplayer[x].bet = 0;
                AIplayer[x].betStatus = Player.betstats.none;
                AIplayer[x].handScore = 0;
                AIplayer[x].isRaising = false;
                AIplayer[x].raiseCount = 0;
                AIplayer[x].winHands = Player.winHand.none;
                AIplayer[x].winHandScore = 0;
                 */
            }

            pot = pot + ante;    //ante for main player
            mainPlayer.money = mainPlayer.money - ante;

            UpdateUI();

            // pick cards for main player
            for (int d = 0; d < 2; d++)
            {
                int n = 0;
                while (n == 0)
                {
                    int cardPick = random1.Next(0, 51);  

                    if (!cardDeck.cards[cardPick].isPicked)
                    {
                        Card cardTemp = new Card();
                        cardTemp = cardDeck.cards[cardPick];
                        mainPlayer.playerHand[d] = cardTemp;
                        cardDeck.cards[cardPick].isPicked = true;
                        n++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            

            // pick cards for AI players
            for (int i = 0; i < 4; i++)
            {
                if (!AIplayer[i].isBust)
                {

                    for (int j = 0; j < 2; j++)
                    {
                        int n = 0;
                        while (n == 0)
                        {
                            int cardPick = random1.Next(0, 51);

                            if (!cardDeck.cards[cardPick].isPicked)
                            {
                                Card cardTemp = new Card();
                                cardTemp = cardDeck.cards[cardPick];
                                AIplayer[i].playerHand[j] = cardTemp;
                                cardDeck.cards[cardPick].isPicked = true;
                                n++;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                
                continue;
            }

            // pick cards for dealer
            for (int d = 0; d < 5; d++)
            {
                int n = 0;
                while (n == 0)
                {
                    int cardPick = random1.Next(0, 51);

                    if (!cardDeck.cards[cardPick].isPicked)
                    {
                        Card cardTemp = new Card();
                        cardTemp = cardDeck.cards[cardPick];
                        Dealer.dealerHand[d] = cardTemp;
                        cardDeck.cards[cardPick].isPicked = true;
                        n++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            
            
            currentBet = 0;
            //int playerNum = 0;
            betRound = 1;
            firstBet = true;


            PlayHand();          
        }


        // create the AI players
        public void CreateAIPlayers(int r)
        {
            // choose name for AI players
            for (int i = 0; i < 4; i++)
            {
                Player playerTemp = new Player(" ");
                AIplayer[i] = playerTemp;
                
                string nameTemp = "";
                int next = 0;

                while (next == 0)
                {
                    int namePick = random1.Next(r, 60);

                    if (playerNames[namePick] != "x")
                    {
                        nameTemp = playerNames[namePick];
                        //Player playerTemp = new Player(nameTemp);
                        AIplayer[i].name = nameTemp;
                        playerNames[namePick] = "x";
                        next = 1;
                    } // end if
                    else
                    {
                        continue;
                    }
                } // end while
            } // for

            

        }//end method

         
        
        public void PlayHand()
        {
            bool isChecked = false;
            
            //reset players
            for (int x = 0; x < 4; x++)
            {
                
                AIplayer[x].aceHigh = false;
                AIplayer[x].aceLow = false;
                AIplayer[x].bet = 0;
                AIplayer[x].betStatus = Player.betstats.none;
                AIplayer[x].handScore = 0;
                AIplayer[x].isRaising = false;
                AIplayer[x].raiseCount = 0;
                AIplayer[x].winHands = Player.winHand.none;
                AIplayer[x].winHandScore = 0;
                AIplayer[x].canRaise = true;
            }

            // reset main player variables
            mainPlayer.aceHigh = false;
            mainPlayer.aceLow = false;
            mainPlayer.bet = 0;
            mainPlayer.betStatus = Player.betstats.none;
            mainPlayer.handScore = 0;
            mainPlayer.isRaising = false;
            mainPlayer.raiseCount = 0;
            mainPlayer.winHands = Player.winHand.none;
            mainPlayer.winHandScore = 0;
            
            
            if (betRound > 4)
            {
                StartHand();
            }
                          
            UpdateCards();

            if (mainPlayer.betStatus == Player.betstats.check || 
                mainPlayer.betStatus == Player.betstats.call)
            {
                betRound++;
                PlayHand();
                mainPlayer.betStatus = Player.betstats.none;

                for (int i = 0; i < 5; i++)
                {
                    AIplayer[i].betStatus = Player.betstats.none;
                }

            }
            AIBet();
            UpdateCards();
            UpdateUI();
            PlayerBet();
            
            //firstBet = false;


            

            if (finishedHand)
            {
                DetermineHandWinner();
            }

               
           
            

            

        }
      
        
        
        public void UpdateCards()
        {
            // update AI cards -------------------
            if (!AIplayer[0].isBust || AIplayer[0].betStatus != Player.betstats.fold)
            {
                PB_player1Card1.Visible = true;
                PB_player1Card1.Image = IL_cardPicsPlayers.Images[52];

                PB_player1Card2.Visible = true;
                PB_player1Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player1Card1.Visible = false;
                PB_player1Card2.Visible = false;
            }


            TimerDelay();


            if (!AIplayer[1].isBust || AIplayer[1].betStatus != Player.betstats.fold)
            {
                PB_player2Card1.Visible = true;
                PB_player2Card1.Image = IL_cardPicsPlayers.Images[52];

                PB_player2Card2.Visible = true;
                PB_player2Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player2Card1.Visible = false;
                PB_player2Card2.Visible = false;
            }


            TimerDelay();

            if (!AIplayer[2].isBust || AIplayer[2].betStatus != Player.betstats.fold)
            {
                PB_player3Card1.Visible = true;
                PB_player3Card1.Image = IL_cardPicsPlayers.Images[52];

                PB_player3Card2.Visible = true;
                PB_player3Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player3Card1.Visible = false;
                PB_player3Card2.Visible = false;
            }
            
            

            TimerDelay();

            if (!AIplayer[3].isBust || AIplayer[3].betStatus != Player.betstats.fold)
            {
                PB_player4Card1.Visible = true;
                PB_player4Card1.Image = IL_cardPicsPlayers.Images[52];

                PB_player4Card2.Visible = true;
                PB_player4Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player4Card1.Visible = false;
                PB_player4Card2.Visible = false;
            }


            TimerDelay();

            // update main player cards -------------------
            if (mainPlayer.betStatus != Player.betstats.fold)
            {

                PB_PlrCard1.Visible = true;
                PB_PlrCard1.Image = IL_cardPicsPlayers.Images[mainPlayer.playerHand[0].cardValue];

                PB_PlrCard2.Visible = true;
                PB_PlrCard2.Image = IL_cardPicsPlayers.Images[mainPlayer.playerHand[1].cardValue];
            }


            // update Dealer cards -------------------
            switch (Dealer.flopDeal)
            {
                case 0:
                    PB_DlrCard1.Visible = false;
                    PB_DlrCard2.Visible = false;
                    PB_DlrCard3.Visible = false;
                    PB_DlrCard4.Visible = false;
                    PB_DlrCard5.Visible = false;
                    break;

                case 1:
                    PB_DlrCard1.Visible = true;
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].cardValue];
                    PB_DlrCard2.Visible = true;
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].cardValue];
                    PB_DlrCard3.Visible = true;
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].cardValue];
                    PB_DlrCard4.Visible = false;
                    PB_DlrCard5.Visible = false;
                    break;

                case 2:
                    PB_DlrCard1.Visible = true;
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].cardValue];
                    PB_DlrCard2.Visible = true;
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].cardValue];
                    PB_DlrCard3.Visible = true;
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].cardValue];
                    PB_DlrCard4.Visible = true;
                    PB_DlrCard4.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[3].cardValue];
                    PB_DlrCard5.Visible = false;
                    break;

                case 3:
                    PB_DlrCard1.Visible = true;
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].cardValue];
                    PB_DlrCard2.Visible = true;
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].cardValue];
                    PB_DlrCard3.Visible = true;
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].cardValue];
                    PB_DlrCard4.Visible = true;
                    PB_DlrCard4.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[3].cardValue];
                    PB_DlrCard5.Visible = true;
                    PB_DlrCard5.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[4].cardValue];
                    break;

            }
        }


        public void AIBet()
        {
           
            int playerNum=0;

                
                  
                    // AI players bet
                    
                    while (!finishedAIBetRound)
                    //for (int i = 0; i < 4; i++)
                    {
                        if (!AIplayer[playerNum].isBust)
                        {
                            if (playerNum == 0 && AIplayer[0].betStatus == Player.betstats.call)
                            {
                                finishedAIBetRound = true;
                                continue;
                            }

                            if (firstBet)
                            {
                                scoreHandAI(playerNum, betRound);

                            }
                            
                            if (AIplayer[playerNum].betStatus != Player.betstats.fold)
                            {
                                AIBetDecide(playerNum, betRound);
                            }
                            
                            
                            playerNum++;

                            if (playerNum > 3)
                            {
                                finishedAIBetRound = true;
                            }

                        }
                       
                    }

                    
                        


                    //}






                    /*
                    // main player bet
                    if (firstBet)
                    {
                        scoreHand(playerNum, betRound, true);
                    }

                    firstBet = false;

                    if (AIplayer[0].betStatus == 1 && AIplayer[1].betStatus == 1 &&
                        AIplayer[2].betStatus == 1 && AIplayer[3].betStatus == 1 &&
                        mainPlayer.betStatus == 1)
                    {
                        finishedHand = true;
                    }
                     

                    if (AIplayer[0].betStatus == Player.betstats.call && AIplayer[1].betStatus == Player.betstats.call &&
                    AIplayer[2].betStatus == Player.betstats.call && AIplayer[3].betStatus == Player.betstats.call &&
                    AIplayer[4].betStatus == Player.betstats.call && mainPlayer.betStatus == Player.betstats.call)
                    {
                        finishedHand = true;
                    }
                    
                }
            */
                }
        


        
         
        

        public void DealFlop()
        {
            Dealer.flopDeal++;
            UpdateCards();
        }

       
       


        #endregion

        #region Main Player methods

        public void PlayerBet()
        {
            //btnAllIn.Enabled = true;
            btnCallCheck.Enabled = true;

            if (AIplayer[3].betStatus == Player.betstats.check)
            {
                btnCallCheck.Text = "CHECK";
            }
            else
                btnCallCheck.Text = "CALL";

            
            btnFold.Enabled = true;
            btnRaise.Enabled = true;

        }

        public void PlayerActions()
        {

        }


        #endregion


        #region Methods: Update UI

        public void InitializeGameUI()
        {
            //initialize cards------------------

            //AI players
            PB_player1Card1.Visible = false;
            PB_player1Card1.Image = IL_cardPicsPlayers.Images[52];
            PB_player1Card2.Visible = false;
            PB_player1Card2.Image = IL_cardPicsPlayers.Images[52];

            PB_player2Card1.Visible = false;
            PB_player2Card1.Image = IL_cardPicsPlayers.Images[52];
            PB_player2Card2.Visible = false;
            PB_player2Card2.Image = IL_cardPicsPlayers.Images[52];

            PB_player3Card1.Visible = false;
            PB_player3Card1.Image = IL_cardPicsPlayers.Images[52];
            PB_player3Card2.Visible = false;
            PB_player3Card2.Image = IL_cardPicsPlayers.Images[52];

            PB_player4Card1.Visible = false;
            PB_player4Card1.Image = IL_cardPicsPlayers.Images[52];
            PB_player4Card2.Visible = false;
            PB_player4Card2.Image = IL_cardPicsPlayers.Images[52];

            //Dealer cards
            PB_DlrCard1.Visible = false;
            PB_DlrCard1.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard2.Visible = false;
            PB_DlrCard2.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard3.Visible = false;
            PB_DlrCard3.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard4.Visible = false;
            PB_DlrCard4.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard5.Visible = false;
            PB_DlrCard5.Image = IL_cardPicsPlayers.Images[52];

            //Player cards
            PB_PlrCard1.Visible = false;
            PB_PlrCard1.Image = IL_cardPicsPlayers.Images[52];

            PB_PlrCard2.Visible = false;
            PB_PlrCard2.Image = IL_cardPicsPlayers.Images[52];

            //end card initialize----------------------

            // initialize labels
            lblPlayerName.Text = mainPlayer.name;
            lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
            lblAnteAmount.Text = "$" + Convert.ToString(ante);
            lblCurrentBet.Text = "$" + Convert.ToString(currentBet);
            lblPlr1Name.Text = AIplayer[0].name;
            lblPlr2Name.Text = AIplayer[1].name;
            lblPlr3Name.Text = AIplayer[2].name;
            lblPlr4Name.Text = AIplayer[3].name;
            lblP1Money.Text = "$" + Convert.ToString(AIplayer[0].money);
            lblP2Money.Text = "$" + Convert.ToString(AIplayer[1].money);
            lblP3Money.Text = "$" + Convert.ToString(AIplayer[2].money);
            lblP4Money.Text = "$" + Convert.ToString(AIplayer[3].money);
            lblPot.Text = "$ " + Convert.ToString(pot);           // display pot amount
            
            //static line
            tbStatusLine.Text = "Welcome to Vegas Hold'em by Ron Dreyfus";

            //buttons
            btnAllIn.Enabled = false;
            btnFold.Enabled = false;
            btnRaise.Enabled = false;
            btnCallCheck.Enabled = false;
            btnCallCheck.Text = "";

            pbChipOne.Enabled = false;
            pbChipFive.Enabled = false;
            pbChipTen.Enabled = false;


        }

        public void UpdateUI()
        {
            lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
            lblAnteAmount.Text = "$" + Convert.ToString(ante);

            lblP1Money.Text = "$" + Convert.ToString(AIplayer[0].money);
            lblP2Money.Text = "$" + Convert.ToString(AIplayer[1].money);
            lblP3Money.Text = "$" + Convert.ToString(AIplayer[2].money);
            lblP4Money.Text = "$" + Convert.ToString(AIplayer[3].money);

            lblPot.Text = "$ " + Convert.ToString(pot);           // display pot amount
            lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
            
        }
     

        public void UpdateDealerCards()
        {

            //Dealer cards
            PB_DlrCard1.Visible = true;
            PB_DlrCard1.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard2.Visible = true;
            PB_DlrCard2.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard3.Visible = true;
            PB_DlrCard3.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard4.Visible = true;
            PB_DlrCard4.Image = IL_cardPicsPlayers.Images[52];

            PB_DlrCard5.Visible = true;
            PB_DlrCard5.Image = IL_cardPicsPlayers.Images[52];
        }


        public void FlipAICards()
        {
            PB_player1Card1.Visible = true;
            PB_player1Card1.Image = IL_cardPicsPlayers.Images[AIplayer[0].playerHand[0].cardValue];
            PB_player1Card2.Visible = true;
            PB_player1Card2.Image = IL_cardPicsPlayers.Images[AIplayer[0].playerHand[1].cardValue];

            PB_player2Card1.Visible = true;
            PB_player2Card1.Image = IL_cardPicsPlayers.Images[AIplayer[1].playerHand[0].cardValue];
            PB_player2Card2.Visible = true;
            PB_player2Card2.Image = IL_cardPicsPlayers.Images[AIplayer[1].playerHand[1].cardValue];

            PB_player3Card1.Visible = true;
            PB_player3Card1.Image = IL_cardPicsPlayers.Images[AIplayer[2].playerHand[0].cardValue];
            PB_player3Card2.Visible = true;
            PB_player3Card2.Image = IL_cardPicsPlayers.Images[AIplayer[2].playerHand[1].cardValue];

            PB_player4Card1.Visible = true;
            PB_player4Card1.Image = IL_cardPicsPlayers.Images[AIplayer[3].playerHand[0].cardValue];
            PB_player4Card2.Visible = true;
            PB_player4Card2.Image = IL_cardPicsPlayers.Images[AIplayer[3].playerHand[1].cardValue];

        }
        #endregion

        #region methods: AI

        public void AIBetDecide(int n, int rnd)
        {
            

            int m = n - 1;
            if (m < 0)
                m = 0;

            if (AIplayer[n].handScore > 200)
            {

                
                
                // test for CHECK
                if ((firstBet && n == 0) || isChecked)
                {
                    if (AIplayer[n].handScore < 500)
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 51)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                        }


                    }
                    else
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 21)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                        }
                    }
                } // end test for check

                
                // test to raise
                if ((firstBet && n == 0) || (isChecked) || (AIplayer[n].raiseCount > 2))
                    AIplayer[n].canRaise = false;

                /*
                if (isChecked)
                    AIplayer[n].canRaise = false;


                if (AIplayer[n].betStatus == Player.betstats.check ||
                    AIplayer[m].betStatus == Player.betstats.check)
                     AIplayer[n].canRaise = false;
                
                if 
                    AIplayer[n].canRaise = false;

                */


                if (AIplayer[n].canRaise)
                    {
                        if (AIplayer[n].handScore < 1000)
                        {
                            int checkScore = random1.Next(1, 100);

                            if (checkScore < 51)
                            {
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                            }

                        }
                        else if (AIplayer[n].handScore < 1400)
                        {
                            int checkScore = random1.Next(1, 100);

                            if (checkScore < 71)
                                {
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                                }
                        }
                        else
                        {
                            int checkScore = random1.Next(1, 100);

                            if (checkScore < 91)
                                {
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                                }
                        }
                    } // end can raise
                 
                
                //test to fold or call
                if (AIplayer[n].betStatus == Player.betstats.none)
                {
                    if (AIplayer[n].handScore > 200)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (foldScore < 11)
                            {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            }
                        else
                            AIplayer[n].betStatus = Player.betstats.call;
                    }
                    else
                    {
                        int foldScore = random1.Next(1, 100);

                        if (foldScore < 51)
                            {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            }
                        else
                            AIplayer[n].betStatus = Player.betstats.call;
                    }
                } // end test fold
        
            }// end handscore > 200


                
            

            else   // low score hands
            {
                // test for CHECK
                if ((firstBet && n == 0) || (isChecked))
                {
                    if (AIplayer[n].handScore < 100)
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 71)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                        }

                    }
                    else
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 31)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                        }
                    }
                }

                // test to raise
                if ((firstBet && n == 0) || (isChecked) || (AIplayer[n].raiseCount > 2))
                    AIplayer[n].canRaise = false;
                
                /*
                if (firstBet && n == 0)
                    AIplayer[n].canRaise = false;

                if (AIplayer[n].betStatus == Player.betstats.check ||
                    AIplayer[m].betStatus == Player.betstats.check)
                    AIplayer[n].canRaise = false;

                if (AIplayer[n].raiseCount > 2)
                    AIplayer[n].canRaise = false;

                if (currentBet == 0)
                    AIplayer[n].canRaise = false;
                 */


                if (AIplayer[n].canRaise)
                {
                    if (AIplayer[n].handScore < 200)
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 11)
                        {
                            AIplayer[n].betStatus = Player.betstats.raise;
                            AIplayer[n].raiseCount++;
                        }

                    }
                    
                    else
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 66)
                        {
                            AIplayer[n].betStatus = Player.betstats.raise;
                            AIplayer[n].raiseCount++;
                        }
                    }
                }

                //test to fold or call
                if (AIplayer[n].betStatus == Player.betstats.none)
                {
                    if (AIplayer[n].handScore < 100)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (foldScore < 71)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.call;
                    }
                    else
                    {
                        int foldScore = random1.Next(1, 100);

                        if (foldScore < 41)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.call;
                    }
                }
            }
            
            // take action
            switch (AIplayer[n].betStatus)
            {
                case Player.betstats.call:
                    if (currentBet == 0)
                    {
                        currentBet = AIBetAmount(n);
                    }
                    AIplayer[n].bet = currentBet;
                    AIplayer[n].money = AIplayer[n].money - AIplayer[n].bet;
                    isChecked = false;
                    break;

                case Player.betstats.check:
                    AIplayer[n].bet = 0;
                    //currentBet = 0;
                    break;

                case Player.betstats.raise:
                    AIplayer[n].bet = currentBet + DetermineRaiseAmount(n);
                    AIplayer[n].money = AIplayer[n].money - AIplayer[n].bet;
                    break;

                case Player.betstats.fold:
                    AIplayer[n].bet = 0;
                    break;
            }

            if (AIplayer[n].money <= 0)
                AIplayer[n].money = 0;

            currentBet = AIplayer[n].bet;
            pot = pot + currentBet;


            





        } // end AIBetDecide method

        public int DetermineRaiseAmount(int n)
        {
            int max = 0;
            int min = 0; 

            if (AIplayer[n].handScore > 1700)
            {
                max = AIplayer[n].money;
                min = currentBet;
            }

            else if (AIplayer[n].handScore > 1300)
            {
                max = AIplayer[n].money - (AIplayer[n].money / 3);
                min = currentBet - (currentBet / 3);
            }
            else if (AIplayer[n].handScore > 800)
            {
                max = AIplayer[n].money - (AIplayer[n].money / 2);
                min = currentBet - (currentBet / 2);
            }
            else if (AIplayer[n].handScore > 200)
            {
                
                max = 200;
                min = currentBet;
            }
            else
            {
                max = 50;
                min = 5;
            }




            int raiseBet = random1.Next(min, max);

            while (raiseBet % 5 != 0)
             {
                 raiseBet++;
             }

             if (raiseBet > AIplayer[n].money)
                 raiseBet = AIplayer[n].money;


             return raiseBet;
        }

        public int AIBetAmount(int n)
        {
            int max = 0;
            int min = 0;

            if (AIplayer[n].handScore > 1700)
            {
                max = AIplayer[n].money;
                min = ante + (AIplayer[n].money / 15);
            }

            else if (AIplayer[n].handScore > 1000)
            {
                max = 200;
                min = ante + (AIplayer[n].money / 30);
            }
            else
            {
                max = 50;
                min = ante;
            }

            if (min < ante)
                min = ante;

            if (max < min)
            {
                min = 1;
                max = AIplayer[n].money;
            }

            int myBet = random1.Next(min, max);

            while (myBet % 5 != 0)
            {
                myBet++;
            }

            if (myBet > AIplayer[n].money)
                myBet = AIplayer[n].money;
            

            return myBet;
        }


        
    


        #endregion

        #region methods: win testing

        public void scoreHandAI(int n, int rnd)
        {
            int myScore = 0;
            int cardsScore = 0;
            int kindScore = 0;
            int royalCount = 0;
            bool winTestTemp;
            bool isStraight = false;
            bool isFlush = false;
            int HighCardScore = 0;
            int adjustScore = 0;
            int cards = 0;

            

            
            


            // bet round 1 testing


            if (rnd < 2)
            {

                if (AIplayer[n].playerHand[0].rank == AIplayer[n].playerHand[1].rank)
                    AIplayer[n].winHands = Player.winHand.pair;

                if (AIplayer[n].winHands != Player.winHand.pair)
                {
                    // test for possible flush
                    if (AIplayer[n].playerHand[0].suit == AIplayer[n].playerHand[1].suit)
                    {
                        AIplayer[n].winHands = Player.winHand.flush;
                        isFlush = true;
                        //AIplayer[n].winHands = Player.winHand.flush;
                    }

                    // test for possible straight 
                    if (AIplayer[n].playerHand[1].cardValue == AIplayer[n].playerHand[0].cardValue + 1 ||
                        AIplayer[n].playerHand[0].cardValue == AIplayer[n].playerHand[1].cardValue + 1 ||
                        (AIplayer[n].playerHand[0].rank == Card.Rank.ace && AIplayer[n].playerHand[1].rank == Card.Rank.deuce) ||
                        (AIplayer[n].playerHand[1].rank == Card.Rank.ace && AIplayer[n].playerHand[0].rank == Card.Rank.deuce))
                    {
                        isStraight = true;

                        // test for possible straight flush
                        if (isStraight)
                        {
                            if (isFlush)
                            {
                                AIplayer[n].winHands = Player.winHand.straightflush;

                                // test for possible royal flush
                                if (AIplayer[n].winHands == Player.winHand.straightflush)
                                {
                                    for (int k = 0; k < 2; k++)
                                    {

                                        if (AIplayer[n].playerHand[k].rank == Card.Rank.ace ||
                                            AIplayer[n].playerHand[k].rank == Card.Rank.king ||
                                            AIplayer[n].playerHand[k].rank == Card.Rank.queen ||
                                            AIplayer[n].playerHand[k].rank == Card.Rank.jack ||
                                            AIplayer[n].playerHand[k].rank == Card.Rank.ten)
                                        {
                                            royalCount++;
                                            continue;
                                        }
                                    }
                                    if (royalCount > 1)
                                        AIplayer[n].winHands = Player.winHand.royalflush;

                                }
                            }
                            else
                                AIplayer[n].winHands = Player.winHand.straight;
                        }
                        

                    }
                }

               

            }

            else     // if rnd > 1
            {
                
                // create temp hand
                switch (rnd)
                {

                    case 2:
                        cards = 5;
                        break;

                    case 3:
                        cards = 6;
                        break;

                    case 4:
                        cards = 7;
                        break;

                }

                Card[] tempHand = new Card[cards];

                for (int c = 0; c < cards; c++)
                {
                    if (c < 2)
                        tempHand[c] = AIplayer[n].playerHand[c];
                    else
                        tempHand[c] = Dealer.dealerHand[c - 2];
                }


                isFlush = TestFlush(n, rnd);
                if (isFlush)
                    AIplayer[n].winHands = Player.winHand.flush;

                isStraight = TestStraight(n, rnd);

                if (isStraight)
                {
                    if (isStraight && isFlush)
                    {
                        if (AIplayer[n].aceHigh)
                            AIplayer[n].winHands = Player.winHand.royalflush;
                        else
                            AIplayer[n].winHands = Player.winHand.straightflush;
                    }
                    else
                        AIplayer[n].winHands = Player.winHand.straight;
                }


                if (isStraight == false && isFlush == false)
                {
                    kindScore = TestPairKind(AIplayer[n], rnd);

                    switch (kindScore)
                    {
                        case 1:
                            AIplayer[n].winHands = Player.winHand.pair;
                            break;

                        case 2:
                            AIplayer[n].winHands = Player.winHand.twopair;
                            break;

                        case 3:
                            AIplayer[n].winHands = Player.winHand.threekind;
                            break;

                        case 4:
                            AIplayer[n].winHands = Player.winHand.fullhouse;
                            break;

                        case 6:
                            AIplayer[n].winHands = Player.winHand.fourkind;
                            break;
                    }

                }

            }

            // give scores and test high cards

            if (AIplayer[n].winHands != Player.winHand.none)
            {

                // score royal flush
                if (AIplayer[n].winHands == Player.winHand.royalflush)
                {
                    if (rnd < 2)
                        AIplayer[n].handScore = 150;
                    else
                        AIplayer[n].handScore = 2000;
                }


                // score straight flush
                if (AIplayer[n].winHands == Player.winHand.straightflush)
                {
                    if (rnd < 2)
                        AIplayer[n].handScore = 150;
                    else
                    {
                        AIplayer[n].handScore = 1500;
                    }

                    
                    
                }

                // score four of a kind
                if (AIplayer[n].winHands == Player.winHand.fourkind)
                {

                    AIplayer[n].handScore = 1200;       
                    
                }

                // score full house
                if (AIplayer[n].winHands == Player.winHand.fullhouse)
                {

                    AIplayer[n].handScore = 1000;

                }


                // score flush
                if (AIplayer[n].winHands == Player.winHand.flush)
                {
                    if (rnd < 2)
                        AIplayer[n].handScore = 100;
                    else
                    {
                        AIplayer[n].handScore = 300;
                    }

                    
                }


                // score straight
                if (AIplayer[n].winHands == Player.winHand.straight)
                {
                    if (rnd < 2)
                        AIplayer[n].handScore = 120;
                    else
                    {
                        AIplayer[n].handScore = 200;
                    }

                    
                   
                }


                // score three of a kind
                if (AIplayer[n].winHands == Player.winHand.threekind)
                {

                    AIplayer[n].handScore = 100;

                    
                }

                // score two pair
                if (AIplayer[n].winHands == Player.winHand.twopair)
                {

                    AIplayer[n].handScore = 100;

                    
                }

                // score pair
                if (AIplayer[n].winHands == Player.winHand.pair)
                {

                    if (rnd < 2)
                        AIplayer[n].handScore = 700;
                    else
                        AIplayer[n].handScore = 100;

                    
                }

            }

            AddRankValues(n, rnd);
                            
                
        } // end scoreHand

        
        
        public int TestPairKind(Player playerTemp, int rnd)
        {
            int s = 0;
            // create temp hand
            Card[] tempHand = new Card[7];
            for (int c = 0; c < 7; c++)
            {
                if (c < 2)
                    tempHand[c] = playerTemp.playerHand[c];
                else
                    tempHand[c] = Dealer.dealerHand[c-2];
            }

            

            for (int x = 0; x < rnd + 4; x++)
            {
                for (int y = x + 1; y < rnd + 3; y++)
                {
                    if (tempHand[x].rank == tempHand[y].rank)
                    {
                        s++;
                    }
                }
                
            }
            
            return s;
        }
        
       

        public bool TestFlush(int n, int rnd)
        {
             // create temp hand
            Card[] tempHand = new Card[7];
            int count = 0;

            // populate hand
            for (int c = 0; c < 7; c++)
            {
                if (c < 2)
                    tempHand[c] = AIplayer[n].playerHand[c];
                else
                    tempHand[c] = Dealer.dealerHand[c-2];
            }

            //test for flush
            for (int x = 0; x < rnd + 4; x++)
            {
                for (int y = x + 1; y < rnd + 3; y++)
                {
                    if (tempHand[x].suit == tempHand[y].suit)
                    {
                        count++;
                        if (count >= 4)
                        {
                            return true;
                        }
                    }
                }
                
                
            }
            return false;
        }

        public bool TestStraight(int n, int rnd)
        {
            // create temp hand
            Card[] tempHand = new Card[7];
            Card[] tempHand2 = new Card[7];
            Card swapCard = new Card();
            int count = 0;
            int straightScore = 0;
            byte aceCount = 0;
            bool straight = false;
            

            // populate hand
            for (int c = 0; c < 7; c++)
            {
                if (c < 2)
                    tempHand[c] = AIplayer[n].playerHand[c];
                else
                    tempHand[c] = Dealer.dealerHand[c-2];

            }

            

            // check for aces
            for (int a = 0; a < rnd+3; a++)
            {
                if (tempHand[a].rank == Card.Rank.ace)
                    aceCount++;
            }


            //sort hand
            for (int x = 0; x < rnd + 4; x++)
            {
                for (int y = x + 1; y < rnd + 3; y++)
                {
                    if (tempHand[x].cardValue > tempHand[y].cardValue)
                    {
                        swapCard = tempHand[y];
                        tempHand[y] = tempHand[x];
                        tempHand[x] = swapCard;
                    }
                }
            }
            
            
          
            
            // test for straight
            for (int x = 0; x < rnd + 2; x++)
            {
                if (tempHand[x].cardValue == tempHand[x+1].cardValue + 1)
                {
                    count++;
                }           
            }

            if (count >= 4)
                straight = true;

   
            // test for high ace
            
            if (aceCount >1 && aceCount < 4)
            {
                
                if (straight == true)
                {
                    if ((tempHand[6].rank == Card.Rank.ace && tempHand[5].rank == Card.Rank.king) ||
                        (tempHand[5].rank == Card.Rank.ace && tempHand[4].rank == Card.Rank.king) ||
                        (tempHand[4].rank == Card.Rank.ace && tempHand[3].rank == Card.Rank.king))
                    {
                        AIplayer[n].aceHigh = true;
                    }
                }
                
                
                
                // test for low ace
                if (!AIplayer[n].aceHigh)
                {
                    tempHand2 = tempHand;

                    // reassign card value
                    for (int z = 0; z < rnd+4; z++)
                    {
                        if (tempHand2[z].cardValue == 14)
                        {
                            tempHand2[z].cardValue = 1;
                        }
                    }

                    // re-sort
                    for (int x = 0; x < rnd + 4; x++)
                    {
                        for (int y = x + 1; y < rnd + 3; y++)
                        {
                            if (tempHand2[x].cardValue > tempHand2[y].cardValue)
                            {
                                swapCard = tempHand2[y];
                                tempHand2[y] = tempHand2[x];
                                tempHand2[x] = swapCard;
                            }
                        }
                    }

                    // re-check for straight
                    int count2 = 0;
                    
                    for (int x = 0; x < rnd + 2; x++)
                    {
                        if (tempHand[x].cardValue == tempHand[x+1].cardValue + 1)
                        {
                            count2++;
                        }  
                    }

                    if (count2 >= 4)
                    {
                        straight = true;
                        AIplayer[n].aceLow = true;
                    }
                }

          
            
            if (straight)
            {
                return straight;
            }
            else
                return false;

        }
        return false;
        }


        public void AddRankValues(int n, int rnd)
        {
            int cards = 0;

            switch (rnd)
            {
                case 1:
                    cards = 2;
                    break;
                case 2:
                    cards = 5;
                    break;
                case 3:
                    cards = 6;
                    break;
                case 4:
                    cards = 7;
                    break;
            }



            for (int c = 0; c < cards; c++)
            {
                

                switch (AIplayer[n].playerHand[c].rank)
                {
                    case Card.Rank.ace:
                        if (AIplayer[n].aceLow)
                            AIplayer[n].handScore = AIplayer[n].handScore + 5;
                        else
                            AIplayer[n].handScore = AIplayer[n].handScore + 200;
                        break;

                    case Card.Rank.king:
                        AIplayer[n].handScore = AIplayer[n].handScore + 100;
                        break;

                    case Card.Rank.queen:
                        AIplayer[n].handScore = AIplayer[n].handScore + 75;
                        break;

                    case Card.Rank.jack:
                        AIplayer[n].handScore = AIplayer[n].handScore + 50;
                        break;

                    case Card.Rank.ten:
                        AIplayer[n].handScore = AIplayer[n].handScore + 30;
                        break;

                    case Card.Rank.nine:
                        AIplayer[n].handScore = AIplayer[n].handScore + 20;
                        break;

                    case Card.Rank.eight:
                        AIplayer[n].handScore = AIplayer[n].handScore + 20;
                        break;

                    case Card.Rank.seven:
                        AIplayer[n].handScore = AIplayer[n].handScore + 20;
                        break;

                    case Card.Rank.six:
                        AIplayer[n].handScore = AIplayer[n].handScore + 10;
                        break;

                    case Card.Rank.five:
                        AIplayer[n].handScore = AIplayer[n].handScore + 10;
                        break;

                    case Card.Rank.four:
                        AIplayer[n].handScore = AIplayer[n].handScore + 10;
                        break;

                    case Card.Rank.three:
                        AIplayer[n].handScore = AIplayer[n].handScore + 5;
                        break;

                    case Card.Rank.deuce:
                        AIplayer[n].handScore = AIplayer[n].handScore + 5;
                        break;
                }


            }
        }

        public void EndGame()
        {

        }

        public void DetermineHandWinner()
        {
        }



        #endregion

        #region methods: utilities

        public void TimerDelay()
        {

            //Thread.Sleep(200);

        }
        #endregion

       







        #endregion

    }
}