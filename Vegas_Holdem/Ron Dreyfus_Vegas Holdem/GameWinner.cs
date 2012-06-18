using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public partial class GameWinner : Form
    {
        Form1 Iwin;

        public GameWinner()
        {
            InitializeComponent();
        }

        public GameWinner(Form1 frm5)
        {
            InitializeComponent();
            Iwin = new Form1();
            Iwin = frm5;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Iwin.StartNewGame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Iwin.Close();
        }
    }
}
