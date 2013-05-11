using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return System.IO.Directory.GetCurrentDirectory() + "/ナッシュさん.exe";
        }
    }
}
