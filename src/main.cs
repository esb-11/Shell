namespace src;

class Program
{
    public static void Main(string[] args)
    {
        Shell shell = new();
        while (shell.Command != "exit")
        {
            shell.Read();
            if (shell.Command.Length == 0) continue;
            string result = shell.Eval();
            if (result.Length > 0) Console.WriteLine(result.Length);
        }
    }
}