using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImgSplice {
    class FontImage {
        public static Bitmap generate(Bitmap src,string str) {
            src = ImgPreprocessor.Resize(src);
            Bitmap res = new Bitmap(src.Width * Configue.FONT_WIDTH, src.Height * Configue.FONT_WIDTH);
            Graphics g = Graphics.FromImage(res);
            g.Clear(Color.Black);
            Font font = new Font("微软雅黑", Configue.FONT_WIDTH*3/4);
            BitmapData bitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int index=0,len=str.Length;
            unsafe {
                byte* ptr = (byte*)bitmapData.Scan0;
                for (int j = 0; j < bitmapData.Height; j++) {
                    for (int i = 0; i < bitmapData.Width; i++) {
                        Color color = Color.FromArgb(ptr[2], ptr[1], ptr[0]);
                        Brush brush = new SolidBrush(color);
                        g.DrawString(str[index].ToString(), font, brush, i * Configue.FONT_WIDTH, j * Configue.FONT_WIDTH);
                        index = (index + 1) % len;
                        ptr += 3;
                    }
                    ptr += bitmapData.Stride - (bitmapData.Width * 3);
                    String log = String.Format("Line:{0} Finish", j);
                    Console.WriteLine(log);
                }
            }
            return res;
        }

    }
}
