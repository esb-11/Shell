namespace src;

public class Env
{
    private string CurrentDirectory { get; set; }
    public Dictionary<string, string> Programs { get; init; }

    public Env()
    {
        CurrentDirectory = Environment.CurrentDirectory;
        Programs = [];

        string? path = Environment.GetEnvironmentVariable("PATH");
        string[] pathDirectories = path is null ? [] : path.Split(":");
        if (path is null) Console.WriteLine("Warning: PATH variable not found");

        foreach (var dir in pathDirectories)
        {
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(dir);
            }
            catch (Exception)
            {
                files = [];
            }
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (!fileInfo.UnixFileMode.HasFlag(UnixFileMode.UserExecute)) continue;

                string programName = file.Split('/').Last();
                Programs.TryAdd(programName, file);
            }
        }
    }

    public Boolean HasProgram(string program)
    {
        return Programs.ContainsKey(program);
    }

    public string GetDirectory()
    {
        return CurrentDirectory;
    }

    public string ChangeDirectory(string dir)
    {
        if (Directory.Exists(dir))
        {
            Directory.SetCurrentDirectory(dir);
            CurrentDirectory = dir;
            return "";
        }

        return $"cd: {dir}: No such file or directory";
    }
}
