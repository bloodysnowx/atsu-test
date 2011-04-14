using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote
{
    /// <summary>
    /// クッキーを扱えるようにしたWebClient
    /// </summary>
    class WebClientEx : System.Net.WebClient
    {
        private System.Net.CookieContainer cookieContainer;

        public System.Net.CookieContainer CookieContainer
        {
            get
            {
                return cookieContainer;
            }
            set
            {
                cookieContainer = value;
            }
        }

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            System.Net.WebRequest webRequest = base.GetWebRequest(address);

            if (webRequest is System.Net.HttpWebRequest)
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)webRequest;
                httpWebRequest.CookieContainer = this.cookieContainer;
            }

            return webRequest;
        }
    }

    /// <summary>
    /// PTRとの接続に関するクラス
    /// </summary>
    class PTRconnection
    {
        /// <summary>PTRのログインURL</summary>
        private const string PTR_LOGIN_URL = "http://www.pokertableratings.com/login_action.php";
        /// <summary>PTRのログインURL</summary>
        private const string PTR_SEARCH_URL = "http://www.pokertableratings.com/stars-player-search/";
        /// <summary>残り検索回数</summary>
        public uint SearchesRemaining { get; private set; }
        /// <summary>ログインユーザ名</summary>
        public string Username { get; set; }
        /// <summary>ログインパスワード</summary>
        public string Password { get; set; }
        /// <summary>PTRとの接続</summary>
        private WebClientEx PTRClient;

        /// <summary>
        /// PTRに接続する
        /// </summary>
        /// <returns></returns>
        public bool PTRConnect()
        {
            // クライアントを初期化する
            PTRClient = new WebClientEx();
            // クッキーを設定する
            PTRClient.CookieContainer = new System.Net.CookieContainer();
            // コレクションを用意し、ログインユーザ名とパスワードをセットする
            System.Collections.Specialized.NameValueCollection vals = new System.Collections.Specialized.NameValueCollection();
            vals.Add("username", Username);
            vals.Add("password", Password);
            // ログイン処理を実行する
            byte[] resData = PTRClient.UploadValues(PTR_LOGIN_URL, vals);
            string str = System.Text.Encoding.UTF8.GetString(resData);
            return str.Contains("\"success\" : true");
        }

        public string GetPTR(string player_name)
        {
            return PTRClient.DownloadString(PTR_SEARCH_URL + player_name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool PTRDisconnect()
        {
            return true;
        }
    }
}
