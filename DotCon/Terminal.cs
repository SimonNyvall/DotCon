using System.Diagnostics;
using System.Text;

namespace DotCon;

public class Terminal
{
    private string _shell;
    private readonly Dictionary<string, string[]> _actions = new();
    private static TerminalOptions _terminalOptions = new();

    public Terminal(string shell)
    {
        _shell = shell;
    }

    /// <summary>
    /// Sets the shell to use bash as the default shell and returns a new instance of Terminal.
    /// </summary>
    /// <returns>Terminal</returns>
    public static Terminal UseBashShell(Action<TerminalOptions> configureOptions = null)
    {
        if (configureOptions != null)
        {
            var options = new TerminalOptions();
            configureOptions(options);   
            _terminalOptions = options;
        }

        return new Terminal("bash");
    }
    
    /// <summary>
    /// Sets the shell to use cmd as the default shell and returns a new instance of Terminal.
    /// </summary>
    /// <returns>Terminal</returns>
    public static Terminal UseCmdShell(Action<TerminalOptions> configureOptions = null)
    {
        if (configureOptions != null)
        {
            var options = new TerminalOptions();
            configureOptions(options);
        }
        
        return new Terminal("cmd.exe");
    }

    /// <summary>
    /// Tries to execute a script action and captures the output or exception message.
    /// </summary>
    /// <param name="action">The script action to execute.</param>
    /// <param name="output">The output of the executed script action or the exception message if an error occurs.</param>
    /// <returns>True if the script action executed successfully; otherwise, false.</returns>
    public bool TryExecuteScript(string action, out string output)
    {
        try
        {
            output = ExecuteScript(action);
            return true;
        }
        catch (Exception exception)
        {
            output = exception.Message;
            return false;
        }
    }
    
    /// <summary>
    /// Executes a script by retrieving the corresponding actions, parsing the arguments, and running the resulting command.
    /// </summary>
    /// <param name="action">The script action or key representing the script to execute.</param>
    /// <returns>The output of the executed script.</returns>
    public string ExecuteScript(string action)
    {
        var actions = GetScriptFromKey(action);

        var command = ParseArgs(actions);

        return Run(command);
    }

    /// <summary>
    /// Executes a script asynchronously by retrieving the corresponding actions, parsing the arguments, and running the resulting command.
    /// </summary>
    /// <param name="action">The script action or key representing the script to execute.</param>
    /// <returns>A task representing the asynchronous operation, returning the output of the executed script.</returns>
    public async Task<string> ExecuteScriptAsync(string action)
    {
       var actions = GetScriptFromKey(action);
       
       var command = ParseArgs(actions);
       
       return await RunAsync(command);
    }

    /// <summary>
    /// Tries to run a shell command and captures the output or exception message.
    /// </summary>
    /// <param name="command">The shell command to run.</param>
    /// <param name="output">The output of the executed command or the exception message if an error occurs.</param>
    /// <returns>True if the command executed successfully; otherwise, false.</returns>
    public bool TryRun(string command, out string output)
    {
        try
        {
            output = Run(command);
            return true;
        }
        catch (Exception exception)
        {
            output = exception.Message;
            return false;
        }
    }

    /// <summary>
    /// Executes a shell command synchronously and captures the output.
    /// </summary>
    /// <param name="command">The shell command to execute.</param>
    /// <returns>The output of the executed command.</returns>
    public string Run(string command)
    {
        var startInfo = GetProcessStartInfo(command);
        
        using var process = Process.Start(startInfo)!;
        var outputBuilder = new StringBuilder();
        while (!process.StandardOutput.EndOfStream)
        {
            var line = process.StandardOutput.ReadLine()!;
            outputBuilder.AppendLine(line);
        }
        var output = outputBuilder.ToString();

        return output;
    }

    /// <summary>
    /// Executes a shell command asynchronously and captures the output.
    /// </summary>
    /// <param name="command">The shell command to execute.</param>
    /// <returns>A task representing the asynchronous operation, returning the output of the executed command.</returns>
    public async Task<string> RunAsync(string command)
    {
        var startInfo = GetProcessStartInfo(command);
    
        using var process = Process.Start(startInfo);
        var outputBuilder = new StringBuilder();
        
        var readOutputTask = ReadOutputAsync(process, outputBuilder);
        await readOutputTask;

        var output = outputBuilder.ToString();
    
        return output;
    }
    
    /// <summary>
    /// Executes a shell command asynchronously with a specified timeout and captures the output.
    /// </summary>
    /// <param name="command">The shell command to execute.</param>
    /// <param name="timeout">The maximum duration to wait for the command to complete.</param>
    /// <returns>A task representing the asynchronous operation, returning the output of the executed command.</returns>
    /// <exception cref="TimeoutException">Thrown when the command execution exceeds the specified timeout.</exception>
    public async Task<string> RunAsync(string command, TimeSpan timeout)
    {
        var startInfo = GetProcessStartInfo(command);

        using var process = Process.Start(startInfo);
        var outputBuilder = new StringBuilder();

        var readOutputTask = ReadOutputAsync(process, outputBuilder);
        var completedTask = await Task.WhenAny(readOutputTask, Task.Delay(timeout));

        if (completedTask == readOutputTask)
        {
            await readOutputTask;
        }
        else
        {
            process.Kill();
            throw new TimeoutException("Command execution timed out.");
        }

        var output = outputBuilder.ToString();

        return output;
    }

    private async Task ReadOutputAsync(Process process, StringBuilder outputBuilder)
    {
        while (process is { StandardOutput.EndOfStream: false })
        {
            var line = await process.StandardOutput.ReadLineAsync() ?? string.Empty;
            outputBuilder.AppendLine(line);
        }
    }

    
    private ProcessStartInfo GetProcessStartInfo(string command)
    {
        return new ProcessStartInfo
        {
            FileName = (string.IsNullOrEmpty(_shell)) ? GetTerminalExecutable() : _shell,
            Arguments = GetTerminalArguments(command),
            RedirectStandardOutput = _terminalOptions.RedirectStandardOutput,
            UseShellExecute = _terminalOptions.UseShellExecute,
            CreateNoWindow = _terminalOptions.CreateNoWindow
        };
    }
    
    private static string ParseArgs(IEnumerable<string> actions)
    {
        var argsBuilder = new StringBuilder();

        foreach (var action in actions)
        {
            argsBuilder.Append(action);
            argsBuilder.Append(" && ");
        }

        return argsBuilder.ToString();
    }
    private string GetTerminalExecutable()
    {
        return (Environment.OSVersion.Platform == PlatformID.Win32NT) ? "cmd.exe" : "bash";
    }

    private string[] GetScriptFromKey(string key)
    {
        if (!_actions.ContainsKey(key)) throw new Exception("Script not found.");
        
        var script = _actions[key];

        return script;
    }
    
    private string GetTerminalArguments(string command, string arguments = "")
    {
        return (Environment.OSVersion.Platform == PlatformID.Win32NT) 
            ? "/c " + command + " " + arguments 
            : "-c \"" + command + " " + arguments + "\"";
    }

    /// <summary>
    /// Stores a script with the specified key and list of actions.
    /// </summary>
    /// <param name="key">The key to associate with the script.</param>
    /// <param name="actions">The list of actions that make up the script.</param>
    public void StoreScript(string key, List<string> actions)
    {
        StoreScript(key, actions.ToArray());
    }
    
    /// <summary>
    /// Stores a script with the specified key and array of actions.
    /// </summary>
    /// <param name="key">The key to associate with the script.</param>
    /// <param name="actions">The array of actions that make up the script.</param>
    public void StoreScript(string key, params string[] actions)
    {
        _actions.Add(key, actions);
    }
    
    /// <summary>
    /// Retrieves all stored scripts as an array of key-value pairs.
    /// </summary>
    /// <returns>An array of key-value pairs representing the stored scripts.</returns>
    public KeyValuePair<string, string[]>[] GetScripts()
    {
        return _actions.ToArray();
    }
    
    /// <summary>
    /// Removes the script associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the script to remove.</param>
    public void RemoveScript(string key)
    {
        _actions.Remove(key);
    }
    
    /// <summary>
    /// Clears all stored scripts.
    /// </summary>
    public void ClearScripts()
    {
        _actions.Clear();
    }
    
    /// <summary>
    /// Updates the script associated with the specified key using the provided list of actions.
    /// </summary>
    /// <param name="key">The key of the script to update.</param>
    /// <param name="actions">The list of actions to replace the existing actions in the script.</param>
    public void UpdateScript(string key, List<string> actions)
    {
        UpdateScript(key, actions.ToArray());
    }
    
    /// <summary>
    /// Updates the script associated with the specified key using the provided array of actions.
    /// </summary>
    /// <param name="key">The key of the script to update.</param>
    /// <param name="actions">The array of actions to replace the existing actions in the script.</param>
    public void UpdateScript(string key, params string[] actions)
    {
        _actions[key] = actions;
    }

    
}