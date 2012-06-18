using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public partial class endGame : Form
    {
        Form1 end;
        
        public endGame()
        {
            InitializeComponent();
        }

        public endGame(Form1 fr2)
        {
            InitializeComponent();
            end = new Form1();
            end = fr2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            end.StartNewGame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            end.Close();
        }
    }
}
