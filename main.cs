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

using System;
using System.Threading;
using System.Drawing.Imaging;
using IronOcr;
using System.Text.RegularExpressions;

namespace metin
{
    public partial class Form1 : Form
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

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

            Bitmap captureBitmap = null;
            try
            {
               
                captureBitmap = new Bitmap(70, 40, PixelFormat.Format32bppArgb);

                Rectangle captureRectangle = new Rectangle(208, 679, 70, 40);
              
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
               
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
              
                //captureBitmap.Save("Capture.jpg", ImageFormat.Jpeg);

                Console.WriteLine("image");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Double hpPercent = 100;
            var ocr = new IronTesseract();
            ocr.Configuration.BlackListCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-.:;<=>?@[\\]^_`{|}~";
            using (var input = new OcrInput(captureBitmap))
            {
                var result = ocr.Read(input);
                String scan = (result.Text);
                Console.WriteLine(scan);

                if (scan.Count(x => x == '/') == 1)
                {
                    string[] substrings = scan.Split('/');
                    int numberBeforeSlash = int.Parse(ReplaceWhitespace(substrings[0].Trim(), ""));
                    int numberAfterSlash = int.Parse(ReplaceWhitespace(substrings[1].Trim(), ""));
                    Console.WriteLine(numberBeforeSlash);
                    Console.WriteLine(numberAfterSlash);
                    label1.Text = "" + numberBeforeSlash + "/" + numberAfterSlash;
                    double percentage = (double)numberBeforeSlash / numberAfterSlash * 100;
                    hpPercent = percentage;
                    Console.WriteLine(percentage);
                }
            }


            BOT button = new BOT();
            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);

            if(hpPercent < 80) button.PressKey(BOT.BT7.KEY_1);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);


            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_2);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);


            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_3);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);


            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_4);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);

            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_5);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);

            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_6);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);

            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_7);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);

            if (hpPercent < 80) button.PressKey(BOT.BT7.KEY_8);
            Thread.Sleep(25);

            button.PressKey(BOT.BT7.KEY_Z);
            Thread.Sleep(25);
        }

        private static Int32 safetyCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            execute();
            safetyCount++;
            if (safetyCount < 0)
            {
                timer1.Dispose();
            }
        }



        public class BOT
        {
            public void PressKey(BOT.BT7 btn)
            {
                BOT.Input[] inputs = new BOT.Input[4];
                BOT.Input input = new BOT.Input();
                input.type = 1U;
                input.i_union.keyboardinput.wVk = (BOT.BT6)0;
                input.i_union.keyboardinput.time = 0;
                input.i_union.keyboardinput.dwExtraInfo = (UIntPtr)0UL;
                input.i_union.keyboardinput.dwFlags = BOT.BT5.SCANCODE;
                input.i_union.keyboardinput.wScan = btn;
                inputs[0] = input;

                int num1 = (int)BOT.SendInput(1U, inputs, BOT.Input.Size);
                Thread.Sleep(50);

                input.i_union.keyboardinput.dwFlags = BOT.BT5.KEYUP;
                inputs[0] = input;

                int num2 = (int)BOT.SendInput(1U, inputs, BOT.Input.Size);
            }



            [DllImport("user32.dll")]
            internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] BOT.Input[] inputs, int size);

            private enum GAME_KEY : short
            {
                SPACE = 57, // 0x0039
            }

            public struct Input
            {
                public uint type;
                public BOT.InputUnion i_union;

                public static int Size => Marshal.SizeOf(typeof(BOT.Input));
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct InputUnion
            {
                [FieldOffset(0)]
                internal BOT.MouseInput mouseinput;
                [FieldOffset(0)]
                internal BOT.KeyboardInput keyboardinput;
                [FieldOffset(0)]
                internal BOT.HardwareInput hardwareinput;
            }

            public struct MouseInput
            {
                internal int int_0;
                internal int int_1;
                internal BOT.BT3 btn3_0;
                internal BOT.BT4 btn4_0;
                internal uint uint_0;
                internal UIntPtr uintptr_0;
            }

            public struct KeyboardInput
            {
                internal BOT.BT6 wVk;
                internal BOT.BT7 wScan;
                internal BOT.BT5 dwFlags;
                internal int time;
                internal UIntPtr dwExtraInfo;
            }

            public struct HardwareInput
            {
                internal int int_0;
                internal short short_0;
                internal short short_1;
            }

            public enum BT2
            {
                SM_CXSCREEN,
                SM_CYSCREEN,
            }

            [Flags]
            public enum BT3 : uint
            {
                Nothing = 0,
                XBUTTON1 = 1,
                XBUTTON2 = 2,
            }

            [Flags]
            public enum BT4 : uint
            {
                ABSOLUTE = 32768, // 0x00008000
                HWHEEL = 4096, // 0x00001000
                MOVE = 1,
                MOVE_NOCOALESCE = 8192, // 0x00002000
                LEFTDOWN = 2,
                LEFTUP = 4,
                RIGHTDOWN = 8,
                RIGHTUP = 16, // 0x00000010
                MIDDLEDOWN = 32, // 0x00000020
                MIDDLEUP = 64, // 0x00000040
                VIRTUALDESK = 16384, // 0x00004000
                WHEEL = 2048, // 0x00000800
                XDOWN = 128, // 0x00000080
                XUP = 256, // 0x00000100
            }

            [Flags]
            public enum BT5 : uint
            {
                KEYDOWN = 0,
                EXTENDEDKEY = 1,
                KEYUP = 2,
                SCANCODE = 8,
                UNICODE = 4,
            }

            public enum BT6 : short
            {
                LBUTTON = 1,
                RBUTTON = 2,
                CANCEL = 3,
                MBUTTON = 4,
                XBUTTON1 = 5,
                XBUTTON2 = 6,
                BACK = 8,
                TAB = 9,
                CLEAR = 12, // 0x000C
                RETURN = 13, // 0x000D
                SHIFT = 16, // 0x0010
                CONTROL = 17, // 0x0011
                MENU = 18, // 0x0012
                PAUSE = 19, // 0x0013
                CAPITAL = 20, // 0x0014
                HANGUL = 21, // 0x0015
                KANA = 21, // 0x0015
                JUNJA = 23, // 0x0017
                FINAL = 24, // 0x0018
                HANJA = 25, // 0x0019
                KANJI = 25, // 0x0019
                ESCAPE = 27, // 0x001B
                CONVERT = 28, // 0x001C
                NONCONVERT = 29, // 0x001D
                ACCEPT = 30, // 0x001E
                MODECHANGE = 31, // 0x001F
                SPACE = 32, // 0x0020
                PRIOR = 33, // 0x0021
                NEXT = 34, // 0x0022
                END = 35, // 0x0023
                HOME = 36, // 0x0024
                LEFT = 37, // 0x0025
                UP = 38, // 0x0026
                RIGHT = 39, // 0x0027
                DOWN = 40, // 0x0028
                SELECT = 41, // 0x0029
                PRINT = 42, // 0x002A
                EXECUTE = 43, // 0x002B
                SNAPSHOT = 44, // 0x002C
                INSERT = 45, // 0x002D
                DELETE = 46, // 0x002E
                HELP = 47, // 0x002F
                KEY_0 = 48, // 0x0030
                KEY_1 = 49, // 0x0031
                KEY_2 = 50, // 0x0032
                KEY_3 = 51, // 0x0033
                KEY_4 = 52, // 0x0034
                KEY_5 = 53, // 0x0035
                KEY_6 = 54, // 0x0036
                KEY_7 = 55, // 0x0037
                KEY_8 = 56, // 0x0038
                KEY_9 = 57, // 0x0039
                KEY_A = 65, // 0x0041
                KEY_B = 66, // 0x0042
                KEY_C = 67, // 0x0043
                KEY_D = 68, // 0x0044
                KEY_E = 69, // 0x0045
                KEY_F = 70, // 0x0046
                KEY_G = 71, // 0x0047
                KEY_H = 72, // 0x0048
                KEY_I = 73, // 0x0049
                KEY_J = 74, // 0x004A
                KEY_K = 75, // 0x004B
                KEY_L = 76, // 0x004C
                KEY_M = 77, // 0x004D
                KEY_N = 78, // 0x004E
                KEY_O = 79, // 0x004F
                KEY_P = 80, // 0x0050
                KEY_Q = 81, // 0x0051
                KEY_R = 82, // 0x0052
                KEY_S = 83, // 0x0053
                KEY_T = 84, // 0x0054
                KEY_U = 85, // 0x0055
                KEY_V = 86, // 0x0056
                KEY_W = 87, // 0x0057
                KEY_X = 88, // 0x0058
                KEY_Y = 89, // 0x0059
                KEY_Z = 90, // 0x005A
                LWIN = 91, // 0x005B
                RWIN = 92, // 0x005C
                APPS = 93, // 0x005D
                SLEEP = 95, // 0x005F
                NUMPAD0 = 96, // 0x0060
                NUMPAD1 = 97, // 0x0061
                NUMPAD2 = 98, // 0x0062
                NUMPAD3 = 99, // 0x0063
                NUMPAD4 = 100, // 0x0064
                NUMPAD5 = 101, // 0x0065
                NUMPAD6 = 102, // 0x0066
                NUMPAD7 = 103, // 0x0067
                NUMPAD8 = 104, // 0x0068
                NUMPAD9 = 105, // 0x0069
                MULTIPLY = 106, // 0x006A
                ADD = 107, // 0x006B
                SEPARATOR = 108, // 0x006C
                SUBTRACT = 109, // 0x006D
                DECIMAL = 110, // 0x006E
                DIVIDE = 111, // 0x006F
                F1 = 112, // 0x0070
                F2 = 113, // 0x0071
                F3 = 114, // 0x0072
                F4 = 115, // 0x0073
                F5 = 116, // 0x0074
                F6 = 117, // 0x0075
                F7 = 118, // 0x0076
                F8 = 119, // 0x0077
                F9 = 120, // 0x0078
                F10 = 121, // 0x0079
                F11 = 122, // 0x007A
                F12 = 123, // 0x007B
                F13 = 124, // 0x007C
                F14 = 125, // 0x007D
                F15 = 126, // 0x007E
                F16 = 127, // 0x007F
                F17 = 128, // 0x0080
                F18 = 129, // 0x0081
                F19 = 130, // 0x0082
                F20 = 131, // 0x0083
                F21 = 132, // 0x0084
                F22 = 133, // 0x0085
                F23 = 134, // 0x0086
                F24 = 135, // 0x0087
                NUMLOCK = 144, // 0x0090
                SCROLL = 145, // 0x0091
                LSHIFT = 160, // 0x00A0
                RSHIFT = 161, // 0x00A1
                LCONTROL = 162, // 0x00A2
                RCONTROL = 163, // 0x00A3
                LMENU = 164, // 0x00A4
                RMENU = 165, // 0x00A5
                BROWSER_BACK = 166, // 0x00A6
                BROWSER_FORWARD = 167, // 0x00A7
                BROWSER_REFRESH = 168, // 0x00A8
                BROWSER_STOP = 169, // 0x00A9
                BROWSER_SEARCH = 170, // 0x00AA
                BROWSER_FAVORITES = 171, // 0x00AB
                BROWSER_HOME = 172, // 0x00AC
                VOLUME_MUTE = 173, // 0x00AD
                VOLUME_DOWN = 174, // 0x00AE
                VOLUME_UP = 175, // 0x00AF
                MEDIA_NEXT_TRACK = 176, // 0x00B0
                MEDIA_PREV_TRACK = 177, // 0x00B1
                MEDIA_STOP = 178, // 0x00B2
                MEDIA_PLAY_PAUSE = 179, // 0x00B3
                LAUNCH_MAIL = 180, // 0x00B4
                LAUNCH_MEDIA_SELECT = 181, // 0x00B5
                LAUNCH_APP1 = 182, // 0x00B6
                LAUNCH_APP2 = 183, // 0x00B7
                OEM_1 = 186, // 0x00BA
                OEM_PLUS = 187, // 0x00BB
                OEM_COMMA = 188, // 0x00BC
                OEM_MINUS = 189, // 0x00BD
                OEM_PERIOD = 190, // 0x00BE
                OEM_2 = 191, // 0x00BF
                OEM_3 = 192, // 0x00C0
                OEM_4 = 219, // 0x00DB
                OEM_5 = 220, // 0x00DC
                OEM_6 = 221, // 0x00DD
                OEM_7 = 222, // 0x00DE
                OEM_8 = 223, // 0x00DF
                OEM_102 = 226, // 0x00E2
                PROCESSKEY = 229, // 0x00E5
                PACKET = 231, // 0x00E7
                ATTN = 246, // 0x00F6
                CRSEL = 247, // 0x00F7
                EXSEL = 248, // 0x00F8
                EREOF = 249, // 0x00F9
                PLAY = 250, // 0x00FA
                ZOOM = 251, // 0x00FB
                NONAME = 252, // 0x00FC
                PA1 = 253, // 0x00FD
                OEM_CLEAR = 254, // 0x00FE
            }

            public enum BT7 : short
            {
                ACCEPT = 0,
                ATTN = 0,
                CONVERT = 0,
                CRSEL = 0,
                EXECUTE = 0,
                EXSEL = 0,
                FINAL = 0,
                HANGUL = 0,
                HANJA = 0,
                JUNJA = 0,
                KANA = 0,
                KANJI = 0,
                LBUTTON = 0,
                MBUTTON = 0,
                MODECHANGE = 0,
                NONAME = 0,
                NONCONVERT = 0,
                OEM_8 = 0,
                OEM_CLEAR = 0,
                PA1 = 0,
                PACKET = 0,
                PAUSE = 0,
                PLAY = 0,
                PRINT = 0,
                PROCESSKEY = 0,
                RBUTTON = 0,
                SELECT = 0,
                SEPARATOR = 0,
                XBUTTON1 = 0,
                XBUTTON2 = 0,
                ESCAPE = 1,
                KEY_1 = 2,
                KEY_2 = 3,
                KEY_3 = 4,
                KEY_4 = 5,
                KEY_5 = 6,
                KEY_6 = 7,
                KEY_7 = 8,
                KEY_8 = 9,
                KEY_9 = 10, // 0x000A
                KEY_0 = 11, // 0x000B
                OEM_MINUS = 12, // 0x000C
                OEM_PLUS = 13, // 0x000D
                BACK = 14, // 0x000E
                TAB = 15, // 0x000F
                KEY_Q = 16, // 0x0010
                MEDIA_PREV_TRACK = 16, // 0x0010
                KEY_W = 17, // 0x0011
                KEY_E = 18, // 0x0012
                KEY_R = 19, // 0x0013
                KEY_T = 20, // 0x0014
                KEY_Y = 21, // 0x0015
                KEY_U = 22, // 0x0016
                KEY_I = 23, // 0x0017
                KEY_O = 24, // 0x0018
                KEY_P = 25, // 0x0019
                MEDIA_NEXT_TRACK = 25, // 0x0019
                OEM_4 = 26, // 0x001A
                OEM_6 = 27, // 0x001B
                RETURN = 28, // 0x001C
                CONTROL = 29, // 0x001D
                LCONTROL = 29, // 0x001D
                RCONTROL = 29, // 0x001D
                KEY_A = 30, // 0x001E
                KEY_S = 31, // 0x001F
                KEY_D = 32, // 0x0020
                VOLUME_MUTE = 32, // 0x0020
                KEY_F = 33, // 0x0021
                LAUNCH_APP2 = 33, // 0x0021
                KEY_G = 34, // 0x0022
                MEDIA_PLAY_PAUSE = 34, // 0x0022
                KEY_H = 35, // 0x0023
                KEY_J = 36, // 0x0024
                MEDIA_STOP = 36, // 0x0024
                KEY_K = 37, // 0x0025
                KEY_L = 38, // 0x0026
                OEM_1 = 39, // 0x0027
                OEM_7 = 40, // 0x0028
                OEM_3 = 41, // 0x0029
                LSHIFT = 42, // 0x002A
                SHIFT = 42, // 0x002A
                OEM_5 = 43, // 0x002B
                KEY_Z = 44, // 0x002C
                KEY_X = 45, // 0x002D
                KEY_C = 46, // 0x002E
                VOLUME_DOWN = 46, // 0x002E
                KEY_V = 47, // 0x002F
                KEY_B = 48, // 0x0030
                VOLUME_UP = 48, // 0x0030
                KEY_N = 49, // 0x0031
                BROWSER_HOME = 50, // 0x0032
                KEY_M = 50, // 0x0032
                OEM_COMMA = 51, // 0x0033
                OEM_PERIOD = 52, // 0x0034
                DIVIDE = 53, // 0x0035
                OEM_2 = 53, // 0x0035
                RSHIFT = 54, // 0x0036
                MULTIPLY = 55, // 0x0037
                LMENU = 56, // 0x0038
                MENU = 56, // 0x0038
                RMENU = 56, // 0x0038
                SPACE = 57, // 0x0039
                CAPITAL = 58, // 0x003A
                F1 = 59, // 0x003B
                F2 = 60, // 0x003C
                F3 = 61, // 0x003D
                F4 = 62, // 0x003E
                F5 = 63, // 0x003F
                F6 = 64, // 0x0040
                F7 = 65, // 0x0041
                F8 = 66, // 0x0042
                F9 = 67, // 0x0043
                F10 = 68, // 0x0044
                NUMLOCK = 69, // 0x0045
                CANCEL = 70, // 0x0046
                SCROLL = 70, // 0x0046
                HOME = 71, // 0x0047
                NUMPAD7 = 71, // 0x0047
                NUMPAD8 = 72, // 0x0048
                UP = 72, // 0x0048
                NUMPAD9 = 73, // 0x0049
                PRIOR = 73, // 0x0049
                SUBTRACT = 74, // 0x004A
                LEFT = 75, // 0x004B
                NUMPAD4 = 75, // 0x004B
                CLEAR = 76, // 0x004C
                NUMPAD5 = 76, // 0x004C
                NUMPAD6 = 77, // 0x004D
                RIGHT = 77, // 0x004D
                ADD = 78, // 0x004E
                END = 79, // 0x004F
                NUMPAD1 = 79, // 0x004F
                DOWN = 80, // 0x0050
                NUMPAD2 = 80, // 0x0050
                NEXT = 81, // 0x0051
                NUMPAD3 = 81, // 0x0051
                INSERT = 82, // 0x0052
                NUMPAD0 = 82, // 0x0052
                DECIMAL = 83, // 0x0053
                DELETE = 83, // 0x0053
                SNAPSHOT = 84, // 0x0054
                OEM_102 = 86, // 0x0056
                F11 = 87, // 0x0057
                F12 = 88, // 0x0058
                LWIN = 91, // 0x005B
                RWIN = 92, // 0x005C
                APPS = 93, // 0x005D
                EREOF = 93, // 0x005D
                SLEEP = 95, // 0x005F
                ZOOM = 98, // 0x0062
                HELP = 99, // 0x0063
                F13 = 100, // 0x0064
                BROWSER_SEARCH = 101, // 0x0065
                F14 = 101, // 0x0065
                BROWSER_FAVORITES = 102, // 0x0066
                F15 = 102, // 0x0066
                BROWSER_REFRESH = 103, // 0x0067
                F16 = 103, // 0x0067
                BROWSER_STOP = 104, // 0x0068
                F17 = 104, // 0x0068
                BROWSER_FORWARD = 105, // 0x0069
                F18 = 105, // 0x0069
                BROWSER_BACK = 106, // 0x006A
                F19 = 106, // 0x006A
                F20 = 107, // 0x006B
                LAUNCH_APP1 = 107, // 0x006B
                F21 = 108, // 0x006C
                LAUNCH_MAIL = 108, // 0x006C
                F22 = 109, // 0x006D
                LAUNCH_MEDIA_SELECT = 109, // 0x006D
                F23 = 110, // 0x006E
                F24 = 118, // 0x0076
            }
        }

        public static String buttonInitText = null;
        private void button1_Click(object sender, EventArgs e)
        {
            Button xd = button1;

            if (xd.Text != "stop")
            {

                if (buttonInitText == null)
                {
                    buttonInitText = xd.Text;
                }

                if (!checkBox1.Checked)
                {
                    return;
                }

                xd.Text = "stop";
                timer1.Start();

                checkBox1.Checked = false;
            }
            else
            {
                xd.Text = buttonInitText;
                timer1.Dispose();
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

