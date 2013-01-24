using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace _27DrawStartingHandCalc
{
    public partial class Form1 : Form
    {
        int count;
        Hand target;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            this.progressBar.Maximum = (int)this.numericUpDown1.Value;
            this.buttonCalc.Enabled = false;
            this.backgroundWorkerCalc.RunWorkerAsync();
        }

        private void buttonCalc2_Click(object sender, EventArgs e)
        {
            this.progressBar.Maximum = (int)this.numericUpDown1.Value;
            this.progressBar.Value = 0;
            Hand target = new Hand(this.textBoxStartingHand.Text);

            var data = Enumerable.Range(0, (int)this.numericUpDown1.Value);
            var count = data.AsParallel().Sum(x =>
            {
                Hand test = new Hand(new DeckForParallel());
                return test.isEqualTo(target) ? 1 : 0;
            });

            this.progressBar.Value = (int)this.numericUpDown1.Value;
            this.textBoxResult.Text = ((decimal)count * 100 / this.numericUpDown1.Value).ToString("F8");
        }

        private int countHandOf(int all)
        {
            Deck deck = new Deck();
            int count = 0;
            Hand test;
            for (int i = 0; i < all; ++i)
            {
                deck.Reset();
                test = new Hand(deck);
                if (test.isEqualTo(target))
                {
                    count++;
                    if (count % 100 == 99) this.backgroundWorkerCalc.ReportProgress(i);
                }
            }
            return count;
        }

        private void backgroundWorkerCalc_DoWork(object sender, DoWorkEventArgs e)
        {
            target = new Hand(this.textBoxStartingHand.Text);
            int thread_count = 1;
            Task<int>[] tasks = new Task<int>[thread_count];
            count = 0;
            for (int i = 0; i < thread_count; ++i)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                    {
                        return countHandOf((int)this.numericUpDown1.Value / thread_count);
                    });
            }
            for (int i = 0; i < thread_count; ++i)
            {
                count += tasks[i].Result;
            }
        }

        private void backgroundWorkerCalc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = (int)this.numericUpDown1.Value;
            this.textBoxResult.Text = ((decimal)count * 100 / this.numericUpDown1.Value).ToString("F8");
            this.buttonCalc.Enabled = true;
        }
    }
}
