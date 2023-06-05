namespace DotCon.Terminals;

public interface ITerminal
{
    bool TryExecuteScript(string filePath, out string output);
    
    string ExecuteScript(string filePath);

    Task<string> ExecuteScriptAsync(string filePath);

    bool TryRun(string command, out string output);
    
    string Run(string command);
    
    Task<string> RunAsync(string command);
    
}