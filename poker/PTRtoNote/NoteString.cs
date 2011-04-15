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
        /// <summary></summary>
        public PTRData data { get; set; }
        
        NoteString(string player_name, string note_str)
        {
        }

        NoteString(string player_name)
        {
        }

    }
}
