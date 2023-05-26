namespace DotCon.Terminals;

public class CmdTerminal : Terminal
{
    public CmdTerminal() : base("cmd")
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