namespace DotCon.Terminals;

public class ZshTerminal : Terminal
{
    public ZshTerminal() : base("zsh")
    {
    }

    protected override string GetTerminalExecutable()
    {
        throw new NotImplementedException();
    }

    protected override string GetTerminalArguments(string command)
    {
        throw new NotImplementedException();
    }

    protected override string GetTerminalArguments(string command, string arguments)
    {
        throw new NotImplementedException();
    }
}