using BandLab.Files.Interfaces;

namespace BandLab.Files;

public class FileStorage(string? directory = null) : IFileStorage, IDisposable
{
    private readonly DirectoryInfo directory = directory is null
        ? Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()))
        : Directory.CreateDirectory(directory);

    public string GetPath(string file) =>
        Path.Combine(directory.FullName, file);

    public async Task<string> SaveAsync(string file, Stream data)
    {
        var path = GetPath(file);
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);

        await data.CopyToAsync(fs);

        return path;
    }

    public FileStream Read(string path) =>
        File.OpenRead(path);

    public void Delete(string path) =>
        File.Delete(path);


    public void Dispose()
    {
        Directory.Delete(directory.FullName);
    }
}
