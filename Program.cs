using System;

// Пользовательский делегат с требуемой сигнатурой
public delegate double MyCustomDelegate(Action<char> printer, bool flag, double value);

namespace LabWorkDelegates
{
    class Program
    {
        // Метод 1: удвоение при flag == true
        static double DoubleIfTrue(Action<char> printer, bool flag, double value)
        {
            if (flag)
            {
                printer('D'); // D = Doubled
                return value * 2.0;
            }
            printer('N'); // N = Not changed
            return value;
        }

        // Метод 2: возведение в квадрат при flag == false
        static double SquareIfFalse(Action<char> printer, bool flag, double value)
        {
            if (!flag)
            {
                printer('S'); // S = Squared
                return value * value;
            }
            printer('I'); // I = Ignored
            return value;
        }

        // Метод 3: добавление бонуса при flag == true
        static double AddBonusIfTrue(Action<char> printer, bool flag, double value)
        {
            if (flag)
            {
                printer('+'); // + = Bonus added
                return value + 10.5;
            }
            printer('0'); // 0 = No bonus
            return value;
        }

        static void Main(string[] args)
        {
            // Реализация Action<char> для вывода символов
            Action<char> charPrinter = c => Console.Write($"[{c}] ");

            Console.WriteLine("Лабораторная работа: Пользовательские делегаты в C#");
            Console.WriteLine("Заданная сигнатура: Func<Action<char>, bool, double, double>\n");

            // Создание экземпляров ПОЛЬЗОВАТЕЛЬСКОГО делегата
            MyCustomDelegate del1 = DoubleIfTrue;
            MyCustomDelegate del2 = SquareIfFalse;
            MyCustomDelegate del3 = AddBonusIfTrue;

            Console.WriteLine("Вызов через ПОЛЬЗОВАТЕЛЬСКИЙ делегат:");
            Console.Write("   Результат DoubleIfTrue(true, 5.0):   ");
            double res1 = del1(charPrinter, true, 5.0);
            Console.WriteLine($"{res1:F1}");

            Console.Write("   Результат SquareIfFalse(false, 3.0): ");
            double res2 = del2(charPrinter, false, 3.0);
            Console.WriteLine($"{res2:F1}");

            Console.Write("   Результат AddBonusIfTrue(true, 4.2): ");
            double res3 = del3(charPrinter, true, 4.2);
            Console.WriteLine($"{res3:F1}");

            Console.Write("   Результат AddBonusIfTrue(false, 4.2): ");
            double res4 = del3(charPrinter, false, 4.2);
            Console.WriteLine($"{res4:F1}");

            Console.WriteLine("\nРабота завершена. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}