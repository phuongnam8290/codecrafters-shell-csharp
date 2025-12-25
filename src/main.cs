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
            tokens = tokens.Length == 0 ? [""] : tokens; // Ensure a non-empty token list for command handling.
            string command = tokens[0];
            
            if (CompareCommand(command, "exit")) { break; }

            if (CompareCommand(command, "echo")) {
                // Print the input message. Index bounds checks are unnecessary because the tokens array is guaranteed to contain at least
                // one element; if only one token exists, there is nothing to print.
                String result = String.Join(" ", tokens, 1, tokens.Length - 1);
                Console.WriteLine(result);
                
                continue;
            }
            
            Console.WriteLine($"{input}: command not found");
        }
    }

    private static bool CompareCommand(string token, string command) {
        return token.Trim().Equals(command, StringComparison.OrdinalIgnoreCase);
    }
}
