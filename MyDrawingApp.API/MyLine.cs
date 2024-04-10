namespace MyDrawingApp.API
{
    public class MyLine
    {
        public MyPoint Start { get; }

        public MyPoint End { get; }

        public double Length { get; }

        public MyPoint Vector { get; }

        public MyPoint Center { get; }

        private MyLine(MyPoint start, MyPoint end)
        {
            Start = start;
            End = end;
            Length = GetLength();
            Vector = GetVector();
            Center = (Start + End) / 2;
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
            MyPoint dif = End - Start;
            double xDiff = Math.Pow(dif.X, 2);
            double yDiff = Math.Pow(dif.Y, 2);
            double zDiff = Math.Pow(dif.Z, 2);

            return Math.Sqrt(xDiff + yDiff + zDiff);
        }

        private MyPoint GetVector()
        {
            MyPoint dif = (End - Start) / Length;
            return dif;
        }
    }
}
