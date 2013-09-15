using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNashCalculator
{
    class CalcByHRC
    {
        const int BM_CLICK = 0x00F5;
        const int WM_COMMAND = 0x0111;
        const int WM_ACTIVATE = 0x0006;

        delegate bool WNDENUMPROC(IntPtr hwnd, int lParam);
        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int EnumChildWindows(IntPtr hWnd, WNDENUMPROC lpEnumFunc, int lParam);
        [DllImport("user32.dll")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        extern static int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nMaxCount);
        [DllImport("user32.dll")]
        extern static bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern Int32 PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);
        [DllImport("user32.dll")]
        static extern uint GetMenuItemID(IntPtr hMenu, int nPos);

        static IntPtr newDialog;
        static IntPtr setupDialog;
        static IntPtr nextButton;
        static IntPtr finishButton;
        static IntPtr pasteButton;

        public static void Calc(TableData tableData, string chips)
        {
            IntPtr hrc = FindWindow(null, "HoldemResources Calculator");
            openBasicHand(hrc);
            newDialog = FindDialog("New");
            nextButton = FindWindowEx(newDialog, IntPtr.Zero, "Button", null);
            EnumChildWindows(newDialog, FindNextButton, 0);
            PostMessage(newDialog, WM_ACTIVATE, 0, 0);
            PostMessage(nextButton, BM_CLICK, 0, 0);

            setupDialog = FindDialog("Basic Hand Setup");

            createClipBoard(tableData, chips);
            EnumChildWindows(setupDialog, FindPasteButton, 0);
            PostMessage(pasteButton, BM_CLICK, 0, 0);
            System.Threading.Thread.Sleep(1000);
            EnumChildWindows(setupDialog, FindFinishButton, 0);
            PostMessage(finishButton, BM_CLICK, 0, 0);
        }

        static void openBasicHand(IntPtr hrc)
        {
            IntPtr menu = GetMenu(hrc);
            IntPtr fileMenu = GetSubMenu(menu, 0);
            uint newMenuItemID = GetMenuItemID(fileMenu, 0);
            PostMessage(hrc, WM_COMMAND, (int)newMenuItemID, 0);
        }

        static IntPtr FindDialog(string name)
        {
            IntPtr dialog = IntPtr.Zero;
            int count = 0;
            while (dialog == IntPtr.Zero)
            {
                dialog = FindWindow(null, name);
                if (count++ > 3)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Find HRC");
                    return dialog;
                }
                System.Threading.Thread.Sleep(1000);
            }
            return dialog;
        }

        static bool FindNextButton(IntPtr hWnd, int lParam)
        {
            StringBuilder title = new StringBuilder(1000);
            GetWindowText(hWnd, title, 1000);
            if (!title.ToString().Equals("&Next >"))
                return true;

            nextButton = hWnd;
            return false;
        }

        static bool FindFinishButton(IntPtr hWnd, int lParam)
        {
            StringBuilder title = new StringBuilder(1000);
            GetWindowText(hWnd, title, 1000);
            if (!title.ToString().Equals("&Finish"))
                return true;

            finishButton = hWnd;
            return false;
        }

        static bool FindPasteButton(IntPtr hWnd, int lParam)
        {
            StringBuilder title = new StringBuilder(1000);
            GetWindowText(hWnd, title, 1000);
            if (!title.ToString().Equals("Paste from Clipboard"))
                return true;

            pasteButton = hWnd;
            return false;
        }

        static void createClipBoard(TableData tableData, string chips)
        {
            StringBuilder handHistory = new StringBuilder();
            handHistory.Append("PokerStars Hand #1: Tournament #1, $72.55+$1.45 USD Hold'em No Limit - Level I (");
            handHistory.Append(tableData.SB);
            handHistory.Append("/");
            handHistory.Append(tableData.BB);
            handHistory.AppendLine(") - 2013/01/01 13:00:00 JST [2013/01/01 00:00:00 ET]");

            handHistory.AppendLine("Table '1 1' 9-max Seat #" + (tableData.getLivePlayerCount() - 2) + " is the button");
            handHistory.AppendLine(chips);

            for (int i = 0; i < tableData.getLivePlayerCount(); ++i)
            {
                handHistory.AppendLine("Player" + (i + 1) + ": posts the ante " + tableData.Ante);
            }

            System.Windows.Forms.Clipboard.SetText(handHistory.ToString());
        }
    }
}
