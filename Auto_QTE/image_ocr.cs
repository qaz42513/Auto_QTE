using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using Tesseract;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Reg;


namespace Auto_QTE
{
    internal class image_ocr
    {
        public static Rectangle[,] show_case = new Rectangle[2, 7];
        public static int y_height = 44;
        public static int x_weight = 37;

        public static void ocr_process(string image_path)
        {
            // 載入圖片
            OpenCvSharp.Mat image = Cv2.ImRead(image_path);

            // 定義裁剪區域 (x, y, width, height)
            OpenCvSharp.Rect roi = new OpenCvSharp.Rect(650, 635, 630, y_height); // 根據需求調整這些值

            // 裁剪出指定區域
            OpenCvSharp.Mat croppedImage = new OpenCvSharp.Mat(image, roi);

            // 將圖片轉換為灰階
            OpenCvSharp.Mat grayImage = new OpenCvSharp.Mat();
            Cv2.CvtColor(croppedImage, grayImage, ColorConversionCodes.BGR2GRAY);

            // 二值化圖片
            OpenCvSharp.Mat threshImage = new OpenCvSharp.Mat();
            Cv2.Threshold(grayImage, threshImage, 128, 255, ThresholdTypes.Binary);

            // 顯示處理後的圖片（可選）
            //Cv2.ImShow("Processed Image", threshImage);
            //Cv2.WaitKey(5000);

            // 將圖片儲存為臨時檔案，以供 Tesseract 使用
            string tempImagePath = Path.GetDirectoryName(image_path) + "\\temp_process_image.png";
            Cv2.ImWrite(tempImagePath, threshImage);

            // 使用 Tesseract 辨識數字
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                // 只識別數字
                engine.SetVariable("tessedit_char_whitelist", "0123456789");

                // 從裁剪的區域進行 OCR
                string test = Path.GetDirectoryName(image_path) + "\\test.png";
                using (var img = Pix.LoadFromFile(test))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("辨識出的數字: " + text);
                    }
                }
            }

            // 清理
            Cv2.DestroyAllWindows();
        }

        public static string emgu_cv_process(string image_path)
        {
            // 讀取漸層色彩圖片
            string imagePath = @image_path;
            var image = new Image<Bgr, byte>(imagePath);

            //定義裁切區域（Rectangle）
            Rectangle cropArea = new Rectangle(655, 675, 650, y_height);

            //裁切圖像
            var croppedImage = new Image<Bgr, byte>(cropArea.Size);  // 創建一個新的圖像以容納裁切的區域
            croppedImage = image.GetSubRect(cropArea);  // 從原圖中裁切指定區域


            //將圖片轉換為灰階
            var grayImage = croppedImage.Convert<Gray, byte>();

            //高斯模糊化
            var blurredImage = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
            CvInvoke.GaussianBlur(grayImage, blurredImage, new System.Drawing.Size(5, 5), 1.5);

            //自適應門檻化處理（適合處理光照不均、漸層色彩背景的情況）
            var binaryImage = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
            //CvInvoke.AdaptiveThreshold(grayImage, binaryImage, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.GaussianC, Emgu.CV.CvEnum.ThresholdType.Binary, 15, 10);
            CvInvoke.AdaptiveThreshold(blurredImage, binaryImage, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.GaussianC, Emgu.CV.CvEnum.ThresholdType.Binary, 13, 10);

            //移除小噪點
            var processedImage = binaryImage.SmoothGaussian(3);


            //進行黑白反轉操作
            //var invertedImage = new Image<Gray, byte>(grayImage.Size);  // 創建一個新的灰階圖像
            //CvInvoke.BitwiseNot(processedImage, invertedImage);  // 反轉圖像

            // 保存處理後的圖片，便於檢查
            string tempImagePath = Path.GetDirectoryName(image_path) + "\\";

            string output = "";
            for (int i = 0; i < 7; i++)
            {
                cropArea = show_case[0, i];
                var temp_image = processedImage.GetSubRect(cropArea);
                string path = tempImagePath + "tmp" + i.ToString() + ".png";
                temp_image.Save(@path);

                // 使用 Tesseract 進行 OCR
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    // 只識別數字
                    engine.SetVariable("tessedit_char_whitelist", "0123456789");

                    try 
                    {
                        using (var img = Pix.LoadFromFile(@path))
                        {
                            // 5. 執行 OCR 辨識
                            using (var page = engine.Process(img, PageSegMode.SingleWord))
                            {
                                // 6. 輸出識別結果
                                string text = page.GetText().Trim();

                                if (text == "8") { text = "3"; }
                                if (Convert.ToInt32(text) >4 ) { text = ""; }

                                output = output + text;
                            }
                        }
                    }
                    catch (Exception e){ }

                    if (i == 2 && output != "") { break; }

                }
            }

            tempImagePath = tempImagePath + "total.png";
            processedImage.Save(@tempImagePath);

            if (output == "") { output = "0"; }
            return output;

        }

        public static void init()
        {
            show_case[0, 0] = new Rectangle(102, 0, x_weight, y_height);
            show_case[0, 1] = new Rectangle(102+198, 0, x_weight, y_height);
            show_case[0, 2] = new Rectangle(102 + 198*2, 0, x_weight, y_height);
            show_case[0, 3] = new Rectangle(0, 0, x_weight, y_height);
            show_case[0, 4] = new Rectangle(198, 0, x_weight, y_height);
            show_case[0, 5] = new Rectangle(198 * 2, 0, x_weight, y_height);
            show_case[0, 6] = new Rectangle(198 * 3, 0, x_weight, y_height);

        }
    }
}
