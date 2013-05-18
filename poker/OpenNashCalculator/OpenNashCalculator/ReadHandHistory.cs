using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using UserValidatorLib;

using System.Text.RegularExpressions;

namespace OpenNashCalculator
{
    public partial class OpenNashCalculatorViewController : Form
    {
        HandHistoryReader reader = new PSHHReader();
        UserValidator validator = new UserValidator();

        bool isHyper()
        {
            return hyperSatBuyinList.Any(buyin => openHandHistoryDialog.FileName.Contains(buyin));
        }
        void setHyperStructure()
        {
            textBoxStructure.Text = "1,1";
        }

        bool shouldCloseTime(DateTime lastWriteTime)
        {
            
            if (DateTime.Now.Minute <= 5 || DateTime.Now.Minute >= 55)
            {
                if (lastWriteTime.AddMinutes(5 + Properties.Settings.Default.AutoCloseMin) < DateTime.Now) return true;
            }
            else if (lastWriteTime.AddMinutes(Properties.Settings.Default.AutoCloseMin) < DateTime.Now) return true;
            return false;
        }

        bool shouldReloadTime(DateTime lastWriteTime)
        {
            return !(hh_back_num == 0 && updateDate >= lastWriteTime);
        }

        bool isMTT()
        {
            return textBoxStructure.Text.Trim() == "1";
        }

        bool shouldCloseSituation(TableData result)
        {
            return (result.getLivePlayerCount() <= 1 && isMTT() == false)
                || (result.getLivePlayerCount() == 2 && isHyper())
                || (result.getHeroChip() <= 0 && isHyper());
        }

        void ReadHandHistory()
        {
            if (openHandHistoryDialog.FileName == String.Empty) return;
            DateTime lastWriteTime = reader.GetLastWriteTime(openHandHistoryDialog.FileName);

            if(checkBoxClose.Checked)
            {
                if (shouldCloseTime(lastWriteTime)) Application.Exit();
            }

            if (shouldReloadTime(lastWriteTime) == false) return;

            updateDate = hh_back_num == 0 ? lastWriteTime : new DateTime();
            TableData result = reader.read(openHandHistoryDialog.FileName, hh_back_num);
            if(!validator.validate(result.heroName, encryptedUserName)) {
                System.Windows.Forms.MessageBox.Show("You cannot use this application");
                Application.Exit();
            }
            if (checkBoxClose.Checked)
            {
                if (shouldCloseSituation(result)) Application.Exit();
            }

            setUpResult(result);
        }

        void addonAll(int addonChip)
        {
            if (openHandHistoryDialog.FileName == String.Empty) return;
            TableData result = reader.read(openHandHistoryDialog.FileName, hh_back_num);
            result.addonAll(result.StartingChip, addonChip);
            result.BB = 12000;
            result.SB = 6000;
            result.Ante = 600;
            setUpResult(result);
        }

        void setUpResult(TableData result)
        {
            // 設定
            textBoxBB.Text = result.BB.ToString();
            currentSB = result.SB.ToString();
            textBoxAnte.Text = result.Ante.ToString();
            SetHeroSeat(SeatLabels[result.getHeroSeat() - 1]);
            if (isHyper()) setHyperStructure();
            textBoxStructure.Text = textBoxStructure.Text.Trim().Replace('+', ',').Replace(' ', ',');
            string[] structureStrs = textBoxStructure.Text.Split(',');
            double[] structures = new double[structureStrs.Length];
            for (int i = 0; i < structureStrs.Length; ++i)
                structures[i] = System.Convert.ToDouble(structureStrs[i]);

            // チップと名前入力
            foreach (TextBox chipTextBox in chipTextBoxes)
                chipTextBox.Text = "";
            for (int i = 0; i < result.MaxSeatNum; ++i)
            {
                if (result.chips[i] <= 0 && result.playerNames[i] != string.Empty && result.seats[i] > 0 && checkBoxRebuy.Checked)
                    result.chips[i] = result.StartingChip;

                if (result.chips[i] > 0)
                {
                    chipTextBoxes[result.seats[i] - 1].Text = result.chips[i].ToString();
                    PlayerNameLabels[result.seats[i] - 1].Text = result.playerNames[i];
                }
            }
            result.calcICMs(structures);
            for (int i = 0, count = 0; i < result.MaxSeatNum; ++i)
            {
                if (result.chips[i] > 0)
                {
                    ICMLabels[i].Text = result.ICMs[count++].ToString();
                }
                else ICMLabels[i].Text = string.Empty;
            }

            // ボタンの位置を決定
            int player_num = result.chips.Count(chip => chip > 0);

            int buttonIndex = Enumerable.Range(0, result.MaxSeatNum).First(i => result.buttonPos == result.seats[i]);

            if (player_num < 3)
            {
                for (int j = 1, count = 0; j < result.MaxSeatNum + 1; ++j)
                {
                    if (result.chips[(buttonIndex + j) % result.MaxSeatNum] > 0)
                    {
                        if (++count == 1)
                        {
                            positionRadioButtons[result.seats[(buttonIndex + j) % result.MaxSeatNum] - 1].Checked = true;
                            break;
                        }
                    }
                }
                // positionRadioButtons[result.buttonPos - 1].Checked = true;
            }
            else
            {
                for (int j = 1, count = 0; j < result.MaxSeatNum + 1; ++j)
                {
                    if (result.chips[(buttonIndex + j) % result.MaxSeatNum] > 0)
                    {
                        if (++count == 2)
                        {
                            positionRadioButtons[result.seats[(buttonIndex + j) % result.MaxSeatNum] - 1].Checked = true;
                            break;
                        }
                    }
                }
            }

            if (Properties.Settings.Default.SetBBLast)
            {
                while (positionRadioButtons[8].Text != Position[0])
                {
                    SeatRotateF();
                    result.seats[result.getHeroIndex()]++;
                }
                if(result.seats[result.getHeroIndex()] > result.MaxSeatNum) result.seats[result.getHeroIndex()] = result.seats[result.getHeroIndex()] % result.MaxSeatNum;
                SetHeroSeat(SeatLabels[result.getHeroSeat() - 1]);
            }
            else if (0 < Properties.Settings.Default.PreferredSeat && Properties.Settings.Default.PreferredSeat < result.MaxSeatNum + 1)
            {
                for (int i = 0; i < (Properties.Settings.Default.PreferredSeat -  result.seats[result.getHeroIndex()] + result.MaxSeatNum) % result.MaxSeatNum; ++i)
                    SeatRotateF();

                SetHeroSeat(SeatLabels[Properties.Settings.Default.PreferredSeat - 1]);
            }

            SetPosition();

            foreach (CheckBox checkBox in AllinCheckBoxes)
                checkBox.Checked = false;

            if (result.chips[result.getHeroIndex()] > 0 && player_num > 1 && checkBoxCalc.Checked)
            {
                Calc();
            }
            else
            {
                recent_web_page = String.Empty;
                foreach (TextBox x in rangeTextBoxes)
                    x.Clear();
            }
        }

        void SeatRotateF()
        {
            string tmp_seat = SeatLabels[8].Text;
            string tmp_chip = chipTextBoxes[8].Text;
            bool tmp_checked = positionRadioButtons[8].Checked;
            string tmp_name = PlayerNameLabels[8].Text;
            string tmp_icm = ICMLabels[8].Text;

            for (int i = 7; i >= 0; --i)
            {
                SeatLabels[i + 1].Text = SeatLabels[i].Text;
                chipTextBoxes[i + 1].Text = chipTextBoxes[i].Text;
                positionRadioButtons[i + 1].Checked = positionRadioButtons[i].Checked;
                PlayerNameLabels[i + 1].Text = PlayerNameLabels[i].Text;
                ICMLabels[i + 1].Text = ICMLabels[i].Text;
            }

            SeatLabels[0].Text = tmp_seat;
            chipTextBoxes[0].Text = tmp_chip;
            positionRadioButtons[0].Checked = tmp_checked;
            PlayerNameLabels[0].Text = tmp_name;
            ICMLabels[0].Text = tmp_icm;

            for (int i = 0; i < chipTextBoxes.Length; ++i)
            {
                if (chipTextBoxes[i].Text.Trim() == String.Empty) PlayerNameLabels[i].Text = String.Empty;
            }
        }

        void SeatRotateR()
        {
        }
    }
}