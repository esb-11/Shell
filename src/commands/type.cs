public class Type
{
    public static void Execute(string[] args)
    {
        foreach (var arg in args)
        {
            if (Shell.BuiltinCommands.Contains($"{Shell.BuiltinPath}/{arg}"))
            {
                Console.WriteLine($"{arg} is a shell builtin");
            }
            else
            {
                string result = SearchPath(arg);
                if (result != "")
                {
                    Console.WriteLine($"{arg} is {result}");
                }
                else
                {
                    Console.WriteLine($"{arg}: not found");
                }
            }
        }
    }

    static string SearchPath(string command)
    {
        foreach (var dir in Shell.PATH)
        {
            string fileName = $"{dir}/{command}";
            if (Shell.PathBinaries.Contains(fileName))
            {
                return fileName;
            }
        }
        
        return "";
    }
}