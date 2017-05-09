using System.Drawing;

namespace NIR_WPF
{
    public class KvaziParams
    {
        public PointF CircleCentr { get; private set; }

        public float RadiusA { get; private set; }
        public float RadiusB { get; private set; }
        public float RadiusC { get; private set; }

        public PointF D { get; set; }
        public PointF E { get; set; }

        public KvaziParams(float centrX, float centrY, float radiusA, float radiusB, float radiusC)
            : this(new PointF(centrX, centrY), radiusA, radiusB, radiusC)
        {
        }

        public KvaziParams(PointF centr, float radiusA, float radiusB, float radiusC)
        {
            CircleCentr = centr;
            RadiusA = radiusA;
            RadiusB = radiusB;
            RadiusC = radiusC;
        }
    }
}
