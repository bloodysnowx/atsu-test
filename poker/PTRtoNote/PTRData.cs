using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote
{
    /// <summary>
    /// PTRのデータに関するクラス
    /// </summary>
    class PTRData
    {
        /// <summary>Player Name</summary>
        public string PlayerName { get; set; }        
        /// <summary>PTR Rating</summary>
        public uint Rating { get; set; }
        /// <summary>Hands</summary>
        public uint Hands { get; set; }
        /// <summary>Earnings</summary>
        public int Earnings { get; set; }
        /// <summary>BB/100</summary>
        public decimal BB_100 { get; set; }
        /// <summary>PTRから取得した日付</summary>
        public DateTime GetDate { get; set; }
        /// <summary>HU時のBB/100</summary>
        public decimal HU_BB_100 { get; set; }
        /// <summary>HU時のHands</summary>
        public uint HU_Hands { get; set; }
        /// <summary>オマハ時のBB/100</summary>
        public decimal O_BB_100 { get; set; }
        /// <summary>オマハ時のHands</summary>
        public uint O_Hands { get; set; }

        /// <summary>log4netのインスタンス</summary>
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        public PTRData()
        {
            Hands = 0;
            Earnings = 0;
            HU_Hands = 0;
            O_Hands = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player_name">プレイヤーID</param>
        public PTRData(string player_name) : this()
        {
            PlayerName = player_name;
        }

        /// <summary>
        /// note文字列からPTRのデータを取り出す
        /// </summary>
        /// <param name="player_name">プレイヤーID</param>
        /// <param name="note_str">note文字列</param>
        public bool MakePTRDataFromNoteStr(string player_name, string note_str)
        {
            try
            {
                string[] tmp_str_1 = note_str.Split(',');
                if (tmp_str_1.Count() < 5)
                {
                    if (logger.IsWarnEnabled) logger.Warn(player_name + "'s note_str.Count() < 5");
                    return false;
                }

                string[] tmp_str_2 = tmp_str_1[0].Split(':');
                if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "R")
                {
                    if (logger.IsWarnEnabled) logger.Warn(player_name + "'s Rating is Broken.");
                    return false;
                }
                Rating = uint.Parse(tmp_str_2[1]);

                tmp_str_2 = tmp_str_1[1].Split(':');
                if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "H")
                {
                    if (logger.IsWarnEnabled) logger.Warn(player_name + "'s Hands is Broken.");
                    return false;
                }
                Hands = uint.Parse(tmp_str_2[1]);

                tmp_str_2 = tmp_str_1[2].Split(':');
                if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "$")
                {
                    if (logger.IsWarnEnabled) logger.Warn(player_name + "'s Earnings is Broken.");
                    return false;
                }
                Earnings = int.Parse(tmp_str_2[1]);

                tmp_str_2 = tmp_str_1[3].Split(':');
                if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "BB")
                {
                    if (logger.IsWarnEnabled) logger.Warn(player_name + "'s BB/100 is Broken.");
                    return false;
                }
                BB_100 = decimal.Parse(tmp_str_2[1]);

                GetDate = DateTime.ParseExact(tmp_str_1[4].Trim().Substring(0, 10), "yyyy/MM/dd", null);

                HU_BB_100 = 0;
                HU_Hands = 0;
                if (tmp_str_1.Count() > 6)
                {
                    tmp_str_2 = tmp_str_1[5].Split(':');
                    if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "HUBB")
                    {
                        if (logger.IsWarnEnabled) logger.Warn(player_name + "'s HUBB is Broken or old type.");
                    }
                    else
                        HU_BB_100 = decimal.Parse(tmp_str_2[1]);

                    tmp_str_2 = tmp_str_1[6].Split(':');
                    if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "HUH")
                    {
                        if (logger.IsWarnEnabled) logger.Warn(player_name + "'s HUH is Broken or old type.");
                    }
                    else
                        HU_Hands = uint.Parse(tmp_str_2[1]);
                }

                O_BB_100 = 0;
                O_Hands = 0;
                if (tmp_str_1.Count() > 8)
                {
                    tmp_str_2 = tmp_str_1[7].Split(':');
                    if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "OBB")
                    {
                        if (logger.IsWarnEnabled) logger.Warn(player_name + "'s OBB is Broken or old type.");
                    }
                    else
                        O_BB_100 = decimal.Parse(tmp_str_2[1]);

                    tmp_str_2 = tmp_str_1[8].Split(':');
                    if (tmp_str_2.Count() != 2 || tmp_str_2[0].Trim() != "OH")
                    {
                        if (logger.IsWarnEnabled) logger.Warn(player_name + "'s OH is Broken or old type.");
                    }
                    else
                        O_Hands = uint.Parse(tmp_str_2[1]);
                }
            }
            catch (Exception e)
            {
                if (logger.IsErrorEnabled)
                    logger.Error(player_name + "'s PTR could not be understood.", e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// PTRのデータからnote文字列を生成する
        /// </summary>
        /// <returns>note文字列</returns>
        public string GetNoteString()
        {
            System.Text.StringBuilder note_str = new StringBuilder();
            note_str.Append("R:");
            note_str.Append(Rating);
            note_str.Append(", H:");
            note_str.Append(Hands);
            note_str.Append(", $:");
            note_str.Append(Earnings.ToString("f0"));
            note_str.Append(", BB:");
            note_str.Append(BB_100.ToString("f2"));
            note_str.Append(", ");
            note_str.Append(GetDate.ToString("yyyy/MM/dd"));
            note_str.Append(", HUBB:");
            note_str.Append(HU_BB_100.ToString("f2"));
            note_str.Append(", HUH:");
            note_str.Append(HU_Hands);
            note_str.Append(", OBB:");
            note_str.Append(O_BB_100.ToString("f2"));
            note_str.Append(", OH:");
            note_str.Append(O_Hands);

            return note_str.ToString();
        }
    }
}
