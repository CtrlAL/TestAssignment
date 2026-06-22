namespace TestAssignment.LogParser;

public class LogEntry
{
    public string Date { get; set; } = "";
    public string Time { get; set; } = "";
    public string Level { get; set; } = "";
    public string CalledMethod { get; set; } = "";
    public string Message { get; set; } = "";
}
