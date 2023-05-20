using System.Diagnostics;
using System.Text;

namespace DotCon;

public class Terminal
{
    private readonly OptionsBuilder _options;
    private readonly Dictionary<string, string[]> _actions = new();

    public Terminal(OptionsBuilder options)
    {
        _options = options;

        if (string.IsNullOrEmpty(_options.ShellPath)) throw new Exception("ShellPath is not configured.");
    }

    public Terminal(string shellPath)
    {
        _options = new OptionsBuilder
        {
            ShellPath = shellPath
        };
    }

    public bool TryRun(string action, out string output)
    {
        try
        {
            output = Run(action);
            return true;
        }
        catch (Exception exception)
        {
            output = exception.Message;
            return false;
        }
    }
    
    public string Run(string action)
    {
        var output = string.Empty;
        var command = _actions[action][0];
        var arguments = ParseArgs(_actions[action][1..]);
        
        var startInfo = new ProcessStartInfo
        {
            FileName = (string.IsNullOrEmpty(_options.ShellPath)) ? GetTerminalExecutable() : _options.ShellPath,
            Arguments = GetTerminalArguments(command, arguments),
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo)!;
        var outputBuilder = new StringBuilder();
        while (!process.StandardOutput.EndOfStream)
        {
            var line = process.StandardOutput.ReadLine()!;
            outputBuilder.AppendLine(line);
        }
        output = outputBuilder.ToString();

        return output;
    }

    public async Task<string> RunAsync(string action)
    {
        var output = string.Empty;
        var command = _actions[action][0];
        var arguments = ParseArgs(_actions[action][1..]);


        var startInfo = new ProcessStartInfo
        {
            FileName = GetTerminalExecutable(),
            Arguments = GetTerminalArguments(command, arguments),
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        var outputBuilder = new StringBuilder();
        while (process is { StandardOutput.EndOfStream: false })
        {
            var line = await process.StandardOutput.ReadLineAsync() ?? string.Empty;
            outputBuilder.AppendLine(line);
        }
        
        output = outputBuilder.ToString();

        return output;
    }

    private static string ParseArgs(IEnumerable<string> actions)
    {
        var argsBuilder = new StringBuilder();

        foreach (var action in actions)
        {
            argsBuilder.Append(action);
            argsBuilder.Append(' ');
        }

        return argsBuilder.ToString();
    }
    private string GetTerminalExecutable()
    {
        return (Environment.OSVersion.Platform == PlatformID.Win32NT) ? "cmd.exe" : "bash";
    }

    private string GetTerminalArguments(string command, string arguments)
    {
        return (Environment.OSVersion.Platform == PlatformID.Win32NT) 
            ? "/c " + command + " " + arguments 
            : "-c \"" + command + " " + arguments + "\"";
    }

    public void StoreScript(string key, List<string> actions)
    {
        StoreScript(key, actions.ToArray());
    }
    
    public void StoreScript(string key, params string[] actions)
    {
        _actions.Add(key, actions);
    }
    
    public KeyValuePair<string, string[]>[] GetScripts()
    {
        return _actions.ToArray();
    }
    
    public void RemoveScript(string key)
    {
        _actions.Remove(key);
    }
    
    public void ClearScripts()
    {
        _actions.Clear();
    }
    
    public void UpdateScript(string key, List<string> actions)
    {
        UpdateScript(key, actions.ToArray());
    }
    
    public void UpdateScript(string key, params string[] actions)
    {
        _actions[key] = actions;
    }

    
}