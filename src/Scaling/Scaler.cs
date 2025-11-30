using BandLab.Files.Interfaces;
using BandLab.Scaling.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BandLab.Scaling;

internal class Scaler(IFileStorage storage, Probes probes, ILogger<Scaler> log) : IScaleProcessing
{
    const int width = 600;
    const int height = 600;

    public async Task<string> Scaling(Scale data)
    {
        try
        {
            var path = storage.GetPath($"{Path.GetFileNameWithoutExtension(data.File)}.jpg");
            using var fs = storage.Read(data.Path);
            using var image = Image.Load(fs);

            image.Mutate(x => x.Resize(width, height));

            await image.SaveAsJpegAsync(path);

            probes.Scaled.Add(1);
            log.LogDebug("Converted file {file} saved to {path}",
                data.File,
                path);

            return path;
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to convert file {file}: {failure}", data.File, e.Message);

            throw;
        }
        finally
        {
            storage.Delete(data.Path);
        }
    }
}
