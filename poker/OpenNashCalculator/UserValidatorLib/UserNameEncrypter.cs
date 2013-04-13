using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserValidatorLib
{
    public class UserNameEncrypter
    {
        public String encryptFromUserName(String userName)
        {
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes("chiyuki" + userName + "血で染まる雪");
            var md5gen = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] encodedData = md5gen.ComputeHash(originalData);
            var result = new System.Text.StringBuilder();
            foreach(byte b in encodedData)
                result.Append(b.ToString("x2"));
            return result.ToString();
        }
    }
}
