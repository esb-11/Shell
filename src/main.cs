using System.Text;

class Shell
{
    public static Boolean Exit = false;
    public static readonly HashSet<string> BuiltinCommands = [];
    public static string[] PATH = [];
    public static readonly HashSet<string> PathBinaries = [];
    public static readonly string BuiltinPath = "src/commands";

    static void Main(string[] args)
    {
        PATH = Environment.GetEnvironmentVariable("PATH")?.Split(":") ?? [];
        LoadPathBinaries();
        LoadBuiltinCommands();
        while (!Exit)
        {
            string input = Read();
            if (input == "exit") break;
            else if (input.Length == 0) continue;
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
        if (command == "echo")
        {
            Echo.Execute(arguments);
        }
        else if (command == "type")
        {
            Type.Execute(arguments);
        }
        else
        {
            CommandNotFound(command);
        }
    }

    public static void CommandNotFound(string command)
    {
        Console.WriteLine($"{command}: not found");
    }

    public static void LoadBuiltinCommands()
    {
        IEnumerable<string> files = Directory.EnumerateFiles(BuiltinPath);

        foreach (var file in files)
        {
            string fileName = file[..file.LastIndexOf('.')];
            BuiltinCommands.Add(fileName);
        }
    }

    public static void LoadPathBinaries()
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

}