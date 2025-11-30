using System.Diagnostics.Metrics;

namespace BandLab.Scaling;

public class Probes
{
    public readonly Counter<int> Scaled;

    public Probes(IMeterFactory core)
    {
        var counters = core.Create("BandLab.Scaling");

        Scaled = counters.CreateCounter<int>("scaling.scaled");
    }
}
