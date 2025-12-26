using System.Text;

class Program
{
    static void Main()
    {
        while (true) {
            Console.Write("$ ");
        
            // Wait for user input
            string?input = Console.ReadLine() ?? "";
            
            // Normalize user input: split by whitespace, remove empty entries.
            string[] tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            
            (string command, string[] arguments) = tokens switch {
                null or [] => ("", Array.Empty<String>()),
                [var cmd] => (cmd, Array.Empty<String>()),
                [var cmd, .. var args] => (cmd, args)
            };
            
            if (CompareCommand(command, "exit")) { break; }

            if (CompareCommand(command, "echo")) {
                // Print the input message. If there are no arguments, the result is an empty string ("").
                String result = String.Join(" ", arguments);
                Console.WriteLine(result);
                
                continue;
            }

            if (CompareCommand(command, "type")) {
                Console.WriteLine(
                    Enum.TryParse<Command>(arguments[0], true, out _)
                    ? $"{arguments[0]} is a shell builtin"
                    : $"{arguments[0]}: not found");

                continue;
            }
            
            Console.WriteLine($"{input}: command not found");
        }
    }

    public enum Command { Exit, Echo, Type}
    
    private static bool CompareCommand(string token, string command) {
        return token.Trim().Equals(command, StringComparison.OrdinalIgnoreCase);
    }
}
