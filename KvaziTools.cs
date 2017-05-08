using System.Drawing;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NIR_WPF
{
    public abstract class KvaziTools
    {
        public static KvaziParams GetParams(List<PointF> points)
        {
            return GetParams(points[0], points[1], points[2]);
        }

        public static KvaziParams GetParams(PointF A, PointF B, PointF C)
        {
            Console.WriteLine("{0}, {1}, {2}", A, B, C);
            var D = new PointF((A.X + B.X) / 2f, (A.Y + B.Y) / 2f);
            var E = new PointF((C.X + B.X) / 2f, (C.Y + B.Y) / 2f);
            float[,] matrix = new float[,]
            {
                {B.X - A.X, B.Y - A.Y}, {C.X - B.X, C.Y - B.Y}
            };

            float[] vector = new float[]
            {
                A.X * (B.X - A.X) + D.Y * (B.Y - A.Y), B.Y*(C.Y-B.Y)+E.X*(C.X-B.X)
            };

            float[] result = solve(matrix, vector);

            Console.WriteLine("Center coords are: " + String.Join(";", result));

            float x0 = result[0]; //система уравнений
            float y0 = result[1];


            var O = new PointF(x0, y0);

            return new KvaziParams(O, GetLengthLine(A, O), GetLengthLine(B, O), GetLengthLine(C, O));
        }

       
        public static float GetLengthLine(PointF first, PointF second)
        {
            return (float)Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
        }

        public static float[] solve(float[,] matrix, float[] vector)
        {
            var A = Matrix<float>.Build.DenseOfArray(matrix);
            var B = Vector<float>.Build.Dense(vector);
            return A.Solve(B).AsArray();
        }
    }
}
