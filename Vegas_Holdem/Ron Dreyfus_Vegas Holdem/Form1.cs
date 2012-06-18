using System;
using System.Collections;
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
        public int ante = 5;
        public int currentBet = 0;
        public int raiseBet = 0;
        public int pot = 0;
        public int betRound;
        public int callCount = 0;
        public int AIfoldCount = 0;
        public int prevBet = 0;
        public int round = 0;

        // strings
        public string mainPlayerName = "";
        
        //........array that stores names of players
        public string[] playerNames = new string[60]{
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
                "Al Brady", "John Dorian", "Amos Bankridge", 
                "Jake Sheppard", "Jim Ford", "Katie Austino",
                "Hermando Reyes", "John Lockford", "Ben Loonis",
                "Jack Bryerman", "Elliot Slade", "Frank Winchester",
                "Justin Schofield", "John Baker", "Bruce Tyson",
                "Clark Morris", "Oliver Potter", "Ron Jones",
                "Wally Greenberg", "Diana O'Dell", "Peter Pearlman",
                "David Banichek", "Steven Rogers", "Luke Davis",
                "Harry Solston", "Tyrone Kirk", "Remey Picard",
                "Henry Smith Jr.", "Paul Muldarano", "Homer Murphy"};

        // bool
        
        public bool qualified = false;
        public bool isChecked = false;
        public bool firstBet = true;
        public bool finishedHand = false;
        public bool finishedAIBetRound = false;
        public bool betBtnClicked = false;
        public bool isRaised = false;
        public bool nextBetRound = false;
        public bool newGame = false;
        

        // declare objects
        public Player mainPlayer = new Player();  // creates the main player
        public Player[] AIplayer = new Player[4];          // creates a collection of AI Players
        public Player winnerHand = new Player();
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

        #region methods: events

        // loads main form
        private void Form1_Load(object sender, EventArgs e)
        {



            PreGame();       // starts a new game
        }


        // actions for "Fold" button 
        private void btnFold_Click(object sender, EventArgs e)
        {
            mainPlayer.betStatus = Player.betstats.fold;

            btnRaise.Enabled = false;
            btnFold.Enabled = false;
            btnCheck.Enabled = false;
            pbChipFive.Enabled = false;
            pbChipTen.Enabled = false;
            btnAllIn.Enabled = false;
            btnFold.Enabled = false;
            btnCallBet.Enabled = false;

            UpdateCards();
           
            PlayHand();

        }

        // actions for "Check" button 
        private void btnCheck_Click(object sender, EventArgs e)
        {
            

            mainPlayer.betStatus = Player.betstats.check;
            isChecked = true;
            
            btnRaise.Enabled = false;
            btnFold.Enabled = false;
            btnCheck.Enabled = false;
            pbChipFive.Enabled = false;
            pbChipTen.Enabled = false;
            btnAllIn.Enabled = false;
            btnFold.Enabled = false;
            btnCallBet.Enabled = false;

            
            PlayHand();

        }

        // actions for "Call / Bet" Button
        private void btnCallBet_Click(object sender, EventArgs e)
        {
            
            
            if (btnCallBet.Text == "MAKE BET" || btnCallBet.Text == "PLACE BET")
            {

                // test if call button has been pressed the first time
                if (!betBtnClicked)
                {
                    //mainPlayer.betStatus = Player.betstats.call;

                    btnRaise.Enabled = false;
                    btnFold.Enabled = false;
                    btnCheck.Enabled = false;
                    pbChipFive.Enabled = true;
                    pbChipTen.Enabled = true;
                    if (betRound == 4)
                    {
                        btnAllIn.Enabled = true;
                    }
                    betBtnClicked = true;
                    btnCallBet.Text = "PLACE BET";
                    isChecked = false;
                    btnCallBet.Enabled = false;
                    
                }
                else
                {
                    if (mainPlayer.betStatus != Player.betstats.raise)
                    {
                        mainPlayer.betStatus = Player.betstats.call;
                      
                        callCount++;
                    }

                    if (isRaised)
                        mainPlayer.bet = currentBet - prevBet;
                    else
                        mainPlayer.bet = currentBet;
                    
                    pot = pot + currentBet;
                    
                    mainPlayer.money = mainPlayer.money - currentBet;

                    if (mainPlayer.money < 0)
                        mainPlayer.money = 0;

                    lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
                    lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
                    lblPot.Text = "$" + Convert.ToString(pot);
                    
                    betBtnClicked = false;
                    btnCallBet.Enabled = false;             

                    btnRaise.Enabled = false;
                    btnFold.Enabled = false;
                    btnCheck.Enabled = false;
                    pbChipFive.Enabled = false;
                    pbChipTen.Enabled = false;
                    btnAllIn.Enabled = false;
                   
                    
                    PlayHand();

                }
            }
            else
            {
                mainPlayer.betStatus = Player.betstats.call;
                callCount++;
                //isRaised = false;
                isChecked = false;
                pot = pot + currentBet;
                mainPlayer.bet = currentBet;
                mainPlayer.money = mainPlayer.money - currentBet;

                if (mainPlayer.money < 0)
                    mainPlayer.money = 0;

                lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
                lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
                lblPot.Text = "$" + Convert.ToString(pot);

                btnCallBet.Enabled = false;
                btnRaise.Enabled = false;
                btnFold.Enabled = false;
                btnCheck.Enabled = false;
                pbChipFive.Enabled = false;
                pbChipTen.Enabled = false;
                btnAllIn.Enabled = false;
                betBtnClicked = false;
               

                PlayHand();

            }

        }
        

        
        // actions for "Raise" button
        private void btnRaise_Click(object sender, EventArgs e)
         {
            mainPlayer.betStatus = Player.betstats.raise;
            isRaised = true;
            btnRaise.Enabled = false;
            btnFold.Enabled = false;
            btnCheck.Enabled = false;
            pbChipFive.Enabled = true;
            pbChipTen.Enabled = true;
            
            if (betRound == 4)
            {
                btnAllIn.Enabled = true;
            }

            betBtnClicked = true;
            btnCallBet.Text = "PLACE BET";
            btnCallBet.Enabled = false;
            callCount = 1;
            prevBet = currentBet;
         }

        // actions for "All In" button 
        private void btnAllIn_Click(object sender, EventArgs e)
        {
            currentBet = mainPlayer.money;
            mainPlayer.money = mainPlayer.money - mainPlayer.money;
            lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
            lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
            btnAllIn.Enabled = false;
            pbChipFive.Enabled = false;
            pbChipTen.Enabled = false;
            btnCallBet.Enabled = true;

        }


        // actions for $5 chip
        private void pbChipFive_Click(object sender, EventArgs e)
        {
            currentBet = currentBet + 5;

            //mainPlayer.money = mainPlayer.money - currentBet;

            if ((mainPlayer.money - currentBet) < 0)
            {
                mainPlayer.money = 0;
                pbChipFive.Enabled = false;
                pbChipTen.Enabled = false;
                btnAllIn.Enabled = false;

            }

            lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
            //lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
            
            btnCallBet.Enabled = true;

        }

        // actions for $10 chip
        private void pbChipTen_Click(object sender, EventArgs e)
        {
            currentBet = currentBet + 10;

            //mainPlayer.money = mainPlayer.money - currentBet;

            if ((mainPlayer.money - currentBet) < 0)
            {
                mainPlayer.money = 0;
                pbChipFive.Enabled = false;
                pbChipTen.Enabled = false;
                btnAllIn.Enabled = false;
            }

            lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);
            //lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);

            btnCallBet.Enabled = true;
        }


        #endregion events


        #region methods: Game

        public void PreGame()
        {
            StartGameUI();


            welcome form5 = new welcome(this);
            form5.Show();
            return;
        }
        
        // sets up new game
        public void StartNewGame()
        {
            mainPlayer.name = mainPlayerName;

            if (newGame)
                ResetAllPlayers();

            StartQualification();       // starts qualification round
            
        }

        // Qualification round
        public void StartQualification()
        {
            CreateAIPlayers(31, 60);  // creates the AI players

            InitializeGameUI();
            
            Qualification();
        }

        public void Qualification()
        {
            if (mainPlayer.money >= 25000 || (AIplayer[0].isBust && AIplayer[1].isBust &&
                AIplayer[2].isBust && AIplayer[3].isBust))
            {
                qualified = true;
                EnterTournament form4 = new EnterTournament(this);
                form4.Show();
                return;
            }
            
            StartHand();

            
        }

        // Tournament mode
        public void StartTournament()
        {

            round = 0;
            StartRound();

            
        }


        // begin new round of tournament
        public void StartRound()
        {

            if (round > 0)
            {
                NextRound form10 = new NextRound(this);
                form10.Show();
                //return;
            }

            round++;

            if (round > 5)
                DetermineGameWinner();

            newGame = true;
            ResetAllPlayers();


            CreateAIPlayers(1, 30);  // creates the AI players
            
            ante = ante + 5;

            InitializeGameUI();

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
                if (!AIplayer[x].isBust)
                {
                    pot = pot + ante;

                    AIplayer[x].money = AIplayer[x].money - ante;

                    if (AIplayer[x].money < 0)
                    {
                        AIplayer[x].money = 0;
                        AIplayer[x].isBust = true;
                    }
                }

            }

            pot = pot + ante;    //ante for main player
            mainPlayer.money = mainPlayer.money - ante;

            if (mainPlayer.money < 0)
            {
                mainPlayer.money = 0;
                EndGame();
                return;
            }



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
            betRound = 1;
            firstBet = true;
            callCount = 0;
            AIfoldCount = 0;
            finishedAIBetRound = false;
            finishedHand = false;
            nextBetRound = false;
            isChecked = false;
            isRaised = false;
            

            ResetAllPlayers();

            PlayHand();          
        }



        public void PlayHand()
        {

            if (isChecked || callCount >= 5)
                nextBetRound = true;

            if (nextBetRound)
            {
                betRound++;
                if (betRound > 4)
                {
                    DetermineHandWinner();
                    return;
                }


                ResetAllPlayers();

                firstBet = true;
                nextBetRound = false;
                Dealer.flopDeal++;
                callCount = 0;
                finishedAIBetRound = false;
                finishedHand = false;
                isChecked = false;
                currentBet = 0;
                isRaised = false;

            }


            UpdateCards();
            UpdateUI();

            finishedAIBetRound = false;
            
            AIBet();
          
            if (finishedHand)
            {
                nextBetRound = true;
                PlayHand();
                return;

            }

            if (mainPlayer.betStatus != Player.betstats.fold)
            {
                PlayerBet();

            }
            else
                PlayHand();

            
        }


        // create the AI players
        public void CreateAIPlayers(int min, int max)
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
                    int namePick = random1.Next(min, max);

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
            } // end for

            

        }//end method

         
        
        
      
        
        
        public void UpdateCards()
        {
            // update AI cards -------------------
            if (!AIplayer[0].isBust && AIplayer[0].betStatus != Player.betstats.fold)
            {
                PB_player1Card1.Visible = true;
                PB_player1Card1.Image = IL_cardPicsPlayers.Images[52];

                TimerDelay();

                PB_player1Card2.Visible = true;
                PB_player1Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player1Card1.Visible = false;
                PB_player1Card2.Visible = false;
            }


            TimerDelay();


            if (!AIplayer[1].isBust && AIplayer[1].betStatus != Player.betstats.fold)
            {
                PB_player2Card1.Visible = true;
                PB_player2Card1.Image = IL_cardPicsPlayers.Images[52];

                TimerDelay();

                PB_player2Card2.Visible = true;
                PB_player2Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player2Card1.Visible = false;
                PB_player2Card2.Visible = false;
            }


            TimerDelay();

            if (!AIplayer[2].isBust && AIplayer[2].betStatus != Player.betstats.fold)
            {
                PB_player3Card1.Visible = true;
                PB_player3Card1.Image = IL_cardPicsPlayers.Images[52];

                TimerDelay();

                PB_player3Card2.Visible = true;
                PB_player3Card2.Image = IL_cardPicsPlayers.Images[52];
            }
            else
            {
                PB_player3Card1.Visible = false;
                PB_player3Card2.Visible = false;
            }
            
            

            TimerDelay();

            if (!AIplayer[3].isBust && AIplayer[3].betStatus != Player.betstats.fold)
            {
                PB_player4Card1.Visible = true;
                PB_player4Card1.Image = IL_cardPicsPlayers.Images[52];

                TimerDelay();

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
                PB_PlrCard1.Image = IL_cardPicsPlayers.Images[mainPlayer.playerHand[0].deckValue];

                TimerDelay();

                PB_PlrCard2.Visible = true;
                PB_PlrCard2.Image = IL_cardPicsPlayers.Images[mainPlayer.playerHand[1].deckValue];
            }
            else
            {
                PB_PlrCard1.Visible = false;
                PB_PlrCard2.Visible = false;
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
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].deckValue];
                    PB_DlrCard2.Visible = true;
                    TimerDelay();
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].deckValue];
                    PB_DlrCard3.Visible = true;
                    TimerDelay();
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].deckValue];
                    PB_DlrCard4.Visible = false;
                    TimerDelay();
                    PB_DlrCard5.Visible = false;
                    break;

                case 2:
                    PB_DlrCard1.Visible = true;
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].deckValue];
                    PB_DlrCard2.Visible = true;
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].deckValue];
                    PB_DlrCard3.Visible = true;
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].deckValue];
                    PB_DlrCard4.Visible = true;
                    PB_DlrCard4.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[3].deckValue];
                    PB_DlrCard5.Visible = false;
                    break;

                case 3:
                    PB_DlrCard1.Visible = true;
                    PB_DlrCard1.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[0].deckValue];
                    PB_DlrCard2.Visible = true;
                    PB_DlrCard2.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[1].deckValue];
                    PB_DlrCard3.Visible = true;
                    PB_DlrCard3.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[2].deckValue];
                    PB_DlrCard4.Visible = true;
                    PB_DlrCard4.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[3].deckValue];
                    PB_DlrCard5.Visible = true;
                    PB_DlrCard5.Image = IL_cardPicsPlayers.Images[Dealer.dealerHand[4].deckValue];
                    break;

            }
        }


        public void AIBet()
        {
           
            int playerNum=0;

                
                  
                    // AI players bet
                    
                    while (!finishedAIBetRound)
                   
                    {
                        if (!AIplayer[playerNum].isBust)
                        {


                            if (callCount >= 5)
                            {
                                finishedAIBetRound = true;

                                continue;
                            }

                            if (firstBet && AIplayer[playerNum].betStatus != Player.betstats.fold)
                            {
                                scoreHandAI(playerNum, betRound);

                            }

                            if (AIplayer[playerNum].betStatus != Player.betstats.fold)
                            {
                                AIBetDecide(playerNum, betRound);

                                if ((callCount >= 5) || (AIfoldCount >= 4))
                                {
                                    finishedAIBetRound = true;
                                    finishedHand = true;
                                    continue;
                                }
                            }
                            else
                            {
                                callCount++;
                                if ((callCount >= 5) || (AIfoldCount >= 4))
                                {
                                    finishedAIBetRound = true;
                                    finishedHand = true;
                                    continue;
                                }
                            }

                            UpdateCards();
                            UpdateUI();

                            playerNum++;

                            if (playerNum > 3)
                            {
                                finishedAIBetRound = true;
                            }
                        }
                        else
                        {
                            playerNum++;
                            if (playerNum > 3)
                            {
                                finishedAIBetRound = true;
                            }
                        }


                    }
                }
        


        
         
        

       

        public void ResetAllPlayers()
        {
            //reset players
            if (!newGame)
            {
                for (int x = 0; x < 4; x++)
                {

                    AIplayer[x].aceHigh = false;
                    AIplayer[x].aceLow = false;
                    AIplayer[x].bet = 0;

                    if ((AIplayer[x].betStatus != Player.betstats.fold) || (betRound < 2))
                    {
                        AIplayer[x].betStatus = Player.betstats.none;
                    }
                    AIplayer[x].handScore = 0;
                    AIplayer[x].isRaising = false;
                    AIplayer[x].raiseCount = 0;
                    AIplayer[x].winHands = Player.winHand.none;
                    AIplayer[x].winHandScore = 0;
                    AIplayer[x].canRaise = true;
                    AIplayer[x].winnings = 0;
                    AIplayer[x].isHandWinner = false;
                    AIplayer[x].firstHandBet = false;

                    if (AIplayer[x].money <= 0)
                        AIplayer[x].isBust = true;

                }
            }

            // reset main player variables
            mainPlayer.aceHigh = false;
            mainPlayer.aceLow = false;
            mainPlayer.bet = 0;
            if ((mainPlayer.betStatus != Player.betstats.fold) || (betRound < 2))
            {
                mainPlayer.betStatus = Player.betstats.none;
            }
            //mainPlayer.betStatus = Player.betstats.none;
            mainPlayer.handScore = 0;
            mainPlayer.isRaising = false;
            mainPlayer.raiseCount = 0;
            mainPlayer.winHands = Player.winHand.none;
            mainPlayer.winHandScore = 0;
            mainPlayer.isHandWinner = false;
            mainPlayer.winnings = 0;

            if (newGame)
                mainPlayer.isBust = false;

            newGame = false;

        }


        #endregion

        #region Main Player methods

        public void PlayerBet()
        {
            //btnAllIn.Enabled = true;

            firstBet = false;

            if (isChecked)
            {
                btnCheck.Enabled = true;
               
                btnCallBet.Enabled = true;
                btnCallBet.Text = "MAKE BET";
                btnRaise.Enabled = false;

            }
            else
            {
               
                btnCallBet.Enabled = true;
                btnCheck.Enabled = false;
                if (currentBet != 0)
                {
                    btnRaise.Enabled = true;
                }
                btnCallBet.Text = "CALL";
            }
            
            btnFold.Enabled = true;
            
            
    }
     

        #endregion


        #region Methods: Update UI

        public void InitializeGameUI()
        {
            
            lblPlayerName.Visible = true;
            lblPlrMoney.Visible = true;
            lblAnteAmount.Visible = true;
            lblCurrentBet.Visible = true;
            lblPlr1Name.Visible = true;
            lblPlr2Name.Visible = true;
            lblPlr3Name.Visible = true;
            lblPlr4Name.Visible = true;
            lblP1Money.Visible = true;
            lblP2Money.Visible = true;
            lblP3Money.Visible = true;
            lblP4Money.Visible = true;
            lblPot.Visible = true;
            lblAIP1stat.Visible = true;
            lblAIP2stat.Visible = true;
            lblAIP3stat.Visible = true;
            lblAIP4stat.Visible = true;
            btnAllIn.Visible = true;
            btnFold.Visible = true;
            btnRaise.Visible = true;
            btnCheck.Visible = true;
            pbChipFive.Visible = true;
            pbChipTen.Visible = true;
            btnCallBet.Visible = true;
            lblPlrBet.Visible = true;
            label3.Visible = true;
            

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
            lblAIP1stat.Text = "";
            lblAIP2stat.Text = "";
            lblAIP3stat.Text = "";
            lblAIP4stat.Text = "";

            
            //static line
            tbStatusLine.Text = "Welcome to Vegas Hold'em by Ron Dreyfus";

            //buttons
            btnAllIn.Enabled = false;
            btnFold.Enabled = false;
            btnRaise.Enabled = false;
            btnCheck.Enabled = false;
            btnCheck.Text = "CHECK";

            //pbChipOne.Enabled = false;
            pbChipFive.Enabled = false;
            pbChipTen.Enabled = false;


        }

        public void StartGameUI()
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
            lblPlayerName.Visible = false;
            lblPlrMoney.Visible = false;
            lblAnteAmount.Visible = false;
            lblCurrentBet.Visible = false;
            lblPlr1Name.Visible = false;
            lblPlr2Name.Visible = false;
            lblPlr3Name.Visible = false;
            lblPlr4Name.Visible = false;
            lblP1Money.Visible = false;
            lblP2Money.Visible = false;
            lblP3Money.Visible = false;
            lblP4Money.Visible = false;
            lblPot.Visible = false;
            lblAIP1stat.Visible = false;
            lblAIP2stat.Visible = false;
            lblAIP3stat.Visible = false;
            lblAIP4stat.Visible = false;
            lblPlrBet.Visible = false;
            label3.Visible = false;


            //static line
            tbStatusLine.Text = "Welcome to Vegas Hold'em by Ron Dreyfus";

            //buttons
            btnAllIn.Visible = false;
            btnFold.Visible = false;
            btnRaise.Visible = false;
            btnCheck.Visible = false;       
            pbChipFive.Visible = false;
            pbChipTen.Visible = false;
            btnCallBet.Visible = false;

        }
        public void UpdateUI()
        {
            lblPlrMoney.Text = "$" + Convert.ToString(mainPlayer.money);
            lblAnteAmount.Text = "$" + Convert.ToString(ante);


            for (int x = 0; x < 4; x++)
            {

                switch (x)
                {
                    case 0:
                        if (!AIplayer[x].isBust)
                        {
                            lblP1Money.Visible = true;
                            lblAIP1stat.Visible = true;
                            lblPlr1Name.Visible = true;
                            lblP1Money.Text = "$" + Convert.ToString(AIplayer[x].money);
                            if (AIplayer[x].betStatus == Player.betstats.none)
                                lblAIP1stat.Text = "";
                            else
                            {
                                if (AIplayer[x].firstHandBet && AIplayer[x].betStatus == Player.betstats.call)
                                {
                                    lblAIP1stat.Text = "Bet";
                                }
                                else
                                    lblAIP1stat.Text = Convert.ToString(AIplayer[x].betStatus);
                            }
                        }
                        else
                        {
                            lblP1Money.Visible = false;
                            lblAIP1stat.Visible = false;
                            lblPlr1Name.Visible = false;
                        }

                        break;

                    case 1:
                        if (!AIplayer[x].isBust)
                        {
                            lblP2Money.Visible = true;
                            lblAIP2stat.Visible = true;
                            lblPlr2Name.Visible = true;
                            lblP2Money.Text = "$" + Convert.ToString(AIplayer[x].money);
                            if (AIplayer[x].betStatus == Player.betstats.none)
                                lblAIP2stat.Text = "";
                            else
                            {
                                if (AIplayer[x].firstHandBet && AIplayer[x].betStatus == Player.betstats.call)
                                {
                                    lblAIP2stat.Text = "Bet";
                                }
                                else
                                    lblAIP2stat.Text = Convert.ToString(AIplayer[x].betStatus);
                            }
                        }
                        else
                        {
                            lblP2Money.Visible = false;
                            lblAIP2stat.Visible = false;
                            lblPlr2Name.Visible = false;
                        }

                        break;

                    case 2:
                        if (!AIplayer[x].isBust)
                        {
                            lblP3Money.Visible = true;
                            lblAIP3stat.Visible = true;
                            lblPlr3Name.Visible = true;
                            lblP3Money.Text = "$" + Convert.ToString(AIplayer[x].money);
                            if (AIplayer[x].betStatus == Player.betstats.none)
                                lblAIP3stat.Text = "";
                            else
                            {
                                if (AIplayer[x].firstHandBet && AIplayer[x].betStatus == Player.betstats.call)
                                {
                                    lblAIP3stat.Text = "Bet";
                                }
                                else
                                    lblAIP3stat.Text = Convert.ToString(AIplayer[x].betStatus);
                            }
                        }
                        else
                        {
                            lblP3Money.Visible = false;
                            lblAIP3stat.Visible = false;
                            lblPlr3Name.Visible = false;
                        }

                        break;


                    case 3:
                        if (!AIplayer[x].isBust)
                        {
                            lblP4Money.Visible = true;
                            lblAIP4stat.Visible = true;
                            lblPlr4Name.Visible = true;
                            lblP4Money.Text = "$" + Convert.ToString(AIplayer[x].money);
                            if (AIplayer[x].betStatus == Player.betstats.none)
                                lblAIP4stat.Text = "";
                            else
                            {
                                if (AIplayer[x].firstHandBet && AIplayer[x].betStatus == Player.betstats.call)
                                {
                                    lblAIP4stat.Text = "Bet";
                                }
                                else
                                    lblAIP4stat.Text = Convert.ToString(AIplayer[x].betStatus);
                            }
                        }
                        else
                        {
                            lblP4Money.Visible = false;
                            lblAIP4stat.Visible = false;
                            lblPlr4Name.Visible = false;
                        }

                        break;
                }





                lblPot.Text = "$ " + Convert.ToString(pot);           // display pot amount
                lblCurrentBet.Text = "$ " + Convert.ToString(currentBet);



            }
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
                if (((firstBet && n == 0) || isChecked || currentBet == 0) &&
                     (AIplayer[n].betStatus != Player.betstats.check || AIplayer[n].betStatus != Player.betstats.raise))
                {
                    if (AIplayer[n].handScore < 700)
                    {
                        int checkScore = random1.Next(1, 100);

                        
                        if (checkScore < 41)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;

                    }

                    else if (AIplayer[n].handScore < 1400)
                    {
                        int checkScore = random1.Next(1, 100);


                        if (checkScore < 21)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;


                    }                  
                    
                    else
                    {
                        int checkScore = random1.Next(1, 100);

                        if (isChecked)
                            checkScore = checkScore - 3;

                        if (checkScore < 8)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;
                    }
                } // end test for check
                else
                    AIplayer[n].betStatus = Player.betstats.none;


                
                // test to raise
                if ((firstBet && n == 0) || (isChecked) || (AIplayer[n].raiseCount > 2))
                    AIplayer[n].canRaise = false;
                else if (currentBet > AIplayer[n].money || AIplayer[n].money <= 0)
                    AIplayer[n].canRaise = false;
                else
                    AIplayer[n].canRaise = true;

               


                if (AIplayer[n].canRaise)
                    {
                        if (AIplayer[n].handScore < 700)
                        {
                            int raiseScore = random1.Next(1, 100);

                            if (isRaised)
                                raiseScore = raiseScore + 10;

                            if (raiseScore < 25)
                            {
                               
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                                isRaised = true;
                                callCount = 1;

                            }

                        }
                        else if (AIplayer[n].handScore < 1400)
                        {
                            int raiseScore = random1.Next(1, 100);

                            if (isRaised)
                                raiseScore = raiseScore + 10;

                            if (raiseScore < 51)
                                {
                                
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                                isRaised = true;
                                callCount = 1;
                                }
                        }
                        else
                        {
                            int raiseScore = random1.Next(1, 100);

                            if (isRaised)
                                raiseScore = raiseScore + 10;

                            if (raiseScore < 71)
                                {
                                
                                AIplayer[n].betStatus = Player.betstats.raise;
                                AIplayer[n].raiseCount++;
                                isRaised = true;
                                callCount = 1;
                                }
                        }
                    } // end can raise
                 
                
                //test to fold or call
                if (AIplayer[n].betStatus == Player.betstats.none)
                {
                    if (AIplayer[n].handScore < 700)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 5;

                        if (isRaised || AIplayer[n].money <= 100)
                            foldScore = foldScore - 15;

                        if (foldScore < 16)
                            {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            callCount++;
                            AIfoldCount++;
                            }
                        else
                        {
                            AIplayer[n].betStatus = Player.betstats.call;
                            callCount++;
                            isRaised = false;
                            isChecked = false;
                        }
                    }
                    else if (AIplayer[n].handScore < 1400)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 3;

                        if (isRaised || AIplayer[n].money <= 100)
                            foldScore = foldScore - 10;

                        if (foldScore < 9)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            callCount++;
                            AIfoldCount++;
                        }
                        else
                        {
                            AIplayer[n].betStatus = Player.betstats.call;
                            callCount++;
                            isRaised = false;
                            isChecked = false;
                        }
                    }
                    else
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 1;

                        if (isRaised || AIplayer[n].money <= 100)
                            foldScore = foldScore - 6;

                        if (foldScore < 5)
                            {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            callCount++;
                            AIfoldCount++;
                            }
                        else
                            {
                            AIplayer[n].betStatus = Player.betstats.call;
                            callCount++;
                            isRaised = false;
                            isChecked = false;
                            }
                    }
                } // end test fold
        
            }// end handscore > 200


                
            

            else   // low score hands
            {
                // test for CHECK
                if (((firstBet && n == 0) || isChecked || currentBet == 0) &&
                    (AIplayer[n].betStatus != Player.betstats.check || AIplayer[n].betStatus != Player.betstats.raise))
                {
                    if (AIplayer[n].handScore < 50)
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 71)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;

                    }
                    else if (AIplayer[n].handScore < 120)
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 51)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;

                    }
                    else
                    {
                        int checkScore = random1.Next(1, 100);

                        if (checkScore < 26)
                        {
                            AIplayer[n].betStatus = Player.betstats.check;
                            isChecked = true;
                            callCount = 0;
                        }
                        else
                            AIplayer[n].betStatus = Player.betstats.none;
                    }
                }
                else
                    AIplayer[n].betStatus = Player.betstats.none;

                // test to raise
                if ((firstBet && n == 0) || (isChecked) || (AIplayer[n].raiseCount > 2) ||
                    (currentBet == 0))
                    AIplayer[n].canRaise = false;
                else if (currentBet > AIplayer[n].money)
                    AIplayer[n].canRaise = false;
                else
                    AIplayer[n].canRaise = true;
                
                

                if (AIplayer[n].canRaise)
                {
                    if (AIplayer[n].handScore < 50)
                    {
                        int raiseScore = random1.Next(1, 100);

                        if (raiseScore < 6)
                        {
                            
                            AIplayer[n].betStatus = Player.betstats.raise;
                            AIplayer[n].raiseCount++;
                            isRaised = true;
                            callCount = 1;
                        }

                    }

                    else if (AIplayer[n].handScore < 100)
                    {
                        int raiseScore = random1.Next(1, 100);

                        if (raiseScore < 19)
                        {
                            
                            AIplayer[n].betStatus = Player.betstats.raise;
                            AIplayer[n].raiseCount++;
                            isRaised = true;
                            callCount = 1;
                        }

                    }
                    
                    
                    else
                    {
                        int raiseScore = random1.Next(1, 100);

                        if (raiseScore < 41)
                        {
                            
                            AIplayer[n].betStatus = Player.betstats.raise;
                            AIplayer[n].raiseCount++;
                            isRaised = true;
                            callCount = 1;
                        }
                    }
                }

                //test to fold or call
                if (AIplayer[n].betStatus == Player.betstats.none ||
                    ((!firstBet) && (AIplayer[n].betStatus == Player.betstats.raise)))
                    
                {
                    if (AIplayer[n].handScore < 50)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 25;

                        if (isRaised)
                            foldScore = foldScore - 25;


                        if (foldScore < 51)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            AIfoldCount++;
                            callCount++;
                        }
                        else
                        {
                            AIplayer[n].betStatus = Player.betstats.call;
                            isRaised = false;
                            callCount++;
                            isChecked = false;
                        }
                    }
                    else if (AIplayer[n].handScore < 100)
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 10;

                        if (isRaised)
                            foldScore = foldScore - 10;

                        if (foldScore < 31)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            AIfoldCount++;
                            callCount++;
                        }
                        else
                        {
                            AIplayer[n].betStatus = Player.betstats.call;
                            isRaised = false;
                            callCount++;
                            isChecked = false;
                        }
                    }
                    else
                    {
                        int foldScore = random1.Next(1, 100);

                        if (isChecked)
                            foldScore = foldScore + 5;

                        if (isRaised)
                            foldScore = foldScore - 5;


                        if (foldScore < 16)
                        {
                            AIplayer[n].betStatus = Player.betstats.fold;
                            callCount++;
                            AIfoldCount++;

                        }
                        else
                        {
                            AIplayer[n].betStatus = Player.betstats.call;
                            isRaised = false;
                            callCount++;
                            isChecked = false;
                        }
                    }


                }
            }
            
            // take action
            switch (AIplayer[n].betStatus)
            {
                case Player.betstats.call:

                    prevBet = currentBet;

                    if (currentBet == 0)
                    {
                        currentBet = AIBetAmount(n);
                        AIplayer[n].firstHandBet = true;
                    }
                    else
                        AIplayer[n].firstHandBet = false;

                    if (isRaised && currentBet != 0)
                    {
                        AIplayer[n].bet = raiseBet;
                    }
                    else
                        AIplayer[n].bet = currentBet;

                    if (AIplayer[n].money > 0)
                        AIplayer[n].money = AIplayer[n].money - AIplayer[n].bet;

                    makePlayerGoBust();

                        if (AIplayer[n].money < 0)
                        AIplayer[n].money = 0;

                    isChecked = false;
                    
                    break;

                case Player.betstats.check:

                    prevBet = currentBet;
                    AIplayer[n].bet = 0;
                    
                    break;

                case Player.betstats.raise:
                    prevBet = currentBet;
                    raiseBet = DetermineRaiseAmount(n);
                    currentBet = raiseBet;
                    AIplayer[n].bet = currentBet + raiseBet;
                    AIplayer[n].money = AIplayer[n].money - AIplayer[n].bet;

                    
                    if (AIplayer[n].money > 0)
                        AIplayer[n].money = AIplayer[n].money - AIplayer[n].bet;
                    
                    if (AIplayer[n].money < 0)
                        AIplayer[n].money = 0;

                    
                    
                    break;

                case Player.betstats.fold:

                    prevBet = currentBet;
                    AIplayer[n].bet = 0;
                    break;
            }

            if (AIplayer[n].money <= 0)
                AIplayer[n].money = 0;

            
            
            pot = pot + AIplayer[n].bet;


        } // end AIBetDecide method

        public int DetermineRaiseAmount(int n)
        {
            int max = 0;
            int min = 0; 

            if (AIplayer[n].handScore > 1800)
            {
                int betallScore = random1.Next(1, 100);
                if (betallScore < 16 && betRound == 4)
                    max = AIplayer[n].money;
                else
                    max = AIplayer[n].money / 2;

                min = currentBet;
            }

            else if (AIplayer[n].handScore > 1300)
            {
                int betallScore = random1.Next(1, 100);
                if (betallScore < 9 && betRound == 4)
                    max = AIplayer[n].money;
                else
                    max = AIplayer[n].money / 5;

                min = currentBet / 2;
            }
            else if (AIplayer[n].handScore > 800)
            {
               int betallScore = random1.Next(1, 100);
               if (betallScore < 3 && betRound == 4)
                    max = AIplayer[n].money;
                else
                    max = AIplayer[n].money / 10;

                 min = currentBet / 3;
            }
            else if (AIplayer[n].handScore > 200)
            {
                
                max =  AIplayer[n].money / 20;
                min = ante;
            }
            else
            {
                max = AIplayer[n].money / 40;
                min = ante;
            }

            if (min > max)
            {
                int t = max;
                max = min;
                min = t;
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

            if (AIplayer[n].handScore > 1800)
            {
                max = AIplayer[n].money;
                min = 50;
            }

            else if (AIplayer[n].handScore > 1200)
            {
                max = AIplayer[n].money / 5 ;
                min = 35;
            }

            else if (AIplayer[n].handScore > 800)
            {
                max = AIplayer[n].money / 20;
                min = 20;
            }

            else if (AIplayer[n].handScore > 300)
            {
                max = AIplayer[n].money / 50;
                min = 10;
            }

            else if (AIplayer[n].handScore > 100)
            {
                max = AIplayer[n].money / 100;
                min = 10;
            }

            else
            {
                max = 25;
                min = 5;
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
           
            int kindScore = 0;
            int royalCount = 0;
            
            bool isStraight = false;
            bool isFlush = false;
           
            int cards = 0;

  
            


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
                // test for flush
                isFlush = TestFlush(n, rnd);
                if (isFlush)
                    AIplayer[n].winHands = Player.winHand.flush;

                // test for straight
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

                // test for pairs or a-kinds
                if (isStraight == false && isFlush == false)
                {
                    kindScore = TestPairKind(n, rnd);

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

        
        
        public int TestPairKind(int n, int rnd)
        {
            int s = 0;
            int s2 = 0;
            int cards = 0;
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
            Card[] tempHand2 = new Card[cards];

            for (int c = 0; c < cards; c++)
            {
                if (c < 2)
                    tempHand[c] = AIplayer[n].playerHand[c].ShallowCopy();
                else
                    tempHand[c] = Dealer.dealerHand[c-2].ShallowCopy();
            }

            

            for (int x = 0; x < cards-1; x++)
            {
                if (tempHand[x].isPaired)
                    continue;

                for (int y = x + 1; y < cards; y++)
                {
                    if (tempHand[x].cardValue == tempHand[y].cardValue)
                    {
                        s++;
                        tempHand2[s - 1] = tempHand[y].ShallowCopy();
                        tempHand[y].isPaired = true;
                       

                    }
                }
                
            }

            if (s > 1)
            {
                for (int a = 0; a < s - 1; a++)
                {
                    for (int b = a + 1; b < s; b++)
                    {
                        if (tempHand2[a].cardValue == tempHand2[b].cardValue)
                            s2++;
                    }
                }

                if ((s == 2 && s2 == 0) || (s == 3 && s2 == 0))
                    return 2;

                if (s == 2 && s2 == 1)
                    return 3;

                if ((s == 3 && s2 == 1) || (s == 4 && s2 == 1) || (s == 4 && s2 == 2))
                    return 4;

                if ((s == 3 && s2 == 3) || (s == 4 && s2 == 3) || (s == 3 && s2 == 2) ||
                    (s == 5 && s2 == 3))
                    return 6;

            }
            else 
                return s;

           
            return s;
        }
        
       

        public bool TestFlush(int n, int rnd)
        {
             // create temp hand
            
            int heartCount = 0;
            int spadeCount = 0;
            int clubCount = 0;
            int diamondCount = 0;
            int cards = 0;


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
           
            // populate hand
            for (int c = 0; c < cards; c++)
            {
                Card tempCard = new Card();

                tempHand[c] = tempCard;
                
                if (c < 2)
                    tempHand[c] = AIplayer[n].playerHand[c];
                else
                    tempHand[c] = Dealer.dealerHand[c-2];
            }

            //test for flush
            for (int x = 0; x < cards; x++)
            {
                if (tempHand[x].suit == Card.Suit.club)
                {
                    clubCount++;
                    if (clubCount >= 5)
                    {
                        return true;
                    }
                }

                if (tempHand[x].suit == Card.Suit.diamond)
                {
                    diamondCount++;
                    if (diamondCount >= 5)
                    {
                        return true;
                    }
                }


                if (tempHand[x].suit == Card.Suit.heart)
                {
                    heartCount++;
                    if (heartCount >= 5)
                    {
                        return true;
                    }
                }


                if (tempHand[x].suit == Card.Suit.spade)
                {
                    spadeCount++;
                
                    if (spadeCount >= 5)
                    {
                        return true;
                    }
                }

                

            }
            return false;
        }


        public bool TestStraight(int n, int rnd)
        {
            // create temp hand
            
            Card swapCard = new Card();
            int count = 0;
            int cards = 0;
            
            byte aceCount = 0;
            bool straight = false;

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
            Card[] tempHand2 = new Card[cards];   

            // populate temporary hand #1
            for (int c = 0; c < cards; c++)
            {


                if (c < 2)
                {
                    tempHand[c] = AIplayer[n].playerHand[c].ShallowCopy();
                    tempHand2[c] = AIplayer[n].playerHand[c].ShallowCopy();
                }
                else
                {
                    tempHand[c] = Dealer.dealerHand[c - 2].ShallowCopy();
                    tempHand2[c] = Dealer.dealerHand[c - 2].ShallowCopy();
                }

            }
         

            // check for aces
            for (int a = 0; a < cards; a++)
            {
                if (tempHand[a].rank == Card.Rank.ace)
                    aceCount++;
            }


            //sort hand
            for (int x = 0; x < cards-1; x++)
            {
                for (int y = x + 1; y < cards; y++)
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
            for (int x = 0; x < cards - 1; x++)
            {
                if (tempHand[x + 1].cardValue == tempHand[x].cardValue + 1)
                {
                    count++;
                }
                else
                    count = 0;
            }

            if (count >= 4)
                straight = true;

   
            // test for high ace
            
            if (aceCount >0 && aceCount < 4)
            {
                
                if (straight == true)
                {
                    if ((tempHand[cards-1].rank == Card.Rank.ace && tempHand[cards-2].rank == Card.Rank.king) ||
                        (tempHand[cards - 2].rank == Card.Rank.ace && tempHand[cards - 3].rank == Card.Rank.king) ||
                        (tempHand[cards - 3].rank == Card.Rank.ace && tempHand[cards - 4].rank == Card.Rank.king))
                    {
                        AIplayer[n].aceHigh = true;
                    }
                }
                
                
                
                // test for low ace
                if (!AIplayer[n].aceHigh)
                {
                    
                    // reassign card value
                    for (int z = 0; z < cards; z++)
                    {
                        if (tempHand2[z].cardValue == 14)
                        {
                            tempHand2[z].cardValue = 1;
                        }
                    }

                    // re-sort
                    for (int x = 0; x < cards-1; x++)
                    {
                        for (int y = x + 1; y < cards; y++)
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
            
            for (int c = 0; c < 2; c++)
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

        

        
        public void ScorePlayerHand()
        {
            bool isFlush = false;
            bool isStraight = false;
           
            int count = 0;
            int heartCount = 0;
            int spadeCount = 0;
            int clubCount = 0;
            int diamondCount = 0;

            //int straightScore = 0;
            byte aceCount = 0;

            Card[] tempHand = new Card[7];
            Card[] tempHand2 = new Card[7];
            Card[] tempHand3 = new Card[7];

            Card swapCard = new Card();

            for (int c = 0; c < 7; c++)
            {
                

                tempHand[c] = new Card(); ;
                tempHand2[c] = new Card(); ;
                tempHand3[c] = new Card(); ;

                if (c < 2)
                {
                    tempHand[c] = mainPlayer.playerHand[c].ShallowCopy();
                    tempHand2[c] = mainPlayer.playerHand[c].ShallowCopy();
                    tempHand3[c] = mainPlayer.playerHand[c].ShallowCopy();
                }
                else
                {
                    tempHand[c] = Dealer.dealerHand[c - 2].ShallowCopy();
                    tempHand2[c] = Dealer.dealerHand[c - 2].ShallowCopy();
                    tempHand3[c] = Dealer.dealerHand[c - 2].ShallowCopy();
                }
            }

            

            for (int x = 0; x < 7; x++)
            {
                if (tempHand[x].suit == Card.Suit.club)
                {
                    clubCount++;
                    if (clubCount >= 5)
                    {
                        mainPlayer.winHands = Player.winHand.flush;
                        break;
                    }
                }

                if (tempHand[x].suit == Card.Suit.diamond)
                {
                    diamondCount++;
                    if (diamondCount >= 5)
                    {
                        mainPlayer.winHands = Player.winHand.flush;
                        break;
                    }
                }

                
                if (tempHand[x].suit == Card.Suit.heart)
                {
                    heartCount++;
                    if (heartCount >= 5)
                    {
                        mainPlayer.winHands = Player.winHand.flush;
                        break;
                    }
                }


                if (tempHand[x].suit == Card.Suit.spade)
                    spadeCount++;
                {
                    if (spadeCount >= 5)
                    {
                        mainPlayer.winHands = Player.winHand.flush;
                        break;
                    }
                }

                

            }
       

   
            // check for aces
            for (int a = 0; a < 7; a++)
            {
                if (tempHand[a].rank == Card.Rank.ace)
                    aceCount++;
            }


            //sort hand
            for (int x = 0; x < 6; x++)
            {
                for (int y = x + 1; y < 7; y++)
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
            for (int x = 0; x < 6; x++)
            {
                if (tempHand[x].cardValue == tempHand[x+1].cardValue + 1)
                {
                    count++;
                }           
            }

            if (count >= 4)
                isStraight = true;

   
            // test for high ace
            
            if (aceCount >1 && aceCount < 4)
            {
                
                if (isStraight == true)
                {
                    if ((tempHand[6].rank == Card.Rank.ace && tempHand[5].rank == Card.Rank.king) ||
                        (tempHand[5].rank == Card.Rank.ace && tempHand[4].rank == Card.Rank.king) ||
                        (tempHand[4].rank == Card.Rank.ace && tempHand[3].rank == Card.Rank.king))
                    {
                        mainPlayer.aceHigh = true;
                    }
                }

                
                // test for low ace
                if (!mainPlayer.aceHigh)
                {
                    tempHand2 = tempHand;

                    // reassign card value
                    for (int z = 0; z < 7; z++)
                    {
                        if (tempHand2[z].cardValue == 14)
                        {
                            tempHand2[z].cardValue = 1;
                            //mainPlayer.playerHand[0].cardValue;
                        }
                    }

                    // re-sort
                    for (int x = 0; x < 6; x++)
                    {
                        for (int y = x + 1; y < 7; y++)
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
                    
                    for (int x = 0; x < 6; x++)
                    {
                        if (tempHand[x].cardValue == tempHand[x+1].cardValue + 1)
                        {
                            count2++;
                        }  
                    }

                    if (count2 >= 4)
                    {
                        isStraight = true;
                        mainPlayer.aceLow = true;
                    }
                }
            }

            if (isStraight)
            {
                if (isFlush)
                {
                    if (mainPlayer.aceHigh)
                    {
                        mainPlayer.winHands = Player.winHand.royalflush;
                    }
                    else
                        mainPlayer.winHands = Player.winHand.straightflush;
                }
                else
                    mainPlayer.winHands = Player.winHand.straight;
            }

            
            // test for pairs or a-kind
            if (mainPlayer.winHands == Player.winHand.none)
            {
                int s = 0;
                int s2 = 0;


            
            for (int x = 0; x < 6; x++)
            {
                if (tempHand[x].isPaired)
                    continue;

                for (int y = x + 1; y < 7; y++)
                {
                    

                    if (tempHand[x].cardValue == tempHand[y].cardValue)
                    {
                        s++;
                        tempHand3[s - 1] = tempHand[y];
                        tempHand[y].isPaired = true;

                    }
                }

            }

            if (s > 1)
            {
                for (int a = 0; a < s - 1; a++)
                {
                    for (int b = a + 1; b < s; b++)
                    {
                        if (tempHand3[a].cardValue == tempHand3[b].cardValue)
                            s2++;
                    }
                }

                if ((s == 2 && s2 == 0) || (s == 3 && s2 == 0))
                    s = 2;

                if (s == 2 && s2 == 1)
                    s = 3;

                if ((s == 3 && s2 == 1) || (s == 4 && s2 == 1) || (s == 4 && s2 == 2))
                    s = 4;

                if ((s == 3 && s2 == 3) || (s == 4 && s2 == 3))
                    s = 6;

            }
            
            
                if (s > 0)
                {
                    switch (s)
                    {
                        case 1:
                            mainPlayer.winHands = Player.winHand.pair;
                            break;

                        case 2:
                            mainPlayer.winHands = Player.winHand.twopair;
                            break;

                        case 3:
                            mainPlayer.winHands = Player.winHand.threekind;
                            break;

                        case 4:
                            mainPlayer.winHands = Player.winHand.fullhouse;
                            break;

                        case 6:
                            mainPlayer.winHands = Player.winHand.fourkind;
                            break;
                    }
                }
                else
                    mainPlayer.winHands = Player.winHand.none;
                
            }
            
        }

        public void DetermineHandWinner()
        {

            if (mainPlayer.betStatus == Player.betstats.fold)
                mainPlayer.winHandScore = 0;
            else
                ScorePlayerHand();

            int adjustScore = 0;
            // create some tempory players
            Player[] tempPlayers = new Player[5];



            for (int i = 0; i < 4; i++)
            {

                if (AIplayer[i].isBust)
                {
                    AIplayer[i].winHandScore = 0;
                }

                tempPlayers[i] = AIplayer[i].ShallowCopy();
            }

            

            tempPlayers[4] = mainPlayer.ShallowCopy();



            // score winning hands
            for (int k = 0; k < 5; k++)
            {

                if (tempPlayers[k].isBust)
                    continue;

                // set up temp hand
                Card[] tempHand = new Card[7];
                Card swapCard = new Card();

                for (int y = 0; y < 7; y++)
                {
                    
                    
                    if (y < 2)
                        tempHand[y] = tempPlayers[k].playerHand[y].ShallowCopy();
                    else
                        tempHand[y] = Dealer.dealerHand[y - 2].ShallowCopy();
                }



                for (int j = 0; j < 7; j++)
                {
                    tempPlayers[k].playerTempHand[j] = tempHand[j].ShallowCopy();
                }


                //sort tempHand
                for (int a = 0; a < 6; a++)
                {
                    for (int b = a + 1; b < 7; b++)
                    {
                        if (tempHand[a].cardValue > tempHand[b].cardValue)
                        {
                            swapCard = tempHand[b];
                            tempHand[b] = tempHand[a];
                            tempHand[a] = swapCard;
                            
                        }
                    }
                }


            


                    if (tempPlayers[k].betStatus == Player.betstats.fold)
                    {
                        tempPlayers[k].winHandScore = 0;

                        continue;
                    }


                switch (tempPlayers[k].winHands)
                {
                    case Player.winHand.none:
                        tempPlayers[k].winHandScore = 0;
                        break;

                    case Player.winHand.pair:
                        tempPlayers[k].winHandScore = 100;
                        break;

                    case Player.winHand.twopair:
                        tempPlayers[k].winHandScore = 200;
                        break;

                    case Player.winHand.threekind:
                        tempPlayers[k].winHandScore = 300;
                        break;

                    case Player.winHand.straight:
                        tempPlayers[k].winHandScore = 400;
                        break;

                    case Player.winHand.flush:
                        tempPlayers[k].winHandScore = 500;
                        break;

                    case Player.winHand.fullhouse:
                        tempPlayers[k].winHandScore = 600;
                        break;

                    case Player.winHand.fourkind:
                        tempPlayers[k].winHandScore = 700;
                        break;

                    case Player.winHand.straightflush:
                        tempPlayers[k].winHandScore = 800;
                        break;

                    case Player.winHand.royalflush:
                        tempPlayers[k].winHandScore = 900;
                        break;
                }

                // adjust scores for high cards

                    for (int a = 0; a < 7; a++)
                    {
                        switch (tempHand[a].rank)
                        {
                            case Card.Rank.ace:
                                if (tempPlayers[k].aceLow)
                                {
                                    adjustScore = adjustScore + 1;
                                }
                                else
                                    adjustScore = adjustScore + 14;

                                break;

                            case Card.Rank.king:
                                adjustScore = adjustScore + 13;
                                break;

                            case Card.Rank.queen:
                                adjustScore = adjustScore + 12;
                                break;

                            case Card.Rank.jack:
                                adjustScore = adjustScore + 11;
                                break;

                            case Card.Rank.ten:
                                adjustScore = adjustScore + 10;
                                break;

                            case Card.Rank.nine:
                                adjustScore = adjustScore + 9;
                                break;

                            case Card.Rank.eight:
                                adjustScore = adjustScore + 8;
                                break;

                            case Card.Rank.seven:
                                adjustScore = adjustScore + 7;
                                break;

                            case Card.Rank.six:
                                adjustScore = adjustScore + 6;
                                break;

                            case Card.Rank.five:
                                adjustScore = adjustScore + 5;
                                break;

                            case Card.Rank.four:
                                adjustScore = adjustScore + 4;
                                break;

                            case Card.Rank.three:
                                adjustScore = adjustScore + 3;
                                break;

                            case Card.Rank.deuce:
                                adjustScore = adjustScore + 2;
                                break;
                        }
                     }
                        
                
                    tempPlayers[k].winHandScore = tempPlayers[k].winHandScore + adjustScore;
                    
            }
            
            
            Player swapPlayerTemp = new Player();

            // find hand winner

            for (int x = 0; x < 4; x++)
            {
                for (int y = x + 1; y < 5; y++)
                {
                    if (tempPlayers[x].winHandScore > tempPlayers[y].winHandScore)
                    {
                        swapPlayerTemp = tempPlayers[y];
                        tempPlayers[y] = tempPlayers[x];
                        tempPlayers[x] = swapPlayerTemp;
                    } 
                }
            }

            // check for ties

            int winner = 1;
            int q = 4;
            
            /*
            for (int i = 4; i < 3; i--)
            {
                if (tempPlayers[i].winHandScore == tempPlayers[i - 1].winHandScore)
                    winner = winner + 1;
                else
                    break;
            }
             */


            if (tempPlayers[4].winHandScore != 0)
            {
                //set winner(s)
                if (tempPlayers[4].name == mainPlayer.name)
                {
                    mainPlayer.isHandWinner = true;
                }
                else
                {
                    for (int z = 0; z < 4; z++)
                    {
                        if (tempPlayers[4].name == AIplayer[z].name)
                            AIplayer[z].isHandWinner = true;
                    }
                }

                if (winner > 1)
                {
                    for (int n = 3; n < 2; n--)
                    {

                        if (tempPlayers[n].name == mainPlayer.name)
                            mainPlayer.isHandWinner = true;
                        else
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                if (tempPlayers[n].name == AIplayer[m].name)
                                    AIplayer[m].isHandWinner = true;
                            }
                        }
                    }
                }

                


                tempPlayers[4].winnings = pot / winner;

                if (mainPlayer.isHandWinner)
                {
                    mainPlayer.winnings = tempPlayers[4].winnings;
                    mainPlayer.money = mainPlayer.money + mainPlayer.winnings;

                }
                else
                {


                    for (int t = 0; t < 4; t++)
                    {
                        if (AIplayer[t].isHandWinner)
                        {
                            AIplayer[t].winnings = tempPlayers[4].winnings;
                            AIplayer[t].money = AIplayer[t].money + AIplayer[t].winnings;


                        }

                        

                        if (AIplayer[t].money <= 0)
                            AIplayer[t].isBust = true;
                    }
                }
            }
            
            // use for testing purposes
            /*
            if (!qualified)
            {
                makePlayerGoBust();
            }
             */


            pot = 0;

            winnerHand = tempPlayers[4].ShallowCopy();

            if (mainPlayer.money <= 0)
            {
                mainPlayer.isBust = true;
                EndGame();
                return;
            }
            

            winResults form2 = new winResults(this);
            form2.Show();

         
            return;



        }

        public void DetermineGameWinner()
        {
            GameWinner form7 = new GameWinner(this);
            form7.Show();

            return;
        }



        #endregion

        #region methods: utilities

        public void TimerDelay()
        {
           // timer1.Interval = 2000;
            //timer1.Start();
           // Thread.Sleep(50);


        }
        #endregion

       public void EndGame()
        {
           if (mainPlayer.isBust)
           {
               endGame form3 = new endGame(this);
               form3.Show();
               return;
           }
           else 
               this.Hide();


            
            
        }
 
        #endregion   // methods

       private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
       {
           PreGame();
           newGame = true;
       }

       private void quitToolStripMenuItem_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       private void handsToolStripMenuItem_Click(object sender, EventArgs e)
       {
           help form6 = new help();
           form6.Show();
           return;
       }


        //--------------testing functions--------------------

       public void makePlayerGoBust()
       {
           
           /*
            AIplayer[3].isBust = true;
           AIplayer[0].isBust = true;
           AIplayer[1].isBust = true;
           AIplayer[2].isBust = true;
            */

           //AIplayer[3].money = -56;

       }

      

    }
}