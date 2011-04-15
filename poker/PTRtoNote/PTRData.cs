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

        /// <summary>
        /// note文字列からPTRのデータを取り出す
        /// </summary>
        /// <param name="note_str">note文字列</param>
        public PTRData(string note_str)
        {
            string[] tmp_str = note_str.Split(',');
            Rating = uint.Parse(tmp_str[0].Split(':')[1]);
            Hands = uint.Parse(tmp_str[1].Split(':')[1]);
            Earnings = int.Parse(tmp_str[2].Split(':')[1]);
            BB_100 = decimal.Parse(tmp_str[3].Split(':')[1]);
            GetDate = DateTime.ParseExact(tmp_str[4], "yyyy/MM/dd", null);
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

            return note_str.ToString();
        }
    }
}
