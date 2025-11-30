namespace BandLab.Files.Interfaces;

public interface IFileStorage
{
    string GetPath(string file);
    Task<string> SaveAsync(string file, Stream data);
    FileStream Read(string path);
    void Delete(string path);
}
