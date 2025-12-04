class Program
{
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
        else
        {
            Console.WriteLine($"{command}: command not found");
        }
    }

    static void Echo(string[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }
}
