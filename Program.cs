using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImgSplice {
    class Program {
        static void Main(string[] args) {
            //运行这一段 预处理样本图片
            ImgPreprocessor.process();

            //运行这一段 生成拼图
            Bitmap src = (Bitmap)Image.FromFile("12.jpg");
            Bitmap res = Combiner.Combine(src);
            res.Save("res.jpg");

            //运行这一段 生成字体拼图
            //Bitmap src = (Bitmap)Image.FromFile("1.jpg");
            //Bitmap res = FontImage.generate(src, "我爱你");
            //res.Save("res2.jpg");
        }
    }
}
