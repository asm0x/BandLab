using BandLab.Scaling.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BandLab.Scaling.Queue;

internal class ScaleProcessing(IOptions<ScalingQueueOptions> options, IScaleProcessing process, ILogger<ScaleProcessing> log)
    : ScaleQueue(options.Value, log)
{
    protected override Task Connected() =>
        Subscribe(tasks,
            async value =>
            {
                var scaled = await process.Scaling(value);

                await Publish(done,
                    new Scale(value.File, scaled));
            });
}
