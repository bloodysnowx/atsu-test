﻿namespace OpenNashCalculator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBB = new System.Windows.Forms.TextBox();
            this.textBoxSB = new System.Windows.Forms.TextBox();
            this.textBoxAnte = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonBBUP = new System.Windows.Forms.Button();
            this.buttonBBDown = new System.Windows.Forms.Button();
            this.textBoxStructure = new System.Windows.Forms.TextBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "BB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "SB";
            // 
            // textBoxBB
            // 
            this.textBoxBB.Location = new System.Drawing.Point(211, 12);
            this.textBoxBB.Name = "textBoxBB";
            this.textBoxBB.Size = new System.Drawing.Size(100, 19);
            this.textBoxBB.TabIndex = 2;
            this.textBoxBB.TextChanged += new System.EventHandler(this.textBoxBB_TextChanged);
            // 
            // textBoxSB
            // 
            this.textBoxSB.Location = new System.Drawing.Point(211, 39);
            this.textBoxSB.Name = "textBoxSB";
            this.textBoxSB.Size = new System.Drawing.Size(100, 19);
            this.textBoxSB.TabIndex = 3;
            // 
            // textBoxAnte
            // 
            this.textBoxAnte.Location = new System.Drawing.Point(211, 64);
            this.textBoxAnte.Name = "textBoxAnte";
            this.textBoxAnte.Size = new System.Drawing.Size(100, 19);
            this.textBoxAnte.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ante";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Structure";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(213, 172);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 8;
            this.textBox1.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Player*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(211, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "Chips";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(213, 197);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 19);
            this.textBox2.TabIndex = 11;
            this.textBox2.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox2.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(213, 223);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 19);
            this.textBox3.TabIndex = 12;
            this.textBox3.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox3.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(213, 249);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 19);
            this.textBox4.TabIndex = 13;
            this.textBox4.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox4.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(213, 275);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 19);
            this.textBox5.TabIndex = 14;
            this.textBox5.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox5.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(213, 301);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 19);
            this.textBox6.TabIndex = 15;
            this.textBox6.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox6.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(211, 327);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 19);
            this.textBox7.TabIndex = 16;
            this.textBox7.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox7.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(213, 353);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 19);
            this.textBox8.TabIndex = 17;
            this.textBox8.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox8.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(211, 379);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 19);
            this.textBox9.TabIndex = 18;
            this.textBox9.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox9.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(236, 418);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Calculate!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonBBUP
            // 
            this.buttonBBUP.Location = new System.Drawing.Point(50, 8);
            this.buttonBBUP.Name = "buttonBBUP";
            this.buttonBBUP.Size = new System.Drawing.Size(51, 23);
            this.buttonBBUP.TabIndex = 25;
            this.buttonBBUP.Text = "BBUP";
            this.buttonBBUP.UseVisualStyleBackColor = true;
            this.buttonBBUP.Click += new System.EventHandler(this.buttonBBUP_Click);
            // 
            // buttonBBDown
            // 
            this.buttonBBDown.Location = new System.Drawing.Point(107, 8);
            this.buttonBBDown.Name = "buttonBBDown";
            this.buttonBBDown.Size = new System.Drawing.Size(51, 23);
            this.buttonBBDown.TabIndex = 26;
            this.buttonBBDown.Text = "Down";
            this.buttonBBDown.UseVisualStyleBackColor = true;
            this.buttonBBDown.Click += new System.EventHandler(this.buttonBBDown_Click);
            // 
            // textBoxStructure
            // 
            this.textBoxStructure.Location = new System.Drawing.Point(211, 89);
            this.textBoxStructure.Name = "textBoxStructure";
            this.textBoxStructure.Size = new System.Drawing.Size(100, 19);
            this.textBoxStructure.TabIndex = 5;
            this.textBoxStructure.Text = "0.65,0.35";
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(133, 42);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(72, 16);
            this.checkBox10.TabIndex = 36;
            this.checkBox10.Text = "BBと連動";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "Seat1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 12);
            this.label8.TabIndex = 38;
            this.label8.Text = "Seat2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "Seat3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 249);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 12);
            this.label10.TabIndex = 40;
            this.label10.Text = "Seat4";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 275);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 12);
            this.label11.TabIndex = 41;
            this.label11.Text = "Seat5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 301);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 12);
            this.label12.TabIndex = 42;
            this.label12.Text = "Seat6";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 327);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 12);
            this.label13.TabIndex = 43;
            this.label13.Text = "Seat7";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 353);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 12);
            this.label14.TabIndex = 44;
            this.label14.Text = "Seat8";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 378);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 12);
            this.label15.TabIndex = 45;
            this.label15.Text = "Seat9";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton9);
            this.groupBox1.Controls.Add(this.radioButton8);
            this.groupBox1.Controls.Add(this.radioButton7);
            this.groupBox1.Controls.Add(this.radioButton6);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(121, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(76, 270);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Checked = true;
            this.radioButton9.Location = new System.Drawing.Point(16, 240);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(39, 16);
            this.radioButton9.TabIndex = 47;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "BB";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(16, 214);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(38, 16);
            this.radioButton8.TabIndex = 7;
            this.radioButton8.Text = "SB";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(16, 188);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(39, 16);
            this.radioButton7.TabIndex = 6;
            this.radioButton7.Text = "BU";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(16, 162);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(39, 16);
            this.radioButton6.TabIndex = 5;
            this.radioButton6.Text = "CO";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(15, 136);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(51, 16);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.Text = "MP+2";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(15, 110);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(51, 16);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "MP+1";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(16, 84);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(39, 16);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "MP";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(16, 58);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "UTG+1";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 35);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(46, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "UTG";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.postionRadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 453);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBox10);
            this.Controls.Add(this.buttonBBDown);
            this.Controls.Add(this.buttonBBUP);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStructure);
            this.Controls.Add(this.textBoxAnte);
            this.Controls.Add(this.textBoxSB);
            this.Controls.Add(this.textBoxBB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "OpenNashCalculator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBB;
        private System.Windows.Forms.TextBox textBoxSB;
        private System.Windows.Forms.TextBox textBoxAnte;
        private System.Windows.Forms.TextBox textBoxStructure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonBBUP;
        private System.Windows.Forms.Button buttonBBDown;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}
