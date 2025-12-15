namespace src;

class Program
{
    public static void Main(string[] args)
    {
        Shell shell = new();
        while (shell.Command != "exit")
        {
            string result = shell.Eval();
            if (result.Length > 0) Console.WriteLine(result);
            shell.Read();
            if (shell.Command.Length == 0) continue;
        }
    }
}