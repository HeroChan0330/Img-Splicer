using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ImgSplice {
    class ImgPreprocessor {
        
       public static void process(){
           DirectoryInfo dirInfo = new DirectoryInfo(Configue.RAW_DIRECTORY);
            FileInfo[] files=dirInfo.GetFiles();
            foreach (FileInfo fileInfo in files) {
                Bitmap src=(Bitmap)Image.FromFile(fileInfo.FullName);
                Bitmap dest = new Bitmap(Configue.SLICE_WIDTH, Configue.SLICE_HEIGHT);
                Graphics g = Graphics.FromImage(dest);
                Rectangle srcRect = getSrcScaleRect(src);
                g.DrawImage(src, new Rectangle(0, 0, Configue.SLICE_WIDTH, Configue.SLICE_HEIGHT),
                            srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel);

                UInt32 color = getAverageColor(dest);
                String destName = String.Format("{0}/{1}.{2}", Configue.SRC_DIRECTORY, color, "jpg");
                dest.Save(destName);
                Console.WriteLine(String.Format("File:{0}\nprocess finished output {1}", fileInfo.FullName, destName));
            }
        }
        static Rectangle getSrcScaleRect(Bitmap src) {
            double destK = (double)Configue.SLICE_WIDTH / (double)Configue.SLICE_HEIGHT;
            double srcK=(double)src.Width / (double)src.Height;
            if (srcK > destK) { //横向比例较大 
                double scale = (double)Configue.SLICE_HEIGHT / (double)src.Height;
                int width = (int)((double)Configue.SLICE_WIDTH/scale);
                //Console.WriteLine(width);
                Rectangle res = new Rectangle((src.Width - width) / 2, 0, width, src.Height);
                return res;
            }
            else {
                double scale = (double)Configue.SLICE_WIDTH / (double)src.Width ;
                int height = (int)((double)Configue.SLICE_HEIGHT/ scale);
                //Console.WriteLine(String.Format("width {0} height {1} scale {2}", src.Width, height, scale));
                Rectangle res = new Rectangle(0, (src.Height-height)/2, src.Width,height);
                return res;
            }
        }
        static UInt32 getAverageColor(Bitmap bitmap) {
            UInt32 rSum = 0,gSum=0,bSum=0;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe {
                byte* ptr = (byte*)bitmapData.Scan0;
                for (int j = 0; j < bitmapData.Height; j++) {
                    for (int i = 0; i < bitmapData.Width; i++) {
                        rSum += ptr[2];
                        gSum += ptr[1];
                        bSum += ptr[0];
                        ptr += 3;
                    }
                    ptr += bitmapData.Stride - (bitmapData.Width * 3);
                }
            }
            UInt32 pixelSum = (UInt32)(bitmapData.Width * bitmapData.Height);
            UInt32 rAverage = rSum / pixelSum,
                   gAverage = gSum / pixelSum,
                   bAverage = bSum / pixelSum;
            rAverage = (rAverage >> 3);
            gAverage = (gAverage >> 3);
            bAverage = (bAverage >> 3);
            return rAverage << 10 | gAverage << 5 | bAverage;
        }
        public static Bitmap Resize(Bitmap src) {
            double scale = (double)Configue.SRC_MAX_WIDTH / (double)src.Width;
            if (src.Height * scale > Configue.SLICE_HEIGHT) {
                scale = (double)Configue.SRC_MAX_HEIGHT / (double)src.Height;
            }
            Bitmap res = new Bitmap((int)(src.Width * scale), (int)(src.Height * scale));
            Graphics g = Graphics.FromImage(res);
            g.DrawImage(src, new Rectangle(0, 0, res.Width, res.Height), 0, 0, src.Width, src.Height, GraphicsUnit.Pixel);
            return res;
        }
    }
}
