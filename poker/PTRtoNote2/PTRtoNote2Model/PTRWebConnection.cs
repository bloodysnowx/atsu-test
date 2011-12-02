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
            isConnected = false;
            searchesRemaining = 10;
            PTRClient = new WebClientEx();
            PTRClient.CookieContainer = new System.Net.CookieContainer();
        }

        /// <summary>
        /// ログインユーザ名
        /// </summary>
        public string[] Users
        {
            get;
            set;
        }

        /// <summary>
        /// ログインパスワード
        /// </summary>
        public string[] Passwords
        {
            get;
            set;
        }

        /// <summary>
        /// PTRとの接続
        /// </summary>
        private WebClientEx PTRClient;

        /// <summary>
        /// PTRと接続されているか
        /// </summary>
        public bool isConnected
        {
            get;
            set;
        }

        public int AccountNumber
        {
            get;
            set;
        }

        public Player GetPTRData(string playerName)
        {
            throw new NotImplementedException();
        }

        private bool Connect()
        {
            // コレクションを用意し、ログインユーザ名とパスワードをセットする
            System.Collections.Specialized.NameValueCollection vals = new System.Collections.Specialized.NameValueCollection();
            vals.Add("username", Users[AccountNumber]);
            vals.Add("password", Passwords[AccountNumber]);
            // ログイン処理を実行する
            byte[] resData = PTRClient.UploadValues(PTR_LOGIN_URL, vals);
            string str = System.Text.Encoding.UTF8.GetString(resData);
            isConnected = str.Contains("\"success\" : true");

            return isConnected;
        }

        /// <summary>
        /// PTRでPlayerを検索する
        /// </summary>
        /// <param name="player_name">Player名</param>
        /// <returns>検索結果のwebページ</returns>
        private string GetPTRWebPage(string player_name)
        {
            return PTRClient.DownloadString(PTR_SEARCH_URL + System.Web.HttpUtility.UrlEncode(player_name));
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

        private bool isHoldemStakes(Stake stake)
        {
            return stake.Game.Name == "H";
        }

        private bool isExStakes(Stake stake)
        {
            return isNLStakes(stake) & stake.Rate > 10;
        }

        private bool isHUStakes(Stake stake)
        {
            return stake.PlayerNum == 2 & isHoldemStakes(stake);
        }

        private bool isOmahaStakes(Stake stake)
        {
            return stake.BetType.Name == "PL" & stake.Game.Name == "O";
        }

        private bool isFLStakes(Stake stake)
        {
            return stake.BetType.Name == "FL" & isHoldemStakes(stake);
        }

        private bool isNLStakes(Stake stake)
        {
            return stake.BetType.Name == "NL" & isHoldemStakes(stake);
        }

        public int SearchedCount
        {
            get;
            set;
        }
    }
}
