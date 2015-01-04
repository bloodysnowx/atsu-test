﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using UserValidatorLib;

namespace WhiteListGeneratorUI
{
    public class ONCPublisher
    {
        UserNameEncrypter encrypter = new UserNameEncrypter();

        public void publish(string userName, string path)
        {
            string filesPath = path + "\\files";
            string zipPath = path + "\\" + userName + ".zip";
            generate(userName, filesPath);
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("Shift_JIS")))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.AddDirectory(filesPath);
                zip.Save(zipPath);
            }
        }

        public void generate(string userName, string path)
        {
            string whiteListPath = path + "\\" + "whiteList";
            System.IO.File.WriteAllText(whiteListPath, encrypter.encryptFromUserName(userName));
        }
    }
}
