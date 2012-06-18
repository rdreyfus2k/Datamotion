using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public partial class welcome : Form
    {
        Form1 w;

        

        public welcome()
        {
            InitializeComponent();
        }

        public welcome(Form1 frm3)
        {
            InitializeComponent();
            w = new Form1();
            w = frm3;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            w.mainPlayerName = this.textBox1.Text;
            this.Hide();
            w.StartNewGame();
                
            

        

        }

        private void welcome_Load(object sender, EventArgs e)
        {
            textBox1.Text = "PLAYER";
        }
    }
}
