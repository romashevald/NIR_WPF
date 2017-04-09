using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace NIR_WPF
{
    class ImageReader
    {
        public static int BITS_PER_PIXEL = 24;
        private Bitmap bmp;
        private byte[,] redChannel;
        private byte[,] blueChannel;
        private byte[,] greenChannel;

        public byte[,] RedChannel
        {
            get { return redChannel; }
        }

        public byte[,] BlueChannel
        {
            get { return blueChannel; }
        }

        public byte[,] GreenChannel
        {
            get { return greenChannel; }
        }

        public ImageReader(String path)
        {
            bmp = new Bitmap(path);
            redChannel = new byte[bmp.Height, bmp.Width];
            blueChannel = new byte[bmp.Height, bmp.Width];
            greenChannel = new byte[bmp.Height, bmp.Width];
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new NotSupportedException("Wrong pixel format");
            }
            FillChannels();
        }

        private unsafe void FillChannels()
        {
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            /*This time we convert the IntPtr to a ptr*/
            byte* scan0 = (byte*)bData.Scan0.ToPointer();
            for (int i = 0; i < bData.Height; ++i)
            {
                for (int j = 0; j < bData.Width; ++j)
                {
                    byte* data = scan0 + i * bData.Stride + j * BITS_PER_PIXEL / 8;
                    blueChannel[i, j] = *(data);
                    greenChannel[i, j] = *(data + 1);
                    redChannel[i, j] = *(data + 2);
                }
            }
            bmp.UnlockBits(bData);
        }

    }
}
