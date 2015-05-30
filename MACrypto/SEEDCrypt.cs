using System;
using System.Collections.Generic;
using System.Text;

namespace MACrypto
{
    public class SEEDCrypt
    {
        private byte[] pbszUserKey = new byte[16] { 0x88, 0xE3, 0x4F, 0x8F, 0x08, 0x17, 0x79, 0xF1, 0xE9, 0xF3, 0x94, 0x37, 0x0A, 0xD4, 0x05, 0x89 };
        private byte[] pbszIV = new byte[16] { 0x26, 0x51, 0x02, 0x4E, 0x1B, 0x5B, 0x0D, 0x08, 0x17, 0x2E, 0x65, 0x71, 0x58, 0x42, 0x6F, 0x73 };
        private _SEED SEED;
        public SEEDCrypt()
        {
            SEED = new _SEED();
            SEED.IV = pbszIV;
            SEED.KeyBytes = pbszUserKey;
            SEED.ModType = _SEED.MODE.AI_CBC;
            SEED.PadType = _SEED.PADDING.AI_PKCS_PADDING;
        }

        public byte[] Encrypt(byte[] _PlainByte)
        {
            return SEED.Encrypt(_PlainByte);
        }

        public byte[] Encrypt(string _PlainString)
        {
            return Encrypt(UTF8Encoding.UTF8.GetBytes(_PlainString));
        }

        public byte[] Decrypt(byte[] _EncByte)
        {
            return SEED.Decrypt(_EncByte);
        }
    }
}
