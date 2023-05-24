

using DotCon;

Console.WriteLine("Hello and welcome to DotCon!\n");

Console.WriteLine("DotCon is a simple library that allows you to run commands in a terminal.");
Console.WriteLine("It is cross-platform and supports Windows, Linux and macOS.");
Console.WriteLine("It is also open-source and available on GitHub.\n");

Console.WriteLine("This is a simple example of how to use DotCon.");

string userInput = string.Empty;
string[] validUserInputs = { "0", "1", "2" };
bool validInput = false;

Console.WriteLine("Please enter a action you would like to do. . .\n");
while (!validInput)
{
    Console.Write("[0] Exit\n[1] Run a command\n[2] Show a basic command\n");
    Console.Write("Enter a number: ");
    userInput = Console.ReadLine();

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
    userInput = Console.ReadLine();
    
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
