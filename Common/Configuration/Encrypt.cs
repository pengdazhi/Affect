using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace Common
{
    public class Encrypt
    {
        private SymmetricAlgorithm DES;
        private const string CIV = "12345678901=";//密钥 
        private const string CKEY = "12345678901=";//初始化向量 

        public Encrypt()
        {
            DES = new DESCryptoServiceProvider();
            //DbProviderFactory.GetDbProvider("System.Data.SqlClient");
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string EncryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            ct = DES.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));
            byt = Encoding.UTF8.GetBytes(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string DecryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            ct = DES.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

    }
}
