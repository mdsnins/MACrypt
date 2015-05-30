using System;
using System.Collections.Generic;
using System.Text;


namespace MAHttp
{
    public class MAHttp
    {
        private string UAgent;
        private WebClientEx HTTP;
        private MACrypto.AESCrypt AES;
        private string _PUB;
        private string _SEED;

        public MAHttp(string UserAgent, string Base64PublicKey)
        {
            UAgent = UserAgent;
            HTTP = new WebClientEx();

            AES = new MACrypto.AESCrypt();

            _PUB = Base64PublicKey;
        }

        public void setUserAgent(string UserAgent)
        {
            UAgent = UserAgent;
        }

        private List<KeyValuePair<string, string>> encodeParam(string PlainParam)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("timestamp", DateTimeExtensions.currentTimeMillis().ToString());
            int cur = 0;
            if (PlainParam == null)
                return null;
            while (true)
            {
                while (PlainParam[cur] == '&')
                    cur++;
                if (cur >= PlainParam.Length)
                    break;

                // Get key
                string key = "";
                int eq = PlainParam.IndexOf("=", cur);
                if (eq != 1)
                    key = PlainParam.Substring(cur, eq - cur);
                else
                    break;

                // Get value
                string value = "";
                int next = PlainParam.IndexOf("&", cur);
                if (next != -1)
                    value = PlainParam.Substring(eq + 1, next - eq - 1);
                else
                    value = PlainParam.Substring(eq + 1);

                paramList.Add(key, value);

                if (next == -1)
                    break;
                cur = next + 1;
            }
            
            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonParam = jsonSerializer.Serialize(paramList);
            MACrypto.SEEDCrypt SEED = new MACrypto.SEEDCrypt();
            MACrypto.MACrypto Clib = new MACrypto.MACrypto();
            Clib.initialize(_PUB, MACrypto.MACrypto.genKey());

            string s1 = Clib.getSignedKey();
            Console.WriteLine("SignedKey : " +  s1);
            Console.WriteLine("\nOriginal JSON : " + jsonParam);
            byte[] bJson = SEED.Encrypt(UTF8Encoding.UTF8.GetBytes(jsonParam));
            string tHex = BitConverter.ToString(bJson).Replace("-", string.Empty);
            
            string s2 = MACrypto.AESCrypt.Encrypt(tHex, Clib.getAESKey());
            
            string s3 = s1 + s2;

            var qparamList = new List<KeyValuePair<string, string>>();
            qparamList.Add(new KeyValuePair<string, string>("q", s3));
            return qparamList;
            /*int cur = 0;
            MACrypto.MACrypto MACrypto = new MACrypto.MACrypto();
            MACrypto.setPUB(_PUB);
            MACrypto.genKey();

            string k = MACrypto.getSignedKey();

            HTTP.Headers.Set("User-Agent", UAgent);
            HTTP.Headers.Set("Aceept-Encoding", "gzip, deflate");
            HTTP.Headers.Set("Content-Type", "application/x-www-form-urlencoded");

            paramList.Add(new KeyValuePair<string, string>("K", k));

            if (PlainParam == null || PlainParam.Length == 0)
                return paramList;

            while (true)
            {
                while (PlainParam[cur] == '&')
                    cur++;
                if (cur >= PlainParam.Length)
                    break;

                // Get key
                string key = "";
                int eq = PlainParam.IndexOf("=", cur);
                if (eq != 1)
                    key = PlainParam.Substring(cur, eq-cur);
                else
                    break;

                // Get value
                string value = "";
                int next = PlainParam.IndexOf("&", cur);
                if (next != -1)
                    value = PlainParam.Substring(eq + 1, next - eq - 1);
                else
                    value = PlainParam.Substring(eq + 1);

                string cryptValue = MACrypto.encryptAES(value);
                paramList.Add(new KeyValuePair<string, string>(key, cryptValue));

                if (next == -1)
                    break;
                cur = next + 1;
            }
            return paramList;
             * */
        }

        public byte[] connectPost(string url, string param)
        {
            try
            {
                if (url.IndexOf("?cyt=1") < 0 && url.IndexOf("check_inspection") < 0)
                    url = url + "?cyt=1";
                byte[] encodedParam =null;
                if (url.IndexOf("check_inspection") < 0)
                    encodedParam = WebClientEx.ImportParmFromList(encodeParam(param));

                HTTP.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                HTTP.Headers.Add("User-Agent", UAgent);
                
                byte[] re = null;
                if (encodedParam == null)
                {
                    HTTP.UploadData(url, "POST", UTF8Encoding.UTF8.GetBytes("cyt=1"));
                    return re;
                }
                else
                    HTTP.UploadData(url, "POST", encodedParam);
                return MACrypto.AESCrypt.Decrypt(re, UTF8Encoding.UTF8.GetBytes("011218525486l6u1"));
            }
            catch (System.Net.WebException ex)
            {
                throw ex;
            }
        }

    }
}
