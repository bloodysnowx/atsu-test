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
            if (saveXMLDialog.ShowDialog() != DialogResult.OK)
                return;

            FileStream fs = new FileStream(saveXMLDialog.FileName, FileMode.Create, FileAccess.Write);
            xmlWriter = new XmlTextWriter(fs, new UTF8Encoding(false));

            // XMLの設定
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';
            // notesXMLの先頭書き込み
            xmlWriter.WriteStartDocument();
            
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.XmlDeclaration) continue;
                if (xmlReader.NodeType == XmlNodeType.Whitespace) continue;
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "notes")
                    {
                        xmlWriter.WriteStartElement("notes");
                        xmlWriter.WriteStartAttribute("version");
                        xmlWriter.WriteString("1");
                        xmlWriter.WriteEndAttribute();
                    }
                    else if (xmlReader.Name == "labels")
                    {
                        xmlWriter.WriteStartElement("labels");
                    }
                    else if (xmlReader.Name == "label")
                    {
                        xmlWriter.WriteStartElement("label");
                        if (xmlReader.MoveToAttribute("index"))
                        {
                            xmlWriter.WriteStartAttribute("index");
                            xmlWriter.WriteString(xmlReader.Value);
                            xmlWriter.WriteEndAttribute();
                        }
                        if (xmlReader.MoveToAttribute("color"))
                        {
                            xmlWriter.WriteStartAttribute("color");
                            xmlWriter.WriteString(xmlReader.Value);
                            xmlWriter.WriteEndAttribute();
                        }
                        xmlWriter.WriteString(xmlReader.ReadString());
                        if (xmlReader.NodeType == XmlNodeType.EndElement)
                            xmlWriter.WriteEndElement();
                    }
                    else if (xmlReader.Name == "note")
                    {
                        xmlReader.MoveToAttribute("player");
                        string player_name = xmlReader.Value;
                        // csvから読み込んだプレイヤー一覧から削除する
                        xmlReader.MoveToAttribute("label");
                        string label = xmlReader.Value;

                        string note_str = xmlReader.ReadString();

                        PTRData data = new PTRData(player_name);
                        if (data.MakePTRDataFromNoteStr(player_name, note_str))
                        {
                        }

                        xmlWriter.WriteStartElement("note");
                        xmlWriter.WriteStartAttribute("player");
                        xmlWriter.WriteString(player_name);
                        xmlWriter.WriteEndAttribute();
                        xmlWriter.WriteStartAttribute("label");
                        xmlWriter.WriteString(label);
                        xmlWriter.WriteEndAttribute();
                        xmlWriter.WriteString(note_str);
                        xmlWriter.WriteEndElement();
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmlReader.Name == "labels")
                        xmlWriter.WriteEndElement();
                    else if (xmlReader.Name == "notes")
                        xmlWriter.WriteEndElement();
                }
            }

            // notesXMLの末尾書き込み
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            PTRconnection conn = new PTRconnection();
            conn.Username = Username[1];
            conn.Password = Password[1];
            if (conn.PTRConnect())
            {
                conn.GetPTRData("KAMOX");
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