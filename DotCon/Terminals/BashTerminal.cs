using DotCon.Models;

namespace DotCon.Terminals;

public sealed class BashTerminal : Terminal
{
    protected override string FileExtension { get; } = ".sh";
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