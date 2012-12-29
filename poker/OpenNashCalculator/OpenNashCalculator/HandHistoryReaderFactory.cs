using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNashCalculator
{
    class HandHistoryReaderFactory
    {
        static public HandHistoryReader create(string fileName)
        {
            HandHistoryReader instance;
            fileName = System.IO.Path.GetFileName(fileName);
            if (fileName.StartsWith("FT"))
            {
                instance = new FTHHReader();
            }
            else
            {
                instance = new PSHHReader();
            }
            return instance;
        }
    }
}
