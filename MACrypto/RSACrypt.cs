using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;

namespace MACrypto
{
    public class RSACrypt
    {
        private AsymmetricKeyParameter asymKeyParam;
        private RsaKeyParameters rsaKeyParam;
        private RSAParameters rsaParam;
        private RSACryptoServiceProvider RSA;

        public byte[] Encrypt (byte[] _PlainByte, string _Base64Key)
        {
            asymKeyParam = PublicKeyFactory.CreateKey(Convert.FromBase64String(_Base64Key));
            rsaKeyParam = (RsaKeyParameters)asymKeyParam;
            rsaParam = new RSAParameters();

            rsaParam.Modulus = rsaKeyParam.Modulus.ToByteArrayUnsigned();
            rsaParam.Exponent = rsaKeyParam.Exponent.ToByteArrayUnsigned();

            RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(rsaParam);

            return RSA.Encrypt(_PlainByte, false);
        }

        public byte[] Encrypt (string _PlainString, string _Base64Key)
        {
            return Encrypt(UTF8Encoding.UTF8.GetBytes(_PlainString), _Base64Key);
        }
    }
}
