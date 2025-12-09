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
                string result = Shell.SearchPath(arg);
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

}