namespace src;

public class Env
{
    public string CurrentDirectory { get; init; }
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
}
