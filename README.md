# Img-Splicer
用大量图片，生成一张拼图，使每个像素点颜色和代表改像素点的图片元素平均颜色相近。和用多种颜色字体拼成一张拼图
### 服用方法:  
首先修改BaiduImage.py里面的keyWord（爬取关键词图片，可以找女神什么的）,执行py  
$ python BaiduImage.py  
然后C#工程取消注释  
    //运行这一段 预处理样本图片  
    ImgPreprocessor.process();  
    //这一段相同的样本只需要执行一次，避免浪费时间  
    //运行这一段 生成拼图  
    Bitmap src = (Bitmap)Image.FromFile("12.jpg");  
    Bitmap res = Combiner.Combine(src);  
    res.Save("res.jpg");  
编译运行  

另外用字体生成拼图  
只需要把上面的都注释，取消注释  
    //运行这一段 生成字体拼图  
    //Bitmap src = (Bitmap)Image.FromFile("1.jpg");  
    //Bitmap res = FontImage.generate(src, "我爱你");  
    //res.Save("res2.jpg");  
编译运行

### 示例:  
![Preview1](https://raw.githubusercontent.com/HeroChan0330/Img-Splicer/master/Sample/Preview1.jpg)
![Detail1](https://raw.githubusercontent.com/HeroChan0330/Img-Splicer/master/Sample/Detail1.jpg)
![Preview2](https://raw.githubusercontent.com/HeroChan0330/Img-Splicer/master/Sample/Preview2.jpg)
![Detail2](https://raw.githubusercontent.com/HeroChan0330/Img-Splicer/master/Sample/Detail2.jpg)

### 潜在Bug:
Bitmap类的Size有限制，要手动调整Configue.cs里面的参数  
    public static int SLICE_WIDTH =50;  
    public static int SLICE_HEIGHT = 50;  
    public static int SRC_MAX_WIDTH =320;  
    public static int SRC_MAX_HEIGHT = 180;  
SRC_MAX_WIDTH SRC_MAX_HEIGHT会自动缩放目标图片至对应scale的图片，SLICE_WIDTH SLICE_HEIGHT为单个贴图Size
