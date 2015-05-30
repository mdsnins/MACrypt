using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MACrypto
{
    public class MACrypto
    {
        private string _PVK;
        private string _PUB;

        private byte[] _AESKey;
        private string _genSEED;
        
        private RSACrypt RSA;
        private SEEDCrypt SEED;
        
        public MACrypto()
        {
            RSA = new RSACrypt();
            SEED = new SEEDCrypt();
        }

        public void initialize(string PublicKey, byte[] AESKey)
        {
            _PUB = PublicKey;
            _AESKey = AESKey;

            string _genKey = Convert.ToBase64String(AESKey);
            
            byte[] _genKeyByte = UTF8Encoding.UTF8.GetBytes(_genKey);

            _genSEED = Convert.ToBase64String(
                SEED.Encrypt(
                    UTF8Encoding.UTF8.GetBytes(
                        BitConverter.ToString(_genKeyByte).Replace("-",string.Empty)
                    )
                )
            );
        }

        public string str2hex(string strData)
        {
            string resultHex = string.Empty;
            byte[] arr_byteStr = Encoding.Default.GetBytes(strData);

            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);

            return resultHex;
        }

        public string getSignedKey()
        {
            byte[] _genSEEDByte = UTF8Encoding.UTF8.GetBytes(_genSEED);
            string _HexedSEED = BitConverter.ToString(_genSEEDByte).Replace("-", string.Empty);
            byte[] _genRSA = RSA.Encrypt(_HexedSEED, _PUB);
            return Convert.ToBase64String(_genRSA);
        }

        public static byte[] genKey()
        {
            string randomstring = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$";
            var rnd = new Random();
            string gennedKey = "";
            for (int i = 0; i < 16; i++) {
                int index = rnd.Next(randomstring.Length);
                gennedKey = gennedKey + randomstring[index];
            }
            return UTF8Encoding.UTF8.GetBytes(gennedKey);
        }

        public byte[] getAESKey()
        {
            return _AESKey;
        }
    }
}
