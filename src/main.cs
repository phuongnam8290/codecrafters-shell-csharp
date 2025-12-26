using System.Text;
using codecrafters_shell.Commands.BuiltIn;

class Program
{
    static void Main()
    {
        while (true) {
            Console.Write("$ ");
        
            // Wait for user input
            string?input = Console.ReadLine() ?? "";
            ParseCommandLine(input, out string command, out string[] arguments);

            if (Enum.TryParse(command, true, out BuiltInCommand builtInCommand)) {
                switch (builtInCommand) {
                    case BuiltInCommand.Echo:
                        HandleEchoCommand(arguments);
                        continue;
                    
                    case BuiltInCommand.Type:
                        HandleTypeCommand(arguments);
                        continue;
                    
                    case BuiltInCommand.Exit: 
                        return;
                }
            }
            
            Console.WriteLine($"{input}: command not found");
        }
    }

    private static void ParseCommandLine(string input, out string command, out string[] arguments) {
        // Normalize user input: split by whitespace, remove empty entries.
        string[] tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
         
        // Command parsing rule:
        // - no tokens → empty command and arguments
        // - one token → command only
        // - multiple tokens → first is command, remainder are arguments
        (command, arguments) = tokens switch {
            null or [] => ("", Array.Empty<String>()),
            [var cmd] => (cmd, Array.Empty<String>()),
            [var cmd, .. var args] => (cmd, args)
        };
    }

    private static void HandleEchoCommand(string[] arguments) {
        string result = string.Join(" ", arguments);
        Console.WriteLine(result);
    }

    private static void HandleTypeCommand(string[] arguments) {
        if (arguments.Length == 0) {
            Console.WriteLine(": not found");
            return;
        }
        
        Console.WriteLine(
            Enum.TryParse<BuiltInCommand>(arguments[0], true, out _)
                ? $"{arguments[0]} is a shell builtin"
                : $"{arguments[0]}: not found");
    }
}
