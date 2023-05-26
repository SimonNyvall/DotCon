using DotCon.Models;

namespace DotCon.Terminals;

public class CmdTerminal : Terminal
{
    public CmdTerminal() : base(Shell.cmd) {}

    protected override string GetTerminalExecutable()
    {
        return Shell.cmd.ToString();
    }

    protected override string GetTerminalArguments(string command)
    {
        return "/c \"" + command + "\" ";
    }

    protected override string GetTerminalArguments(string command, string arguments)
    {
        return "/c \"" + command + "\" " + arguments;
    }
}