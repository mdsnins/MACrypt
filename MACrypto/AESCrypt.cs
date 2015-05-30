using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MACrypto
{
    public class AESCrypt
    {
        public static string Encrypt(string _PlainString, byte[] _Key)
        {
            return Encrypt(ASCIIEncoding.ASCII.GetBytes(_PlainString), _Key);
        }    
        public static string Encrypt(byte[] _PlainByte, byte[] _Key)
        {
            RijndaelManaged _AES = new RijndaelManaged();
            _AES.Mode = CipherMode.ECB;
            _AES.KeySize = 128;
            _AES.Padding = PaddingMode.PKCS7;

            _AES.Key = _Key;
            string _res;
            try
            {
                ICryptoTransform _Cryptor = _AES.CreateEncryptor();
                _res = Convert.ToBase64String(_Cryptor.TransformFinalBlock(_PlainByte, 0, _PlainByte.Length));
            } 
            catch(Exception ex)
            {
                throw ex;
            }

            return _res;
        }

        public static byte[] Decrypt(byte[] _EncByte, byte[] _Key)
        {
            RijndaelManaged _AES = new RijndaelManaged();
            _AES.Mode = CipherMode.ECB;
            _AES.KeySize = 128;
            _AES.Padding = PaddingMode.PKCS7;
            _AES.Key = _Key;
            try
            {
                ICryptoTransform _Cryptor = _AES.CreateDecryptor();
                return _Cryptor.TransformFinalBlock(_EncByte, 0, _EncByte.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}