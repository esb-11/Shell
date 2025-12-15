using System.Text;
using System.Diagnostics;
namespace src;

class Shell
{
    private Env Env { get; init; }
    private Builtins Builtins { get; init; }
    public string Command = "";
    public List<string> Arguments = [];

    public Shell()
    {
        Env = new Env();
        Builtins = new Builtins(Env);
    }

    public void Read()
    {
        Console.Write("$ ");
        string input = Console.ReadLine() ?? "";

        (Command, Arguments) = ParseInput(input);
    }

    static private (string command, List<string> arguments) ParseInput(string input)
    {
        List<string> args = [.. input.Split(' ')];
        args.RemoveAll((str) => string.IsNullOrEmpty(str));

        return (args[0], args[1..]);
    }

    public string Eval()
    {
        if (Builtins.HasCommand(Command)) return Builtins.Run(Command, Arguments);
        else if (Env.HasProgram(Command)) return ExecuteProgram();
        return $"{Command}: not found";
    }

    private string ExecuteProgram()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = string.Join(" ", Arguments),
            RedirectStandardOutput = true,
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }
}