using System.Text;
using System.Diagnostics;
namespace src;

class Shell
{
    private Env Env { get; init; }
    private Builtins  Builtins { get; init; }
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
        StringBuilder sb = new();
        string command = "";
        List<string> arguments = [];

        for (int i = 0; i <= input.Length; i++)
        {
            char c = i < input.Length ? input[i] : ' ';
            // when we find the first empty space, define the command as the current sb value
            if (c == ' ' && command == "")
            {
                command = sb.ToString();
                sb.Clear();
            }
            // after the command was found, start populating the arguments array
            else if (c == ' ' && sb.Length > 0)
            {
                arguments.Add(sb.ToString());
                sb.Clear();
            }
            else if (c != ' ')
            {
                sb.Append(c);
            }
        }

        return (command, arguments);
    }

    public string Eval()
    {
        if (Builtins.HasCommand(Command)) return Builtins.Run(Command, Arguments);
        else if (Env.HasProgram(Command)) return ExecuteProgram();
        Command = "";
        Arguments = [];
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