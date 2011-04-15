using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;

namespace PTRtoNote
{
    public partial class FormMain : Form
    {
        string[] Username;
        string[] Password;

        // notesXMLオリジナル読み込み用
        XmlTextReader xmlReader;

        // notesXML更新版生成用
        MemoryStream mem;
        XmlTextWriter xmlWriter;

        /// <summary>log4netのインスタンス</summary>
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FormMain()
        {
            InitializeComponent();

            // PTRのアカウント情報を読み込む
            Username = Properties.Settings.Default.Logins.Split(',');
            Password = Properties.Settings.Default.Passwords.Split(',');

            // Logins, Password, AccountNumが不一致な場合
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
            if (openXMLDialog.ShowDialog() == DialogResult.OK)
            {
                xmlReader = new XmlTextReader(openXMLDialog.FileName);
            }
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            // メモリ上に書き込む
            mem = new MemoryStream();
            xmlWriter = new XmlTextWriter(mem, new UTF8Encoding(false));
            // XMLの設定
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';
            // notesXMLの先頭書き込み
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("notes");
            xmlWriter.WriteStartAttribute("version");
            xmlWriter.WriteString("1");
            xmlWriter.WriteEndAttribute();

            while (false)
            {
            }

            // notesXMLの末尾書き込み
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            PTRconnection conn = new PTRconnection();
            conn.Username = Username[0];
            conn.Password = Username[0];
            if (conn.PTRConnect())
            {
                string str = conn.GetPTRWebPage("chiyuki");
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