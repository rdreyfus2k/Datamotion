using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public partial class NextRound : Form
    {
        Form1 nr;

        public NextRound()
        {
            InitializeComponent();
        }

        public NextRound(Form1 frm)
        {
            InitializeComponent();
            nr = new Form1();
            nr = frm;
        }

        private void NextRound_Load(object sender, EventArgs e)
        {
            label1.Text = "Congradulations! You won round #" + nr.round;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            //nr.StartRound();
        }
    }
}
