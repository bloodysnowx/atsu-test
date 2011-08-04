using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

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
        /// <summary>PTRの検索URL</summary>
        private const string PTR_SEARCH_URL = "http://www.pokertableratings.com/stars-player-search/";
        /// <summary>残り検索回数</summary>
        public uint SearchesRemaining { get; set; }
        /// <summary>ログインユーザ名</summary>
        public string Username { get; set; }
        /// <summary>ログインパスワード</summary>
        public string Password { get; set; }
        /// <summary>PTRとの接続</summary>
        private WebClientEx PTRClient;
        /// <summary>PTRと接続されているか</summary>
        public bool isConnected { get; private set; }

        /// <summary>log4netのインスタンス</summary>
        private static readonly log4net.ILog logger
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PTRconnection()
        {
            isConnected = false;
            SearchesRemaining = 10;
        }

        /// <summary>
        /// PTRに接続する
        /// </summary>
        /// <returns>ログイン処理に成功したかどうか</returns>
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
            isConnected = str.Contains("\"success\" : true");

            if (isConnected)
            {
                if(logger.IsInfoEnabled)
                    logger.Info("login = " + Username + " : success : " + str);
            }
            else
            {
                if (logger.IsErrorEnabled)
                    logger.Error("login = " + Username + " : failed : " + str);
            }

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

        private bool isExStakes(string stakes_name)
        {
            bool ret = true;

            if (stakes_name.IndexOf("NLH") < 0) ret = false;
            else if (stakes_name.IndexOf("HU") > -1) ret = false;
            else if (stakes_name.IndexOf("0.02") > -1) ret = false;
            else if (stakes_name.IndexOf("0.05") > -1) ret = false;

            return ret;
        }

        private bool isHUStakes(string stakes_name)
        {
            bool ret = true;
            if (stakes_name.IndexOf("NLH HU") < 0) ret = false;

            return ret;
        }

        private bool isOmahaStakes(string stakes_name)
        {
            bool ret = true;
            if (stakes_name.IndexOf("PLO") < 0) ret = false;
            else if (stakes_name.IndexOf("HU") > -1) ret = false;
            else if (stakes_name.IndexOf("0.02") > -1) ret = false;
            else if (stakes_name.IndexOf("0.05") > -1) ret = false;

            return ret;
        }

        /// <summary>
        /// PTRのページからデータを取得する
        /// </summary>
        /// <param name="web_page"></param>
        /// <returns></returns>
        private PTRData GetPTRDataFromWebPage(string web_page)
        {
            PTRData data = new PTRData();
            data.Rating = getRate(web_page);
            data.BB_100 = getBB(web_page);
            if (web_page.IndexOf("<table id=\"sortable-data-table\" cellpadding=\"0\" cellspacing=\"0\">") < 0) throw new System.Net.WebException();
            web_page = web_page.Substring(web_page.IndexOf("<table id=\"sortable-data-table\" cellpadding=\"0\" cellspacing=\"0\">"));
            // web_page = web_page.Substring(web_page.IndexOf("<td class=\"overview-stakes\">"));

            Regex regex = new Regex(">([^<]+?)<");
            MatchCollection matchCol;
            decimal bb_sum = 0;
            decimal hu_bb_sum = 0;
            decimal o_bb_sum = 0;

            while (true)
            {
                if (web_page.IndexOf("<tr class=", 1) < 0) break;
                // 列まで読み進める
                web_page = web_page.Substring(web_page.IndexOf("<tr class=", 1));

                matchCol = regex.Matches(web_page);

                if (isExStakes(matchCol[1].Groups[1].Value))
                {
                    uint tmp_hands = uint.Parse(matchCol[3].Groups[1].Value.Replace(",", ""));
                    data.Hands += tmp_hands;
                    data.Earnings += int.Parse(matchCol[5].Groups[1].Value.Replace(",", "").Replace("$", "").Replace("&#8364;", ""));
                    bb_sum += decimal.Parse(matchCol[7].Groups[1].Value.Replace(",", "")) * tmp_hands;
                }
                else if (isHUStakes(matchCol[1].Groups[1].Value))
                {
                    uint tmp_hands = uint.Parse(matchCol[3].Groups[1].Value.Replace(",", ""));
                    data.HU_Hands += tmp_hands;
                    hu_bb_sum += decimal.Parse(matchCol[7].Groups[1].Value.Replace(",", "")) * tmp_hands;
                }
                else if (isOmahaStakes(matchCol[1].Groups[1].Value))
                {
                    uint tmp_hands = uint.Parse(matchCol[3].Groups[1].Value.Replace(",", ""));
                    data.O_Hands += tmp_hands;
                    o_bb_sum += decimal.Parse(matchCol[7].Groups[1].Value.Replace(",", "")) * tmp_hands;
                }
            }
            if (data.Hands != 0) data.BB_100 = bb_sum / data.Hands;
            if (data.HU_Hands != 0) data.HU_BB_100 = hu_bb_sum / data.HU_Hands;
            if (data.O_Hands != 0) data.O_BB_100 = o_bb_sum / data.O_Hands;

            data.GetDate = System.DateTime.Today;

            web_page = web_page.Substring(web_page.IndexOf("<div id=\"searches\" class=\"right_module\">"));
            regex = new Regex("<h1>([0-9|,]+)</h1>");
            matchCol = regex.Matches(web_page);
            if (matchCol.Count > 0)
                SearchesRemaining = uint.Parse(matchCol[0].Groups[1].Value.Replace(",", ""));

            return data;
        }

        /// <summary>
        /// PTRのページから正規表現を用いてRatingsを抜き出す
        /// </summary>
        /// <param name="web_page">PTRのページ</param>
        /// <returns>Ratings</returns>
        private uint getRate(string web_page)
        {
            uint ratings = 0;
            Regex regex = new Regex("<div id=\"number_reading\">(.*?)</div>");
            MatchCollection matchCol = regex.Matches(web_page);
            if (matchCol.Count > 0)
                ratings = uint.Parse(matchCol[0].Groups[1].Value);

            return ratings;
        }

        /// <summary>
        /// PTRのページから正規表現を用いてBB/100を抜き出す
        /// </summary>
        /// <param name="web_page">PTRのページ</param>
        /// <returns>BB/100</returns>
        private decimal getBB(string web_page)
        {
            decimal bb_100 = 0;
            Regex regex = new Regex("<div class=\"stat_icon\"></div><span>(-?[0-9|,]+" + Regex.Escape(".") + "[0-9]+)</span>");
            MatchCollection matchCol = regex.Matches(web_page);
            if (matchCol.Count > 0)
                bb_100 = decimal.Parse(matchCol[0].Groups[1].Value);

            return bb_100;
        }

        /// <summary>
        /// 検索に失敗した場合はnullを返す
        /// </summary>
        /// <param name="player_name"></param>
        /// <returns></returns>
        public PTRData GetPTRData(string player_name)
        {
            string web_page = GetPTRWebPage(player_name);
            if (web_page.IndexOf("Unlock Full Access to Premium Content") > -1) return null;
            else if (web_page.IndexOf("We didn't find this player, here are some similar names.") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("You&#8217;ve mis-spelled") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("doesn&#8217;t play at PokerStars.") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("doesn&#8217;t exist.") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("has never played at any tables.") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("doesn&#8217;t like you!") > -1) throw new System.Net.WebException();
            else if (web_page.IndexOf("You&#8217;re trying to hack our system, but it didn&#8217;t work.") > -1) return null;
            else if (web_page.IndexOf("You&#8217;re having some bad luck, we hope you have better luck.") > -1) return null;
            else if (web_page.IndexOf("You&#8217;re having a bad day, we apologize.") > -1) return null;

            PTRData data = GetPTRDataFromWebPage(web_page);
            data.PlayerName = player_name;
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PTRDisconnect()
        {
            isConnected = false;
            return true;
        }
    }
}
