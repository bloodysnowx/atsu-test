using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenNashCalculator
{
    class CalcByHRC
    {
        const int BM_CLICK = 0x00F5;
        const int WM_COMMAND = 0x0111;
        const int WM_ACTIVATE = 0x0006;
        const int CB_SETCURSEL = 0x014E;
        const int EM_SETSEL = 0x00B1;
        const int WM_COPY = 0x0301;
        const int CBN_SELCHANGE = 0x0001;

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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
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
        static IntPtr calculatingDialog;
        static IntPtr exportStrategiesDialog;
        static IntPtr selectedSpotComboBox;
        static IntPtr rangeTextBox;
        static IntPtr exportStrategiesDialogOKButton;

        static string rangeText;

        static int MakeWParam(int loWord, int hiWord)
        {
            return (loWord & 0xFFFF) + ((hiWord & 0xFFFF) << 16);
        }

        public static void clearCalc() { rangeText = null; }

        public static string[] Calc(TableData tableData, string chips)
        {   
            IntPtr hrc = FindWindow(null, "HoldemResources Calculator");
            openBasicHand(hrc);
            try
            {
                newDialog = FindDialog("New");

                while (newDialog != IntPtr.Zero)
                {
                    nextButton = FindWindowEx(newDialog, IntPtr.Zero, "Button", null);
                    EnumChildWindows(newDialog, FindNextButton, 0);
                    SendMessage(newDialog, (uint)WM_ACTIVATE, IntPtr.Zero, IntPtr.Zero);
                    SendMessage(nextButton, (uint)BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                    newDialog = FindWindow(null, "New");
                    System.Threading.Thread.Sleep(10);
                }

                setupDialog = FindDialog("Basic Hand Setup");

                createClipBoard(tableData, chips);
                EnumChildWindows(setupDialog, FindPasteButton, 0);
                if (pasteButton == null) throw new ApplicationException();
                for (int i = 0; i < 5; ++i)
                {
                    SendMessage(pasteButton, (uint)BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                    System.Threading.Thread.Sleep(10);
                }
                EnumChildWindows(setupDialog, FindFinishButton, 0);
                if (finishButton == null) throw new ApplicationException();
                SendMessage(finishButton, (uint)BM_CLICK, IntPtr.Zero, IntPtr.Zero);

                calculatingDialog = FindDialog("Basic Hand Setup");
                while (calculatingDialog != IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(100);
                    calculatingDialog =  FindWindow(null, "Basic Hand Setup");
                }

                openExportStrategies(hrc);
                exportStrategiesDialog = FindDialog("Export Strategies");

                IntPtr swtWindowTop = FindWindowEx(exportStrategiesDialog, IntPtr.Zero, "SWT_Window0", null);
                IntPtr swtWindowNext = FindWindowEx(swtWindowTop, IntPtr.Zero, "SWT_Window0", null);
                selectedSpotComboBox = FindWindowEx(swtWindowNext, IntPtr.Zero, "ComboBox", null);
                selectedSpotComboBox = FindWindowEx(swtWindowNext, selectedSpotComboBox, "ComboBox", null);
                PostMessage(selectedSpotComboBox, CB_SETCURSEL, 0, 0);
                SendMessage(selectedSpotComboBox, (uint)CB_SETCURSEL, IntPtr.Zero, IntPtr.Zero);
                PostMessage(exportStrategiesDialog, WM_COMMAND, MakeWParam(0, CBN_SELCHANGE), selectedSpotComboBox.ToInt32());

                rangeTextBox = FindWindowEx(swtWindowNext, IntPtr.Zero, "Edit", null);
                PostMessage(rangeTextBox, EM_SETSEL, 0, -1);
                System.Threading.Thread.Sleep(100);
                SendMessage(rangeTextBox, WM_COPY, IntPtr.Zero, IntPtr.Zero);
                rangeText = Clipboard.GetText();

                swtWindowNext = FindWindowEx(swtWindowTop, swtWindowNext, "SWT_Window0", null);
                exportStrategiesDialogOKButton = FindWindowEx(swtWindowNext, IntPtr.Zero, "Button", "OK");
                SendMessage(exportStrategiesDialogOKButton, (uint)BM_CLICK, IntPtr.Zero, IntPtr.Zero);

                return parseRangeText(rangeText);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot Find HRC" + System.Environment.NewLine + ex);
                return null;
            }
        }

        public static string getOverCallRange(int first, int second)
        {
            return null;
        }

        static string[] parseRangeText(string rangeText)
        {
            return new string[10];
        }

        static void openExportStrategies(IntPtr hrc)
        {
            IntPtr menu = GetMenu(hrc);
            uint handMenuItemID = GetMenuItemID(menu, 1);
            PostMessage(hrc, WM_COMMAND, (int)handMenuItemID, 0);
            IntPtr handMenu = GetSubMenu(menu, 1);
            uint exportStrategiesMenuItemID = GetMenuItemID(handMenu, 1);
            PostMessage(hrc, WM_COMMAND, (int)exportStrategiesMenuItemID, 0);
        }

        static void openBasicHand(IntPtr hrc)
        {
            IntPtr menu = GetMenu(hrc);
            uint fileMenuItemID = GetMenuItemID(menu, 0);
            PostMessage(hrc, WM_COMMAND, (int)fileMenuItemID, 0);
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
                if (count++ > 30)
                {
                    ApplicationException ex = new ApplicationException();
                    throw ex;
                    
                }
                System.Threading.Thread.Sleep(100);
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
