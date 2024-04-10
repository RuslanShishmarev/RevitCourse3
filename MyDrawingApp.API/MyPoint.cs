using System.Diagnostics.CodeAnalysis;

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

        public static MyPoint operator +(MyPoint left, MyPoint right)
        {
            return new MyPoint(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static MyPoint operator -(MyPoint left, MyPoint right)
        {
            return new MyPoint(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static MyPoint operator /(MyPoint left, double num)
        {
            return new MyPoint(left.X / num, left.Y / num, left.Z / num);
        }

        public static MyPoint operator *(MyPoint left, double num)
        {
            return new MyPoint(left.X * num, left.Y * num, left.Z * num);
        }

        public static bool operator ==(MyPoint left, MyPoint right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(MyPoint left, MyPoint right)
        {
            return !(left == right);
        }
    }
}
