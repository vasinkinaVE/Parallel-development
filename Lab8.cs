using System;
using System.Threading.Tasks;

namespace MatrixTransformApp
{
    class Program
    {
        // Метод преобразования матрицы: кратные 3 → в квадрат
        static int[,] TransformMatrix(int rows, int cols)
        {
            int[,] matrix = new int[rows, cols];

            // Используем Random.Shared 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = Random.Shared.Next(1, 31); // числа от 1 до 30
                }
            }

            Console.WriteLine($"[Выполнение задачи {Task.CurrentId ?? 0}] Исходная матрица:");
            PrintMatrix(matrix);

            // Преобразуем: если элемент кратен 3 — заменяем на квадрат
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] % 3 == 0)
                    {
                        matrix[i, j] = matrix[i, j] * matrix[i, j];
                    }
                }
            }

            Console.WriteLine($"\n[Выполнение задачи {Task.CurrentId ?? 0}] Преобразованная матрица:");
            PrintMatrix(matrix);

            return matrix;
        }

        // Вспомогательный метод для вывода матрицы
        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],6}");
                }
                Console.WriteLine();
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Преобразование матрицы с использованием Task ===\n");

            // Ввод размеров
            Console.Write("Введите количество строк: ");
            string rowsStr = Console.ReadLine();
            int rows = Convert.ToInt32(rowsStr);

            Console.Write("Введите количество столбцов: ");
            string colsStr = Console.ReadLine();
            int cols = Convert.ToInt32(colsStr);

            // Запуск задачи
            Task<int[,]> task = Task.Run(() => TransformMatrix(rows, cols));

            Console.WriteLine("\nЗадача запущена. Ожидание завершения...\n");

            int[,] result = await task;

            Console.WriteLine("\nПреобразование завершено.");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}