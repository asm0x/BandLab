using BandLab.Scaling.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BandLab.Scaling.Queue;

internal class ScaleTasks(IOptions<ScalingQueueOptions> options, ILogger<ScaleTasks> log)
    : ScaleQueue(options.Value, log), IScaleTasks
{
    public ValueTask RunScale(string file, string path) =>
        Publish(tasks, new Scale(file, path));
}
