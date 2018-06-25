using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgSplice {
    class Configue {
        public static string RAW_DIRECTORY = "raw/";
        public static string SRC_DIRECTORY = "img/";
        public static int SLICE_WIDTH =50;
        public static int SLICE_HEIGHT = 50;

        public static int SRC_MAX_WIDTH =320;
        public static int SRC_MAX_HEIGHT = 180;

        public static String DEFAULT_IMG = "Default.jpg";

        public static int TOLERANCE_RANGE = 32;//RGB各通道容差范围 超出就使用default

        public static int FONT_WIDTH = 30;
    }
}
