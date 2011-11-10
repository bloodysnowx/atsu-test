using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public class WebClientEx : System.Net.WebClient
    {
    
        public System.Net.CookieContainer CookieContainer { get; set; }

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            System.Net.WebRequest webRequest = base.GetWebRequest(address);

            if (webRequest is System.Net.HttpWebRequest)
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)webRequest;
                httpWebRequest.CookieContainer = this.CookieContainer;
            }

            return webRequest;
        }
    }
}
