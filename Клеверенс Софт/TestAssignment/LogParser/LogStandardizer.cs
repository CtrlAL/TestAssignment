namespace TestAssignment.LogParser;

public static class LogStandardizer
{
    public static void Process(string inputPath, string outputPath, string problemsPath)
    {
        var lines = File.ReadAllLines(inputPath);
        var validLines = new List<string>();
        var invalidLines = new List<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var entry = LogFormatParser.TryParse(line);
            if (entry != null)
                validLines.Add(FormatOutput(entry));
            else
                invalidLines.Add(line);
        }

        File.WriteAllLines(outputPath, validLines);
        if (invalidLines.Count > 0)
            File.WriteAllLines(problemsPath, invalidLines);
    }

    private static string FormatOutput(LogEntry entry)
    {
        return $"{entry.Date}\t{entry.Time}\t{entry.Level}\t{entry.CalledMethod}\t{entry.Message}";
    }
}
