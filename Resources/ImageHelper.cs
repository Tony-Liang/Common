using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace LCW.Framework.Common.Resources
{
    /// <summary>
    /// 图片大小的存储基本单位是字节（byte）。
    /// 每个字节是由8个比特（bit）组成，1字节（Byte）= 8位（bit）。 
    /// 所以，一个字节在十进制中的范围是[0~255],即256个数。
    /// 指针的准确定位
    ///32位RGB：假设X、Y为位图中像素的坐标，则其在内存中的地址为scan0+Y*stride+X*4。此时指针指向蓝色，其后分别是绿色、红色，alpha分量。
    ///24位RGB：scan0+Y*stride+X*3。此时指针指向蓝色，其后分别是绿色和红色。
    ///8位索引：scan0+Y*stride+X。当前指针指向图像的调色盘。
    ///4位索引：scan0+Y*stride+（X/2）。当前指针所指的字节包括两个像素，通过高位和低位索引16色调色盘，其中高位表示左边的像素，低位表示右边的像素。
    ///1位索引：scan0+Y*stride+X/8。当前指针所指的字节中的每一位都表示一个像素的索引颜色，调色盘为两色，最左边的像素为8，最右边的像素为0。
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 积木效果
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image BlockEffect(Image image)
        {
            Image imageclone = image.Clone() as Image;
            Bitmap map = new Bitmap(imageclone);
            int width = map.Width;
            int height = map.Height;
            Color color = new Color();
            int avg = 0,iPixel=0;
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color=map.GetPixel(i, j);
                    avg = (color.R + color.G + color.B) / 3;
                    if (avg >= 128)
                        iPixel = 255;
                    else
                        iPixel = 0;
                    color = Color.FromArgb(255, iPixel, iPixel, iPixel);
                    map.SetPixel(i, j, color);
                }
            }
            return map;
        }
        /// <summary>
        /// 底片效果
        /// 原理: GetPixel方法获得每一点像素的值, 然后再使用SetPixel方法将取反后的颜色值设置到对应的点.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image NegativeEffect(Image image)
        {
            Image imageclone = image.Clone() as Image;
            Bitmap map = new Bitmap(imageclone);
            int width = map.Width;
            int height = map.Height;
            Color color = new Color();
            int red=0,green=0,blue=0;
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = map.GetPixel(i, j);
                    red =255- color.R;
                    green =255- color.G;
                    blue =255- color.B;
                    color = Color.FromArgb(red,green,blue);
                    map.SetPixel(i, j, color);
                }
            }
            return map;
        }

        /// <summary>
        /// 浮雕效果
        /// 原理: 对图像像素点的像素值分别与相邻像素点的像素值相减后加上128, 然后将其作为新的像素点的值
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image RelievoEffect(Image image)
        {
            Bitmap oldmap = new Bitmap(image);
            Bitmap map = new Bitmap(image.Width,image.Height);
            int width = map.Width;
            int height = map.Height;
            Color piexl1 = new Color();
            Color piexl2 = new Color();
            int red = 0, green = 0, blue = 0;
            try
            {
                for (int i = 0; i < width - 1; i++)
                {
                    for (int j = 0; j < height-1; j++)
                    {
                        piexl1 = oldmap.GetPixel(i, j);
                        piexl2 = oldmap.GetPixel(i + 1, j + 1);

                        red = Math.Abs(piexl1.R - piexl2.R + 128);
                        green = Math.Abs(piexl1.G - piexl2.G + 128);
                        blue = Math.Abs(piexl1.B - piexl2.B + 128);
                        if (red > 255)
                            red = 255;
                        if (red < 0)
                            red = 0;

                        if (green > 255)
                            green = 255;
                        if (green < 0)
                            green = 0;

                        if (blue > 255)
                            blue = 255;
                        if (blue < 0)
                            blue = 0;
                        map.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return map;
        }
        public unsafe static Image RelievoEffect_Unsafe(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            int rr = 0, gg = 0, bb = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    bb = (ptr[0] - ptr[4] + 128);//B
                    gg = (ptr[1] - ptr[5] + 128);//G
                    rr = (ptr[2] - ptr[6] + 128); ;//R

                    //处理溢出
                    if (rr > 255) rr = 255;
                    if (rr < 0) rr = 0;
                    if (gg > 255) gg = 255;
                    if (gg < 0) gg = 0;
                    if (bb > 255) bb = 255;
                    if (bb < 0) bb = 0;

                    ptr[0] = (byte)bb;
                    ptr[1] = (byte)gg;
                    ptr[2] = (byte)rr;

                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        /// <summary>
        /// 黑白效果
        /// 原理: 彩色图像处理成黑白效果通常有3种算法；
        ///(1).最大值法: 使每个像素点的 R, G, B 值等于原像素点的 RGB (颜色值) 中最大的一个；
        ///(2).平均值法: 使用每个像素点的 R,G,B值等于原像素点的RGB值的平均值；
        ///(3).加权平均值法: 对每个像素点的 R, G, B值进行加权
        ///---自认为第三种方法做出来的黑白效果图像最 "真实
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image MonochromeEffect(Image image)
        {
            Bitmap oldmap = new Bitmap(image);
            Bitmap map = new Bitmap(image.Width,image.Height);
            int width = map.Width;
            int height = map.Height;
            Color color = new Color();
            int red = 0, green = 0, blue = 0,Result=0;
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = oldmap.GetPixel(i, j);
                    red = color.R;
                    green = color.G;
                    blue = color.B;
                    //实例程序以加权平均值法产生黑白图像
                    int iType =2;
                    switch (iType)
                    {
                        case 0://平均值法
                            Result = ((red + green + blue) / 3);
                            break;
                        case 1://最大值法
                            Result = red > green ? red : green;
                            Result = Result > blue ? Result : blue;
                            break;
                        case 2://加权平均值法
                            Result = ((int)(0.7 * red) + (int)(0.2 * green) + (int)(0.1 * blue));
                            break;
                    }
                    map.SetPixel(i, j, Color.FromArgb(Result, Result, Result));
                }
            }
            return map;
        }
        /// <summary>
        /// 柔化效果
        /// 原理: 当前像素点与周围像素点的颜色差距较大时取其平均值.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image SoftenEffect(Image image)
        {
            Bitmap oldmap = new Bitmap(image);
            Bitmap map = new Bitmap(image.Width, image.Height);
            int width = map.Width;
            int height = map.Height;
            Color pixel;
                //高斯模板
            try
            {
                int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
                for (int x = 1; x < width - 1; x++)
                    for (int y = 1; y < height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        int Index = 0;
                        for (int col = -1; col <= 1; col++)
                            for (int row = -1; row <= 1; row++)
                            {
                                pixel = oldmap.GetPixel(x + row, y + col);
                                r += pixel.R * Gauss[Index];
                                g += pixel.G * Gauss[Index];
                                b += pixel.B * Gauss[Index];
                                Index++;
                            }
                        r /= 16;
                        g /= 16;
                        b /= 16;
                        //处理颜色值溢出
                        r = r > 255 ? 255 : r;
                        r = r < 0 ? 0 : r;
                        g = g > 255 ? 255 : g;
                        g = g < 0 ? 0 : g;
                        b = b > 255 ? 255 : b;
                        b = b < 0 ? 0 : b;
                        map.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return map;
        }
        /// <summary>
        /// 锐化效果
        /// 原理:突出显示颜色值大(即形成形体边缘)的像素点.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image SharpenEffect(Image image)
        {
            Bitmap oldmap = new Bitmap(image);
            Bitmap map = new Bitmap(image.Width, image.Height);
            int width = map.Width;
            int height = map.Height;
            Color pixel = new Color();
            try
            {
                //拉普拉斯模板
                int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
                for (int x = 1; x < width - 1; x++)
                    for (int y = 1; y < height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        int Index = 0;
                        for (int col = -1; col <= 1; col++)
                            for (int row = -1; row <= 1; row++)
                            {
                                pixel = oldmap.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
                                g += pixel.G * Laplacian[Index];
                                b += pixel.B * Laplacian[Index];
                                Index++;
                            }
                        //处理颜色值溢出
                        r = r > 255 ? 255 : r;
                        r = r < 0 ? 0 : r;
                        g = g > 255 ? 255 : g;
                        g = g < 0 ? 0 : g;
                        b = b > 255 ? 255 : b;
                        b = b < 0 ? 0 : b;
                        map.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return map;
        }
        /// <summary>
        /// 雾化效果
        /// 原理: 在图像中引入一定的随机值, 打乱图像中的像素值
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image AtomizationEffect(Image image)
        {
            Bitmap oldmap = new Bitmap(image);
            Bitmap map = new Bitmap(image.Width, image.Height);
            int width = map.Width;
            int height = map.Height;
            Color pixel = new Color();
            try
            {
                for (int x = 1; x < width - 1; x++)
                    for (int y = 1; y < height - 1; y++)
                    {
                        System.Random MyRandom = new Random();
                        int k = MyRandom.Next(123456);
                        //像素块大小
                        int dx = x + k % 19;
                        int dy = y + k % 19;
                        if (dx >= width)
                            dx = width - 1;
                        if (dy >= height)
                            dy = height - 1;
                        if (dx < 0)
                            dx = 0;
                        if (dy < 0)
                            dy = 0;
                        pixel = oldmap.GetPixel(dx, dy);
                        map.SetPixel(x, y, pixel);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return map;
        }
        public unsafe static Image AtomizationEffect_unsafe(Image image)
        {
            Bitmap map = new Bitmap(image);
            int width = map.Width;
            int height = map.Height;
            int N = 19;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpdata = map.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte* ptr=(byte*)(bmpdata.Scan0);
            Random rnd = new Random();
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k = rnd.Next(-12345,12345);
                    //像素块大小 常量N的大小决定雾化模糊度
                    int dj = j + k % N;//水平向右方向像素偏移后
                    int di = i + k % N;//垂直向下方向像素偏移后
                    if (dj >= width) dj = width - 1;
                    if (di >= height) di = height - 1;
                    if (di < 0)
                        di = 0;
                    if (dj < 0)
                        dj = 0;
                    //针对Format32bppArgb格式像素，指针偏移量为4的倍数 4*dj  4*di
                    //g(i,j)=f(di,dj)
                    ptr[bmpdata.Stride * i + j * 4 + 0] = ptr[bmpdata.Stride * di + dj * 4 + 0];//B
                    ptr[bmpdata.Stride * i + j * 4 + 1] = ptr[bmpdata.Stride * di + dj * 4 + 1];//G
                    ptr[bmpdata.Stride * i + j * 4 + 2] = ptr[bmpdata.Stride * di + dj * 4 + 2];//R
                    //ptr += 4;  注意此处指针没移动，始终以bmpData.Scan0开始
                }
                //  ptr += bmpData.Stride - width * 4;
            }
            map.UnlockBits(bmpdata);
            return map;
        }
        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }
        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn">二进制图片流</param>
        /// <returns>Image</returns>
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
            {
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }

        /// <summary>
        /// 霓虹处理
        /// 霓虹处理算法：同样以3*3的点阵为例，目标像素g(i,j)应当以f(i,j)与f(i,j+1)，f(i,j)与f(i+1,j)的梯度作为R,G,B分量，我们不妨设f(i,j)的RGB分量为(r1, g1, b1), f(i,j+1)为(r2, g2, b2), f(i+1,j)为(r3, g3, b3), g(i, j)为(r, g, b),那么结果应该为
        ///r = 2 * sqrt( (r1 - r2)^2 + (r1 - r3)^2 )
        ///g = 2 * sqrt( (g1 - g2)^2 + (g1 - g3)^2 )
        ///b = 2 * sqrt( (b1 - b2)^2 + (b1 - b3)^2 )
        ///f(i,j)=2*sqrt[(f(i,j)-f(i+1,j))^2+(f(i,j)-f(,j+1))^2]
        /// </summary>
        /// <param name="image"></param>
        public static Image NeonEffect(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;
            for (int i = 0; i < width - 1; i++)//注意边界的控制
            {
                for (int j = 0; j < height - 1; j++)
                {
                    Color cc1 = bmp.GetPixel(i, j);
                    Color cc2 = bmp.GetPixel(i, j + 1);
                    Color cc3 = bmp.GetPixel(i + 1, j);

                    int rr = 2 * (int)Math.Sqrt((cc3.R - cc1.R) * (cc3.R - cc1.R) + (cc2.R - cc1.R) * (cc2.R - cc1.R));
                    int gg = 2 * (int)Math.Sqrt((cc3.G - cc1.G) * (cc3.G - cc1.G) + (cc2.G - cc1.G) * (cc2.G - cc1.G));
                    int bb = 2 * (int)Math.Sqrt((cc3.B - cc1.B) * (cc3.B - cc1.B) + (cc2.B - cc1.B) * (cc2.B - cc1.B));


                    if (rr > 255) rr = 255;
                    if (gg > 255) gg = 255;
                    if (bb > 255) bb = 255;
                    bmp.SetPixel(i, j, Color.FromArgb(rr, gg, bb));
                }
            }
            return bmp;
        }
        public unsafe static Image NeonEffect_Unsafe(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    byte bb = (byte)(2 * Math.Sqrt((ptr[4] - ptr[0]) * (ptr[4] - ptr[0])) + (ptr[bmpData.Stride] - ptr[0]) * (ptr[bmpData.Stride] - ptr[0]));//b;
                    byte gg = (byte)(2 * Math.Sqrt((ptr[5] - ptr[1]) * (ptr[5] - ptr[1])) + (ptr[bmpData.Stride + 1] - ptr[1]) * (ptr[bmpData.Stride + 1] - ptr[1]));//g
                    byte rr = (byte)(2 * Math.Sqrt((ptr[6] - ptr[2]) * (ptr[6] - ptr[2])) + (ptr[bmpData.Stride + 2] - ptr[2]) * (ptr[bmpData.Stride + 2] - ptr[2]));//r
                    if (rr > 255) rr = 255;
                    if (gg > 255) gg = 255;
                    if (bb > 255) bb = 255;
                    ptr[0] = bb;
                    ptr[1] = gg;
                    ptr[2] = rr;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        /// <summary>
        /// 马赛克
        /// 马赛克算法很简单，说白了就是把一张图片分割成若干个val * val像素的小区块（可能在边缘有零星的小块，但不影响整体算法,val越大，马赛克效果越明显），每个小区块的颜色都是相同的
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image MosaicEffect(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;
            const int N = 5;//效果粒度，值越大码越严重
            int r = 0, g = 0, b = 0;
            Color c;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if (y % N == 0)
                    {
                        if (x % N == 0)//整数倍时，取像素赋值
                        {
                            c = bmp.GetPixel(x, y);
                            r = c.R;
                            g = c.G;
                            b = c.B;
                        }
                        else
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }
                    else //复制上一行
                    {
                        Color colorPreLine = bmp.GetPixel(x, y - 1);
                        bmp.SetPixel(x, y, colorPreLine);
                    }
                }
            }
            return bmp;
        }
        public unsafe static Image MosaicEffect_Unsafe(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;
            const int N = 5;//效果粒度，值越大码越严重
            int r = 0, g = 0, b = 0;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y % N == 0)
                    {
                        if (x % N == 0)
                        {
                            r = ptr[2];
                            g = ptr[1];
                            b = ptr[0];
                        }
                        else
                        {
                            ptr[2] = (byte)r;
                            ptr[1] = (byte)g;
                            ptr[0] = (byte)b;
                        }
                    }
                    else //复制上一行
                    {
                        ptr[0] = ptr[0 - bmpData.Stride];//b;
                        ptr[1] = ptr[1 - bmpData.Stride];//g;
                        ptr[2] = ptr[2 - bmpData.Stride];//r
                    }
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        public static void SaveImage(Image image,string filename,System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (image != null)
            {
                try
                {
                    image.Save(filename, imageFormat);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    /*
     * 在Visual c#中对图像进行处理或访问，需要先建立一个Bitmap对象，
     * 然后通过其LockBits方法来获得一个BitmapData类的对象，
     * 然后通过获得其像素数据的首地址来对Bitmap对象的像素数据进行操作。
     * 当然，一种简单但是速度慢的方法是用Bitmap类的GetPixel和SetPixel方法。
     * 其中BitmapData类的Stride属性为每行像素所占的字节。
     */

    class GrayBitmapData
    {
        public byte[,] Data;//保存像素矩阵
        public int Width;//图像的宽度
        public int Height;//图像的高度
        public GrayBitmapData()
        {
            this.Width = 0;
            this.Height = 0;
            this.Data = null;
        }
        public GrayBitmapData(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            this.Width = bmpData.Width;
            this.Height = bmpData.Height;
            Data = new byte[Height, Width];
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
    //将24位的RGB彩色图转换为灰度图
                        int temp = (int)(0.114 * (*ptr++)) + (int)(0.587 * (*ptr++))+(int)(0.299 * (*ptr++));
                        Data[i, j] = (byte)temp;
                    }
                    ptr += bmpData.Stride - Width * 3;//指针加上填充的空白空间
                }
            }
            bmp.UnlockBits(bmpData);
        }
        public GrayBitmapData(string path)
            : this(new Bitmap(path))
        {
        }
        public Bitmap ToBitmap()
        {
            Bitmap bmp=new Bitmap(Width,Height,PixelFormat.Format24bppRgb);
            BitmapData bmpData=bmp.LockBits(new Rectangle(0,0,Width,Height),ImageLockMode.WriteOnly,PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr=(byte*)bmpData.Scan0.ToPointer();
                for(int i=0;i<Height;i++)
                {
                    for(int j=0;j<Width;j++)
                    {
                        *(ptr++)=Data[i,j];
                        *(ptr++)=Data[i,j];
                        *(ptr++)=Data[i,j];
                    }
                    ptr+=bmpData.Stride-Width*3;
                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        //public void ShowImage(PictureBox pbx)
        //{
        //    Bitmap b = this.ToBitmap();
        //    pbx.Image = b;
        //    //b.Dispose();
        //}
        public void SaveImage(string path)
        {
            Bitmap b=ToBitmap();
            b.Save(path);
            //b.Dispose();
        }
//均值滤波
        public void AverageFilter(int windowSize)
        {
            if (windowSize % 2 == 0)
            {
                return;
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int sum = 0;
                    for (int g = -(windowSize - 1) / 2; g <= (windowSize - 1) / 2; g++)
                    {
                        for (int k = -(windowSize - 1) / 2; k <= (windowSize - 1) / 2; k++)
                        {
                            int a = i + g, b = j + k;
                            if (a < 0) a = 0;
                            if (a > Height - 1) a = Height - 1;
                            if (b < 0) b = 0;
                            if (b > Width - 1) b = Width - 1;
                            sum += Data[a, b];
                        }
                    }
                    Data[i,j]=(byte)(sum/(windowSize*windowSize));
                }
            }
        }
//中值滤波
        public void MidFilter(int windowSize)
        {
            if (windowSize % 2 == 0)
            {
                return;
            }
            int[] temp = new int[windowSize * windowSize];
            byte[,] newdata = new byte[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int n = 0;
                    for (int g = -(windowSize - 1) / 2; g <= (windowSize - 1) / 2; g++)
                    {
                        for (int k = -(windowSize - 1) / 2; k <= (windowSize - 1) / 2; k++)
                        {
                            int a = i + g, b = j + k;
                            if (a < 0) a = 0;
                            if (a > Height - 1) a = Height - 1;
                            if (b < 0) b = 0;
                            if (b > Width - 1) b = Width - 1;
                            temp[n++]= Data[a, b];
                        }
                    }
                    newdata[i, j] = GetMidValue(temp,windowSize*windowSize);
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Data[i, j] = newdata[i, j];
                }
            }
        }
//获得一个向量的中值
        private byte GetMidValue(int[] t, int length)
        {
            int temp = 0;
            for (int i = 0; i < length - 2; i++)
            {
                for (int j = i + 1; j < length - 1; j++)
                {
                    if (t[i] > t[j])
                    {
                        temp = t[i];
                        t[i] = t[j];
                        t[j] = temp;
                    }
                }
            }
            return (byte)t[(length - 1) / 2];
        }
//一种新的滤波方法，是亮的更亮、暗的更暗
        public void NewFilter(int windowSize)
        {
            if (windowSize % 2 == 0)
            {
                return;
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int sum = 0;
                    for (int g = -(windowSize - 1) / 2; g <= (windowSize - 1) / 2; g++)
                    {
                        for (int k = -(windowSize - 1) / 2; k <= (windowSize - 1) / 2; k++)
                        {
                            int a = i + g, b = j + k;
                            if (a < 0) a = 0;
                            if (a > Height - 1) a = Height - 1;
                            if (b < 0) b = 0;
                            if (b > Width - 1) b = Width - 1;
                            sum += Data[a, b];
                        }
                    }
                    double avg = (sum+0.0) / (windowSize * windowSize);
                    if (avg / 255 < 0.5)
                    {
                        Data[i, j] = (byte)(2 * avg / 255 * Data[i, j]);
                    }
                    else
                    {
                        Data[i,j]=(byte)((1-2*(1-avg/255.0)*(1-Data[i,j]/255.0))*255);
                    }
                }
            }
        }
//直方图均衡
        public void HistEqual()
        {
            double[] num = new double[256] ;
            for(int i=0;i<256;i++) num[i]=0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    num[Data[i, j]]++;
                }
            }
            double[] newGray = new double[256];
            double n = 0;
            for (int i = 0; i < 256; i++)
            {
                n += num[i];
                newGray[i] = n * 255 / (Height * Width);
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Data[i,j]=(byte)newGray[Data[i,j]];
                }
            }
        }
    }
}
