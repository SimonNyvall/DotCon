using DotCon.Models;

namespace DotCon.Tests;

public class Terminal_Tests
{
    [Fact]
    public void Run_ReturnsHelloWorld_WithInputEchoHelloWorld()
    {
        // Arrange
        var terminal = Terminal.UseBashShell();

        // Act
        var result = terminal.Run("echo 'HelloWorld'");

        // Assert
        Assert.Equal("HelloWorld\n", result);
    }

    [Fact]
    public void TryRun_ReturnsHelloWorld_WithInputEchoHelloWorld()
    {
        // Arrange
        var terminal = Terminal.UseBashShell();

        // Act
        var result = terminal.TryRun("echo 'HelloWorld'", out var output);

        // Assert
        Assert.Equal("HelloWorld\n", output);
        Assert.True(result);
    }
    
    [Fact]
    public async Task RunAsync_ReturnsHelloWorld_WithInputEchoHelloWorld()
    {
        // Arrange
        var terminal = Terminal.UseBashShell();

        // Act
        var result = await terminal.RunAsync("echo 'HelloWorld'");

        // Assert
        Assert.Equal("HelloWorld\n", result);
    }
    
    [Fact]
    public void Execute_ReturnsHelloWorld_WithInputEchoHelloWorld()
    {
        // Arrange
        var terminal = Terminal.UseBashShell();
        terminal.StoreScript("echo", "echo 'HelloWorld'");
        
        // Act
        var result = terminal.ExecuteScript("echo");

        // Assert
        Assert.Equal("HelloWorld\n", result);
    }
}