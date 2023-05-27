using System.Threading.Channels;
using DotCon;

Console.WriteLine("Hello and welcome to DotCon!\n");

Console.WriteLine("""
DotCon is a lightweight library that provides a simple and consistent interface for executing shell commands in various terminal environments. 
It follows the Factory Method design pattern to create terminal instances based on different shells (Bash, Cmd, PowerShell, Zsh).
""");


string userInput = string.Empty;
string[] validUserInputs = { "0", "1", "2" };
bool validInput = false;
Console.WriteLine("Please enter a action you would like to do. . .\n");
while (!validInput)
{
    Console.Write("[0] Exit\n[1] Run a command\n[2] Show a basic command\n");
    Console.Write("Enter a number: ");
    userInput = Console.ReadLine() ?? string.Empty;

    if (userInput == string.Empty || !validUserInputs.Contains(userInput))
    {
        Console.WriteLine("Invalid input. Please try again.\n");
        continue;
    }

    validInput = true;
}

if (userInput == "0")
{
    Console.WriteLine("Exiting. . .");
    return;
}

if (userInput == "1")
{
    Console.WriteLine("Please enter a command you would like to run. . .\n");
    Console.Write("Enter a command: ");
    userInput = Console.ReadLine() ?? string.Empty;

    var terminal = Terminal.UseBashShell();
    var result = terminal.Run(userInput);

    Console.WriteLine($"\nResult: {result}");
    return;
}

if (userInput == "2")
{
    Console.WriteLine("""
var terminal = Terminal.UseBashShell();
terminal.StoreScript("echo", "echo 'HelloWorld'\");

// Output: HelloWorld\n

""");
}