namespace src;

using FuncType = Func<List<string>, Env, string>;

public class Builtins(Env env)
{
    private Env Env { get; init; } = env;
    private readonly Dictionary<string, FuncType> Commands = GetBuiltins();

    public string Run(string command, List<string> args)
    {
        string result = Commands[command](args, Env);
        if (string.IsNullOrEmpty(result)) return "";
        return result + "\n";
    }

    public Boolean HasCommand(string command)
    {
        return Commands.ContainsKey(command);
    }

    private static Dictionary<string, FuncType> GetBuiltins()
    {
        Dictionary<string, FuncType> builtins = [];

        builtins.Add("exit", (_, _) => "");
        builtins.Add("echo", (args, _) => string.Join(" ", args));
        builtins.Add("type", MakeType(builtins));
        builtins.Add("pwd", (_, env) => env.GetDirectory());
        builtins.Add("cd", (args, env) => env.ChangeDirectory(args[0]));

        return builtins;
    }

    private static FuncType MakeType(Dictionary<string, FuncType> builtins)
    {
        return (args, env) =>
        {
            if (args.Count == 0) return string.Empty;

            string command = args.First();

            if (builtins.ContainsKey(command)) return $"{command} is a shell builtin";

            return env.Programs.TryGetValue(command, out var path)
                ? $"{command} is {path}"
                : $"{command}: not found";
        };
    }
}