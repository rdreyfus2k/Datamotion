using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ron_Dreyfus_Vegas_Holdem
{
    public partial class EnterTournament : Form
    {
        Form1 T;

        public EnterTournament()
        {
            InitializeComponent();
        }

        public EnterTournament(Form1 frm3)
        {
            InitializeComponent();
            T = new Form1();
            T = frm3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            T.StartTournament();
            return;
        }
    }
}
