class Program
{
    static public string[] commandsList = ["exit", "echo", "type"];
    static void Main()
    {
        string input = "";
        while (input != "exit")
        {
            input = Read();
            if (input == "exit") break;
            Evaluate(input);
        }
    }

    static string Read()
    {
        Console.Write("$ ");
        string input = Console.ReadLine() ?? "";

        return input;
    }

    static void Evaluate(string input)
    {
        if (input.Length == 0) return;

        string[] args = input.Split(" ");
        string command = args[0];

        if (command == "echo")
        {
            Echo(args[1..]);
        }
        else if (command == "type")
        {
            Type(args[1]);
        }
        else
        {
            CommandNotFound(command);
        }
    }

    static void Echo(string[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }

    static void Type(string commandName)
    {
        if (commandsList.Contains(commandName))
        {
            Console.WriteLine($"{commandName} is a shell builtin");
        }
        else
        {
            CommandNotFound(commandName);
        }
    }

    static void CommandNotFound(string command)
    {
        Console.WriteLine($"{command}: not found");
    }
}
