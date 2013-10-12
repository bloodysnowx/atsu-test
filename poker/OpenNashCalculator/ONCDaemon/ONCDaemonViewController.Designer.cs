namespace ONCDaemon
{
    partial class ONCDaemonViewController
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ONCDaemonViewController));
            this.label1 = new System.Windows.Forms.Label();
            this.labelFolderPS = new System.Windows.Forms.Label();
            this.labelFolderFT = new System.Windows.Forms.Label();
            this.PSFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.FTFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Gainsboro;
            this.label1.Name = "label1";
            // 
            // labelFolderPS
            // 
            resources.ApplyResources(this.labelFolderPS, "labelFolderPS");
            this.labelFolderPS.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelFolderPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFolderPS.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelFolderPS.Name = "labelFolderPS";
            this.labelFolderPS.Click += new System.EventHandler(this.labelFolderPS_Click);
            // 
            // labelFolderFT
            // 
            resources.ApplyResources(this.labelFolderFT, "labelFolderFT");
            this.labelFolderFT.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelFolderFT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFolderFT.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelFolderFT.Name = "labelFolderFT";
            // 
            // PSFolderBrowserDialog
            // 
            resources.ApplyResources(this.PSFolderBrowserDialog, "PSFolderBrowserDialog");
            this.PSFolderBrowserDialog.ShowNewFolderButton = false;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingStripMenuItem,
            this.exitStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // settingStripMenuItem
            // 
            this.settingStripMenuItem.Name = "settingStripMenuItem";
            resources.ApplyResources(this.settingStripMenuItem, "settingStripMenuItem");
            this.settingStripMenuItem.Click += new System.EventHandler(this.settingStripMenuItem_Click);
            // 
            // exitStripMenuItem
            // 
            this.exitStripMenuItem.Name = "exitStripMenuItem";
            resources.ApplyResources(this.exitStripMenuItem, "exitStripMenuItem");
            this.exitStripMenuItem.Click += new System.EventHandler(this.exitStripMenuItem_Click);
            // 
            // ONCDaemonViewController
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ONCDaemon.Properties.Resources._4;
            this.Controls.Add(this.labelFolderFT);
            this.Controls.Add(this.labelFolderPS);
            this.Controls.Add(this.label1);
            this.Name = "ONCDaemonViewController";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ONCDaemonViewController_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFolderPS;
        private System.Windows.Forms.Label labelFolderFT;
        private System.Windows.Forms.FolderBrowserDialog PSFolderBrowserDialog;
        private System.Windows.Forms.FolderBrowserDialog FTFolderBrowserDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitStripMenuItem;


    }
}

