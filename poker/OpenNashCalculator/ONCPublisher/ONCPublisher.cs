using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserValidatorLib;
using System.IO.Compression;

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
            ZipFile.CreateFromDirectory(filesPath, zipPath);
        }

        public void generate(string userName, string path)
        {
            string whiteListPath = path + "\\" + "whiteList";
            System.IO.File.WriteAllText(whiteListPath, encrypter.encryptFromUserName(userName));
        }
    }
}
