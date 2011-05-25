namespace PTRtoNote
{
    partial class FormMain
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
            this.openXMLDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveXMLDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelOpen = new System.Windows.Forms.Label();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.labelExecute = new System.Windows.Forms.Label();
            this.buttonCSV = new System.Windows.Forms.Button();
            this.labelCSV = new System.Windows.Forms.Label();
            this.textBoxNewComer = new System.Windows.Forms.TextBox();
            this.openCSVDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonOther = new System.Windows.Forms.Button();
            this.labelOther = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageNew = new System.Windows.Forms.TabPage();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.textBoxUpdate = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openPlayerNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConvertedCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAnotherXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.convertCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgWorkerExecute = new System.ComponentModel.BackgroundWorker();
            this.numericUpDownStart = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEnd = new System.Windows.Forms.NumericUpDown();
            this.labelAccount = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageNew.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd)).BeginInit();
            this.SuspendLayout();
            // 
            // openXMLDialog
            // 
            this.openXMLDialog.DefaultExt = "xml";
            this.openXMLDialog.Filter = "notesXML(*.xml)|*.xml";
            this.openXMLDialog.RestoreDirectory = true;
            this.openXMLDialog.Title = "openXML";
            // 
            // saveXMLDialog
            // 
            this.saveXMLDialog.DefaultExt = "xml";
            this.saveXMLDialog.Filter = "notesXML(*.xml)|*.xml";
            this.saveXMLDialog.RestoreDirectory = true;
            this.saveXMLDialog.Title = "saveXML";
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 27);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "openXML";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelOpen
            // 
            this.labelOpen.AutoSize = true;
            this.labelOpen.Location = new System.Drawing.Point(93, 27);
            this.labelOpen.Name = "labelOpen";
            this.labelOpen.Size = new System.Drawing.Size(148, 12);
            this.labelOpen.TabIndex = 1;
            this.labelOpen.Text = "notesXML is not opened..........";
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(12, 114);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 3;
            this.buttonExecute.Text = "Execute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // labelExecute
            // 
            this.labelExecute.AutoSize = true;
            this.labelExecute.Location = new System.Drawing.Point(93, 114);
            this.labelExecute.Name = "labelExecute";
            this.labelExecute.Size = new System.Drawing.Size(136, 12);
            this.labelExecute.TabIndex = 4;
            this.labelExecute.Text = "PTR is not connected..........\r\n";
            // 
            // buttonCSV
            // 
            this.buttonCSV.Location = new System.Drawing.Point(12, 56);
            this.buttonCSV.Name = "buttonCSV";
            this.buttonCSV.Size = new System.Drawing.Size(75, 23);
            this.buttonCSV.TabIndex = 6;
            this.buttonCSV.Text = "openCSV";
            this.buttonCSV.UseVisualStyleBackColor = true;
            this.buttonCSV.Click += new System.EventHandler(this.buttonCSV_Click);
            // 
            // labelCSV
            // 
            this.labelCSV.AutoSize = true;
            this.labelCSV.Location = new System.Drawing.Point(93, 56);
            this.labelCSV.Name = "labelCSV";
            this.labelCSV.Size = new System.Drawing.Size(187, 12);
            this.labelCSV.TabIndex = 7;
            this.labelCSV.Text = "player name CSV is not opened..........";
            // 
            // textBoxNewComer
            // 
            this.textBoxNewComer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNewComer.Location = new System.Drawing.Point(3, 3);
            this.textBoxNewComer.Multiline = true;
            this.textBoxNewComer.Name = "textBoxNewComer";
            this.textBoxNewComer.Size = new System.Drawing.Size(483, 590);
            this.textBoxNewComer.TabIndex = 8;
            // 
            // openCSVDialog
            // 
            this.openCSVDialog.DefaultExt = "csv";
            this.openCSVDialog.Filter = "プレイヤー名csv(*.csv)|*.csv";
            this.openCSVDialog.RestoreDirectory = true;
            this.openCSVDialog.Title = "openCSV";
            // 
            // buttonOther
            // 
            this.buttonOther.Enabled = false;
            this.buttonOther.Location = new System.Drawing.Point(12, 85);
            this.buttonOther.Name = "buttonOther";
            this.buttonOther.Size = new System.Drawing.Size(158, 23);
            this.buttonOther.TabIndex = 9;
            this.buttonOther.Text = "open another XML for Merge";
            this.buttonOther.UseVisualStyleBackColor = true;
            this.buttonOther.Click += new System.EventHandler(this.buttonOther_Click);
            // 
            // labelOther
            // 
            this.labelOther.AutoSize = true;
            this.labelOther.Location = new System.Drawing.Point(177, 85);
            this.labelOther.Name = "labelOther";
            this.labelOther.Size = new System.Drawing.Size(170, 12);
            this.labelOther.TabIndex = 10;
            this.labelOther.Text = "another notesXML is not opened";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageNew);
            this.tabControl.Controls.Add(this.tabPageUpdate);
            this.tabControl.Location = new System.Drawing.Point(12, 143);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(497, 621);
            this.tabControl.TabIndex = 11;
            // 
            // tabPageNew
            // 
            this.tabPageNew.Controls.Add(this.textBoxNewComer);
            this.tabPageNew.Location = new System.Drawing.Point(4, 21);
            this.tabPageNew.Name = "tabPageNew";
            this.tabPageNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNew.Size = new System.Drawing.Size(489, 596);
            this.tabPageNew.TabIndex = 0;
            this.tabPageNew.Text = "new";
            this.tabPageNew.UseVisualStyleBackColor = true;
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.Controls.Add(this.textBoxUpdate);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 21);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(489, 596);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Update";
            this.tabPageUpdate.UseVisualStyleBackColor = true;
            // 
            // textBoxUpdate
            // 
            this.textBoxUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUpdate.Location = new System.Drawing.Point(3, 3);
            this.textBoxUpdate.Multiline = true;
            this.textBoxUpdate.Name = "textBoxUpdate";
            this.textBoxUpdate.Size = new System.Drawing.Size(483, 590);
            this.textBoxUpdate.TabIndex = 9;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(521, 24);
            this.menuStrip.TabIndex = 12;
            this.menuStrip.Text = "menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openXMLToolStripMenuItem,
            this.toolStripSeparator1,
            this.openPlayerNamesToolStripMenuItem,
            this.openConvertedCSVToolStripMenuItem,
            this.openAnotherXMLToolStripMenuItem,
            this.toolStripSeparator2,
            this.convertCSVToolStripMenuItem,
            this.toolStripSeparator3,
            this.executeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openXMLToolStripMenuItem
            // 
            this.openXMLToolStripMenuItem.Name = "openXMLToolStripMenuItem";
            this.openXMLToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openXMLToolStripMenuItem.Text = "openXML";
            this.openXMLToolStripMenuItem.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // openPlayerNamesToolStripMenuItem
            // 
            this.openPlayerNamesToolStripMenuItem.Enabled = false;
            this.openPlayerNamesToolStripMenuItem.Name = "openPlayerNamesToolStripMenuItem";
            this.openPlayerNamesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openPlayerNamesToolStripMenuItem.Text = "openPlayerNames";
            this.openPlayerNamesToolStripMenuItem.Click += new System.EventHandler(this.buttonCSV_Click);
            // 
            // openConvertedCSVToolStripMenuItem
            // 
            this.openConvertedCSVToolStripMenuItem.Enabled = false;
            this.openConvertedCSVToolStripMenuItem.Name = "openConvertedCSVToolStripMenuItem";
            this.openConvertedCSVToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openConvertedCSVToolStripMenuItem.Text = "openConvertedCSV";
            // 
            // openAnotherXMLToolStripMenuItem
            // 
            this.openAnotherXMLToolStripMenuItem.Enabled = false;
            this.openAnotherXMLToolStripMenuItem.Name = "openAnotherXMLToolStripMenuItem";
            this.openAnotherXMLToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openAnotherXMLToolStripMenuItem.Text = "openAnotherXML";
            this.openAnotherXMLToolStripMenuItem.Click += new System.EventHandler(this.buttonOther_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // convertCSVToolStripMenuItem
            // 
            this.convertCSVToolStripMenuItem.Enabled = false;
            this.convertCSVToolStripMenuItem.Name = "convertCSVToolStripMenuItem";
            this.convertCSVToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.convertCSVToolStripMenuItem.Text = "convertCSV";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(166, 6);
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.executeToolStripMenuItem.Text = "Execute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // bgWorkerExecute
            // 
            this.bgWorkerExecute.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerExecute_DoWork);
            this.bgWorkerExecute.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorkerExecute_ProgressChanged);
            this.bgWorkerExecute.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerExecute_RunWorkerCompleted);
            // 
            // numericUpDownStart
            // 
            this.numericUpDownStart.Location = new System.Drawing.Point(378, 30);
            this.numericUpDownStart.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStart.Name = "numericUpDownStart";
            this.numericUpDownStart.Size = new System.Drawing.Size(51, 19);
            this.numericUpDownStart.TabIndex = 13;
            this.numericUpDownStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownEnd
            // 
            this.numericUpDownEnd.Location = new System.Drawing.Point(458, 30);
            this.numericUpDownEnd.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownEnd.Name = "numericUpDownEnd";
            this.numericUpDownEnd.Size = new System.Drawing.Size(51, 19);
            this.numericUpDownEnd.TabIndex = 14;
            this.numericUpDownEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownEnd.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(435, 34);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(17, 12);
            this.labelAccount.TabIndex = 15;
            this.labelAccount.Text = "～";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(521, 776);
            this.Controls.Add(this.labelAccount);
            this.Controls.Add(this.numericUpDownEnd);
            this.Controls.Add(this.numericUpDownStart);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.labelOther);
            this.Controls.Add(this.buttonOther);
            this.Controls.Add(this.labelExecute);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.labelCSV);
            this.Controls.Add(this.buttonCSV);
            this.Controls.Add(this.labelOpen);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.Opacity = 0.9D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PTRtoNote";
            this.tabControl.ResumeLayout(false);
            this.tabPageNew.ResumeLayout(false);
            this.tabPageNew.PerformLayout();
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageUpdate.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openXMLDialog;
        private System.Windows.Forms.SaveFileDialog saveXMLDialog;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelOpen;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Label labelExecute;
        private System.Windows.Forms.Button buttonCSV;
        private System.Windows.Forms.Label labelCSV;
        private System.Windows.Forms.OpenFileDialog openCSVDialog;
        private System.Windows.Forms.TextBox textBoxNewComer;
        private System.Windows.Forms.Button buttonOther;
        private System.Windows.Forms.Label labelOther;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageNew;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.TextBox textBoxUpdate;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPlayerNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openAnotherXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem convertCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openConvertedCSVToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bgWorkerExecute;
        private System.Windows.Forms.NumericUpDown numericUpDownStart;
        private System.Windows.Forms.NumericUpDown numericUpDownEnd;
        private System.Windows.Forms.Label labelAccount;
    }
}

