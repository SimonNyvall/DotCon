using DotCon.Models;

namespace DotCon.Terminals;

public class BashTerminal : Terminal
{
    public BashTerminal() : base(Shell.bash) {}

    protected override string GetTerminalExecutable()
    {
        return Shell.bash.ToString();
    }

    protected override string GetTerminalArguments(string command)
    {
        return $"-c \"{command}\"";
    }

    protected override string GetTerminalArguments(string command, string arguments)
    {
        return $"-c \"{command}\" {arguments}";
    }
}