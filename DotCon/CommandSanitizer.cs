namespace DotCon;

public class CommandSanitizer
{
    private static readonly string[] SpecialCharacters = { "&", "|", ";", "$", ">", "<", "`", "\\", "'", "\"" };
    
    public static string SanitizeCommand(string command)
    {
        foreach (var character in SpecialCharacters)
        {
            command = command.Replace(character, "\\" + character);
        }

        return command;
    }
    
    public static string DeSensitiveCommand(string command)
    {
        foreach (var character in SpecialCharacters)
        {
            command = command.Replace("\\" + character, character);
        }

        return command;
    }
}