using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace ONCDaemon
{
    public class FilePathManager
    {
        public string getHyperSatBuyinListPath()
        {
            return System.IO.Directory.GetCurrentDirectory() + "\\"
                + Properties.Settings.Default.HyperSatBuyinListName;
        }

        public string getONCPath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = assembly.Location;
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string file in files)
            {
                if (file.Contains(".exe") && !file.Contains("vshost") && !file.Contains("config") && !file.Equals(path))
                {
                    return file;
                }
            }
            return System.IO.Directory.GetCurrentDirectory() + "/正宗.exe";
        }
    }
}
