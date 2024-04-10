using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawingApp.API
{
    public class MySquare : MyFigure
    {
        private MySquare(MyPoint center, List<MyLine> lines) 
            : base(center, lines)
        {
        }

        public static MySquare Create(MyPoint center, double sideLength)
        {
            // create all lines
            double sideLengthHalf = sideLength / 2;
            var point1 = new MyPoint(center.X - sideLengthHalf, center.Y - sideLengthHalf, center.Z);
            var point2 = new MyPoint(center.X - sideLengthHalf, center.Y + sideLengthHalf, center.Z);
            var point3 = new MyPoint(center.X + sideLengthHalf, center.Y + sideLengthHalf, center.Z);
            var point4 = new MyPoint(center.X + sideLengthHalf, center.Y - sideLengthHalf, center.Z);

            var lines = new List<MyLine>
            {
                MyLine.Create(point1, point2),
                MyLine.Create(point2, point3),
                MyLine.Create(point3, point4),
                MyLine.Create(point4, point1),
            };

            return new MySquare(center, lines);
        }

        public override double GetArea()
        {
            return this.Lines[0].Length * this.Lines[1].Length;
        }

        public override string ToString()
        {
            double perimeter = GetPerimeter();
            double area = GetArea();

            return $"Квадрат\nПериметр: {perimeter}мм\nПлощадь: {area} кв.мм.";
        }

        public List<MyPoint> GetTops()
        {
            var result = new List<MyPoint>();
            foreach (MyLine line in Lines)
            {
                if (!result.Contains(line.Start))
                {
                    result.Add(line.Start);
                }
                if (!result.Contains(line.End))
                {
                    result.Add(line.End);
                }
            }

            return result;
        }
    }
}
