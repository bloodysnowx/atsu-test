using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.IO;
using System.Xml;

namespace PTRtoNote
{
    public partial class FormMain : Form
    {
        #region MEMBER
        /// <summary>ログインユーザ名</summary>
        string[] Username;
        /// <summary>ログインパスワード</summary>
        string[] Password;
        /// <summary>現在のアカウント番号</summary>
        int account_number = 0;

        /// <summary>notesXMLオリジナル読み込み用リーダ</summary>
        XmlTextReader xmlReader;
        /// <summary>notesXML更新版生成用ファイルストリーム</summary>
        FileStream fs;
        /// <summary>notesXML更新版生成用ライタ</summary>
        XmlTextWriter xmlWriter;

        /// <summary>PTRとの接続</summary>
        PTRconnection conn;
        /// <summary>PTRとの接続フラグ</summary>
        bool CannotConnect = false;

        /// <summary>PTRで検索を実行した回数</summary>
        uint searchedCount = 0;
        /// <summary>noteが"a"のままの数</summary>
        uint aCount = 0;
        /// <summary>現時点までで読み込んだプレイヤー数</summary>
        uint player_count = 0;

        /// <summary>新しいプレイヤー名のリスト</summary>
        ArrayList newPlayerList = null;

        Random rnd = new Random();

        /// <summary>log4netのインスタンス</summary>
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        /// <summary>
        /// コンストラクタ(PTRのアカウント情報読み込み、初期化処理)
        /// </summary>
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
            else
            {
                if (logger.IsInfoEnabled)
                    logger.InfoFormat("{0} accounts are read.", Properties.Settings.Default.AccountNum);
            }

            // 初期化処理
            conn = new PTRconnection();
            conn.Username = Username[0];
            conn.Password = Password[0];
            numericUpDownStart.Maximum = Properties.Settings.Default.AccountNum;
            numericUpDownEnd.Maximum = Properties.Settings.Default.AccountNum;
            numericUpDownMaxSearch.Value = Properties.Settings.Default.MaxSearchNum;
        }

        /// <summary>
        /// openXMLを押した時の処理(notesXMLを開く)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openXMLDialog.ShowDialog() == DialogResult.OK)
            {
                xmlReader = new XmlTextReader(openXMLDialog.FileName);
                this.labelOpen.Text = "notesXML is opened and ready to Execute...";
            }
        }

        /// <summary>
        /// Executeを押した時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExecute_Click(object sender, EventArgs e)
        {
            account_number = System.Convert.ToInt32(numericUpDownStart.Value) - 1;
            player_count = 0;
            uint old_searchedCount = searchedCount;

            #region XML_INIT
            if (saveXMLDialog.ShowDialog() != DialogResult.OK) return;

            #region STATE_CHANGE
            this.buttonExecute.Enabled = false;
            this.buttonOpen.Enabled = false;
            this.buttonCSV.Enabled = false;
            this.openXMLToolStripMenuItem.Enabled = false;
            this.openPlayerNamesToolStripMenuItem.Enabled = false;
            this.executeToolStripMenuItem.Enabled = false;
            #endregion

            // XMLを書き込み用に開く
            fs = new FileStream(saveXMLDialog.FileName, FileMode.Create, FileAccess.Write);
            xmlWriter = new XmlTextWriter(fs, new UTF8Encoding(false));
            // XMLの設定をする
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';

            // notesXMLの先頭書き込み
            xmlWriter.WriteStartDocument();
            #endregion

            #region SMART_BUDDY_OPEN
            // string group_3_path = saveXMLDialog.InitialDirectory + "group_3.txt";
            // FileStream group_3_fs = new FileStream(group_3_path, FileMode.Create, FileAccess.Write);
            // StreamWriter group_3_sw = new StreamWriter(group_3_fs);
            string group_4_path = saveXMLDialog.InitialDirectory + "group_4.txt";
            FileStream group_4_fs = new FileStream(group_4_path, FileMode.Create, FileAccess.Write);
            StreamWriter group_4_sw = new StreamWriter(group_4_fs);
            string group_5_path = saveXMLDialog.InitialDirectory + "group_5.txt";
            FileStream group_5_fs = new FileStream(group_5_path, FileMode.Create, FileAccess.Write);
            StreamWriter group_5_sw = new StreamWriter(group_5_fs);
            #endregion

            // xmlReader.Readをtry-catchに修正
            while (xmlReader.Read())
            {
                #region READ_HEADER
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
                    else if (xmlReader.Name == "labels") xmlWriter.WriteStartElement("labels");
                    // ラベル情報をコピーする
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
                #endregion
                    else if (xmlReader.Name == "note")
                    {
                        ++player_count;
                        // PlayerIDを読み込む
                        xmlReader.MoveToAttribute("player");
                        string player_name = xmlReader.Value;
                        // System.Diagnostics.Debug.WriteLine(player_name);
                        // ToDO : csvから読み込んだプレイヤー一覧から削除する処理を実装

                        // ラベル情報を読み込む
                        xmlReader.MoveToAttribute("label");
                        string label = xmlReader.Value;

                        // updateを読み込む
                        string update = null;
                        if (xmlReader.MoveToAttribute("update"))
                        {
                            update = xmlReader.Value;
                        }

                        // noteの内容を読み込む
                        string note_str = "";
                        try
                        {
                            note_str = xmlReader.ReadString();
                        }
                        catch (Exception ex)
                        {
                            if (logger.IsErrorEnabled) logger.Error(player_name + "'s note is broken.", ex);
                        }

                        PTRData data = new PTRData(player_name);

                        // 取得指示があった場合
                        if (note_str.Length < 1 || note_str == "a" || (note_str[0] == 'a' && note_str[1] == '\n'))
                        {
                            // 取得実行
                            data = GetPTR(player_name);
                            if (data != null)
                            {
                                if (note_str == "a" || note_str.Length < 1)
                                {
                                    note_str = data.GetNoteString();
                                    addToNewComer(player_name, data);
                                }
                                else note_str = data.GetNoteString() + note_str.Substring(1);
                            }
                            else ++aCount;
                        }
                        // データが無い場合
                        else if (data.MakePTRDataFromNoteStr(player_name, note_str) == false)
                        {
                            // データを先頭に付与
                            data = GetPTR(player_name);
                            if (data != null)
                            {
                                note_str = data.GetNoteString() + '\n' + note_str;
                                addToUpdateComer(player_name, data);
                            }
                        }
                        // データがある場合
                        else
                        {
                            // データが古い場合は再取得
                            if (data.GetDate < System.DateTime.Today - new System.TimeSpan(Properties.Settings.Default.ReacquisitionSpanDays, 0, 0, 0))
                            {
                                PTRData new_data = GetPTR(player_name);
                                if (new_data != null && new_data.Hands >= data.Hands)
                                {
                                    data = new_data;
                                    note_str = data.GetNoteString() + '\n' + note_str;

                                    addToUpdateComer(player_name, data);
                                }
                            }
                                // ラベル6でデータが古い場合は再取得
                            else if (label == "6" && data.GetDate < System.DateTime.Today - new System.TimeSpan(Properties.Settings.Default.Label6ReacquisitionSpanDays, 0, 0, 0))
                            {
                                PTRData new_data = GetPTR(player_name);
                                if (new_data != null && new_data.Hands >= data.Hands)
                                {
                                    data = new_data;
                                    note_str = data.GetNoteString() + '\n' + note_str;

                                    addToUpdateComer(player_name, data);
                                }
                            }
                        }

                        // ラベル更新処理
                        if (label == "7")
                        {
                        }
                        else if (data != null) label = DecideLabel(data);
                        #region SMART_BUDDY_WRITE
                        if (data != null)
                        {
                            if (label == "5") // (data.BB_100 <= Properties.Settings.Default.Label_4_Min)
                            {
                                group_5_sw.WriteLine(makeSmartBuddyString(player_name, data));
                            }
                            else if (label == "4") //(data.BB_100 <= Properties.Settings.Default.Label_3_Min)
                            {
                                group_4_sw.WriteLine(makeSmartBuddyString(player_name, data));
                            }
                            // else if (data.BB_100 <= Properties.Settings.Default.Label_2_Min)
                                // group_3_sw.WriteLine(makeSmartBuddyString(player_name, data));
                        }
                        #endregion
                        // XMLに要素を書き込む
                        xmlWriter.WriteStartElement("note");
                        xmlWriter.WriteStartAttribute("player");
                        xmlWriter.WriteString(player_name);
                        xmlWriter.WriteEndAttribute();
                        xmlWriter.WriteStartAttribute("label");
                        xmlWriter.WriteString(label);
                        xmlWriter.WriteEndAttribute();
                        if (update != null)
                        {
                            xmlWriter.WriteStartAttribute("update");
                            xmlWriter.WriteString(update);
                            xmlWriter.WriteEndAttribute();
                        }
                        xmlWriter.WriteString(note_str);
                        xmlWriter.WriteEndElement();

                        if (searchedCount != old_searchedCount || player_count % 1000 == 0)
                        {
                            updateLabelExecute(account_number, searchedCount, player_count, aCount);

                            this.Refresh();
                            Application.DoEvents();
                            old_searchedCount = searchedCount;
                        }
                    }
                }
                #region
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmlReader.Name == "labels")
                        xmlWriter.WriteEndElement();
                    else if (xmlReader.Name == "notes")
                        xmlWriter.WriteEndElement();
                }
                #endregion
            } // while
            #region XML_FIN
            // notesXMLの末尾書き込み
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            fs.Close();
            xmlReader.Close();
            #endregion
            #region SMART_BUDDY_CLOSE
            // group_3_sw.Close();
            // group_3_fs.Close();
            group_4_sw.Close();
            group_4_fs.Close();
            group_5_sw.Close();
            group_5_fs.Close();
            #endregion
            updateLabelExecute(account_number, searchedCount, player_count, aCount);
        }

        private void addToNewComer(string player_name, PTRData data)
        {
            // Label 3 以上のカモを発見した場合は、ニューカマーリストに表示する
            if (data.BB_100 < Properties.Settings.Default.Label_3_Min && data.Hands > Properties.Settings.Default.Label_6_Hand_Max)
            {
                string str = "2" + ";" + player_name + ";" + "" + ";" + "" + ";" + data.GetNoteString();
                this.textBoxNewComer.Text += str + System.Environment.NewLine;
                System.Diagnostics.Debug.WriteLine(str);
            }
        }

        private void addToUpdateComer(string player_name, PTRData data)
        {
            // Label 3 以上のカモを発見した場合は、ニューカマーリストに表示する
            if (data.BB_100 < Properties.Settings.Default.Label_3_Min && data.Hands > Properties.Settings.Default.Label_6_Hand_Max)
            {
                this.textBoxUpdate.Text += "2" + ";" + player_name + ";" + "" + ";" + "" + ";" + data.GetNoteString() + System.Environment.NewLine;
            }
        }

        private void updateLabelExecute(int account_number, uint searchedCount, uint player_count, uint aCount)
        {
            this.labelExecute.Text = account_number.ToString() + " accounts were used, "
                + searchedCount.ToString() + " players were searched at PTR, " + '\n'
                + player_count.ToString() + " labels were updated, "
                + "could not search " + aCount.ToString() + " players.";
        }

        private void buttonCSV_Click(object sender, EventArgs e)
        {
            if (openCSVDialog.ShowDialog() == DialogResult.OK)
            {
                newPlayerList = new ArrayList();
                StreamReader sr = new StreamReader(openCSVDialog.FileName);

                string new_name = "";

                while ((new_name = sr.ReadLine()) != null)
                {
                    newPlayerList.Add(new_name.Trim());
                }

                this.labelCSV.Text = "CSV is opened and " + newPlayerList.Count + " players are read...";
            }
        }

        private PTRData GetPTR(string player_name)
        {
            if (CannotConnect == true) return null;

            while (account_number < numericUpDownEnd.Value && searchedCount < numericUpDownMaxSearch.Value)
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
                        
                        System.Threading.Thread.Sleep(Properties.Settings.Default.WaitMainTime + rnd.Next(Properties.Settings.Default.WaitRandTime));

                        if (data == null)
                        {
                            conn.PTRDisconnect();
                            ++account_number;
                            conn.SearchesRemaining = 10;
                            continue;
                        }
                        else
                        {
                            ++searchedCount;
                            if (searchedCount >= numericUpDownMaxSearch.Value)
                                CannotConnect = true;

                            if (logger.IsDebugEnabled)
                            {
                                logger.DebugFormat("{0} : {1} {2}", searchedCount, player_name, data.GetNoteString());
                            }
                            return data;
                        }
                    }
                    catch (System.Net.WebException e)
                    {
                        if (logger.IsErrorEnabled)
                            logger.Error(player_name + "'s PTR returns error.", e);
                        return null;
                    }
                    catch (Exception e)
                    {
                        if (logger.IsErrorEnabled)
                            logger.Error(player_name + "'s PTR returns error.", e);
                        return null;
                    }
                }
            }
            CannotConnect = true;
            return null;
        }

        /// <summary>
        /// PTRDataからラベルを決定する
        /// </summary>
        /// <param name="data">PTRのデータ</param>
        /// <returns>決定したラベル</returns>
        private string DecideLabel(PTRData data)
        {
            decimal label_bb = data.BB_100;
            uint label_hand = data.Hands;
            if (data.HU_Hands > data.Hands)
            {
                label_bb = data.HU_BB_100;
                label_hand = data.HU_Hands;
            }

            string label = "5";
            if (label_bb > Properties.Settings.Default.Label_0_Min) label = "0";
            else if (label_bb > Properties.Settings.Default.Label_1_Min) label = "1";
            else if (label_bb > Properties.Settings.Default.Label_2_Min) label = "2";
            else if (label_hand < Properties.Settings.Default.Label_6_Hand_Max) label = "6";
            else if (label_bb > Properties.Settings.Default.Label_3_Min) label = "3";
            else if (label_bb > Properties.Settings.Default.Label_4_Min) label = "4";

            return label;
        }

        private void buttonOther_Click(object sender, EventArgs e)
        {

        }

        private void bgWorkerExecute_DoWork(object sender, DoWorkEventArgs e)
        {
            // テキストボックスの内容更新にデリゲート？
            // ラベルアップデートはProgressChanged
        }

        private void bgWorkerExecute_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            updateLabelExecute(account_number, searchedCount, player_count, aCount);
        }

        private void bgWorkerExecute_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private string makeSmartBuddyString(string player_name, PTRData data)
        {
            string smart_buddy_str = "2" + ";" + player_name + ";" + "" + ";" + "Yellow" + ";" + data.GetNoteString();

            return smart_buddy_str;
        }
    }
}