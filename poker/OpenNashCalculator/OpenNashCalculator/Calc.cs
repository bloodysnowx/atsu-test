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

using System.Text.RegularExpressions;

namespace OpenNashCalculator
{
    public partial class Form1 : Form
    {
        private void Calc()
        {
            if (textBoxAnte.Text.Trim() == "")
                textBoxAnte.Text = "0";

            string URL = "http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&bb=";
            URL += textBoxBB.Text;
            URL += "&sb=" + currentSB.Trim();
            URL += "&ante=" + textBoxAnte.Text.Trim();
            textBoxStructure.Text = textBoxStructure.Text.Replace('+', ',').Replace(' ', ',');
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

            foreach (TextBox rangeTextBox in rangeTextBoxes)
                rangeTextBox.Text = "";

            // System.Net.WebClient client = new System.Net.WebClient();
            // System.Windows.Forms.WebBrowser client = new WebBrowser();

            hero_num = getHeroNum();
            hero_pos = "";
            hero_pos = positionRadioButtons[hero_num].Text;

            webBrowser1.Navigate(URL);

            if (checkBoxWeb.Checked == false)
                System.Diagnostics.Process.Start(URL);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowserTimer.Enabled = true;
        }


        private void webBrowserTimer_Tick(object sender, EventArgs e)
        {
            recent_web_page = webBrowser1.Document.Body.OuterHtml;
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
        }

    }
}