using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Text.RegularExpressions;

namespace OpenNashCalculator
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, WNDENUMPROC lpEnumFunc, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int RegisterWindowMessage(string lpString);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)] //
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam,
        int lparam);

        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;

        public void RegisterControlforMessages()
        {
            RegisterWindowMessage("WM_GETTEXT");
        }

        public string GetControlText(IntPtr hWnd)
        {

            StringBuilder title = new StringBuilder();

            // Get the size of the string required to hold the window title. 
            Int32 size = SendMessage((int)hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

            // If the return is 0, there is no title. 
            if (size > 0)
            {
                title = new StringBuilder(size + 1);

                SendMessage(hWnd, (int)WM_GETTEXT, title.Capacity, title);


            }
            return title.ToString();
        }

        bool FindTournamentWindow()
        {
            bool result = false;
            close_flg = true;
            EnumWindows(EnumerateWindow, IntPtr.Zero);
            if (close_flg == true && checkBoxClose.Checked)
                Application.Exit();
            return result;
        }

        bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            Process p = Process.GetProcessById(processId);
            // PokerStarsのウィンドウかプロセスを確認

            if (p.ProcessName == "PokerStars")
            {
                StringBuilder caption = new StringBuilder(0x1000);
                GetWindowText(hWnd, caption, caption.Capacity);
                if (IsWindowVisible(hWnd) && caption.ToString().Contains(tourney_ID))
                {
					close_flg = false;
                    // EnumChildWindows(hWnd, EnumerateChildWindow, lParam);
                    GetWindowRect(hWnd, out tourneyWindowRect);
                }
            }
            return true;
        }

        bool EnumerateChildWindow(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                string title = GetControlText(hWnd);
            }
            return true;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        void GoBack()
        {
            this.Location = new Point(tourneyWindowRect.left + 20, tourneyWindowRect.top - 25);

            IntPtr tmpdesktop = FindWindow(null, "Program Manager");
#if true
            if (tmpdesktop != null)
                SetParent(this.Handle, tmpdesktop);

            SetParent(this.Handle, IntPtr.Zero);

#endif
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.Dll")]
        static extern int GetWindowRect(IntPtr hWnd, out RECT rect);
    }
}