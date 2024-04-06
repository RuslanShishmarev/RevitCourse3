namespace MyDrawingApp.API
{
    public struct MyPoint
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public MyPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public MyPoint(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public override string ToString()
        {
            return $"Точка (X={X}, Y={Y}, Z={Z})";
        }
    }
}
