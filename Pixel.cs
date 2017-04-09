using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIR_WPF
{
    class Pixel
    {
        public Pixel(int x, int y, byte value)
        {
            this.X = x;
            this.Y = y;
            this.Value = value;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public byte Value { get; set; }



        public override string ToString()
        {

            return String.Format("({0}, {1}), value={2}", X, Y, Value);

        }
    }
}
