namespace MyDrawingApp.API
{
    public class MyLine
    {
        public MyPoint Start { get; }

        public MyPoint End { get; }

        public double Length { get; }

        public MyPoint Vector { get; }

        private MyLine(MyPoint start, MyPoint end)
        {
            Start = start;
            End = end;
            Length = GetLength();
            Vector = GetVector();
        }

        public static MyLine Create(MyPoint start, MyPoint end)
        {
            return new MyLine(start, end);
        }

        public override string ToString()
        {
            return $"Линия. Длина: {Length}";
        }

        private double GetLength()
        {
            double xDiff = Math.Pow(End.X - Start.X, 2);
            double yDiff = Math.Pow(End.Y - Start.Y, 2);
            double zDiff = Math.Pow(End.Z - Start.Z, 2);

            return Math.Sqrt(xDiff + yDiff + zDiff);
        }

        private MyPoint GetVector()
        {
            throw new NotImplementedException();
        }
    }
}
