using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserValidatorLib;
using Ionic.Zip;

namespace WhiteListGeneratorUI
{
    public partial class WhiteListGeneratorViewController : Form
    {
        UserNameEncrypter encrypter;
        public WhiteListGeneratorViewController()
        {
            InitializeComponent();
            encrypter = new UserNameEncrypter();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (userNameTextBox.Text.Length == 0) return;
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            System.IO.File.WriteAllText(saveFileDialog.FileName, encrypter.encryptFromUserName(userNameTextBox.Text));
        }

        private void releaseButton_Click(object sender, EventArgs e)
        {
            if (userNameTextBox.Text.Length == 0) return;
            String filesPath = System.IO.Directory.GetCurrentDirectory() + "\\files\\";
            System.IO.File.WriteAllText(filesPath + "whiteList", encrypter.encryptFromUserName(userNameTextBox.Text));
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("Shift_JIS")))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.AddDirectory(filesPath);
                zip.Save(System.IO.Directory.GetCurrentDirectory() + "\\" + userNameTextBox.Text + ".zip");
                System.Windows.Forms.MessageBox.Show("Release Zip Complete!");
            }
        }
    }
}
