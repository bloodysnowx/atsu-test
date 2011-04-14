using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote
{
    /// <summary>
    /// PTRとの接続に関するクラス
    /// </summary>
    class PTRconnection
    {
        /// <summary>残り検索回数</summary>
        public uint SearchesRemaining { get; private set; }
        /// <summary>ログインユーザ名</summary>
        public string Username { get; set; }
        /// <summary>ログインパスワード</summary>
        public string Password { get; set; }

        bool PTRConnect();
        bool PTRDisconnect();

    }
}
