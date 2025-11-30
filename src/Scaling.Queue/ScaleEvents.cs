using BandLab.Scaling.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BandLab.Scaling.Queue;

internal class ScaleEvents(IOptions<ScalingQueueOptions> options, IScaleEvents process, ILogger<ScaleEvents> log)
    : ScaleQueue(options.Value, log)
{
    protected override Task Connected() =>
        Subscribe(done, process.Done);
}
