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
        string[] Username;
        string[] Password;

        /// <summary>log4netのインスタンス</summary>
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FormMain()
        {
            InitializeComponent();

            // PTRのアカウント情報を読み込む
            Username = Properties.Settings.Default.Logins.Split(',');
            Password = Properties.Settings.Default.Passwords.Split(',');
            if (Username.Count() != Properties.Settings.Default.AccountNum
                || Password.Count() != Properties.Settings.Default.AccountNum)
            {
                string error_message = "Error at Logins, Password or AccountNum.";
                if (logger.IsErrorEnabled) logger.Error(error_message);
                System.Windows.Forms.MessageBox.Show(error_message);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {

        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            PTRconnection conn = new PTRconnection();
            conn.Username = Username[0];
            conn.Password = Username[0];
            if (conn.PTRConnect())
            {
                string str = conn.GetPTR("chiyuki");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void buttonCSV_Click(object sender, EventArgs e)
        {

        }
    }
}
