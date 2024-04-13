// See https://aka.ms/new-console-template for more information

Console.WriteLine("Привет. Давай соеденим два числа!");

Console.WriteLine("Введите первое число:");
double num1 = GetNumFromUser().Value;

Console.WriteLine("Супер! Введите второе число:");
double num2 = GetNumFromUser().Value;

var result = num1 + num2;
Console.WriteLine("Результат: " + result);

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
        catch(FormatException) 
        {
            Console.WriteLine("Введите пожалуйста число:");
        }
    }

    return result;
}

// калькулятор с помощью делегатов
var allOperations = new Dictionary<string, Func<double, double, double>>();

allOperations.Add("+", (n1, n2) => n1 + n2);
allOperations.Add("-", (n1, n2) => n1 - n2);
allOperations.Add("*", (n1, n2) => n1 * n2);

if (allOperations.TryGetValue("+", out Func<double, double, double> operation))
{
    var result1 = operation(num1, num2);
    Console.WriteLine(result1);
}
else
{
    Console.WriteLine("Нет такой операции");
}
