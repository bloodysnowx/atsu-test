using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _27DrawStartingHandCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            this.progressBar.Maximum = (int)this.numericUpDown1.Value;
            Hand target = new Hand(this.textBoxStartingHand.Text);
            Deck deck = new Deck();
            int count = 0;
            Hand test;
            for (int i = 0; i < this.numericUpDown1.Value; ++i)
            {
                deck.Reset();
                test = new Hand(deck);
                if (test.isEqualTo(target))
                {
                    count++;
                    this.progressBar.Value = i;                    
                }
            }
            this.progressBar.Value = (int)this.numericUpDown1.Value;
            this.textBoxResult.Text = ((decimal)(count * 100) / this.numericUpDown1.Value).ToString("F8");
        }
    }
}
