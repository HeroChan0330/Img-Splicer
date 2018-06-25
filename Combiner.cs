using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace ImgSplice {
    class Combiner {
        public static Bitmap Combine(Bitmap src) {
            src = ImgPreprocessor.Resize(src);
            Bitmap defaultImage = (Bitmap)Image.FromFile(Configue.DEFAULT_IMG);

            BitmapData bitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            Bitmap res = new Bitmap(src.Width * Configue.SLICE_WIDTH, src.Height * Configue.SLICE_HEIGHT);
            
            Graphics g = Graphics.FromImage(res);
            unsafe {
                byte*ptr=(byte*)bitmapData.Scan0;
                for (int j = 0; j < src.Height; j++) {
                    for (int i = 0; i < src.Width; i++) {
                        Bitmap slice = getSimiliarColorImg(ptr[2], ptr[1], ptr[0]);
                        if (slice == null) slice = defaultImage;
                        g.DrawImage(slice, i * Configue.SLICE_WIDTH, j * Configue.SLICE_HEIGHT);
                        ptr += 3;
                    }
                    String log = String.Format("Line:{0} Finish",j);
                    Console.WriteLine(log);
                    ptr += bitmapData.Stride - (bitmapData.Width * 3);
                }
            }
            return res;
            
        }

        static int rgbLimit(int component) {
            if (component < 0) return 0;
            if (component > 255) return 255;
            return component;
        }
        static Bitmap getSimiliarColorImg(int originR, int originG, int originB) {//小容差 算的慢 不采取
            //UInt32 r, g, b;
            //for (r = rgbLimit(originR - Configue.TOLERANCE_RANGE) << 16; r < rgbLimit(originR + Configue.TOLERANCE_RANGE) << 16; r += 0x10000) {
            //    for (g = rgbLimit(originG - Configue.TOLERANCE_RANGE) << 16; g < rgbLimit(originG + Configue.TOLERANCE_RANGE) << 16; g += 0x100) {
            //        for (b = rgbLimit(originB - Configue.TOLERANCE_RANGE) << 16; b < rgbLimit(originR + Configue.TOLERANCE_RANGE) << 16; b += 0x1) {
            //            UInt32 color = r << 16 | g << 8 | b;
            //            //if(File.Exists(color.ToString)
            //        }
            //    }
            //}
            int[] offset = { 0, 0, 0 };
            for (offset[0] = 0; offset[0] < Configue.TOLERANCE_RANGE; offset[0]+=8) {
                for (offset[1] = 0; offset[1] < Configue.TOLERANCE_RANGE; offset[1]+=8) {
                    for (offset[2] = 0; offset[2] < Configue.TOLERANCE_RANGE; offset[2]+=8) {
                        for (int rMult = -1; rMult < 1; rMult += 2) {
                            for (int gMult = -1; gMult < 1; gMult += 2) {
                                for (int bMult = -1; bMult < 1; bMult += 2) {
                                    int r = (int)rgbLimit(offset[0] * rMult + originR);
                                    int g = (int)rgbLimit(offset[1] * gMult + originG);
                                    int b = (int)rgbLimit(offset[2] * bMult + originB);
                                    //uint color = r << 16 | g << 8 | b;
                                    //String fileName = Configue.SRC_DIRECTORY + color.ToString() + ".jpg";
                                    ////Console.WriteLine(fileName);
                                    //if(File.Exists(fileName)){
                                    //    return (Bitmap)Image.FromFile(fileName);
                                    //}
                                    if (checkImgExist(r, g, b)) {
                                        return imgFromColor18(r, g, b);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        static bool checkImgExist(int r, int g, int b) {
            r >>= 3;
            g >>= 3;
            b >>= 3;
            uint color = (uint)(r << 10 | g << 5 | b);
            if (File.Exists(Configue.SRC_DIRECTORY+color.ToString() + ".jpg")) {
                return true;
            }
            return false;
        }
        static Bitmap imgFromColor18(int r, int g, int b) {
            r >>= 3;
            g >>= 3;
            b >>= 3;
            uint color = (uint)(r << 10 | g << 5 | b);
            return (Bitmap)Image.FromFile(Configue.SRC_DIRECTORY + color.ToString() + ".jpg");
        }
        //这个命中率低 没什么卵用
        //static Bitmap getSimiliarColorImg2(int originR, int originG, int originB) {
        //    for (int offset = 0; offset < Configue.TOLERANCE_RANGE; offset+=8) {
        //        int r1 = rgbLimit(originR - offset), r2 = rgbLimit(originR + offset),
        //            g1 = rgbLimit(originG - offset), g2 = rgbLimit(originG + offset),
        //            b1 = rgbLimit(originB - offset), b2 = rgbLimit(originB + offset);
        //        if (checkImgExist(r1, originG, originB)) return imgFromColor18(r1, originG, originB);
        //        if (checkImgExist(r2, originG, originB)) return imgFromColor18(r2, originG, originB);
        //        if (checkImgExist(originR, g1, originB)) return imgFromColor18(originR, g1, originB);
        //        if (checkImgExist(originR, g2, originB)) return imgFromColor18(originR, g2, originB);
        //        if (checkImgExist(originR, originG, b1)) return imgFromColor18(originR, originG, b1);
        //        if (checkImgExist(originR, originG, b2)) return imgFromColor18(originR, originG, b2);

        //        if (checkImgExist(r1, g1, b1)) return imgFromColor18(r1, g1, b1);
        //        if (checkImgExist(r2, g2, b2)) return imgFromColor18(r2, g2, b2);
        //    }
        //    return null;
        //}

    }
}
