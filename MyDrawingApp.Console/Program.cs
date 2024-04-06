// See https://aka.ms/new-console-template for more information
using MyDrawingApp.API;

Console.WriteLine("Приветствую в чертежной программе");

MyPoint firstPoint = GetPointFromUser("1");
Console.WriteLine(firstPoint);

MyPoint secondPoint = GetPointFromUser("2");
Console.WriteLine(secondPoint);

MyLine newLine = MyLine.Create(firstPoint, secondPoint);
Console.WriteLine(newLine);

MyPoint GetPointFromUser(string pointName)
{
    Console.WriteLine("Введите координаты точки " + pointName);
    Console.WriteLine("X: ");
    double x = GetNumFromUser().Value;

    Console.WriteLine("Y: ");
    double y = GetNumFromUser().Value;

    Console.WriteLine("Z: ");
    double z = GetNumFromUser().Value;

    return new MyPoint(x, y, z);
}

double? GetNumFromUser()
{
    double? result = null;
    bool isContinue = true;

    while (isContinue)
    {
        string? userAnswer = Console.ReadLine()?.Replace('.', ',');
        try
        {
            result = double.Parse(userAnswer);
            isContinue = false;
        }
        catch (FormatException)
        {
            Console.WriteLine("Введите пожалуйста число:");
        }
    }

    return result;
}
