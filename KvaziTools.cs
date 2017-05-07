using System.Drawing;
using System;

namespace NIR_WPF
{
    public abstract class KvaziTools
    {
        public static KvaziParams GetParams(PointF A, PointF B, PointF C)
        {
            var d = new PointF((A.X + B.X) / 2f, (A.Y + B.Y) / 2f);
            var f = new PointF((C.X + B.X) / 2f, (C.Y + B.Y) / 2f);

            float x0 = 0; //система уравнений
            float y0 = 0;


            var O = new PointF(x0, y0);

            return new KvaziParams(O, GetLengthLine(A, O), GetLengthLine(B, O), GetLengthLine(C, O));
        }

        private static double GetLengthLine(PointF first, PointF second)
        {
            return Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }
    }
}
