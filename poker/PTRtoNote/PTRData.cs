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

        public PTRData(string note_str)
        {
        }

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
