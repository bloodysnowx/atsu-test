using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public interface IConfig
    {
        int[] Label_Mins
        {
            get;
        }

        int Label_Last_Hand_Max
        {
            get;
        }

        int Reacquisition_Span_Days
        {
            get;
        }

        int Label_Last_Reacquisition_Span_Days
        {
            get;
        }

        string[] Logins
        {
            get;
        }

        string[] Passwords
        {
            get;
        }

        int Wait_Main_Time
        {
            get;
        }

        int Wait_Rand_Time
        {
            get;
        }

        int Max_Search_Num
        {
            get;
        }

        int Max_BB_For_Listing
        {
            get;
        }

        int Old_Data_Delete_Span_Days
        {
            get;
        }
    }
}
