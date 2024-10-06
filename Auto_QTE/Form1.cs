using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenCvSharp.Internal;
using WindowsInput;


namespace Auto_QTE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
            image_ocr.init();
        }

        bool start_flag = false;
        string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
        string image_folderName = "temp";
        string image_name = "window_screenshot.png";

        [DllImport("Cuser32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private void start_btn_Click(object sender, EventArgs e)
        {
            if (start_btn.Text == "�}�l")
            {
                start_btn.Text = "�B�椤...";
                start_btn.BackColor = Color.Green;
                start_flag = true;
                bw_run.RunWorkerAsync();  // �Ұʫ�x�u�@
            }
            else 
            {
                start_btn.Text = "�}�l";
                start_btn.BackColor = Color.Gray;
                start_flag = false;
                bw_run.CancelAsync(); // ���ը�������
            }
        }

        public void init()
        {
            string folderPath = Path.Combine(projectRoot, image_folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            start_btn.Text = "�}�l";
            start_btn.BackColor = Color.Gray;
            start_btn.Font = new Font(start_btn.Font, FontStyle.Bold);
            start_btn.ForeColor = Color.Black;

            bw_run.WorkerSupportsCancellation = true;
        }
        public void run_process()
        {
            string temp_str = "";

            while (start_flag)
            {
                string image_path = projectRoot + image_folderName + "\\" + image_name;

                //���եε{���X
                //screen_class.game_screen();
                //screen_class.CaptureWindow(image_path);
                //screen_class.NativeMethods.total_name();
                //image_ocr.ocr_process(image_path);

                //screen_class.CaptureWindow(image_path);
                string result = "0";
                //result = image_ocr.emgu_cv_process(image_path);
                //reverse_and_push(result);

                keyborad_input.push_btn(0x43);

                #region Btn UI�ʵe
                if (start_btn.Text == "�B�椤...") { temp_str = "�B�椤.."; }
                else { temp_str = "�B�椤..."; }

                if (start_btn.InvokeRequired)
                {
                    // �p�G�ݭn�b UI ������W����A�h�ϥ� Invoke
                    start_btn.Invoke(new Action(() => start_btn.Text = temp_str));
                }
                else
                {
                    // �p�G�w�g�b UI ������W�A������s
                    start_btn.Text = temp_str;
                }

                //list_text operation
                if (result != "0" || list_text.Items.Count>20)
                {
                    if (list_text.InvokeRequired || result != "0")
                    {
                        // �p�G�ݭn�b UI ������W����A�h�ϥ� Invoke
                        if (list_text.Items.Count > 20) { list_text.Invoke(new Action(() => list_text.Items.Clear())); }
                        list_text.Invoke(new Action(() => list_text.Items.Add(DateTime.Now.ToString("HH:mm") + "�w���ѡG" + result)));
                    }
                    else
                    {
                        list_text.Items.Clear();
                        // �p�G�w�g�b UI ������W�A������s
                        if (result != "0") { list_text.Items.Add(DateTime.Now.ToString("HH:mm") + "�w���ѡG" + result); }
                    }
                }

                #endregion

                Thread.Sleep(1000);
            }

        }


        public void reverse_and_push(string result)
        {
            string reversed = ReverseString(result);

            int temp = Convert.ToInt32(reversed);
            while (temp > 0)
            {
                push_num_button(temp % 10);
                temp = temp / 10;
            }
        }

        public static string ReverseString(string str)
        {
            char[] reversed = new char[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                reversed[i] = str[str.Length - 1 - i];
            }
            return new string(reversed);
        }

        public static void push_num_button(int input)
        {
            //SendKeys.SendWait(input.ToString());
            uint[] temp = { (uint)Keys.NumPad1, (uint)Keys.NumPad2, (uint)Keys.NumPad3, (uint)Keys.NumPad4 };
            moniter_input.SendKeys(new ushort[] { Convert.ToUInt16(temp[input-1]) });
            Thread.Sleep(100);
        }


        public static void test_btn()
        {
            uint[] temp = { (uint)Keys.NumPad1, (uint)Keys.NumPad2, (uint)Keys.NumPad3, (uint)Keys.NumPad4 };
            moniter_input.SendKeys(new ushort[] { Convert.ToUInt16(temp[2]) });
            //moniter_input.SendKeys(new ushort[] { 0x31 });
            //SendKeys.SendWait("{i}");
            //var sim = new InputSimulator();
            //sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            //keyborad_input.PressAKeyBySendMessage();
            Thread.Sleep(500);
        }

        private void bw_run_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            run_process();
        }
    }
}
