using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public class PTRWebConnection : IConnection
    {
        /// <summary>PTRのログインURL</summary>
        private const string PTR_LOGIN_URL = "http://www.pokertableratings.com/login_action.php";
        /// <summary>PTRの検索URL</summary>
        private const string PTR_SEARCH_URL = "http://www.pokertableratings.com/stars-ptrDB-search/";
        /// <summary>
        /// 残り検索回数
        /// </summary>
        private int searchesRemaining;

        public PTRWebConnection()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// ログインユーザ名
        /// </summary>
        public string[] Users
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// ログインパスワード
        /// </summary>
        public string[] Passwords
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// PTRとの接続
        /// </summary>
        public WebClientEx PTRClient
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// PTRと接続されているか
        /// </summary>
        public bool isConnected
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int AccountNumber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Player GetPTRData(string playerName)
        {
            throw new NotImplementedException();
        }

        private void Connect()
        {
            throw new System.NotImplementedException();
        }

        public Stake GetStakes(string stakesName)
        {
            throw new System.NotImplementedException();
        }

        public Player GetPTRDataFromWebPage(string webPage)
        {
            throw new System.NotImplementedException();
        }

        public int GetRating(string webPage)
        {
            throw new System.NotImplementedException();
        }

        public decimal GetBB_100(string webPage)
        {
            throw new System.NotImplementedException();
        }

        public int SearchedCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
