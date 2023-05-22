namespace DotCon;

public class TerminalOptions
{
    public bool CreateNoWindow { get; set; } = true;
    public bool RedirectStandardOutput { get; set; } = true;
    public bool UseShellExecute { get; set; } = false;
}