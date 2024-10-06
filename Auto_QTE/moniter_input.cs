using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auto_QTE
{
    class moniter_input
    {
        // 定義 INPUT 結構
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);




        private const uint MAPVK_VK_TO_CHAR = 2;
        private const uint MAPVK_VK_TO_VSC = 0;
        private const uint MAPVK_VSC_TO_VK = 1;
        private const uint MAPVK_VSC_TO_VK_EX = 3;


        const int INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_KEYDOWN = 0x0000;
        const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public static void SendKeys(ushort[] keys)
        {

            string windowTitle = "Astral Tale              ";


            INPUT[] inputs = new INPUT[keys.Length * 2]; // 每個鍵有按下和釋放兩個事件

            for (int i = 0; i < keys.Length; i++)
            {
                // 鍵按下
                inputs[i * 2] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = keys[i],
                            wScan = (ushort)MapVirtualKey(keys[i], 0),
                            //wScan = 0,
                            dwFlags = KEYEVENTF_KEYDOWN,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                // 鍵釋放
                inputs[i * 2 + 1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = keys[i],
                            //wScan = 0,
                            wScan = (ushort)MapVirtualKey(keys[i], 0),
                            dwFlags = KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }

            // 發送輸入
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendKeys_test(ushort[] keys)
        {
            // 模擬按下 A 鍵
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = keys[0];
            inputs[0].u.ki.wScan = 0;
            inputs[0].u.ki.dwFlags = 0;
            inputs[0].u.ki.time = 0;
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;

            // 發送按鍵輸入
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));

            // 模擬釋放 A 鍵
            inputs[0].u.ki.dwFlags = KEYEVENTF_KEYUP;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }



    }
}
