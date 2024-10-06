using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auto_QTE
{
    internal class keyborad_input
    {
        // 宣告 Input 類型
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        const int INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_KEYUP = 0x0002;

        public static void push_btn(ushort keyCode)
        {
            // 模擬按下 W 鍵
            SendKey(0x43, false);  // 0x57 是 W 鍵的虛擬鍵碼
            Thread.Sleep(50);     // 模擬按住 1 秒
            SendKey(0x43, true);   // 釋放 W 鍵
        }

        static void SendKey(ushort keyCode, bool keyUp)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = keyCode; // 虛擬鍵碼
            inputs[0].u.ki.wScan = keyCode; // 硬件掃描碼，0 表示忽略
            inputs[0].u.ki.dwFlags = keyUp ? KEYEVENTF_KEYUP : 0; // 設置為 0 表示按下，KEYEVENTF_KEYUP 表示釋放
            inputs[0].u.ki.time = 0; // 使用系統自動生成的時間
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero; // 附加訊息

            // 發送模擬的鍵盤輸入
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

    }
}
