using System.Text.RegularExpressions;

namespace TestAssignment.LogParser;

public static partial class LogFormatParser
{
    private static readonly Regex Format1Regex = Format1Pattern();
    private static readonly Regex Format2Regex = Format2Pattern();

    public static LogEntry? TryParse(string line)
    {
        return TryParseFormat1(line) ?? TryParseFormat2(line);
    }

    private static LogEntry? TryParseFormat1(string line)
    {
        var match = Format1Regex.Match(line);
        if (!match.Success)
            return null;

        var dateTime = match.Groups[1].Value;
        var level = match.Groups[2].Value;
        var message = match.Groups[3].Value;

        var parts = dateTime.Split(' ');
        var date = parts[0];
        var time = parts[1];

        return new LogEntry
        {
            Date = NormalizeDate(date),
            Time = time,
            Level = NormalizeLevel(level),
            CalledMethod = "DEFAULT",
            Message = message
        };
    }

    private static LogEntry? TryParseFormat2(string line)
    {
        var match = Format2Regex.Match(line);
        if (!match.Success)
            return null;

        var dateTime = match.Groups[1].Value;
        var level = match.Groups[2].Value.Trim();
        var calledMethod = match.Groups[3].Value.Trim();
        var message = match.Groups[4].Value.Trim();

        var parts = dateTime.Split(' ');
        var date = parts[0];
        var time = parts[1];

        return new LogEntry
        {
            Date = NormalizeDate(date),
            Time = time,
            Level = NormalizeLevel(level),
            CalledMethod = string.IsNullOrEmpty(calledMethod) ? "DEFAULT" : calledMethod,
            Message = message
        };
    }

    private static string NormalizeDate(string date)
    {
        if (date.Contains('.'))
        {
            var p = date.Split('.');
            return $"{p[2]}-{p[1]:D2}-{p[0]:D2}";
        }
        return date;
    }

    private static string NormalizeLevel(string level)
    {
        return level.ToUpperInvariant() switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            _ => level.ToUpperInvariant()
        };
    }

    [GeneratedRegex(@"^(\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d+) (INFORMATION|INFO|WARNING|WARN|ERROR|DEBUG) (.+)$")]
    private static partial Regex Format1Pattern();

    [GeneratedRegex(@"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+)\| *([^|]+)\| *\d+\| *([^|]*)\|(.+)$")]
    private static partial Regex Format2Pattern();
}
