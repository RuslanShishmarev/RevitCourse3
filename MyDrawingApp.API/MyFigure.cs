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
    }
}
