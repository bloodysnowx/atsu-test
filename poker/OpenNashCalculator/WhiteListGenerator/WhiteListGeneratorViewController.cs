using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WhiteListGeneratorUI
{
    public partial class WhiteListGeneratorViewController : Form
    {
        public WhiteListGeneratorViewController()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (userNameTextBox.Text.Length == 0) return;
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            
        }
    }
}
