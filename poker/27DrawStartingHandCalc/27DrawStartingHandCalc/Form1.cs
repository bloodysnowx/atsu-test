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
using System.Diagnostics;

namespace _27DrawStartingHandCalc
{
    public partial class Form1 : Form
    {
        int count;
        Hand target;
        Button[] buttons;
        Stopwatch sw = new Stopwatch(); 

        public Form1()
        {
            InitializeComponent();
            buttons = new Button[] { buttonCalc, buttonCalcAll, buttonCalcAllAsyncTwo };
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            sw.Start();
            this.progressBar.Maximum = (int)this.numericUpDownCount.Value;
            foreach (Button button in buttons) button.Enabled = false;
            this.backgroundWorkerCalc.RunWorkerAsync();
        }

        private void buttonCalc2_Click(object sender, EventArgs e)
        {
            this.progressBar.Maximum = (int)this.numericUpDownCount.Value;
            this.progressBar.Value = 0;
            Hand target = new Hand(this.textBoxStartingHand.Text);

            var data = Enumerable.Range(0, (int)this.numericUpDownCount.Value);
            var count = data.AsParallel().Sum(x =>
            {
                Hand test = new Hand(new DeckForParallel());
                return test.isEqualTo(target) ? 1 : 0;
            });

            this.progressBar.Value = (int)this.numericUpDownCount.Value;
            this.textBoxResult.Text = ((decimal)count * 100 / this.numericUpDownCount.Value).ToString("F8");
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
                        return countHandOf((int)this.numericUpDownCount.Value / thread_count);
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
            this.progressBar.Value = this.progressBar.Maximum;
            this.textBoxResult.Text = ((decimal)count * 100 / (decimal)this.progressBar.Maximum).ToString("F8");
            foreach (Button button in buttons) button.Enabled = true;
            sw.Stop();
            this.textBoxTime.Text = sw.ElapsedMilliseconds.ToString();
            sw.Reset();
        }

        private void buttonCalcAll_Click(object sender, EventArgs e)
        {
            sw.Start();
            this.progressBar.Maximum = AllHands.MAX_LENGTH;
            this.progressBar.Value = 0;
            foreach (Button button in buttons) button.Enabled = false;
            if (sender == buttonCalcAll)
                this.backgroundWorkerCalcAll.RunWorkerAsync();
            else if (sender == buttonCalcAllAsyncTwo)
                this.backgroundWorkerCalcAllAsyncTwo.RunWorkerAsync();
            else if (sender == buttonCalcAllAsyncFour)
                this.backgroundWorkerCalcAllAsyncFour.RunWorkerAsync();
        }

        private void backgroundWorkerCalcAll_DoWork(object sender, DoWorkEventArgs e)
        {
            target = new Hand(this.textBoxStartingHand.Text);

            AllHands hands = new AllHands(AllHands.ORDER.ASC);
            count = 0;
            Hand test;
            for (int i = 0; i < AllHands.MAX_LENGTH; ++i)
            {
                test = hands.Next();
                if (test.isEqualTo(target))
                {
                    count++;
                    if (count % 100 == 99) this.backgroundWorkerCalcAll.ReportProgress(i);
                }
            }
        }

        private void backgroundWorkerCalcAllAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            target = new Hand(this.textBoxStartingHand.Text);

            Task<int> taskAsc = Task<int>.Factory.StartNew(() =>
            {
                AllHands handsAsc = new AllHands(AllHands.ORDER.ASC);
                Hand test;
                int countAsc = 0;
                for (int i = 0; i < AllHands.MAX_LENGTH / 2; ++i)
                {
                    test = handsAsc.Next();
                    if (test.isEqualTo(target)) countAsc++;
                }
                return countAsc;
            });

            Task<int> taskDesc = Task<int>.Factory.StartNew(() =>
            {
                AllHands handsDesc = new AllHands(AllHands.ORDER.DESC);
                Hand test;
                int countDesc = 0;
                for (int i = 0; i < AllHands.MAX_LENGTH / 2; ++i)
                {
                    test = handsDesc.Prev();
                    if (test.isEqualTo(target)) countDesc++;
                }
                return countDesc;
            });

            count = taskAsc.Result + taskDesc.Result;
        }

        private int countStartWithPosition(AllHands.POSITION pos)
        {
            AllHands hands = new AllHands(pos);
            Hand test;
            int count = 0;
            for (int i = 0; i < AllHands.MAX_LENGTH / 4; ++i)
            {
                test = hands.Next();
                if (test.isEqualTo(target)) count++;
            }
            return count;
        }

        private void backgroundWorkerCalcAllAsyncFour_DoWork(object sender, DoWorkEventArgs e)
        {
            target = new Hand(this.textBoxStartingHand.Text);

            Task<int> taskA = Task<int>.Factory.StartNew(() =>
            {
                return countStartWithPosition(AllHands.POSITION.ZERO);
            });
            Task<int> taskB = Task<int>.Factory.StartNew(() =>
            {
                return countStartWithPosition(AllHands.POSITION.ONE);
            });
            Task<int> taskC = Task<int>.Factory.StartNew(() =>
            {
                return countStartWithPosition(AllHands.POSITION.TWO);
            });
            Task<int> taskD = Task<int>.Factory.StartNew(() =>
            {
                return countStartWithPosition(AllHands.POSITION.THREE);
            });
            count = taskA.Result + taskB.Result + taskC.Result + taskD.Result;
        }
    }
}
