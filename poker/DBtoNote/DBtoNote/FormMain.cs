using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SQLite;
using System.Xml;
using System.IO;
using System.Web;

namespace DBtoNote
{
    public partial class FormMain : Form
    {
        FileStream fs;
        XmlTextWriter xmlWriter;
        SQLiteConnection conn;
        SQLiteCommand com;

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openDBDialog.ShowDialog() == DialogResult.OK)
            {
                conn = new SQLiteConnection("Data Source=" + openDBDialog.FileName);
                com = conn.CreateCommand();

                conn.Open();
                com.CommandText = "SELECT COUNT(PlayerID) FROM ptr_L_summary";
                SQLiteDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    label_status.Text = dr.GetInt32(0).ToString() + " records are found.";
                }
                com.Dispose();
                conn.Close();

                buttonSave.Enabled = true;
           }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (saveXMLDialog.ShowDialog() == DialogResult.OK)
            {
                conn.Open();
                com.CommandText = "SELECT PlayerID, LastDate, Summary_Rate, Summary_exHands, Summary_exBB100, Summary_exEarn FROM ptr_L_summary"
                    + " WHERE LastDate > '" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
                SQLiteDataReader dr = com.ExecuteReader();

                fs = new FileStream(saveXMLDialog.FileName, FileMode.Create, FileAccess.Write);
                xmlWriter = new XmlTextWriter(fs, new UTF8Encoding(false));
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.IndentChar = '\t';

                xmlWriter.WriteStartDocument();
                // xmlWriter.WriteWhitespace("\n");
                xmlWriter.WriteStartElement("notes");
                xmlWriter.WriteStartAttribute("version");
                xmlWriter.WriteString("1");
                xmlWriter.WriteEndAttribute();
                // xmlWriter.WriteWhitespace("\n");

                DateTime last_date = dateTimePicker.Value;

                while (dr.Read())
                {
                    xmlWriter.WriteStartElement("note");
                    // プレイヤー名書き込み
                    xmlWriter.WriteStartAttribute("player");
                    // xmlWriter.WriteString(dr.GetString(0));
                    xmlWriter.WriteString(HttpUtility.UrlDecode(dr.GetString(0)));
                    xmlWriter.WriteEndAttribute();
                    // ラベル
                    // 0 Normal, 1 bb 1-25, 2 bb 25-40, 3 bb 40-60, 4 bb 60-, 5 Check, 6 -1k hands, 7 Pickup 
                    // 判定条件は exBB/100＞-1 ならNormal のこりのうちexBB/100＞-25 なら　1-25 のこりのうちハンド数<1000なら　-1k hands のこりのうちexBB/100＞-40　なら　20-40 のこりのうちexBB/100＞-60　なら　40-60 のこりのうちexBB/100<=-60　なら　60-
                    string label_num;
                    if (dr.GetDouble(4) > -1) label_num = "0";
                    else if (dr.GetDouble(4) > -25) label_num = "1";
                    else if (dr.GetInt32(3) < 1000) label_num = "6";
                    else if (dr.GetDouble(4) > -40) label_num = "2";
                    else if (dr.GetDouble(4) > -60) label_num = "3";
                    else label_num = "4";

                    xmlWriter.WriteStartAttribute("label");
                    xmlWriter.WriteString(label_num);
                    xmlWriter.WriteEndAttribute();

                    string summary = "R:" + dr.GetInt32(2).ToString() + ", H:" + dr.GetInt32(3).ToString()
                        + ", $:" + dr.GetDouble(5).ToString("f0") + ", BB:" + dr.GetDouble(4).ToString("f2")
                        + ", " + dr.GetDateTime(1).ToString("yyyy/MM/dd");
                    xmlWriter.WriteString(summary);

                    xmlWriter.WriteEndElement();
                    // xmlWriter.WriteWhitespace("\n");

                    if (last_date < dr.GetDateTime(1)) last_date = dr.GetDateTime(1);
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
                fs.Close();

                com.Dispose();
                conn.Close();

                DBtoNote.Properties.Settings.Default.LastCheckDate = last_date;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dateTimePicker.Value = DBtoNote.Properties.Settings.Default.LastCheckDate;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DBtoNote.Properties.Settings.Default.Save();
        }
    }
}