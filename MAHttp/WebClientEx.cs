using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;

namespace MAHttp
{
    class WebClientEx : WebClient
    {
        private CookieContainer cc;

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest req = (HttpWebRequest)base.GetWebRequest(address);
            req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            req.CookieContainer = cc;
            return req;
        }

        public WebClientEx()
        {
            cc = new CookieContainer();
        }

        public void addCookie(string Key, string Value)
        {
            cc.Add(new Uri("http://ma.actoz.com:10001/"), new Cookie(Key,Value));

        }

        public static byte[] ImportParmFromList(List<KeyValuePair<string, string>> ParamList)
        {
            string prm = "";
            bool first = true;

            foreach(KeyValuePair<string, string> ParamPair in ParamList)
            {
                if (first)
                {
                    prm = ParamPair.Key + "=" + HttpUtility.UrlEncode(ParamPair.Value);
                    first = false;
                }
                else
                    prm = prm + "&" + ParamPair.Key + "=" + HttpUtility.UrlEncode(ParamPair.Value);
            }
            prm = prm.Replace("%3d", "=");
            prm = prm.Replace("%3D", "=");
            prm = prm.Replace("%2f", "/");
            return UTF8Encoding.UTF8.GetBytes(prm);
        }

    }
}
