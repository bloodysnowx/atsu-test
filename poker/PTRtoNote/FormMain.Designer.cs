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
            this.tabControl.SuspendLayout();
            this.tabPageNew.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
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
            this.buttonOpen.Location = new System.Drawing.Point(12, 12);
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
            this.labelOpen.Location = new System.Drawing.Point(93, 12);
            this.labelOpen.Name = "labelOpen";
            this.labelOpen.Size = new System.Drawing.Size(148, 12);
            this.labelOpen.TabIndex = 1;
            this.labelOpen.Text = "notesXML is not opened..........";
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(12, 99);
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
            this.labelExecute.Location = new System.Drawing.Point(93, 99);
            this.labelExecute.Name = "labelExecute";
            this.labelExecute.Size = new System.Drawing.Size(136, 12);
            this.labelExecute.TabIndex = 4;
            this.labelExecute.Text = "PTR is not connected..........\r\n";
            // 
            // buttonCSV
            // 
            this.buttonCSV.Enabled = false;
            this.buttonCSV.Location = new System.Drawing.Point(12, 41);
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
            this.labelCSV.Location = new System.Drawing.Point(93, 41);
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
            this.textBoxNewComer.Size = new System.Drawing.Size(483, 605);
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
            this.buttonOther.Location = new System.Drawing.Point(12, 70);
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
            this.labelOther.Location = new System.Drawing.Point(177, 70);
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
            this.tabControl.Location = new System.Drawing.Point(12, 128);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(497, 636);
            this.tabControl.TabIndex = 11;
            // 
            // tabPageNew
            // 
            this.tabPageNew.Controls.Add(this.textBoxNewComer);
            this.tabPageNew.Location = new System.Drawing.Point(4, 21);
            this.tabPageNew.Name = "tabPageNew";
            this.tabPageNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNew.Size = new System.Drawing.Size(489, 611);
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
            this.tabPageUpdate.Size = new System.Drawing.Size(489, 611);
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
            this.textBoxUpdate.Size = new System.Drawing.Size(483, 605);
            this.textBoxUpdate.TabIndex = 9;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(521, 776);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.labelOther);
            this.Controls.Add(this.buttonOther);
            this.Controls.Add(this.labelExecute);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.labelCSV);
            this.Controls.Add(this.buttonCSV);
            this.Controls.Add(this.labelOpen);
            this.Controls.Add(this.buttonOpen);
            this.Name = "FormMain";
            this.Opacity = 0.9D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PTRtoNote";
            this.tabControl.ResumeLayout(false);
            this.tabPageNew.ResumeLayout(false);
            this.tabPageNew.PerformLayout();
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageUpdate.PerformLayout();
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
    }
}

