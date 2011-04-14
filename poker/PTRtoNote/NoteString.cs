using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote
{
    /// <summary>
    /// プレイヤー毎のnote文字列に関するクラス
    /// </summary>
    class NoteString
    {
        /// <summary>Player Name</summary>
        public string PlayerName { get; set; }
        /// <summary>note文字列</summary>
        public string str { get; private set; }
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

        
        NoteString(string player_name, string note_str)
        {
        }

        NoteString(string player_name)
        {
        }

    }
}
