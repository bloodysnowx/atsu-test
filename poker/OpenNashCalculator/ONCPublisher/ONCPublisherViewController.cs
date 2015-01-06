using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhiteListGeneratorUI
{
    public partial class ONCPublisherViewController : Form
    {
        ONCPublisher publisher = new ONCPublisher();

        public ONCPublisherViewController()
        {
            InitializeComponent();
        }

        private async void publishButton_Click(object sender, EventArgs e)
        {
            if (userNameTextBox.Text.Length == 0) return;
            await Task.Run(() => { publisher.publish(userNameTextBox.Text, System.IO.Directory.GetCurrentDirectory()); });
            System.Windows.Forms.MessageBox.Show("Release Zip Complete!");
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            publisher.generate(userNameTextBox.Text, System.IO.Directory.GetCurrentDirectory());
        }
    }
}
