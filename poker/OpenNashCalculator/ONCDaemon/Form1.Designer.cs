namespace ONCDaemon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.labelFolderPS = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelFolderFT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelFolderPS
            // 
            resources.ApplyResources(this.labelFolderPS, "labelFolderPS");
            this.labelFolderPS.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelFolderPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFolderPS.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelFolderPS.Name = "labelFolderPS";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // labelCount
            // 
            resources.ApplyResources(this.labelCount, "labelCount");
            this.labelCount.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelCount.Name = "labelCount";
            // 
            // labelFolderFT
            // 
            resources.ApplyResources(this.labelFolderFT, "labelFolderFT");
            this.labelFolderFT.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelFolderFT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFolderFT.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelFolderFT.Name = "labelFolderFT";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelFolderFT);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelFolderPS);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFolderPS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelFolderFT;


    }
}

