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
        int account_number;

        // notesXMLオリジナル読み込み用
        XmlTextReader xmlReader;

        // notesXML更新版生成用
        XmlTextWriter xmlWriter;

        // PTRとの接続
        PTRconnection conn;

        // PTRとの接続フラグ
        bool CannotConnect;

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

            conn = new PTRconnection();
            account_number = 0;
            conn.Username = Username[0];
            conn.Password = Password[0];
            CannotConnect = false;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openXMLDialog.ShowDialog() == DialogResult.OK)
            {
                xmlReader = new XmlTextReader(openXMLDialog.FileName);
                this.labelOpen.Text = "notesXML is opend and ready to Execute...";
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

                        // 取得指示があった場合
                        if (note_str == "a" || (note_str[0] == 'a' && note_str[1] == '\n'))
                        {
                            // 取得実行
                            data = GetPTR(player_name);
                            if (data != null)
                            {
                                if (note_str == "a") note_str = data.GetNoteString();
                                else note_str = data.GetNoteString() + note_str.Substring(1);
                            }
                        }
                        // データが無い場合
                        else if (data.MakePTRDataFromNoteStr(player_name, note_str) == false)
                        {
                            // データを先頭に付与
                            data = GetPTR(player_name);
                            if (data != null)
                            {
                                note_str = data.GetNoteString() + '\n' + note_str;
                            }
                        }
                        // データある場合
                        else
                        {
                            // データが古い場合は再取得
                            if (data.GetDate < System.DateTime.Today - new System.TimeSpan(Properties.Settings.Default.ReacquisitionSpanDays, 0, 0, 0))
                            {
                                PTRData new_data = GetPTR(player_name);
                                if(new_data != null && new_data.Hands >= data.Hands)
                                {
                                    data = new_data;
                                    note_str = data.GetNoteString() + '\n' + note_str;
                                }
                            }
                        }

                        // ラベル更新処理
                        if (data != null)
                        {
                            label = DecideLabel(data);
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void buttonCSV_Click(object sender, EventArgs e)
        {

        }

        private PTRData GetPTR(string player_name)
        {
            if (CannotConnect == true) return null;

            while (account_number < Properties.Settings.Default.AccountNum)
            {
                conn.Username = Username[account_number];
                conn.Password = Password[account_number];

                // 検索回数が無い場合は次のアカウントに切替
                if (conn.SearchesRemaining == 0)
                {
                    conn.PTRDisconnect();
                    ++account_number;
                    conn.SearchesRemaining = 10;
                    continue;
                }

                // 接続されていない場合はログイン
                for (int i = 0; i < 3; ++i)
                {
                    if (conn.isConnected == true) break;
                    conn.PTRConnect();
                }
                if (conn.isConnected == false)
                {
                    ++account_number;
                    conn.SearchesRemaining = 10;
                    continue;
                }
                else
                {
                    PTRData data;
                    try
                    {
                        data = conn.GetPTRData(player_name);
                        System.Threading.Thread.Sleep(1000);

                        if (data == null)
                        {
                            conn.PTRDisconnect();
                            ++account_number;
                            conn.SearchesRemaining = 10;
                            continue;
                        }
                        else return data;
                    }
                    catch (System.Net.WebException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            CannotConnect = true;
            return null;
        }

        private string DecideLabel(PTRData data)
        {
            string label = "5";
            if (data.BB_100 > Properties.Settings.Default.Label_0_Min) label = "0";
            else if (data.Hands < Properties.Settings.Default.Label_6_Hand_Max) label = "6";
            else if (data.BB_100 > Properties.Settings.Default.Label_1_Min) label = "1";
            else if (data.BB_100 > Properties.Settings.Default.Label_2_Min) label = "2";
            else if (data.BB_100 > Properties.Settings.Default.Label_3_Min) label = "3";
            else if (data.BB_100 > Properties.Settings.Default.Label_4_Min) label = "4";

            return label;
        }
    }
}