using System.Text;

class Program
{
    static void Main()
    {
        while (true) {
            Console.Write("$ ");
        
            // Wait for user input
            string?input = Console.ReadLine() ?? "";
            
            string[] tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            tokens = tokens.Length == 0 ? [""] : tokens;

            string command = tokens[0];
            
            if (CompareCommand(command, "exit")) { break; }

            if (CompareCommand(command, "echo")) {
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
