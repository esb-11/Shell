class Program
{
    static void Main()
    {
        string input = Read();
        while (input != "exit")
        {
            input = Read();
        }
    }

    static string Read()
    {
        Console.Write("$ ");
        string input = Console.ReadLine() ?? "";

        if (input.Length > 0 && !ValidateCommand(input))
            Console.WriteLine($"{input}: command not found");

        return input;
    }

    static bool ValidateCommand(string command)
    {
        if (command == "exit")
        {
            return true;
        }

        return false;
    }
}
