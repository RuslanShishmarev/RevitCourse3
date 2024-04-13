namespace MyDrawingApp.API
{
    public class MyCircle : MyFigure
    {
        public double Radius { get; }

        private MyCircle(MyPoint center, List<MyLine> lines, double radius)
            : base(center, lines)
        {
            Radius = radius;
        }

        public static MyCircle Create(MyPoint center, double radius)
        {
            var allPoints = new List<MyPoint>();
            double alphaStep = 2 * Math.PI / 360;
            for (double alpha = 0; alpha < 2 * Math.PI; alpha += alphaStep)
            {
                var newPoint = new MyPoint(
                    center.X + radius * Math.Cos(alpha), 
                    center.Y + radius * Math.Sin(alpha),
                    center.Z);
               
                allPoints.Add(newPoint);
            }

            var allLines = new List<MyLine>();

            for (int pointIndex = 1; pointIndex < allPoints.Count; pointIndex++)
            {
                var point1 = allPoints[pointIndex - 1];
                var point2 = allPoints[pointIndex];

                var newLinwe = MyLine.Create(point1, point2);
                allLines.Add(newLinwe);
            }

            return new MyCircle(center, allLines, radius);
        }

        public override double GetArea() => Math.PI * Math.Pow(Radius, 2);
        
        public double GetCircleLength() => 2 * Math.PI * Radius;
    }
}
