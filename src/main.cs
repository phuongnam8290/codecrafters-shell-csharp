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
        
        if (Enum.TryParse<BuiltInCommand>(arguments[0], true, out _)) {
            Console.WriteLine($"{arguments[0]} is a shell builtin");
            return;
        }
        
        if (IsExecutableFileExist(arguments[0], out string pathIfExists)) {
            Console.WriteLine($"{arguments[0]} is {pathIfExists}");
            return;
        }
        
        Console.WriteLine(
            Enum.TryParse<BuiltInCommand>(arguments[0], true, out _)
                ? $"{arguments[0]} is a shell builtin"
                : $"{arguments[0]}: not found");
    }
    
    private static bool IsExecutableFileExist(string fileName, out string pathIfExists) {
        string? path = Environment.GetEnvironmentVariable("PATH");
        string[] paths = (path ?? string.Empty).Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
        
        if (OperatingSystem.IsWindows()) {
            string[] pathext = (Environment.GetEnvironmentVariable("PATHEXT") ?? string.Empty)
                .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string filePath in paths) {
                if (!Directory.Exists(filePath)) { continue; }

                string[] files = Directory.GetFiles(filePath);
                bool exists = (
                    from file in files
                    let name = Path.GetFileNameWithoutExtension(file)
                    let ext = Path.GetExtension(file)
                    where name.Equals(fileName, StringComparison.OrdinalIgnoreCase) &&
                          pathext.Contains(ext, StringComparer.OrdinalIgnoreCase)
                    select file)
                    .Any();

                if (exists) {
                    pathIfExists = Path.Combine(filePath, fileName);
                    return true;
                }
            }
            
        } else if (OperatingSystem.IsLinux()) {
            foreach (string filePath in paths) {
                if (!Directory.Exists(filePath)) { continue; }
                
                string[] files = Directory.GetFiles(filePath);
                bool exists = (from file in files
                    let name = Path.GetFileName(file)
                    let fileMode = File.GetUnixFileMode(file)
                    where name.Equals(fileName, StringComparison.CurrentCultureIgnoreCase) &&
                          fileMode.HasFlag(UnixFileMode.UserExecute | 
                                           UnixFileMode.GroupExecute | 
                                           UnixFileMode.OtherExecute)
                    select file).Any();
                
                if (exists) {
                    pathIfExists = Path.Combine(filePath, fileName);
                    return true;
                }
            }
        }

        pathIfExists = String.Empty;
        return false;
    }
} 
