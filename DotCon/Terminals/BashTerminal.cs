namespace DotCon.Terminals;

public class BashTerminal : Terminal
{
    public BashTerminal() : base("bash")
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