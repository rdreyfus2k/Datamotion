using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    
    
    public partial class winResults : Form
    {

        Form1 f;
        
        
        public winResults(Form1 fr1)
        {
            InitializeComponent();
            f = new Form1();
            f = fr1;
            
            
        }

        public winResults()
        {
            InitializeComponent();
            
          
        }

        private void mymethod()
        {
            //lbl_winMsg1.Left = (this.Width / 2) - (lbl_winMsg1.Text.Length);

            string win = "";
            string playerName = "";
            string winMessage = "";
            
            if (f.winnerHand.winHands != Player.winHand.none)
            {
                playerName = f.winnerHand.name + " won $" + Convert.ToString(f.winnerHand.winnings);
                
                switch (f.winnerHand.winHands)
                {
                    case Player.winHand.pair:
                        win = " with a Pair";
                        break;

                    case Player.winHand.twopair:
                       win = " with Two Pairs!";
                        break;

                    case Player.winHand.threekind:
                        win = " with Three of a Kind!";
                        break;

                    case Player.winHand.fullhouse:
                        win = " with a Full House!!";
                        break;

                    case Player.winHand.flush:
                        win = " with a flush!!";
                        break;

                    case Player.winHand.straight:
                        win = " with a Straight!!";
                        break;

                    case Player.winHand.fourkind:
                        win = " with Four of a Kind!!!";
                        break;

                    case Player.winHand.straightflush:
                        win = " with a Straight Flush!!!";
                        break;

                    case Player.winHand.royalflush:
                        win = " with a Royal Flush!!!!";
                        break;
                }

                winMessage = playerName + win;
            }
            else
            {
                if (f.AIfoldCount >= 4 && f.mainPlayer.betStatus == Player.betstats.fold)
                {
                    winMessage = "Sorry, no one wins. Everyone has folded";

                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;
                    pictureBox7.Visible = false;
                }
                else if (f.AIfoldCount >= 4)

                    winMessage = f.winnerHand.name + " won $" + Convert.ToString(f.winnerHand.winnings)+ " All other players folded!";
                
                else
                    {
                       playerName = f.winnerHand.name + " won $" + Convert.ToString(f.winnerHand.winnings);
                       winMessage = playerName + " with a High Card";
                    }
                
            } 
            

            
            lbl_winMsg1.Text = winMessage;

            

            pictureBox1.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[0].deckValue];
            pictureBox2.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[1].deckValue];
            pictureBox3.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[2].deckValue];
            pictureBox4.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[3].deckValue];
            pictureBox5.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[4].deckValue];
            pictureBox6.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[5].deckValue];
            pictureBox7.Image = IL_cardPicsPlayers.Images[f.winnerHand.playerTempHand[6].deckValue];


        }

        private void winResults_Load(object sender, EventArgs e)
        {
            mymethod();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (f.qualified)
            {
                if (f.AIplayer[0].isBust && f.AIplayer[1].isBust && f.AIplayer[2].isBust
                    && f.AIplayer[3].isBust)
                {
                    NextRound form10 = new NextRound(f);
                    form10.Show();
                    return;
                }
                else
                {
                    f.StartHand();
                    return;
                }
            }

                    
            
            else
            {
                f.Qualification();
                return;
            }
            
           
        }

        
    }

    

   
}
