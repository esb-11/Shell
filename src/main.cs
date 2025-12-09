using System.Text;
using System.Diagnostics;

class Shell
{
    public static Boolean End = false;
    public static readonly HashSet<string> BuiltinCommands = [];
    public static string[] PATH = [];
    public static readonly HashSet<string> PathBinaries = [];
    public static readonly string BuiltinPath = "src/commands";
    public static string WorkingDirectory = "";

    static void Main(string[] args)
    {
        WorkingDirectory = Directory.GetCurrentDirectory();
        PATH = Environment.GetEnvironmentVariable("PATH")?.Split(":") ?? [];
        LoadPathBinaries();
        LoadBuiltinCommands();
        while (!End)
        {
            string input = Read();
            if (input.Length == 0) continue;
            var (command, arguments) = ParseInput(input);
            Evaluate(command, arguments);
        }
    }

    static string Read()
    {
        Console.Write("$ ");
        string input = Console.ReadLine() ?? "";

        return input;
    }

    static (string command, string[] arguments) ParseInput(string input)
    {
        StringBuilder sb = new();
        string command = "";
        List<string> arguments = [];

        for (int i = 0; i <= input.Length; i++)
        {
            char c = i < input.Length ? input[i] : ' ';
            // when we find an empty space, define the command as the current sb value
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

        return (command, arguments.ToArray());
    }

    static void Evaluate(string command, string[] arguments)
    {
        switch (command)
        {
            case "echo":
                Echo.Execute(arguments);
                break;
            case "type":
                Type.Execute(arguments);
                break;
            case "exit":
                Exit.Execute();
                break;
            case "pwd":
                PWD.Execute();
                break;
            default:
                string commandPath = SearchPath(command);
                if (commandPath.Length > 0)
                    ExecuteProgram(command, string.Join(" ", arguments));
                else CommandNotFound(command);
                break;
        }
    }

    public static void CommandNotFound(string command)
    {
        Console.WriteLine($"{command}: not found");
    }

    static void LoadBuiltinCommands()
    {
        IEnumerable<string> files = Directory.EnumerateFiles(BuiltinPath);

        foreach (var file in files)
        {
            string fileName = file[..file.LastIndexOf('.')];
            BuiltinCommands.Add(fileName);
        }
    }

    static void LoadPathBinaries()
    {
        foreach (var folder in PATH)
        {
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(folder);
            }
            catch (System.Exception)
            {
                files = [];
            }

            foreach (var file in files)
            {
                UnixFileMode fileMode = File.GetUnixFileMode(file);
                if ((fileMode & UnixFileMode.UserExecute) == UnixFileMode.UserExecute)
                {
                    PathBinaries.Add(file);
                }
            }

        }
    }

    public static string SearchPath(string command)
    {
        foreach (var dir in PATH)
        {
            string fileName = $"{dir}/{command}";
            if (PathBinaries.Contains(fileName))
            {
                return fileName;
            }
        }

        return "";
    }

    static void ExecuteProgram(string command, string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        Console.Write(output);
    }
}