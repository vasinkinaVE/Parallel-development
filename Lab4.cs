using System;
using System.Threading;

namespace VectorAdditionApp
{
    // 1. Объявляем делегат для обратного вызова
    public delegate void ResultCallback(int[] result);

    class Program
    {
        // 2. Асинхронный метод с обратным вызовом
        static void AddRandomVectorsAsync(int size, ResultCallback callback)
        {
            if (size <= 0)
                throw new ArgumentException("Размер вектора должен быть положительным.");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    Random rand = new Random();
                    int[] vectorA = new int[size];
                    int[] vectorB = new int[size];
                    int[] sum = new int[size];

                    Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: генерация векторов...");

                    // Заполняем векторы случайными числами и вычисляем сумму
                    for (int i = 0; i < size; i++)
                    {
                        vectorA[i] = rand.Next(1, 101); // от 1 до 100
                        vectorB[i] = rand.Next(1, 101);
                        sum[i] = vectorA[i] + vectorB[i];
                    }

                    // Выводим исходные векторы
                    Console.WriteLine("Вектор A: [" + string.Join(", ", vectorA) + "]");
                    Console.WriteLine("Вектор B: [" + string.Join(", ", vectorB) + "]");

                    Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: вычисления завершены.");

                    // 3. Вызываем обратный вызов с результатом
                    callback?.Invoke(sum);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в фоновом потоке: {ex.Message}");
                }
            });
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Асинхронное сложение случайных векторов с обратным вызовом ===\n");

            // Ввод размера
            Console.Write("Введите размер векторов: ");
            if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0)
            {
                Console.WriteLine("Некорректный размер.");
                return;
            }
            // 4. Создаём объект синхронизации
            var doneEvent = new ManualResetEvent(false);

            // 5. Запускаем асинхронную операцию с обратным вызовом в виде ЛЯМБДА-ВЫРАЖЕНИЯ
            AddRandomVectorsAsync(size, result =>
            {
                Console.WriteLine("\n[Обратный вызов] Результат сложения векторов:");
                Console.WriteLine("[" + string.Join(", ", result) + "]");

                // Сигнализируем, что обратный вызов завершён
                doneEvent.Set();
            });

            // 6. Ждём завершения фоновой операции
            Console.WriteLine("\nОжидание результата из фонового потока...");
            doneEvent.WaitOne(); // Блокирует до вызова Set()

            // 7. Теперь логично завершаем программу
            Console.WriteLine("\nНажмите любую клавишу для завершения.");
            Console.ReadKey();
        }
    }
}