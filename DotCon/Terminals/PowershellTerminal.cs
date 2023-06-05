using DotCon.Models;

namespace DotCon.Terminals;

public sealed class PowershellTerminal : Terminal
{
    protected override string FileExtension { get; } = ".ps1";
    public PowershellTerminal() : base(Shell.powershell) {}

    protected override string GetTerminalExecutable()
    {
        return Shell.powershell.ToString();
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