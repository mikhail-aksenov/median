using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace median
{
    class PartialFilter
    {
        public static readonly int MaskSize = 9;
        public static readonly int Med = (int)Math.Round(MaskSize / 2.0);
        public static readonly int Diff = 3;

        private byte[] source;
        private byte[] target;
        private readonly int step;
        private readonly int offset;
        private readonly int width;
        private readonly int height;
        private readonly int stride;

        public byte[] RawData { get { return target; } }

        private void CalcValue(int row, int col, int shift, int width)
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

        public void StartComputation()
        {
            for (int i = offset + 1; i < height - offset - 1; i += step)
            {
                for (int j = 1; j < width; j++)
                {
                    CalcValue(i, j * 3, 0, stride);
                    CalcValue(i, j * 3, 1, stride);
                    CalcValue(i, j * 3, 2, stride);
                }
            } 
        }

        // Don't do this, unless you clearly understand what's going on!
        public PartialFilter(int step, 
            int offset, 
            int width,
            int height, 
            int stride, 
            byte[] source, 
            byte[] target)
        {
            this.step = step;
            this.offset = offset;
            this.source = source;
            this.target = target;
            this.width = width;
            this.height = height;
            this.stride = stride;

            StartComputation();
        }
    }
}
