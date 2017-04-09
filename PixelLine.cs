using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace NIR_WPF
{
    class PixelLine
    {

        private byte[,] img;
        private int x1, x2, y1, y2;

        public PixelLine(Line line, byte[,] img) : this((int)line.X1, (int)line.Y1, (int)line.X2, (int)line.Y2, img) { }
        public PixelLine(int x1, int y1, int x2, int y2, byte[,] img)
        {
            this.x1 = Math.Min(x1, x2);
            this.y1 = Math.Min(y1, y2);
            this.x2 = Math.Max(x1, x2);
            this.y2 = Math.Max(y1, y2);
            if (this.x1 == this.x2 && this.y1 == this.y2)
            {
                throw new ArithmeticException("Equal coords");
            }
            this.img = img;
        }

        public List<Pixel> GetLine()
        {
            List<Pixel> line = new List<Pixel>();
            HashSet<Pixel> pixels = new HashSet<Pixel>();
            for (int x = x1; x <= x2; x++)//todo: fix
            {
                int y = (int)Math.Round(Line(x));
                line.Add(new Pixel(x, y, img[y, x]));
            }

            for (int y = y1; y <= y2; y++)//todo: fix
            {
                int x = (int)Math.Round(ReverseLine(y));
                if (!PixelExists(line, x, y))
                {
                    line.Add(new Pixel(x, y, img[y, x]));
                }
            }
            return line;
        }

        private static bool PixelExists(List<Pixel> line, int x, int y)
        {
            return line.Find(pixel => pixel.X == x && pixel.Y == y) != null;
        }

        private double Line(int x)
        {
            return (double)(x - x1) * (y2 - y1) / (x2 - x1) + y1;
        }

        private double ReverseLine(int y)
        {
            return (double)(y - y1) * (x2 - x1) / (y2 - y1) + x1;
        }
    }
}
