using System.Drawing;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NIR_WPF
{
    public abstract class KvaziTools
    {
        public static KvaziParams GetParams(List<Point> points)
        {
            return GetParams(points[0], points[1], points[2]);
        }

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

        private static double[] solve(double[,] matrix, double[] vector)
        {
            var A = Matrix<double>.Build.DenseOfArray(matrix);
            var B = Vector<double>.Build.Dense(vector);
            return A.Solve(B).AsArray();
        }
    }
}
