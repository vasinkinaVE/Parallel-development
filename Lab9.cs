using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContinuationTasksApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задачи продолжения: генерация массива и анализ ===\n");

            // Ввод размера массива
            Console.Write("Введите размер массива: ");
            string sizeInput = Console.ReadLine();
            int size = Convert.ToInt32(sizeInput);

            if (size <= 0)
            {
                Console.WriteLine("Размер должен быть положительным.");
                return;
            }

            // 1. Основная задача: генерация массива
            Task<int[]> generateTask = Task.Run(() =>
            {
                Console.WriteLine("[Основная задача] Генерация массива случайных чисел...");
                Random.Shared.NextBytes(new byte[1]); // просто для инициализации
                int[] array = new int[size];
                for (int i = 0; i < size; i++)
                {
                    array[i] = Random.Shared.Next(1, 101); // от 1 до 100
                }
                Console.WriteLine("[Основная задача] Массив сгенерирован.");
                return array;
            });

            // 2. Задача продолжения 1: подсчёт элементов, делящихся на 3
            Task<int> countDivisibleBy3Task = generateTask.ContinueWith(prevTask =>
            {
                int[] arr = prevTask.Result;
                int count = 0;
                foreach (int x in arr)
                {
                    if (x % 3 == 0)
                        count++;
                }
                Console.WriteLine($"[Продолжение 1] Количество элементов, делящихся на 3: {count}");
                return count;
            });

            // 3. Задача продолжения 2: поиск минимального элемента
            Task<int> findMinTask = generateTask.ContinueWith(prevTask =>
            {
                int[] arr = prevTask.Result;
                int min = arr[0];
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i] < min)
                        min = arr[i];
                }
                Console.WriteLine($"[Продолжение 2] Минимальный элемент: {min}");
                return min;
            });

            // 4. Финальная задача: дождаться обеих продолжений и вывести итог
            Task finalTask = Task.WhenAll(countDivisibleBy3Task, findMinTask).ContinueWith(_ =>
            {
                Console.WriteLine("\nВсе задачи завершены.");
            });

            // Ожидание завершения всей цепочки
            finalTask.Wait();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}