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
            URL += "&s1=" + chipTextBoxes[(bb_pos + 1) % 9].Text.Trim();
            URL += "&s2=" + chipTextBoxes[(bb_pos + 2) % 9].Text.Trim();
            URL += "&s3=" + chipTextBoxes[(bb_pos + 3) % 9].Text.Trim();
            URL += "&s4=" + chipTextBoxes[(bb_pos + 4) % 9].Text.Trim();
            URL += "&s5=" + chipTextBoxes[(bb_pos + 5) % 9].Text.Trim();
            URL += "&s6=" + chipTextBoxes[(bb_pos + 6) % 9].Text.Trim();
            URL += "&s7=" + chipTextBoxes[(bb_pos + 7) % 9].Text.Trim();
            URL += "&s8=" + chipTextBoxes[(bb_pos + 8) % 9].Text.Trim();
            URL += "&s9=" + chipTextBoxes[(bb_pos + 9) % 9].Text.Trim();
            // http://www.holdemresources.net/hr/sngs/icmcalculator.html?action=calculate&
            // bb=200&sb=100&ante=0&structure=0.5%2C0.3%2C0.2&s1=100&s2=100&s3=100&s4=100&s5=100&s6=100&s7=100&s8=100&s9=100

            foreach (TextBox rangeTextBox in rangeTextBoxes)
                rangeTextBox.Text = "";

            System.Net.WebClient client = new System.Net.WebClient();

            int hero_num = getHeroNum();
            string hero_pos = "";
            hero_pos = positionRadioButtons[hero_num].Text;

            recent_web_page = client.DownloadString(URL);
            Regex regex = new Regex("<td>" + Regex.Escape(hero_pos) + "</td><td /><td>(.*?)</td>");
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

            regex = new Regex("<td>" + Regex.Escape(hero_pos) + "</td><td /><td /><td>(.*?)</td>");
            matchCol = regex.Matches(recent_web_page);
            string pushRange = "";
            if (matchCol.Count > 0) pushRange = matchCol[0].Groups[1].Value;

            rangeTextBoxes[hero_num].Text = pushRange;

            if (checkBoxWeb.Checked == false)
                System.Diagnostics.Process.Start(URL);
        }

    }
}