using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2
{
    public interface INotes
    {
        PTRtoNote2.PTRData[] Datas
        {
            get;
            set;
        }
    
        void Load();

        void Save();
    }
}
