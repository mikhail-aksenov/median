using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace median
{
    class Program
    {
        static readonly int MaskSize = 9;
        static readonly int Med = (int) Math.Round(MaskSize / 2.0);
        static readonly int Diff = 3;
        
        static void CalcValue(int row, int col, int shift, int width, ref byte[] source, ref byte[] target)
        {
            byte[] mask = new byte[MaskSize];
            mask[0] = source[(row - 1) * width + col - Diff + shift];
            mask[1] = source[(row - 1) * width + col + shift];
            mask[2] = source[(row - 1) * width + col + Diff + shift];

            mask[3] = source[(row * width) + col - Diff + shift];
            mask[4] = source[(row * width) + col + shift];
            mask[5] = source[(row * width) + col + Diff + shift];

            mask[6] = source[(row + 1) * width + col - Diff + shift];
            mask[7] = source[(row + 1) * width + col + shift];
            mask[8] = source[(row + 1) * width + col + Diff + shift];

            Array.Sort(mask);

            target[(row * width) + col + shift] = mask[Med];
        }


        /// Single-Thread filter
        static void STFilter(string inputPath, string outputPath)
        {
            var img = new Bitmap(inputPath);
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            var bitmapData = img.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var ptr = bitmapData.Scan0;
            var length = Math.Abs(bitmapData.Stride) * img.Height;
            var sourceBytes = new byte[length];
            var targetBytes = new byte[length];
            var width = img.Width * 3;

            System.Runtime.InteropServices.Marshal.Copy(ptr, sourceBytes, 0, length);

            for (int i = 1; i < img.Height - 1; i++)
            {
                for (int j = 1; j < img.Width - 1; j++)
                {
                    CalcValue(i, j * 3, 0, width, ref sourceBytes, ref targetBytes);
                    CalcValue(i, j * 3, 1, width, ref sourceBytes, ref targetBytes);
                    CalcValue(i, j * 3, 2, width, ref sourceBytes, ref targetBytes);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(targetBytes, 0, ptr, length);

            img.UnlockBits(bitmapData);
            img.Save(outputPath, ImageFormat.Bmp);
        }

        static void InvokeFilter(ref byte[] source, ref byte[] target, int step, int w, int h)
        { 
        }

        static void MTFilter(string inputPath, string outputPath)
        {
            var img = new Bitmap(inputPath);
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            var bitmapData = img.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var ptr = bitmapData.Scan0;
            var length = Math.Abs(bitmapData.Stride) * img.Height;
            var sourceBytes = new byte[length];
            var targetBytes = new byte[length];
            var width = img.Width * 3;

            System.Runtime.InteropServices.Marshal.Copy(ptr, sourceBytes, 0, length);

            for (int i = 1; i < img.Height - 1; i++)
            {
                for (int j = 1; j < img.Width - 1; j++)
                {
                    CalcValue(i, j * 3, 0, width, ref sourceBytes, ref targetBytes);
                    CalcValue(i, j * 3, 1, width, ref sourceBytes, ref targetBytes);
                    CalcValue(i, j * 3, 2, width, ref sourceBytes, ref targetBytes);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(targetBytes, 0, ptr, length);

            img.UnlockBits(bitmapData);
            img.Save(outputPath);
        }


        static void Main(string[] args)
        {
            var time = DateTime.Now.Millisecond;
            STFilter(args[0], args[1]);
            Console.WriteLine("Single-Threaded taken {0} ms", DateTime.Now.Millisecond - time);
        }
    }
}
