using System;
using System.Threading;

namespace MatrixMaxSearchApp
{
    class Program
    {
        // 1. Метод поиска максимального элемента в матрице
        static void FindMaxInMatrix()
        {
            // Получаем ID текущего потока для вывода
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Поток {threadId}. Создание матрицы...");

            // Размер матрицы
            int rows = 5;
            int cols = 5;
            int[,] matrix = new int[rows, cols];

            // Инициализируем матрицу случайными числами
            Random rnd = new Random(threadId); // используем ID потока как seed, чтобы числа были разными
            Console.WriteLine($"Поток {threadId}. Инициализация элементов матрицы...");

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rnd.Next(1, 101); // от 1 до 100
                }
            }

            // Поиск максимального элемента
            Console.WriteLine($"Поток {threadId}. Поиск максимального элемента...");
            int max = matrix[0, 0];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] > max)
                        max = matrix[i, j];
                }
            }

            // Вывод результата
            Console.WriteLine($"Поток {threadId}. Максимальный элемент = {max}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Поиск максимального элемента в матрице с использованием потоков ===\n");

            // 2. Создаем массив потоков
            int numberOfThreads = 5; 
            Thread[] threads = new Thread[numberOfThreads];

            Console.WriteLine($"Создание {numberOfThreads} потоков...\n");

            // 3. Создаем и запускаем каждый поток
            for (int i = 0; i < threads.Length; i++)
            {
                // Используем делегат Action (без параметров, без возврата)
                threads[i] = new Thread(FindMaxInMatrix);
                threads[i].Start();
            }

            // 4. Ожидаем завершения всех потоков
            Console.WriteLine("\nОжидание завершения всех потоков...");
            foreach (Thread t in threads)
            {
                t.Join(); // ждём, пока поток не завершится
            }

            Console.WriteLine("\nВсе потоки завершили работу.");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}