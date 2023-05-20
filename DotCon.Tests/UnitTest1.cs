using DotCon.Models;

namespace DotCon.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // // Arrange
        // var script = new Script
        // {
        //     Name = "Test",
        //     Action = "echo 'Hello World!'"
        // };
        // var terminal = new Terminal("bash");
        //
        // terminal.StoreScript(script.Name, script.Action);
        //
        // // Act
        // var result = await terminal.RunAsync(script.Name);
        //
        // // Assert
        // Assert.Equal("Hello World!", result);

        var terminal = new Terminal("bash");

        terminal.StoreScript("echo", "echo 'Hello World!'");
        
        var commandOutput = await terminal.RunAsync("echo");
        
        //var commandOutput = terminal..runExecuteCommand("echo 'Hello World!'");
        
        Console.WriteLine(commandOutput);
    }
}