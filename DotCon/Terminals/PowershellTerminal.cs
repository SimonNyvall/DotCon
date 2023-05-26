namespace DotCon.Terminals;

public class PowershellTerminal : Terminal
{
    public PowershellTerminal() : base("powershell")
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