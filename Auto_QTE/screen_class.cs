using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenCvSharp.Internal;
using System.Text;
using System.Diagnostics;

namespace Auto_QTE
{
    internal class screen_class
    {
        public static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

            [DllImport("user32.dll")]
            public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

            [DllImport("user32.dll")]
            public static extern Boolean GetWindowRect(IntPtr hWnd, ref Rectangle bounds);

            #region 捕捉程式名稱
            private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll")]
            private static extern int GetWindowTextLength(IntPtr hWnd);

            private static bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
            {
                // 獲取窗口標題長度
                int length = GetWindowTextLength(hWnd);
                if (length > 0)
                {
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    Console.WriteLine($"窗口句柄: {hWnd}, 標題: {sb}");
                }
                return true; // 繼續枚舉
            }
            public static void total_name()
            {
                // 開始枚舉所有窗口
                EnumWindows(EnumWindowsCallback, IntPtr.Zero);
            }
            #endregion


            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }
        }

        public static void game_screen()
        {
            try 
            {
                int SW_RESTORE = 9;  // 從最小化狀態恢復窗口

                // 替換為你要截圖的視窗名稱或類名，例如 Notepad 的視窗標題為 "Untitled - Notepad"
                //string windowTitle = "MapleStory";
                string windowTitle = "Astral Tale              ";

                // 找到指定視窗的句柄
                IntPtr hWnd = NativeMethods.FindWindow(null, windowTitle);
                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("找不到視窗：" + windowTitle);
                    return;
                }
                else
                {
                    NativeMethods.ShowWindow(hWnd, SW_RESTORE);
                    NativeMethods.SetForegroundWindow(hWnd);
                }

                // 取得視窗的矩形區域
                NativeMethods.RECT rect;
                if (!NativeMethods.GetWindowRect(hWnd, out rect))
                {
                    Console.WriteLine("無法取得視窗矩形");
                    return;
                }

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                // 創建一個位圖來保存視窗畫面
                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 取得視窗的設備上下文
                    IntPtr hdcBitmap = graphics.GetHdc();

                    // 使用 PrintWindow 將視窗畫面繪製到位圖
                    NativeMethods.PrintWindow(hWnd, hdcBitmap, 0);

                    // 釋放設備上下文
                    graphics.ReleaseHdc(hdcBitmap);
                }

                // 保存位圖為圖片檔案
                string image_folderName = AppDomain.CurrentDomain.BaseDirectory + "temp";
                string filePath = image_folderName + "\\window_screenshot.png";
                bitmap.Save(filePath, ImageFormat.Png);
            }
            catch (Exception e){ }

        }

        public static void CaptureWindow(string output_path)
        {
            int SW_RESTORE = 9;  // 從最小化狀態恢復窗口

            //string windowTitle = "MapleStory";
            string windowTitle = "Astral Tale              ";
            Process[] process = Process.GetProcessesByName("game.bin");

            try 
            {
                // 找到指定視窗的句柄
                IntPtr hWnd = NativeMethods.FindWindow(null, windowTitle);
                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("找不到視窗：" + windowTitle);
                    return;
                }
                else
                {
                    NativeMethods.ShowWindow(hWnd, SW_RESTORE);
                    NativeMethods.SetForegroundWindow(hWnd);
                }

                // 取得視窗的矩形區域
                NativeMethods.RECT rect;
                if (!NativeMethods.GetWindowRect(hWnd, out rect))
                {
                    Console.WriteLine("無法取得視窗矩形");
                    return;
                }

                /* 取得該視窗的大小與位置 */
                Rectangle bounds = new Rectangle();
                NativeMethods.GetWindowRect(process[0].MainWindowHandle, ref bounds);

                /* 抓取截圖 */
                Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
                Graphics gfx = Graphics.FromImage(screenshot);
                gfx.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);

                // 保存位圖為圖片檔案
                screenshot.Save(output_path, ImageFormat.Png);
            }
            catch (Exception e){ }

        }


    }
}
