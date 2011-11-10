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
        private const string PTR_SEARCH_URL = "http://www.pokertableratings.com/stars-player-search/";
    
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

        public PTRData GetPTRData(string player_name)
        {
            throw new NotImplementedException();
        }
    }
}
