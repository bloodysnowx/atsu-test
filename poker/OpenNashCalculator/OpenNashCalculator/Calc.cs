using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using System.Data.SQLite;

using System.Text.RegularExpressions;

namespace OpenNashCalculator
{
    public partial class OpenNashCalculatorViewController : Form
    {
        InputXMLCreator creator = new InputXMLCreator();
        XmlDocument resultXML;
        System.Diagnostics.Process currentProcess;

        private void Calc()
        {
            if (chipTextBoxes.Count(x => x.Text != string.Empty && System.Convert.ToInt32(x.Text) > 0) <= 1) return;
            textBoxStructure.Text = textBoxStructure.Text.Replace('+', ',').Replace(' ', ',');
            this.buttonCalc.Enabled = false;
            CalcByWeb();
        }

        private bool CanCalcByCLI()
        {
            return System.IO.File.Exists("net.holdemresources.cli.jar") && hh_back_num > 0;
        }

        private string getSQLKey()
        {
            return currentTableData.Structure + currentTableData.BB + currentTableData.SB + currentTableData.Ante + currentTableData.stacks;
        }

        private string getInputXMLname()
        {
            string tourneyID = this.reader.getTourneyID(openHandHistoryDialog.FileName);
            return "input_" + tourneyID + ".xml";
        }

        private string getOutputXMLname()
        {
            string tourneyID = this.reader.getTourneyID(openHandHistoryDialog.FileName);
            return "output_" + tourneyID + ".xml";
        }

        private bool searchCache()
        {
            using (var conn = new SQLiteConnection("Data Source=calc.db"))
            {
                conn.Open();
                using (var selectCommand = conn.CreateCommand())
                {
                    selectCommand.CommandText = "select data from CalculatedData where id = @id";
                    selectCommand.Parameters.Add("id", System.Data.DbType.String);
                    selectCommand.Prepare();
                    selectCommand.Parameters["id"].Value = getSQLKey();

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string resultData = reader.GetString(0);
                            if (resultData.Length < 30)
                            {
                                using (var deleteCommand = conn.CreateCommand())
                                {
                                    deleteCommand.CommandText = "delete from CalculatedData where id = @id";
                                    deleteCommand.Parameters.Add("id", System.Data.DbType.String);
                                    deleteCommand.Prepare();
                                    deleteCommand.Parameters["id"].Value = getSQLKey();
                                    deleteCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                resultXML = new XmlDocument();
                                resultXML.LoadXml(resultData);
                                return true;
                            }
                        }
                    }

                }
                conn.Close();
            }
            return false;
        }

        private void CalcByCLICommandExecute()
        {
            var inputXML = creator.create(currentTableData);
            inputXML.Save(getInputXMLname());

            if (currentProcess != null && currentProcess.HasExited == false)
            {
                currentProcess.EnableRaisingEvents = false;
                currentProcess.Kill();
            }
            currentProcess = new System.Diagnostics.Process();
            currentProcess.StartInfo = new ProcessStartInfo("java.exe", "-Xmx600m -jar net.holdemresources.cli.jar " + getInputXMLname() + " " + getOutputXMLname()) { CreateNoWindow = true, UseShellExecute = false };
            currentProcess.SynchronizingObject = this;
            currentProcess.Exited += new EventHandler(CalcByCLI_Exited);
            currentProcess.EnableRaisingEvents = true;
            currentProcess.Start();
        }

        private void setupCurrentTableData()
        {
            int allHandCount = 1;
            if (currentTableData != null)
            {
                allHandCount = currentTableData.allHandCount;
            }
            currentTableData = new TableData();
            currentTableData.allHandCount = allHandCount;
            currentTableData.Structure = textBoxStructure.Text.Trim();
            currentTableData.BB = System.Convert.ToInt32(textBoxBB.Text.Trim());
            currentTableData.SB = currentTableData.BB / 2;
            currentTableData.Ante = System.Convert.ToInt32(textBoxAnte.Text.Trim());
            currentTableData.stacks = string.Empty;

            int unit = currentTableData.Ante < 10 ? 10 : currentTableData.Ante;
            for (int i = 1; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    if (currentTableData.stacks.Length > 0) currentTableData.stacks += ",";
                    int chip = System.Convert.ToInt32(chipTextBoxes[(bb_pos + i) % 9].Text.Trim());
                    currentTableData.chips[(bb_pos + i) % 9] = chip;
                    if (currentTableData.playerNames[(bb_pos + i) % 9] == null || currentTableData.playerNames[(bb_pos + i) % 9] == string.Empty) currentTableData.playerNames[(bb_pos + i) % 9] = "Player" + (bb_pos + i) % 9;
                    chip = (int)(Math.Round((double)chip / (double)unit) * unit);
                    currentTableData.stacks += chip.ToString();
                }
            }
        }

        private void CalcByCLI()
        {
            foreach (TextBox rangeTextBox in rangeTextBoxes) rangeTextBox.Clear();
            setupCurrentTableData();

            resultXML = null;
        }

        private void readFromXML()
        {
            int heroCount = 0;
            for (int i = 1, j = 0; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    if (SeatLabels[(bb_pos + i) % 9].Text == "H")
                    {
                        heroCount = j;
                        break;
                    }
                    ++j;
                }
            }
            for (int i = 1, j = 0; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    if (j < heroCount)
                    {
                        var spotPct = resultXML.SelectSingleNode(@"//spot[@pu=" + j.ToString() + "][@ca=" + heroCount.ToString() + "]/rangepct");
                        var spotRange = resultXML.SelectSingleNode(@"//spot[@pu=" + j.ToString() + "][@ca=" + heroCount.ToString() + "]/range");
                        if (spotPct != null)
                            rangeTextBoxes[(bb_pos + i) % 9].Text = spotPct.InnerText + " " + spotRange.InnerText;
                        ++j;
                    }
                    else
                    {
                        var spotPct = resultXML.SelectSingleNode(@"//spot[@pu=" + heroCount.ToString() + "]/rangepct");
                        var spotRange = resultXML.SelectSingleNode(@"//spot[@pu=" + heroCount.ToString() + "]/range");
                        if (spotPct != null)
                            rangeTextBoxes[(bb_pos + i) % 9].Text = spotPct.InnerText + " " + spotRange.InnerText;
                        break;
                    }
                }
            }
            this.buttonCalc.Enabled = true;
        }

        private void CalcByCLI_Exited(object sender, EventArgs e)
        {
            var sr = new System.IO.StreamReader(getOutputXMLname());
            string resultData = sr.ReadToEnd();
            sr.Close();
            System.IO.File.Delete(getInputXMLname());
            System.IO.File.Delete(getOutputXMLname());

            using (var conn = new SQLiteConnection("Data Source=calc.db"))
            {
                conn.Open();
                using (var insertCommand = conn.CreateCommand())
                {
                    insertCommand.CommandText = "insert into CalculatedData values (@id, @data)";
                    insertCommand.Parameters.Add("id", System.Data.DbType.String);
                    insertCommand.Parameters.Add("data", System.Data.DbType.String);
                    insertCommand.Prepare();
                    insertCommand.Parameters["id"].Value = getSQLKey();
                    insertCommand.Parameters["data"].Value = resultData;
                    insertCommand.ExecuteNonQuery();
                }
                conn.Close();
            }

            resultXML = new XmlDocument();
            resultXML.LoadXml(resultData);
            readFromXML();
        }

        private void CalcByWeb()
        {
            webBrowserTimer.Enabled = false;

            setupCurrentTableData();

            if (textBoxAnte.Text.Trim() == "")
                textBoxAnte.Text = "0";

            string URL = "http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&bb=";
            URL += textBoxBB.Text;
            URL += "&sb=" + currentSB.Trim();
            URL += "&ante=" + textBoxAnte.Text.Trim();
            URL += "&structure=" + HttpUtility.UrlEncode(textBoxStructure.Text.Trim());
            for (int i = 1, j = 1; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    URL += "&s" + (j++).ToString() + "=" + chipTextBoxes[(bb_pos + i) % 9].Text.Trim();
                }
            }

            foreach (TextBox rangeTextBox in rangeTextBoxes)
                rangeTextBox.Text = "";

            hero_num = getHeroNum();
            hero_pos = "";
            hero_pos = positionRadioButtons[hero_num].Text;

            webBrowser1.Navigate(URL);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowserTimer.Enabled = true;
        }

        private void webBrowserTimer_Tick(object sender, EventArgs e)
        {
            recent_web_page = webBrowser1.Document.Body.OuterHtml;
            if (last_web_page == recent_web_page) return;
            last_web_page = recent_web_page;
            int range_num = recent_web_page.IndexOf("<TH>Range</TH></TR>");
            if (range_num < 1) return;
            recent_web_page = recent_web_page.Substring(range_num);
            // recent_web_page = client.DownloadString(URL);
            Regex regex = new Regex("<TR>\r\n<TD>\r\n<TD>" + Regex.Escape(hero_pos) + "</TD>\r\n<TD>\r\n<TD>(.*?)</TD>");
            MatchCollection matchCol = regex.Matches(recent_web_page);
            int callRangeCount = matchCol.Count - 1;

            for (int i = 1; i < 9 && callRangeCount >= 0; ++i)
            {
                if (positionRadioButtons[(hero_num - i + 9) % 9].Enabled)
                {
                    rangeTextBoxes[(hero_num - i + 9) % 9].Text = matchCol[callRangeCount--].Groups[1].Value;
                }
            }

#if false
            if (hero_pos == Position[0])
            {
                rangeTextBoxes[(hero_num + 1) % 9].Text = matchCol[callRangeCount--].Groups[1].Value;
            }
#endif

            regex = new Regex("<TD>" + Regex.Escape(hero_pos) + "</TD>\r\n<TD>\r\n<TD>\r\n<TD>(.*?)</TD>");
            matchCol = regex.Matches(recent_web_page);
            string pushRange = "";
            if (matchCol.Count > 0) pushRange = matchCol[0].Groups[1].Value;

            rangeTextBoxes[hero_num].Text = pushRange;
            webBrowserTimer.Enabled = false;
            this.buttonCalc.Enabled = true;

             AllinCheckBox_CheckedChanged(null, null);
        }

    }
}