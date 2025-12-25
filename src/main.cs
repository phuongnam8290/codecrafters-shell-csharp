class Program
{
    static void Main()
    {
        while (true) {
            Console.Write("$ ");
        
            // Wait for user input
            string? command = Console.ReadLine() ?? "";
            bool isExitCommand = command.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase);
            
            if (isExitCommand)
            {
                break;
            }
            Console.WriteLine($"{command}: command not found");
        }
    }
}
