namespace MyDrawingApp.API
{
    public abstract class MyFigure
    {
        public MyPoint Center { get; }

        public List<MyLine> Lines { get; }

        public MyFigure(MyPoint center, List<MyLine> lines)
        {
            Center = center;
            Lines = lines;
        }

        public virtual double GetPerimeter()
        {
            return Lines.Sum(l => l.Length);
        }

        public abstract double GetArea();

        protected static List<MyLine> CreatePolygon(MyPoint center, double radius, int sides)
        {
            var allPoints = new List<MyPoint>();
            double alphaStep = 2 * Math.PI / sides;
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

            return allLines;
        }
    }
}
