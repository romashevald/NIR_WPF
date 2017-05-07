using System.Drawing;

namespace NIR_WPF
{
    public class KvaziParams
    {
        public PointF CircleCentr { get; private set; }

        public double RadiusA { get; private set; }
        public double RadiusB { get; private set; }
        public double RadiusC { get; private set; }

        public KvaziParams(float centrX, float centrY, double radiusA, double radiusB, double radiusC)
            : this(new PointF(centrX, centrY), radiusA, radiusB, radiusC)
        {
        }

        public KvaziParams(PointF centr, double radiusA, double radiusB, double radiusC)
        {
            CircleCentr = centr;
            RadiusA = radiusA;
            RadiusB = radiusB;
            RadiusC = radiusC;
        }
    }
}
