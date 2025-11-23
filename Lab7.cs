using System;
using System.Collections.Generic;
using System.Threading;

namespace EncryptionApp
{
    // Класс элемента коллекции: исходная строка и зашифрованная
    public class EncryptedString
    {
        public string Original { get; }
        public string Encrypted { get; set; }

        public EncryptedString(string original)
        {
            Original = original ?? throw new ArgumentNullException(nameof(original));
            Encrypted = string.Empty;
        }
    }

    // Класс управления шифрованием с использованием пула потоков
    public class EncryptionManager
    {
        private readonly List<EncryptedString> _items = new List<EncryptedString>();
        private readonly int _shift; // сдвиг для шифрования (n)

        public EncryptionManager(int shift = 3)
        {
            _shift = shift;
        }

        // Добавляет строку в коллекцию и отправляет задачу шифрования в пул потоков
        public void Add(string originalString)
        {
            var item = new EncryptedString(originalString);
            lock (_items)
            {
                _items.Add(item);
            }

            // Отправляем задачу в пул потоков
            ThreadPool.QueueUserWorkItem(EncryptItem, item);
        }

        // Метод, выполняемый в фоновом потоке
        private void EncryptItem(object? state)
        {
            if (state is EncryptedString item)
            {
                try
                {
                    string encrypted = EncryptString(item.Original, _shift);
                    item.Encrypted = encrypted;

                    // Вывод результата с ID потока
                    Console.WriteLine(
                        $"Поток {Thread.CurrentThread.ManagedThreadId}: " +
                        $"'{item.Original}' → '{item.Encrypted}'"
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в потоке: {ex.Message}");
                }
            }
        }

        // Метод шифрования с задержкой
        private string EncryptString(string input, int shift)
        {
            // Искусственная задержка
            Thread.Sleep(2000);

            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (char)(input[i] + shift);
            }
            return new string(result);
        }
    }

    // Основной класс программы
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Шифрование строк с использованием пула потоков ===\n");

            // Создаём менеджер с заданным сдвигом (по умолчанию +3)
            var manager = new EncryptionManager(shift: 3);

            // Добавляем несколько строк для шифрования
            manager.Add("Hello");
            manager.Add("World");
            manager.Add("School");

            Console.WriteLine("\nЗадачи отправлены в пул потоков.");
            Console.WriteLine("Ожидание завершения (3 секунды)...\n");

            // Ждём завершения всех фоновых задач
            Thread.Sleep(5000); 

            Console.WriteLine("\nЗавершено.");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}