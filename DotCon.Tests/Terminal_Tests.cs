using DotCon.Models;
using DotCon.Terminals;

namespace DotCon.Tests;

public class Terminal_Tests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var bash = Terminal.UseBashShell();
        
        // Act
        var result = bash.Run("echo 'Hello World!'");
        
        // Assert
        Assert.Equal("Hello World!\n", result);
    }
}