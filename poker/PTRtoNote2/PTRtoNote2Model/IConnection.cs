using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public interface IConnection
    {
        string[] Users
        {
            get;
            set;
        }

        string[] Passwords
        {
            get;
            set;
        }

        bool isConnected
        {
            get;
            set;
        }

        int SearchedCount
        {
            get;
            set;
        }

        Player GetPTRData(string player_name);
    }
}
