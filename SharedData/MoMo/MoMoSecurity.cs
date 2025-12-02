using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedData.MoMo
{
    public class MoMoSecurity
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public MoMoSecurity()
        {
        }
        public string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
        public string RSAEncrypt(string data, string publicKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(publicKeyXml);
                    var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
                    return Convert.ToBase64String(encryptedData);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
