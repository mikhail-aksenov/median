using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace median
{
    class Program
    {
        static void Median(int pos, ref byte[] img) 
        {

        }

        static void Filter(string inputPath, string outputPath)
        {
            var img = new Bitmap(inputPath);
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData bitmapData = img.LockBits(rect, ImageLockMode.ReadWrite, img.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;
            int length = Math.Abs(bitmapData.Stride) * img.Height;
            byte[] data = new byte[length];

            System.Runtime.InteropServices.Marshal.Copy(ptr, data, 0, length);

            for (int i = 2; i < data.Length; i += 3)
            {
                data[i] = 255;
            }

            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, length);

            img.UnlockBits(bitmapData);
            img.Save(outputPath);
        }


        static void Main(string[] args)
        {
            Filter(args[0], args[1]);
            //Filter(@"C:\dev\metro\median\maxresdefault.jpg", "some.jpg");
        }
    }
}
