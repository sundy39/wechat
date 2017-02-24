using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Data.Security
{
    public class SHA1Hasher
    {
        public string Hash(string plainText)
        {
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA1");
            byte[] buffer = Encoding.UTF8.GetBytes(plainText);
            byte[] result = hashAlgorithm.ComputeHash(buffer);
            string str = string.Empty;
            for (int i = 0; i < result.Length; i++)
            {
                str += string.Format("{0:x2}", result[i]);
            }
            return str;
        }
    }
}
