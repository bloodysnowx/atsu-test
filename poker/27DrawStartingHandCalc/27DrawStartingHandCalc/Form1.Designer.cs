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
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCalc = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonCalc2 = new System.Windows.Forms.Button();
            this.backgroundWorkerCalc = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "確率(%)";
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
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(127, 39);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 19);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
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
            // buttonCalc2
            // 
            this.buttonCalc2.Enabled = false;
            this.buttonCalc2.Location = new System.Drawing.Point(85, 160);
            this.buttonCalc2.Name = "buttonCalc2";
            this.buttonCalc2.Size = new System.Drawing.Size(75, 23);
            this.buttonCalc2.TabIndex = 9;
            this.buttonCalc2.Text = "並列？";
            this.buttonCalc2.UseVisualStyleBackColor = true;
            this.buttonCalc2.Visible = false;
            this.buttonCalc2.Click += new System.EventHandler(this.buttonCalc2_Click);
            // 
            // backgroundWorkerCalc
            // 
            this.backgroundWorkerCalc.WorkerReportsProgress = true;
            this.backgroundWorkerCalc.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalc_DoWork);
            this.backgroundWorkerCalc.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerCalc_ProgressChanged);
            this.backgroundWorkerCalc.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalc_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonCalc2);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonCalc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.textBoxStartingHand);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStartingHand;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCalc;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonCalc2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalc;
    }
}

