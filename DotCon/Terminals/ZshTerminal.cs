using DotCon.Models;

namespace DotCon.Terminals;

public sealed class ZshTerminal : Terminal
{
    protected override string FileExtension { get; } = ".zsh";
    public ZshTerminal() : base(Shell.zsh) {}

    protected override string GetTerminalExecutable()
    {
        return Shell.zsh.ToString();
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