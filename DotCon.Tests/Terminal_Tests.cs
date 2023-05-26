using DotCon.Models;

namespace DotCon.Tests;

public class Terminal_Tests
{
    [Fact]
    public void Test1()
    {
        var terminal = Terminal.CreateTerminal("bash");
        
        terminal.StoreScript("echo", "echo 'hello world'");

        var result = terminal.Run("echo 'hello world'");

        Console.WriteLine(result);
    }
}