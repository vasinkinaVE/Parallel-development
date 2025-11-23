using System;
using System.Threading;

namespace CharacterSearchApp
{
    class Program
    {
        // Структура для передачи данных в поток
        public struct SearchData
        {
            public string Text;      // строка для поиска
            public char TargetChar;  // искомый символ
        }

        // Метод, который будет выполняться в потоке
        static void SearchCharacterInString(object dataObject)
        {
            // Приводим объект к нужному типу
            SearchData data = (SearchData)dataObject;

            // Получаем ID текущего потока для вывода
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Поток {threadId}: Начинаю поиск символа '{data.TargetChar}' в строке: \"{data.Text}\"");

            // Выполняем поиск
            bool found = data.Text.Contains(data.TargetChar);

            // Выводим результат
            if (found)
                Console.WriteLine($"Поток {threadId}: Символ '{data.TargetChar}' найден!");
            else
                Console.WriteLine($"Поток {threadId}: Символ '{data.TargetChar}' НЕ найден.");

        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Поиск символа в строке с использованием параметризированного потока ===\n");

            // Ввод строки и символа
            Console.Write("Введите строку: ");
            string inputText = Console.ReadLine();

            Console.Write("Введите искомый символ: ");
            char targetChar = Console.ReadKey().KeyChar;
            Console.WriteLine(); // переводим курсор на новую строку

            // Создаем структуру с данными
            SearchData searchData = new SearchData
            {
                Text = inputText,
                TargetChar = targetChar
            };

            // Создаем поток, передавая ему метод с параметром
            Thread searchThread = new Thread(SearchCharacterInString);
            searchThread.Start(searchData); // передаём данные в Start()

            // Ожидаем завершения потока
            Console.WriteLine("\nОжидание завершения потока...");
            searchThread.Join();

            Console.WriteLine("\nПоиск завершён.");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}