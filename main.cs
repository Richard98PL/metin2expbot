using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;
using WindowsInput;

namespace metin
{
    public partial class Form1 : Form
    {


        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("User32.dll")]
        private static extern Int32 SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, StringBuilder lParam);
        public Form1()
        {
            InitializeComponent();
            execute();
            timer1.Start();
        }

        private static Process target = null;
        private void execute()
        {
            if (target == null)
            {
                Process[] processList = Process.GetProcesses();

                foreach (Process P in processList)
                {
                    if (P.ProcessName.ToLower().Contains("metin2client"))
                    {
                        target = P;
                    }
                }
            }

            if (target == null)
            {
                throw new FileNotFoundException("process not found!");
            }
            else
            {
                //Console.Out.WriteLine(target.Id + " -> " + target.ProcessName);
            }

            IntPtr edit = target.MainWindowHandle;
            //ShowWindow(edit, 3);
            SwitchToThisWindow(edit, true);
            SetForegroundWindow(edit);

            EButton.Button btn = new EButton.Button();
            for (int i = 0; i < 5; i++)
            {
                short Z = (short)EButton.Button.BT7.KEY_Z;
                btn.PressKey(Z);
            }
        }

        private static Int32 safetyCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            execute();
            safetyCount++;
            if(safetyCount <  0)
            {
                timer1.Dispose();
            }
        }

   
    }
}
