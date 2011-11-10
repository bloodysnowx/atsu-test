using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.CUI
{
    public class AppConfig : PTRtoNote2Model.IConfig
    {
        public AppConfig()
        {
        }

        public int[] Label_Mins
        {
            get { throw new NotImplementedException(); }
        }

        public int Label_Last_Hand_Max
        {
            get { throw new NotImplementedException(); }
        }

        public int Reacquisition_Span_Days
        {
            get { throw new NotImplementedException(); }
        }

        public int Label_Last_Reacquisition_Span_Days
        {
            get { throw new NotImplementedException(); }
        }

        public string[] Logins
        {
            get { throw new NotImplementedException(); }
        }

        public string[] Passwords
        {
            get { throw new NotImplementedException(); }
        }

        public int Wait_Main_Time
        {
            get { throw new NotImplementedException(); }
        }

        public int Wait_Rand_Time
        {
            get { throw new NotImplementedException(); }
        }

        public int Max_Search_Num
        {
            get { throw new NotImplementedException(); }
        }

        public int Max_BB_For_Listing
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Old_Data_Delete_Span_Days
        {
            get { throw new NotImplementedException(); }
        }
    }
}
