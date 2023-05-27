using DotCon.Models;

namespace DotCon.Terminals;

public class PowershellTerminal : Terminal
{
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