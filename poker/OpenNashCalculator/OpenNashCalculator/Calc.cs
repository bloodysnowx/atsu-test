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

        private void clearCalc()
        {
            resultXML = null;
            recent_web_page = null;
            CalcByHRC.clearCalc();
            foreach (TextBox rangeTextBox in rangeTextBoxes) rangeTextBox.Clear();
        }

        private void Calc()
        {
            clearCalc();
            if (chipTextBoxes.Count(x => x.Text != string.Empty && System.Convert.ToInt32(x.Text) > 0) <= 1) return;
            textBoxStructure.Text = textBoxStructure.Text.Replace('+', ',').Replace(' ', ',');
            this.buttonCalc.Enabled = false;
            if (CanCalcByCLI()) CalcByCLI();
            else CalcByWeb();
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
            return false;

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

        List<bool> isForceAllInList;

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

            isForceAllInList = new List<bool>();

            int unit = currentTableData.Ante < 10 ? 10 : currentTableData.Ante;
            for (int i = 1; i < 10; ++i)
            {
                if (chipTextBoxes[(bb_pos + i) % 9].Text.Trim() != string.Empty)
                {
                    currentTableData.seats[(bb_pos + i) % 9] = i;
                    if (currentTableData.stacks.Length > 0) currentTableData.stacks += ",";
                    int chip = System.Convert.ToInt32(chipTextBoxes[(bb_pos + i) % 9].Text.Trim());
                    currentTableData.chips[(bb_pos + i) % 9] = chip;
                    bool isForceAllIn = false;
                    isForceAllIn = chip <= currentTableData.Ante;
                    if (this.positionRadioButtons[(bb_pos + i) % 9].Text == "SB") isForceAllIn = chip <= currentTableData.Ante + currentTableData.SB;
                    isForceAllInList.Add(isForceAllIn);
                    if (currentTableData.playerNames[(bb_pos + i) % 9] == null || currentTableData.playerNames[(bb_pos + i) % 9] == string.Empty) currentTableData.playerNames[(bb_pos + i) % 9] = "Player" + (bb_pos + i) % 9;
                    chip = (int)(Math.Round((double)chip / (double)unit) * unit);
                    currentTableData.stacks += chip.ToString();
                    if (SeatLabels[(bb_pos + i) % 9].Text == "H")
                        currentTableData.heroName = currentTableData.playerNames[(bb_pos + i) % 9];
                    if (this.positionRadioButtons[(bb_pos + i) % 9].Text == "BU")
                        currentTableData.buttonPos = i;
                }
            }
        }

        private void CalcByCLI()
        {

            setupCurrentTableData();

            resultXML = null;
            if (searchCache()) readFromXML();
            else CalcByCLICommandExecute();
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
            if (hh_back_num > 0 && checkBoxAutoBack.Checked)
            {
                if (currentTableData.allHandCount > hh_back_num)
                    this.button_back_Click(null, null);
                else Application.Exit();
            }
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
            if (searchCache()) { readFromXML(); return; }

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
            // http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&
            // bb=200&sb=100&ante=0&structure=0.5%2C0.3%2C0.2&s1=100&s2=100&s3=100&s4=100&s5=100&s6=100&s7=100&s8=100&s9=100

            // System.Net.WebClient client = new System.Net.WebClient();
            // System.Windows.Forms.WebBrowser client = new WebBrowser();

            hero_num = getHeroNum();
            hero_pos = "";
            hero_pos = positionRadioButtons[hero_num].Text;

            webKitBrowser.Navigate(URL);

            if (checkBoxWeb.Checked == false)
                System.Diagnostics.Process.Start(URL);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowserTimer.Enabled = true;
        }

        private static string getPushRange(string webPage, string position)
        {
            Regex regex = new Regex(Regex.Escape(position) + "([0-9]+" + Regex.Escape(".") + "[0-9]+%, .*?)\n");
            MatchCollection matchCol = regex.Matches(webPage);
            String pushRange = webPage.Substring(webPage.LastIndexOf(matchCol[matchCol.Count - 1].Value));
            return position == "BB" ? "" : pushRange;
        }

        private static string getCallRange(string pushRanges, string position)
        {
            Regex regex = new Regex(Regex.Escape(position) + "([0-9]+" + Regex.Escape(".") + "[0-9]+%, .*?)\n");
            MatchCollection matchCol = regex.Matches(pushRanges);
            String callRange = "";
            if (matchCol.Count == 2 || matchCol.Count == 1)
            {
                callRange = matchCol[0].Groups[1].Value;
            }
            else
            {
                regex = new Regex("BB[0-9]+" + Regex.Escape(".") + "[0-9]+%, .*?\n" +
                    Regex.Escape(position) + "([0-9]+" + Regex.Escape(".") + "[0-9]+%, .*?)\n");
                matchCol = regex.Matches(pushRanges);
                if (matchCol.Count > 0)
                    callRange = matchCol[0].Groups[1].Value;
            }
            return callRange;
        }

        private void webBrowserTimer_Tick(object sender, EventArgs e)
        {
            if (webKitBrowser.Document.ChildNodes.Count() < 2) return;
            recent_web_page = webKitBrowser.Document.ChildNodes[1].TextContent;
            if (last_web_page == recent_web_page) return;
            last_web_page = recent_web_page;
            int range_num = recent_web_page.IndexOf("PUCAOCRange");
            if (range_num < 1) return;
            recent_web_page = recent_web_page.Substring(range_num + 11);

            for (int i = 1; i < 9; ++i)
            {
                RadioButton currentRadioButton = positionRadioButtons[(hero_num - i + 9) % 9];
                if (currentRadioButton.Enabled)
                {
                    String opponentPushRange = recent_web_page.Substring(recent_web_page.LastIndexOf(getPushRange(recent_web_page, currentRadioButton.Text)));
                    rangeTextBoxes[(hero_num - i + 9) % 9].Text = getCallRange(opponentPushRange, hero_pos);
                }
            }

#if false
            if (hero_pos == Position[0])
            {
                rangeTextBoxes[(hero_num + 1) % 9].Text = matchCol[callRangeCount--].Groups[1].Value;
            }
#endif

            string pushRange = getPushRange(recent_web_page, hero_pos);
            rangeTextBoxes[hero_num].Text = pushRange;
            webBrowserTimer.Enabled = false;
            this.buttonCalc.Enabled = true;

            AllinCheckBox_CheckedChanged(null, null);
        }

    }
}