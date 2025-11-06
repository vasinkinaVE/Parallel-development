using System;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixRangeApp
{
    class Program
    {
        // Метод, соответствующий Func<int, int, int>
        static int CalculateMatrixRange(int rows, int cols)
        {
            Console.WriteLine("Генерация матрицы...");

            Random rand = new Random();
            int[,] matrix = new int[rows, cols];

            // Заполняем матрицу случайными числами
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rand.Next(1, 101); // числа от 1 до 100
                }
            }

            // Находим минимум и максимум
            int min = matrix[0, 0];
            int max = matrix[0, 0];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] < min) min = matrix[i, j];
                    if (matrix[i, j] > max) max = matrix[i, j];
                }
            }

            Console.WriteLine($"Минимум: {min}, Максимум: {max}");
            return max - min;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Асинхронное вычисление разницы (макс - мин) в матрице ===\n");

            // Ввод размеров матрицы
            Console.Write("Введите количество строк: ");
            if (!int.TryParse(Console.ReadLine(), out int rows) || rows <= 0)
            {
                Console.WriteLine("Некорректное значение строк.");
                return;
            }

            Console.Write("Введите количество столбцов: ");
            if (!int.TryParse(Console.ReadLine(), out int cols) || cols <= 0)
            {
                Console.WriteLine("Некорректное значение столбцов.");
                return;
            }

            // Создаём экземпляр библиотечного делегата Func<int, int, int>
            Func<int, int, int> matrixRangeDelegate = CalculateMatrixRange;

            Console.WriteLine("\nЗапуск асинхронного метода...\n");

            Task<int> task = Task.Run(() => matrixRangeDelegate(rows, cols));

            // Мониторинг: показываем, что основной поток не заблокирован
            while (!task.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(300);
            }

            Console.WriteLine("\n\nМетод завершён!");

            // Получаем результат
            int result = await task;
            Console.WriteLine($"\nРазница между максимальным и минимальным элементами: {result}");

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}