using BandLab.Files.Interfaces;
using BandLab.Scaling.Interfaces;

namespace BandLab.CDN;

public class Uploader(IFileStorage storage, ILogger<Uploader> log) : IScaleEvents
{
    public async Task Done(Scale data)
    {
        try
        {
            using var fs = storage.Read(data.Path);

            await storage.SaveAsync(data.File, fs);

        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to upload file {file}: {failure}", data.File, e.Message);

            throw;
        }
        finally
        {
            storage.Delete(data.Path);
        }
    }
}
