using System.Text;

HashSet<string> builtin = ["exit", "echo", "type"];

string input = "";
while (input != "exit")
{
    input = Read();
    if (input == "exit") break;
    else if (input.Length == 0) return;
    var (command, arguments) = ParseInput(input);
    Evaluate(command, arguments);
}

string Read()
{
    Console.Write("$ ");
    string input = Console.ReadLine() ?? "";

    return input;
}

(string command, string arguments) ParseInput(string input)
{
    StringBuilder command = new();

    foreach (char c in input)
    {
        if (c == ' ' && command.Length > 0)
        {
            break;
        }
        else
        {
            command.Append(c);
        }
    }

    string arguments = input[command.Length..].Trim(' ');

    return (command.ToString(), arguments);
}

void Evaluate(string command, string arguments)
{
    if (command == "echo")
    {
        Echo(arguments);
    }
    else if (command == "type")
    {
        Type(arguments);
    }
    else
    {
        CommandNotFound(command);
    }
}

void Echo(string arguments)
{
    Console.WriteLine(string.Join(" ", arguments));
}

void Type(string command)
{
    if (builtin.Contains(command))
    {
        Console.WriteLine($"{command} is a shell builtin");
    }
    else
    {
        CommandNotFound(command);
    }
}

void CommandNotFound(string command)
{
    Console.WriteLine($"{command}: not found");
}
