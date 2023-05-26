using System.Diagnostics;
using System.Text;
using DotCon.Terminals;

namespace DotCon;

public class Terminal : ScriptManager
{
    private static TerminalOptions _terminalOptions = new();

    public string Shell { get; }
    protected Terminal(string shell)
    {
        Shell = shell;
    }
    
    public static Terminal CreateTerminal(string shell, Action<TerminalOptions>? configureOptions = null)
    {
        ConfigureOptions(configureOptions);

        return shell.ToLower() switch
        {
            "bash" => new BashTerminal(),
            "cmd" => new CmdTerminal(),
            "powershell" => new PowershellTerminal(),
            "zsh" => new ZshTerminal(),
            _ => throw new ArgumentException("Invalid shell specified.")
        };
    }
    
    private static void ConfigureOptions(Action<TerminalOptions>? configureOptions)
    {
        if (configureOptions == null) return;

        var options = new TerminalOptions();
        configureOptions(options);
        _terminalOptions = options;
    }
}