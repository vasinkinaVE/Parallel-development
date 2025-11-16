using System;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixRangeApp
{
    class Program
    {
        // Метод с поддержкой отмены и прогресса
        static int CalculateMatrixRange(int rows, int cols, CancellationToken cancellationToken)
        {
            Console.WriteLine("Генерация матрицы...");

            Random rand = new Random();
            int[,] matrix = new int[rows, cols];

            // Заполняем матрицу случайными числами
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException(cancellationToken);

                    matrix[i, j] = rand.Next(1, 201); // числа от 1 до 200
                }
            }

            Console.WriteLine("Поиск минимума и максимума...");

            // Находим минимум и максимум
            int min = matrix[0, 0];
            int max = matrix[0, 0];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException(cancellationToken);

                    if (matrix[i, j] < min) min = matrix[i, j];
                    if (matrix[i, j] > max) max = matrix[i, j];
                }
            }

            Console.WriteLine($"Минимум: {min}, Максимум: {max}");
            return max - min;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Асинхронное вычисление разницы (макс - мин) в матрице с тайм-аутом ===\n");

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

            // Настройка тайм-аута: 5 секунд
            int timeoutMs = 5000;
            Console.WriteLine($"\nЗапуск асинхронного метода с тайм-аутом {timeoutMs / 1000} секунд...\n");

            using var cts = new CancellationTokenSource(timeoutMs); // автоматическая отмена через 5 сек
            var token = cts.Token;

            try
            {
                Task<int> task = Task.Run(() => CalculateMatrixRange(rows, cols, token), token);

                // Асинхронный мониторинг выполнения без блокировки потока
                while (!task.IsCompleted)
                {
                    Console.Write(".");
                    await Task.Delay(300); // асинхронная задержка вместо Thread.Sleep
                }

                Console.WriteLine("\n\nМетод завершён!");

                int result = await task; // может выбросить исключение, если отменено
                Console.WriteLine($"\nРазница между максимальным и минимальным элементами: {result}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n\nОперация превысила время ожидания ({timeoutMs / 1000} сек) и была отменена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}
