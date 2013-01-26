namespace _27DrawStartingHandCalc
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxStartingHand = new System.Windows.Forms.TextBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonCalc = new System.Windows.Forms.Button();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonCalcParallel = new System.Windows.Forms.Button();
            this.backgroundWorkerCalc = new System.ComponentModel.BackgroundWorker();
            this.buttonCalcAll = new System.Windows.Forms.Button();
            this.backgroundWorkerCalcAll = new System.ComponentModel.BackgroundWorker();
            this.buttonCalcAllAsyncTwo = new System.Windows.Forms.Button();
            this.backgroundWorkerCalcAllAsyncTwo = new System.ComponentModel.BackgroundWorker();
            this.labelTime = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.buttonCalcAllAsyncFour = new System.Windows.Forms.Button();
            this.backgroundWorkerCalcAllAsyncFour = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxStartingHand
            // 
            this.textBoxStartingHand.Location = new System.Drawing.Point(127, 13);
            this.textBoxStartingHand.Name = "textBoxStartingHand";
            this.textBoxStartingHand.Size = new System.Drawing.Size(100, 19);
            this.textBoxStartingHand.TabIndex = 0;
            this.textBoxStartingHand.Text = "23457";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(127, 93);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(100, 19);
            this.textBoxResult.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "スターティングハンド";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "試行回数";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(13, 100);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(43, 12);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "確率(%)";
            // 
            // buttonCalc
            // 
            this.buttonCalc.Location = new System.Drawing.Point(85, 64);
            this.buttonCalc.Name = "buttonCalc";
            this.buttonCalc.Size = new System.Drawing.Size(75, 23);
            this.buttonCalc.TabIndex = 6;
            this.buttonCalc.Text = "計算実行";
            this.buttonCalc.UseVisualStyleBackColor = true;
            this.buttonCalc.Click += new System.EventHandler(this.buttonCalc_Click);
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCount.Location = new System.Drawing.Point(127, 39);
            this.numericUpDownCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownCount.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownCount.Name = "numericUpDownCount";
            this.numericUpDownCount.Size = new System.Drawing.Size(120, 19);
            this.numericUpDownCount.TabIndex = 7;
            this.numericUpDownCount.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 130);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(259, 23);
            this.progressBar.TabIndex = 8;
            // 
            // buttonCalcParallel
            // 
            this.buttonCalcParallel.Enabled = false;
            this.buttonCalcParallel.Location = new System.Drawing.Point(166, 64);
            this.buttonCalcParallel.Name = "buttonCalcParallel";
            this.buttonCalcParallel.Size = new System.Drawing.Size(75, 23);
            this.buttonCalcParallel.TabIndex = 9;
            this.buttonCalcParallel.Text = "並列？";
            this.buttonCalcParallel.UseVisualStyleBackColor = true;
            this.buttonCalcParallel.Visible = false;
            this.buttonCalcParallel.Click += new System.EventHandler(this.buttonCalc2_Click);
            // 
            // backgroundWorkerCalc
            // 
            this.backgroundWorkerCalc.WorkerReportsProgress = true;
            this.backgroundWorkerCalc.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalc_DoWork);
            this.backgroundWorkerCalc.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerCalc_ProgressChanged);
            this.backgroundWorkerCalc.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalc_RunWorkerCompleted);
            // 
            // buttonCalcAll
            // 
            this.buttonCalcAll.Location = new System.Drawing.Point(12, 159);
            this.buttonCalcAll.Name = "buttonCalcAll";
            this.buttonCalcAll.Size = new System.Drawing.Size(75, 23);
            this.buttonCalcAll.TabIndex = 10;
            this.buttonCalcAll.Text = "総当り";
            this.buttonCalcAll.UseVisualStyleBackColor = true;
            this.buttonCalcAll.Click += new System.EventHandler(this.buttonCalcAll_Click);
            // 
            // backgroundWorkerCalcAll
            // 
            this.backgroundWorkerCalcAll.WorkerReportsProgress = true;
            this.backgroundWorkerCalcAll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalcAll_DoWork);
            this.backgroundWorkerCalcAll.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerCalc_ProgressChanged);
            this.backgroundWorkerCalcAll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalc_RunWorkerCompleted);
            // 
            // buttonCalcAllAsyncTwo
            // 
            this.buttonCalcAllAsyncTwo.Location = new System.Drawing.Point(93, 159);
            this.buttonCalcAllAsyncTwo.Name = "buttonCalcAllAsyncTwo";
            this.buttonCalcAllAsyncTwo.Size = new System.Drawing.Size(75, 23);
            this.buttonCalcAllAsyncTwo.TabIndex = 11;
            this.buttonCalcAllAsyncTwo.Text = "並列(2)";
            this.buttonCalcAllAsyncTwo.UseVisualStyleBackColor = true;
            this.buttonCalcAllAsyncTwo.Click += new System.EventHandler(this.buttonCalcAll_Click);
            // 
            // backgroundWorkerCalcAllAsyncTwo
            // 
            this.backgroundWorkerCalcAllAsyncTwo.WorkerReportsProgress = true;
            this.backgroundWorkerCalcAllAsyncTwo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalcAllAsync_DoWork);
            this.backgroundWorkerCalcAllAsyncTwo.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerCalc_ProgressChanged);
            this.backgroundWorkerCalcAllAsyncTwo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalc_RunWorkerCompleted);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(13, 192);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(76, 12);
            this.labelTime.TabIndex = 12;
            this.labelTime.Text = "計算時間(ms)";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(127, 189);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(100, 19);
            this.textBoxTime.TabIndex = 13;
            // 
            // buttonCalcAllAsyncFour
            // 
            this.buttonCalcAllAsyncFour.Location = new System.Drawing.Point(175, 159);
            this.buttonCalcAllAsyncFour.Name = "buttonCalcAllAsyncFour";
            this.buttonCalcAllAsyncFour.Size = new System.Drawing.Size(75, 23);
            this.buttonCalcAllAsyncFour.TabIndex = 14;
            this.buttonCalcAllAsyncFour.Text = "並列(4)";
            this.buttonCalcAllAsyncFour.UseVisualStyleBackColor = true;
            this.buttonCalcAllAsyncFour.Click += new System.EventHandler(this.buttonCalcAll_Click);
            // 
            // backgroundWorkerCalcAllAsyncFour
            // 
            this.backgroundWorkerCalcAllAsyncFour.WorkerReportsProgress = true;
            this.backgroundWorkerCalcAllAsyncFour.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalcAllAsyncFour_DoWork);
            this.backgroundWorkerCalcAllAsyncFour.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerCalc_ProgressChanged);
            this.backgroundWorkerCalcAllAsyncFour.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalc_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonCalcAllAsyncFour);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.buttonCalcAllAsyncTwo);
            this.Controls.Add(this.buttonCalcAll);
            this.Controls.Add(this.buttonCalcParallel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.numericUpDownCount);
            this.Controls.Add(this.buttonCalc);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.textBoxStartingHand);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStartingHand;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button buttonCalc;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonCalcParallel;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalc;
        private System.Windows.Forms.Button buttonCalcAll;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalcAll;
        private System.Windows.Forms.Button buttonCalcAllAsyncTwo;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalcAllAsyncTwo;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Button buttonCalcAllAsyncFour;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalcAllAsyncFour;
    }
}

