using DotCon.Models;

namespace DotCon.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        

        var terminal = new Terminal("bash");

        terminal.StoreScript("echo", "echo 'Hello World!'");
        
        //var commandOutput = terminal.Execute("echo");

        var commandOutput = terminal.Run("cd ~/Desktop && mkdir test123456789");
        Console.WriteLine(commandOutput);
    }
}