using System.Diagnostics;
using System.Text;
using DotCon.Models;
using DotCon.Terminals;

namespace DotCon;

public abstract class Terminal : ITerminal
{
    protected abstract string GetTerminalExecutable();
    protected abstract string GetTerminalArguments(string command);
    protected abstract string FileExtension { get; }
    
    private static TerminalOptions _terminalOptions = new();
    private Shell Shell { get; }
    
    protected Terminal(Shell shell)
    {
        Shell = shell;
    }

    public static Terminal UseBashShell(Action<TerminalOptions>? configureOptions = null)
    {
        ConfigureOptions(configureOptions);
        return new BashTerminal();
    }
    
    public static Terminal UseCmdShell(Action<TerminalOptions>? configureOptions = null)
    {
        ConfigureOptions(configureOptions);
        return new CmdTerminal();
    }
    
    public static Terminal UsePowershellShell(Action<TerminalOptions>? configureOptions = null)
    {
        ConfigureOptions(configureOptions);
        return new PowershellTerminal();
    }
    
    public static Terminal UseZshShell(Action<TerminalOptions>? configureOptions = null)
    {
        ConfigureOptions(configureOptions);
        return new ZshTerminal();
    }
    
    private static void ConfigureOptions(Action<TerminalOptions>? configureOptions)
    {
        if (configureOptions == null) return;

        var options = new TerminalOptions();
        configureOptions(options);
        _terminalOptions = options;
    }

    protected abstract string GetTerminalArguments(string command, string arguments);

    private ProcessStartInfo GetProcessStartInfo(string command)
    {
        return new ProcessStartInfo
        {
            FileName = string.IsNullOrEmpty(Shell.ToString()) ? GetTerminalExecutable() : Shell.ToString(),
            Arguments = File.Exists(command) ? command : GetTerminalArguments(command),
            RedirectStandardOutput = _terminalOptions.RedirectStandardOutput,
            UseShellExecute = _terminalOptions.UseShellExecute,
            CreateNoWindow = _terminalOptions.CreateNoWindow,
            WorkingDirectory = _terminalOptions.WorkingDirectory
        };
    }

    private static async Task ReadOutputAsync(Process process, StringBuilder outputBuilder)
    {
        while (!process.StandardOutput.EndOfStream)
        {
            var line = await process.StandardOutput.ReadLineAsync() ?? string.Empty;
            outputBuilder.AppendLine(line);
        }
    }
    
    /// <summary>
    /// Tries to execute a script action and captures the output or exception message.
    /// </summary>
    /// <param name="filePath">The script file to execute.</param>
    /// <param name="output">The output of the executed script action or the exception message if an error occurs.</param>
    /// <returns>True if the script action executed successfully; otherwise, false.</returns>
    public bool TryExecuteScript(string filePath, out string output)
    {
       if (!File.Exists(filePath)) throw new FileNotFoundException($"The file '{filePath}' does not exist.");

       if (_terminalOptions.CheckForFileExtension) CheckFileExtension(filePath);

       try
       {
           output = Run(filePath);
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
    /// <param name="filePath">The script file to execute.</param>
    /// <returns>The output of the executed script.</returns>
    public string ExecuteScript(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"The file '{filePath}' does not exist.");
        
        if (_terminalOptions.CheckForFileExtension) CheckFileExtension(filePath);
        
        return Run(filePath);
    }

    /// <summary>
    /// Executes a script asynchronously by retrieving the corresponding actions, parsing the arguments, and running the resulting command.
    /// </summary>
    /// <param name="filePath">The script file to execute.</param>
    /// <returns>A task representing the asynchronous operation, returning the output of the executed script.</returns>
    public async Task<string> ExecuteScriptAsync(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"The file '{filePath}' does not exist.");
        
        if (_terminalOptions.CheckForFileExtension) CheckFileExtension(filePath);
        
        return await RunAsync(filePath);
    }
    
    private void CheckFileExtension(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        if (extension != FileExtension) throw new ArgumentException($"The file extension '{extension}' is not supported.");
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

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start process.");
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

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start process.");
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
}