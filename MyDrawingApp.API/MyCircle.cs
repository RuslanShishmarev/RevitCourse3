namespace MyDrawingApp.API
{
    public class MyCircle : MyFigure, IMyCircle
    {
        public double Radius { get; }

        private MyCircle(MyPoint center, List<MyLine> lines, double radius)
            : base(center, lines)
        {
            Radius = radius;
        }

        public static MyCircle Create(MyPoint center, double radius)
        {
            var allLines = CreatePolygon(center, radius, 360);

            return new MyCircle(center, allLines, radius);
        }

        public override double GetArea() => Math.PI * Math.Pow(Radius, 2);
        
        public double GetCircleLength() => 2 * Math.PI * Radius;
    }
}
