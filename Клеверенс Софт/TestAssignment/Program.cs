using TestAssignment.Compression;
using TestAssignment.LogParser;
using TestAssignment.ThreadSafety;

Console.WriteLine("=== Тестовое задание ===");
Console.WriteLine();

while (true)
{
    Console.WriteLine("Выберите задачу:");
    Console.WriteLine("1 - Компрессия строк");
    Console.WriteLine("2 - Thread-safe счётчик");
    Console.WriteLine("3 - Парсер логов");
    Console.WriteLine("0 - Выход");
    Console.Write("> ");

    var key = Console.ReadKey(true).KeyChar;

    switch (key)
    {
        case '1':
            DemoCompression();
            break;
        case '2':
            DemoCounter();
            break;
        case '3':
            DemoLogParser();
            break;
        case '0':
            return;
    }
}

static void DemoCompression()
{
    Console.Clear();
    Console.WriteLine("=== Компрессия строк ===");
    Console.Write("Введите строку: ");
    var input = Console.ReadLine() ?? "";

    var compressed = StringCompressor.Compress(input);
    var decompressed = StringCompressor.Decompress(compressed);

    Console.WriteLine($"Исходная:     {input}");
    Console.WriteLine($"Сжатая:       {compressed}");
    Console.WriteLine($"Восстановленная: {decompressed}");
    Console.WriteLine($"Совпадают:    {input == decompressed}");
    Console.WriteLine("Нажмите любую клавишу...");
    Console.ReadKey(true);
    Console.Clear();
}

static void DemoCounter()
{
    Console.Clear();
    Console.WriteLine("=== Thread-safe счётчик ===");
    Console.WriteLine("Демонстрация параллельного чтения/записи");

    var threads = new List<Thread>();
    for (int i = 0; i < 5; i++)
    {
        int id = i;
        var t = new Thread(() =>
        {
            for (int j = 0; j < 3; j++)
            {
                if (id % 2 == 0)
                {
                    ConcurrentCounter.AddToCount(id + 1);
                    Console.WriteLine($"[Поток {id}] Добавил {id + 1}, значение: {ConcurrentCounter.GetCount()}");
                }
                else
                {
                    Console.WriteLine($"[Поток {id}] Прочитал: {ConcurrentCounter.GetCount()}");
                }
                Thread.Sleep(10);
            }
        });
        threads.Add(t);
        t.Start();
    }

    foreach (var t in threads) t.Join();

    Console.WriteLine($"Финальное значение: {ConcurrentCounter.GetCount()}");
    Console.WriteLine("Нажмите любую клавишу...");
    Console.ReadKey(true);
    Console.Clear();
}

static void DemoLogParser()
{
    Console.Clear();
    Console.WriteLine("=== Парсер логов ===");
    Console.Write("Путь к входному файлу: ");
    var input = Console.ReadLine() ?? "";
    Console.Write("Путь к выходному файлу: ");
    var output = Console.ReadLine() ?? "";

    var problems = Path.Combine(
        Path.GetDirectoryName(output) ?? ".",
        "problems.txt");

    try
    {
        LogStandardizer.Process(input, output, problems);
        Console.WriteLine("Готово!");
        Console.WriteLine($"Выходные данные: {output}");
        Console.WriteLine($"Проблемные записи: {problems}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

    Console.WriteLine("Нажмите любую клавишу...");
    Console.ReadKey(true);
    Console.Clear();
}
