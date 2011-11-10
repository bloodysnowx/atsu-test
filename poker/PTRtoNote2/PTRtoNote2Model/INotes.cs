using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public interface INotes
    {
        PTRData[] Datas
        {
            get;
            set;
        }
    
        void Load();

        void Save();
    }
}
