using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PTRtoNote
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {

        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            PTRconnection conn = new PTRconnection();
            conn.Username = "shobon2";
            conn.Password = "shakin";
            if (conn.PTRConnect())
            {
                string str = conn.GetPTR("chiyuki");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }
    }
}
